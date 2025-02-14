using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Migracion.Talento.WebAPI.DataConnection;
using Migracion.Talento.WebAPI;
using Migracion.Talento.Models;
using AutoMapper;
using CommonTools.DTOs.Query;
using CommonTools.Enums;
using CommonTools.DTOs.Register;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Migracion.Talento.CoreWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AirPortsController : ControllerBase
    {
        private AppDbContext _context;
        private readonly IMapper _mapper;

        public AirPortsController(AppDbContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        // GET: api/<AirPortsController>
        [HttpGet]
        public async Task<ActionResult<ResponseDto>> Get()
        {
            try
            {
                if (!await _context.CAT_AIRPORTS.AnyAsync())
                    return Ok(new ResponseDto(ResponseDtoEnum.NoData));

                ResponseDto response = new ResponseDto(ResponseDtoEnum.Success);
                response.response = await _context.CAT_AIRPORTS.OrderBy(p => p.DESC_AIRPORT_SP).ToListAsync();
                return Ok(response);
            }
            catch(Exception ex)
            {
                return BadRequest(new ResponseDto(ResponseDtoEnum.Error).message + ex.Message);
            }

        }

        [HttpGet("GetById")]
        public async Task<ActionResult<ResponseDto>> GetById(int id)
        {
            if (!await _context.CAT_AIRPORTS.AnyAsync(even => even.ID_AIRPORT == (id)))
                return Ok(new ResponseDto(ResponseDtoEnum.NoData));

            var item = await _context.CAT_AIRPORTS
                .Where((even) => even.ID_AIRPORT == id).FirstOrDefaultAsync();

            ResponseDto result = new ResponseDto(ResponseDtoEnum.Success);
            result.response = item;

            return Ok(result);
        }


        // POST api/<ValuesController>
        [HttpPost("Save")]
        public async Task<ActionResult> Save([FromBody] AirPortRegisterDto value)
        {
            try
            {
                var exist = await _context.CAT_AIRPORTS.AnyAsync(g => g.DESC_AIRPORT_SP == value.DESC_AIRPORT_SP
                || g.DESC_AIRPORT_EN == value.DESC_AIRPORT_EN);
                if (exist)
                    return Ok(new ResponseDto(ResponseDtoEnum.Duplicated));

                if (string.IsNullOrEmpty(value.IATA))
                    value.IATA = "IAT";

                if (string.IsNullOrEmpty(value.LOCATION))
                    value.LOCATION = "LOCATION";

                var Event = _mapper.Map<AirPorts>(value);
                await _context.CAT_AIRPORTS.AddAsync(Event);
                await _context.SaveChangesAsync();
                return Ok(new ResponseDto(ResponseDtoEnum.Success));
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseDto(ResponseDtoEnum.Error).message + ex.Message);
            }

        }


        internal async Task<ResponseDto> SaveIfNotExist([FromBody] AirPortRegisterDto data)
        {
            try
            {
                ResponseDto res = new ResponseDto(ResponseDtoEnum.Success);
                var exist = await _context.CAT_AIRPORTS.AnyAsync(g => g.DESC_AIRPORT_SP == data.DESC_AIRPORT_SP
                || g.DESC_AIRPORT_EN == data.DESC_AIRPORT_EN);
                if (exist)
                {
                    var item = await _context.CAT_AIRPORTS
                        .Where((even) => even.DESC_AIRPORT_SP == data.DESC_AIRPORT_SP).FirstOrDefaultAsync();

                    res.response = item;
                    return res;
                }
                else
                {
                    if (string.IsNullOrEmpty(data.IATA))
                        data.IATA = "IAT";

                    if (string.IsNullOrEmpty(data.LOCATION))
                        data.LOCATION = "LOCATION";

                    var model = _mapper.Map<AirPorts>(data);
                    await _context.CAT_AIRPORTS.AddAsync(model);
                    await _context.SaveChangesAsync();
                    res.response = model;
                    return res;
                }
            }
            catch (Exception ex)
            {
                return new ResponseDto(ResponseDtoEnum.Error) { message = ex.Message };
            }
        }


        // PUT api/<EventsController>/5
        [HttpPost("Update")]
        public async Task<ActionResult> Update(int id, [FromBody] AirPortRegisterDto data)
        {
            try
            {
                var portDb = await _context.CAT_AIRPORTS.AnyAsync(g => g.ID_AIRPORT == id);
                if (!portDb)
                    return Ok(new ResponseDto(ResponseDtoEnum.NoData));

                var item = await _context.CAT_AIRPORTS
                     .Where((even) => even.ID_AIRPORT == id).FirstOrDefaultAsync();


                item.DESC_AIRPORT_SP = data.DESC_AIRPORT_SP;
                item.DESC_AIRPORT_EN = data.DESC_AIRPORT_EN;
                item.MODIFY_BY= data.MODIFY_BY;
                item.MODIFY_DATE= DateTime.Now;

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
        public async Task<ActionResult> Delete([FromBody] AirPortsDto data)
        {
            try
            {
                int id = data.ID_AIRPORT;
                var Event = await _context.CAT_AIRPORTS.FirstOrDefaultAsync(g => g.ID_AIRPORT.Equals(id));

                if (Event == null)
                    return Ok(new ResponseDto(ResponseDtoEnum.NoData));

                _context.Remove(Event);
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
