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
    public class AvisoPrivacidadController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public AvisoPrivacidadController(AppDbContext context,IMapper mapper)
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

                ResponseDto response = new ResponseDto( ResponseDtoEnum.Success)
                {
                    response = await _context.CAT_AVISO_PRIVACIDAD.FirstOrDefaultAsync(),
                };
                return Ok(response);

            }catch (Exception ex)
            {
                return BadRequest(new ResponseDto(ResponseDtoEnum.Error).message + ex.Message);
            }

            //return new List<AirLines>();
        }

        // PUT api/<EventsController>/5
        [HttpPost("Update")]
        public async Task<ActionResult> Update(int id, [FromBody] AvisoPrivacidadDto data)
        {
            try
            {
                var item = await _context.CAT_AVISO_PRIVACIDAD.FirstOrDefaultAsync();


                item.AVISO_ESP = data.AVISO_ESP;
                item.AVISO_ENG= data.AVISO_ENG;

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
