using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Migracion.Talento.Models;
using Migracion.Talento.WebAPI.DataConnection;
using CommonTools.DTOs.Query;
using CommonTools.DTOs.Register;
using CommonTools.Enums;
using static iTextSharp.text.pdf.AcroFields;
using Microsoft.Extensions.Logging;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Migracion.Talento.CoreWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public RolesController(AppDbContext context,IMapper mapper)
        {
            _context  = context;
            _mapper = mapper;
        }
        // GET: api/<ValuesController>
        [HttpGet]
        public async Task<ActionResult<ResponseDto>> Get()
        {
            try
            {
                if (!await _context.CAT_ROLE_BY_COMPANIES.AnyAsync())
                    return Ok(new ResponseDto(ResponseDtoEnum.NoData));

                ResponseDto response = new ResponseDto( ResponseDtoEnum.Success)
                {
                    response = await _context.CAT_ROLE_BY_COMPANIES.OrderBy(p => p.DESC_ROLE_SP).ToListAsync(),
                };
                return Ok(response);

            }catch (Exception ex)
            {
                return BadRequest(new ResponseDto(ResponseDtoEnum.Error).message + ex.Message);
            }
        }

        [HttpGet("GetById")]
        public async Task<ActionResult<ResponseDto>> GetById(int id)
        {
            ResponseDto result;
            try
            {
                if (!await _context.CAT_ROLE_BY_COMPANIES.AnyAsync(even => even.ID_ROLE == (id)))
                    return Ok(new ResponseDto(ResponseDtoEnum.NoData));

                var item = await _context.CAT_ROLE_BY_COMPANIES
                    .Where((even) => even.ID_ROLE == id).FirstOrDefaultAsync();

                result = new ResponseDto(ResponseDtoEnum.Success);
                result.response = item;

                return Ok(result);
            }
            catch (Exception ex)
            {
                result = new ResponseDto(ResponseDtoEnum.Error);
                result.message = ex.Message;
                return result;
            }
        }


        // POST api/<ValuesController>
        [HttpPost("SaveRolesAndPermissions")]
        public async Task<ActionResult> SaveRolesAndPermissions([FromBody] CatalogoPerfilesDto data)
        {
            try
            {
                var exist = await _context.CAT_ROLE_BY_COMPANIES.AnyAsync(g => g.DESC_ROLE_SP == data.Descripcion);
                if (exist)
                    return Ok(new ResponseDto(ResponseDtoEnum.Duplicated));


                var item = new RoleByCompany() { ACTIVE=true, CREATED_BY=data.CREATED_BY, CREATED_DATE=DateTime.Now, 
                    DESC_ROLE_SP = data.Descripcion, DESC_ROLE_EN=data.Descripcion, ID_ROLE=data.IdRol };

                await _context.CAT_ROLE_BY_COMPANIES.AddAsync(item);
                await _context.SaveChangesAsync();

                //Actualizar los permisos 
                foreach (var currentProcess in data.Modulos)
                {
                    /* 
                     * Escenarios 
                     * el modulo llego en true - se debe insertar el registro
                     */
                    var moduloAsignado = currentProcess.moduloActivo;
                    if (moduloAsignado)
                    {
                        // Agregar modulo
                        RoleProcess nuevoRegistro = new RoleProcess();
                        nuevoRegistro.ID_ROLE = item.ID_ROLE;
                        nuevoRegistro.CREATED_BY = data.MODIFY_BY;
                        nuevoRegistro.CREATED_DATE = DateTime.Now;
                        nuevoRegistro.ACTIVE = true;
                        nuevoRegistro.ID_PROCESS = currentProcess.ID_PROCESS;
                        await _context.ROL_PROCESS.AddAsync(nuevoRegistro);
                        await _context.SaveChangesAsync();
                    }

                }


                return Ok(new ResponseDto(ResponseDtoEnum.Success));
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseDto(ResponseDtoEnum.Error).message + ex.Message);
            }

        }

       

        [HttpPost("UpdateRolesAndPermissions")]
        public async Task<ActionResult> UpdateRolesAndPermissions(int id, [FromBody] CatalogoPerfilesDto data)
        {
            try
            {
                //Modificar el registro principal del catalogo de rol
                var genderDb = await _context.CAT_ROLE_BY_COMPANIES.AnyAsync(g => g.ID_ROLE == id);
                if (!genderDb)
                    return Ok(new ResponseDto(ResponseDtoEnum.NoData));

                var item = await _context.CAT_ROLE_BY_COMPANIES
                     .Where((even) => even.ID_ROLE == id).FirstOrDefaultAsync();

                item.DESC_ROLE_SP = data.Descripcion;
                item.DESC_ROLE_EN = data.Descripcion;
                item.MODIFY_BY = data.MODIFY_BY;
                item.MODIFY_DATE = DateTime.Now;
                await _context.SaveChangesAsync();


                //Actualizar los permisos 
                foreach (var currentProcess in data.Modulos)
                {
                    /* 
                     * Escenarios 
                     * el modulo llego en true - se debe buscar el registro, si no existe se agrega
                     * El modulo llego en false - se debe buscar el registro, si existe se borra
                     */
                    var existeModuloRol = await _context.ROL_PROCESS.AnyAsync(g => g.ID_ROLE == data.IdRol && g.ID_PROCESS == currentProcess.ID_PROCESS);
                    var moduloAsignado = currentProcess.moduloActivo;
                    if (moduloAsignado)
                    {
                        if(!existeModuloRol)
                        {
                            // Agregar modulo
                            RoleProcess nuevoRegistro = new RoleProcess();
                            nuevoRegistro.ID_ROLE = id;
                            nuevoRegistro.CREATED_BY = data.MODIFY_BY;
                            nuevoRegistro.CREATED_DATE = DateTime.Now;
                            nuevoRegistro.ACTIVE = true;
                            nuevoRegistro.ID_PROCESS = currentProcess.ID_PROCESS;
                            await _context.ROL_PROCESS.AddAsync(nuevoRegistro);
                            await _context.SaveChangesAsync();
                        }
                    }
                    else
                    {
                        if (existeModuloRol)
                        {
                            // Eliminar modulo
                            var rolModulo = await _context.ROL_PROCESS.Where(g => g.ID_ROLE == data.IdRol && g.ID_PROCESS == currentProcess.ID_PROCESS).FirstOrDefaultAsync();
                            if (rolModulo != null)
                            {
                                _context.Remove(rolModulo);
                                await _context.SaveChangesAsync();
                            }
                        }
                    }

                }

                return Ok(new ResponseDto(ResponseDtoEnum.Success));
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseDto(ResponseDtoEnum.Error).message + ex.Message);
            }
        }


        // DELETE api/<EventsController>/5
        [HttpPost("Delete")]
        public async Task<ActionResult> Delete([FromBody] RoleByCompanyDto data)
        {
            try
            {
                // Buscar usuarios asignados al rol
                var ExisteUsuarioAsignado = await _context.CAT_USERS.AnyAsync(g => g.ID_ROLE == data.ID_ROLE);
                if (ExisteUsuarioAsignado)
                    return BadRequest(new ResponseDto(ResponseDtoEnum.Error) { code=405, error=true, message="Hay al menos un usuario asignado a este rol, no se puede eliminar de esta forma."});


                var ExisteModuloAsignado = await _context.ROL_PROCESS.AnyAsync(g => g.ID_ROLE == data.ID_ROLE);
                if (ExisteModuloAsignado)
                    return BadRequest(new ResponseDto(ResponseDtoEnum.Error) { code = 405, error = true, message = "Hay al menos un módulo asignado a este rol, no se puede eliminar de esta forma." });


                var Item = await _context.CAT_ROLE_BY_COMPANIES.FirstOrDefaultAsync(g => g.ID_ROLE == data.ID_ROLE);
                if (Item != null)
                {
                    _context.Remove(Item);
                    await _context.SaveChangesAsync();
                    return Ok(new ResponseDto(ResponseDtoEnum.Success));
                }
                else
                {
                    return Ok(new ResponseDto(ResponseDtoEnum.Error) { message=$"No se encontro el rol para borrar"});
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseDto(ResponseDtoEnum.Error).message + ex.Message);
            }
        }



      
    }
}
