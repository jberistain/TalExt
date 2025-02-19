using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Migracion.Talento.Models;
using Migracion.Talento.WebAPI.DataConnection;
using CommonTools.DTOs.Query;
using CommonTools.DTOs.Register;
using CommonTools.Enums;
using Migracion.Talento.Entities.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Migracion.Talento.CoreWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public EventsController(AppDbContext context,IMapper mapper)
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
                if (!await _context.CAT_EVENTS.AnyAsync(e => e.ACTIVE == true)) 
                    return Ok(new ResponseDto(ResponseDtoEnum.NoData));

                ResponseDto response = new ResponseDto(ResponseDtoEnum.Success);
                response.response = await _context.CAT_EVENTS.OrderBy(p => p.DESC_EVENT_SP).Where(e => e.ACTIVE == true).ToListAsync();
                return Ok(response);
                //return new List<AirLines>();
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseDto(ResponseDtoEnum.Error).message + ex.Message);
            }

         
        }

        [HttpGet("GetById")]
        public async Task<ActionResult<ResponseDto>> GetById(int id)
        {
            if (!await _context.CAT_EVENTS.AnyAsync(even => even.ID_EVENT == (id)))
                return Ok(new ResponseDto(ResponseDtoEnum.NoData));

            var item = await _context.CAT_EVENTS
                .Where((even) => even.ID_EVENT == id).FirstOrDefaultAsync();

            ResponseDto result = new ResponseDto(ResponseDtoEnum.Success);
            result.response = item;

            return Ok(result);
        }


        // POST api/<ValuesController>
        [HttpPost("Save")]
        public async Task<ActionResult> Save([FromBody] EventRegisterDto value)
        {
            try
            {
                var exist = await _context.CAT_EVENTS.AnyAsync(g => g.DESC_EVENT_SP == value.DESC_EVENT_SP
                || g.DESC_EVENT_SP == value.DESC_EVENT_SP);
                if (exist)
                    return Ok(new ResponseDto(ResponseDtoEnum.Duplicated));
                var Event = _mapper.Map<Events>(value);

                await _context.CAT_EVENTS.AddAsync(Event);
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
        public async Task<ActionResult> Update(int id, [FromBody] EventRegisterDto data)
        {
            try
            {
                var genderDb = await _context.CAT_EVENTS.AnyAsync(g => g.ID_EVENT == id);
                if (!genderDb)
                    return Ok(new ResponseDto(ResponseDtoEnum.NoData));

                var item = await _context.CAT_EVENTS
                     .Where((even) => even.ID_EVENT == id).FirstOrDefaultAsync();

                item.ID_ESTATE = data.ID_ESTATE;
                item.DESC_LOCATION = data.DESC_LOCATION;
                item.DESC_EVENT_EN = data.DESC_EVENT_EN;
                item.DESC_EVENT_SP = data.DESC_EVENT_SP;
                item.EMAIL1 = data.EMAIL1;
                item.EMAIL2 = data.EMAIL2;
                // item.ID_COMPANY = data.ID_COMPANY;
                item.DATE_INI = Convert.ToDateTime( data.DATE_INI);
                item.DATE_FIN = Convert.ToDateTime( data.DATE_FIN);
                item.ID_EVENT_TYPE = data.ID_EVENT_TYPE;
                item.MODIFY_BY = data.MODIFY_BY;
                item.MODIFY_DATE = DateTime.Now;

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
        public async Task<ActionResult> Delete([FromBody] EventsDto data)
        {
            try
            {
                int id = data.ID_EVENT;
                var Event = await _context.CAT_EVENTS.FirstOrDefaultAsync(g => g.ID_EVENT.Equals(id));

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
