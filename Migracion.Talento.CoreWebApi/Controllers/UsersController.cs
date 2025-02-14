using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Migracion.Talento.WebAPI.DataConnection;
using Migracion.Talento.WebAPI;
using Migracion.Talento.Models;
using AutoMapper;
using CommonTools.DTOs.Query;
using CommonTools.Enums;
using CommonTools.DTOs.Register;
using SkiaSharp;
using CommonTools.DTOs;
using Migracion.Talento.CoreWebApi.Interfaces;
using Microsoft.Extensions.Options;
using CommonTools.Pdf;
using Migracion.Talento.CoreWebApi.Services;
using System.Linq;
using System;
using static iTextSharp.text.pdf.AcroFields;
using System.Net.Mail;

namespace Migracion.Talento.CoreWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController:ControllerBase
    {
        private AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IEmailSender _emailSender;
        private readonly ApplicationProperties _configuration;

        public UsersController(AppDbContext context,
            IEmailSender emailSender,
            IOptions<ApplicationProperties> configuration,
            IMapper mapper)
        {
            _context = context;
            _emailSender = emailSender;
            _configuration = configuration.Value;
            _mapper = mapper;
        }
        // GET: api/<AirPortsController>
        [HttpGet]
        public async Task<ActionResult<ResponseDto>> Get()
        {
            try
            {
                if (!await _context.CAT_USERS.AnyAsync())
                    return Ok(new ResponseDto(ResponseDtoEnum.NoData));

                ResponseDto response = new ResponseDto(ResponseDtoEnum.Success);
                response.response = await _context.CAT_USERS.OrderBy(p => p.NAME_USER).ToListAsync();
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseDto(ResponseDtoEnum.Error).message + ex.Message);
            }

        }

        [HttpGet("GetById")]
        public async Task<ActionResult<ResponseDto>> GetById(int id)
        {
            if (!await _context.CAT_USERS.AnyAsync(even => even.ID_USER == (id)))
                return Ok(new ResponseDto(ResponseDtoEnum.NoData));

            var item = await _context.CAT_USERS
                .Where((even) => even.ID_USER == id).FirstOrDefaultAsync();

            ResponseDto result = new ResponseDto(ResponseDtoEnum.Success);
            result.response = item;

            return Ok(result);
        }

        [HttpPost("Login")]
        public async Task<ActionResult<ResponseDto>> Login(UserRegisterDto user)
        {
            try
            {
                if (!await _context.CAT_USERS.AnyAsync(even => even.USERNAME.Equals(user.USERNAME) && even.PASSWORD_USER.Equals(user.PASSWORD_USER)))
                    return Ok(new ResponseDto(ResponseDtoEnum.NoData) { message="Usuario o contraseña incorrectos"});

                var item = await _context.CAT_USERS
                    .Where(even => even.USERNAME.Equals(user.USERNAME) && even.PASSWORD_USER.Equals(user.PASSWORD_USER)).FirstOrDefaultAsync();

                if(item != null && !item.ACTIVE)
                    return Ok(new ResponseDto(ResponseDtoEnum.Error) { message="El usuario no se encuentra activo, contacte a su administrador"});

                ResponseDto result = new ResponseDto(ResponseDtoEnum.Success);
                result.response = item;
                return Ok(result);
            }
            catch (Exception e)
            {
                return Ok(new ResponseDto(ResponseDtoEnum.NoData) { message = e.Message});
            }
        }


        // POST api/<ValuesController>
        [HttpPost("Save")]
        public async Task<ActionResult> Save([FromBody] UserRegisterDto value)
        {
            try
            {
                var exist = await _context.CAT_USERS.AnyAsync(g => g.USERNAME == value.USERNAME);
                if (exist)
                    return Ok(new ResponseDto(ResponseDtoEnum.Duplicated) { message="Ya existe un registro con este nombre de usuario"});
                var Event = _mapper.Map<User>(value);

                await _context.CAT_USERS.AddAsync(Event);
                await _context.SaveChangesAsync();
                return Ok(new ResponseDto(ResponseDtoEnum.Success));
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseDto(ResponseDtoEnum.Error).message + ex.Message);
            }

        }

        // PUT api/<EventsController>/5
        [HttpPost("Update")]
        public async Task<ActionResult> Update(int id, [FromBody] UserRegisterDto data)
        {
            try
            {
                var userDb = await _context.CAT_USERS.AnyAsync(g => g.ID_USER == id);
                if (!userDb)
                    return Ok(new ResponseDto(ResponseDtoEnum.NoData));

                //Obtener todos los usuarios con ese username
                var usuariosList = await _context.CAT_USERS
                .Where((even) => even.USERNAME == data.USERNAME).ToListAsync();

                bool actualizarUsuario = true;
                foreach (User usuario in usuariosList)
                {
                    if(usuario.ID_USER != id)
                    {
                        actualizarUsuario = false;
                        break;
                    }
                }


                if (!actualizarUsuario)
                    return Ok(new ResponseDto(ResponseDtoEnum.Duplicated) { message = "Ya existe otro registro con este nombre de usuario" });

                var item = await _context.CAT_USERS
                     .Where((even) => even.ID_USER == id).FirstOrDefaultAsync();

                item.ID_ROLE = data.ID_ROLE;
                item.NAME_USER = data.NAME_USER;
                item.LAST_NAME_USER = data.LAST_NAME_USER;
                item.CVE_USER = data.CVE_USER;
                item.USERNAME = data.USERNAME;
                item.PASSWORD_USER = data.PASSWORD_USER;
                item.MODIFY_BY = data.MODIFY_BY;
                item.MODIFY_DATE = DateTime.Now;
                item.EMAIL_USER = data.EMAIL_USER;

                await _context.SaveChangesAsync();

                return Ok(new ResponseDto(ResponseDtoEnum.Success));
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseDto(ResponseDtoEnum.Error).message + ex.Message);
            }
        }

        // DELETE api/<EventsController>/5
        [HttpPost("Delete")]
        public async Task<ActionResult> Delete([FromBody] UserDto data) //REVISAR USER DTO
        {
            try
            {
                int id = data.ID_USER;
                var Event = await _context.CAT_USERS.FirstOrDefaultAsync(g => g.ID_USER.Equals(id));

                if (Event == null)
                    return Ok(new ResponseDto(ResponseDtoEnum.NoData));

                //var genderDb = await _context.CAT_ACTIVITIES.AnyAsync(g => g.ID_ACTIVITY == id);
                //if (genderDb)
                //    return Ok(new ResponseDto(ResponseDtoEnum.RegisterWithDependency));

                Event.ACTIVE = false;
                // _context.Remove(Event);
                await _context.SaveChangesAsync();
                return Ok(new ResponseDto(ResponseDtoEnum.Success));
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseDto(ResponseDtoEnum.Error).message + ex.Message);
            }
        }

        // DELETE api/<EventsController>/5
        [HttpPost("Reactive")]
        public async Task<ActionResult> Reactive([FromBody] UserDto data) //REVISAR USER DTO
        {
            try
            {
                int id = data.ID_USER;
                var Event = await _context.CAT_USERS.FirstOrDefaultAsync(g => g.ID_USER.Equals(id));

                if (Event == null)
                    return Ok(new ResponseDto(ResponseDtoEnum.NoData));

                //var genderDb = await _context.CAT_ACTIVITIES.AnyAsync(g => g.ID_ACTIVITY == id);
                //if (genderDb)
                //    return Ok(new ResponseDto(ResponseDtoEnum.RegisterWithDependency));

                Event.ACTIVE = true;
                // _context.Remove(Event);
                await _context.SaveChangesAsync();
                return Ok(new ResponseDto(ResponseDtoEnum.Success));
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseDto(ResponseDtoEnum.Error).message + ex.Message);
            }
        }

        // PUT api/<EventsController>/5
        [HttpPost("UpdatePassword")]
        public async Task<ActionResult> Update(int id, [FromBody] UserRegisterUpdatePwdDto data)
        {
            try
            {
                var userDb = await _context.CAT_USERS.AnyAsync(g => g.ID_USER == id);
                if (!userDb)
                    return Ok(new ResponseDto(ResponseDtoEnum.NoData));

                var item = await _context.CAT_USERS
                     .Where((even) => even.ID_USER == id).FirstOrDefaultAsync();


                item.PASSWORD_USER = data.NewPassword;
                item.MODIFY_BY = data.ID_USER;
                item.MODIFY_DATE = DateTime.Now;

                await _context.SaveChangesAsync();

                return Ok(new ResponseDto(ResponseDtoEnum.Success));
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseDto(ResponseDtoEnum.Error).message + ex.Message);
            }
        }

        private Random random = new Random();
        private string RandomString(int length)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789#$%&()";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        [HttpPost("RecuperaPasswordPorCorreo")]
        public async Task<ActionResult<ResponseDto>> RecuperaPasswordPorCorreo([FromBody] UserRegisterDto model)
        {
            ResponseDto responseDto = new ResponseDto();
            try
            {
                string userName = model.USERNAME;
                string email = model.EMAIL_USER;

                // Buscar usuario y password para confirmar que son correctos.
                if(!await _context.CAT_USERS.AnyAsync(row => row.USERNAME.Equals(userName) && row.EMAIL_USER.Equals(email)))
                {
                    return Ok(new ResponseDto(ResponseDtoEnum.NoData) { message="No se encontró ningún usuario con esta dirección de correo. Ingrese los datos correctos o consulte al administrador"});
                }

                var usuario = await _context.CAT_USERS
                       .Where(row => row.USERNAME.Equals(userName) && row.EMAIL_USER.Equals(email)).FirstOrDefaultAsync();

                // Generar una nueva contraseña aleatoria
                string secredCode = RandomString(10);

                //Guardar en BD la nueva informacion
                usuario.PASSWORD_USER = secredCode;
                var usuarioActualizado = await _context.SaveChangesAsync();

                // Enviar correo electronico
                InvitationMailTemplate dataTemplate = new InvitationMailTemplate()
                {
                    Email = email,
                    Name = $"{usuario.NAME_USER} {usuario.LAST_NAME_USER}",
                    Evento = "",
                    NumSolicitud = $"{usuario.PASSWORD_USER}",
                    Subject = "OCESA - Recuperación de contraseña - Talento Extranjero",
                    UrlToConfirm = "",
                    attachments = new List<AttachmentFileDto>(),
                };
                responseDto = await Template(TemplateTypeEnum.CambioPassword, dataTemplate);

                    // desconectar envio de smtp
                await _emailSender.DisconnectSmtpClient();

                string mensajeRespuesta = "Se envió el nuevo password al correo electrónico.";
                if (responseDto.code == (int)ResponseDtoEnum.Success)
                {
                    responseDto.error = false;
                    responseDto.message = mensajeRespuesta;
                    return Ok(responseDto);
                }
                else
                {
                    responseDto.code = (int)ResponseDtoEnum.NoData;
                    responseDto.error = true;
                    mensajeRespuesta = "Hubo un error al enviar el correo electrónico";
                    responseDto.message = mensajeRespuesta;
                    return Ok(responseDto);
                }

            }
            catch (Exception ex)
            {
                responseDto.error = true;
                responseDto.message = ex.Message;
                responseDto.code = (int)ResponseDtoEnum.NoData;
            }

            return Ok(responseDto);
        }

        private async Task<ResponseDto> Template(TemplateTypeEnum templateType, InvitationMailTemplate invitationMail)
        {
            try
            {
                // Create MailData object
                MailDataDto mailData = new MailDataDto()
                {
                    To = invitationMail.Email,
                    Subject = invitationMail.Subject,
                    Body = _emailSender.GetWelcomeTemplateEmail(templateType.ToString(), invitationMail),
                    Attachments = invitationMail.attachments

                };

                await _emailSender.SendEmailAsync(mailData);
                return new ResponseDto(ResponseDtoEnum.Success);
            }
            catch (Exception ex)
            {
                var error = new ResponseDto(ResponseDtoEnum.Error);
                error.message += ex.Message;
                return error;
            }
        }
    }
}
