using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Migracion.Talento.Models;
using Migracion.Talento.WebAPI.DataConnection;
using CommonTools.DTOs.Query;
using CommonTools.DTOs.Register;
using CommonTools.Enums;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Migracion.Talento.CoreWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesProcessController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public RolesProcessController(AppDbContext context,IMapper mapper)
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
                if (!await _context.ROL_PROCESS.AnyAsync())
                    return Ok(new ResponseDto(ResponseDtoEnum.NoData));

                ResponseDto response = new ResponseDto( ResponseDtoEnum.Success)
                {
                    response = await _context.ROL_PROCESS.OrderBy(p => p.ID_ROL_PROCCESS).ToListAsync(),
                };
                return Ok(response);

            }catch (Exception ex)
            {
                return BadRequest(new ResponseDto(ResponseDtoEnum.Error).message + ex.Message);
            }

            //return new List<AirLines>();
        }

        [HttpGet("GetProcessByIdRol")]
        public async Task<ActionResult<ResponseDto>> GetProcessByIdRol(int idRol)
        {
            ResponseDto result;
            try
            {
                if (!await _context.ROL_PROCESS.AnyAsync(even => even.ID_ROLE == idRol && even.ACTIVE == true))
                    return Ok(new ResponseDto(ResponseDtoEnum.NoData));

                var item = await _context.ROL_PROCESS
                    .Where((even) => even.ID_ROLE == idRol && even.ACTIVE == true).ToListAsync();

                result = new ResponseDto(ResponseDtoEnum.Success);
                result.response = item;
                return Ok(result);
            }
            catch(Exception ex)
            {
                result = new ResponseDto(ResponseDtoEnum.Error);
                result.message = ex.Message;
                return BadRequest(result);
            }
        }


        [HttpGet("GetProcessByIdUsuario")]
        public async Task<ActionResult<ResponseDto>> GetProcessByIdUsuario(int id)
        {
            ResponseDto result;
            try
            {
                if (!await _context.CAT_USERS.AnyAsync(even => even.ID_USER == id))
                    return Ok(new ResponseDto(ResponseDtoEnum.NoData));

                var Usuario = await _context.CAT_USERS
                    .Where(even => even.ID_USER == id).FirstOrDefaultAsync();


                if(Usuario == null || Usuario.ID_ROLE == null || Usuario.ID_ROLE == 0)
                    return Ok(new ResponseDto(ResponseDtoEnum.NoData) { message="El usuario no tiene se tiene un rol asignado"});


                int idRol = (int)Usuario.ID_ROLE;

                if (!await _context.ROL_PROCESS.AnyAsync(even => even.ID_ROLE == idRol))
                    return Ok(new ResponseDto(ResponseDtoEnum.NoData));

                var item = await _context.ROL_PROCESS
                    .Where((even) => even.ID_ROLE == idRol).ToListAsync();

                List<Process> processList = new List<Process>();
                foreach(var rolProcess in item)
                {
                    var modulo = await _context.CAT_PROCESS
                        .Where(even => even.ID_PROCESS == rolProcess.ID_PROCESS && even.ACTIVE == true).FirstOrDefaultAsync();
                    if (modulo != null)
                        processList.Add(modulo);
                }

                result = new ResponseDto(ResponseDtoEnum.Success);
                result.response = processList;
                return Ok(result);
            }
            catch (Exception ex)
            {
                result = new ResponseDto(ResponseDtoEnum.Error);
                result.message = ex.Message;
                return BadRequest(result);
            }
        }


        // POST api/<ValuesController>
        [HttpPost("Save")]
        public async Task<ActionResult> Save([FromBody] RolProcessRegisterDto value)
        {
            try
            {
                var exist = await _context.ROL_PROCESS.AnyAsync(g => g.ID_ROLE == value.ID_ROLE && g.ID_PROCESS == value.ID_PROCESS);
                if (exist)
                    return Ok(new ResponseDto(ResponseDtoEnum.Duplicated));
                var item = _mapper.Map < RoleProcess>(value);

                await _context.ROL_PROCESS.AddAsync(item);
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
        public async Task<ActionResult> Update(int id, [FromBody] RolProcessDto data)
        {
            try
            {
                var genderDb = await _context.ROL_PROCESS.AnyAsync(g => g.ID_ROLE == id);
                if (!genderDb)
                    return Ok(new ResponseDto(ResponseDtoEnum.NoData));

                var item = await _context.ROL_PROCESS
                     .Where((even) => even.ID_ROLE == id).FirstOrDefaultAsync();


                item.ID_ROLE = data.ID_ROLE;

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
        public async Task<ActionResult> Delete([FromBody] RolProcessDto data)
        {
            try
            {
                var Item = await _context.ROL_PROCESS.FirstOrDefaultAsync(g => g.ID_ROL_PROCCESS.Equals(data.ID_ROL_PROCCESS));

                if (Item == null)
                    return Ok(new ResponseDto(ResponseDtoEnum.NoData));

                _context.Remove(Item);
                await _context.SaveChangesAsync();
                return Ok(new ResponseDto(ResponseDtoEnum.Success));
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseDto(ResponseDtoEnum.Error).message + ex.Message);
            }
        }



      
    }
}
