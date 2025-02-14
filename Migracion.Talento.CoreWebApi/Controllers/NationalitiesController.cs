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
    public class NationalitiesController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public NationalitiesController(AppDbContext context,IMapper mapper) {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/<ValuesController>
        [HttpGet]
        public async Task<ActionResult<ResponseDto>> Get()
        {
            try
            {
                if (!await _context.CAT_NATIONALITIES.AnyAsync())
                    return Ok(new ResponseDto(ResponseDtoEnum.NoData));

                ResponseDto response = new ResponseDto(ResponseDtoEnum.Success);
                response.response = await _context.CAT_NATIONALITIES.OrderBy(p => p.DESC_NACIONALITY_SP).ToListAsync();
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
            if (!await _context.CAT_NATIONALITIES.AnyAsync(even => even.ID_NATIONALITY == (id)))
                return Ok(new ResponseDto(ResponseDtoEnum.NoData));

            var item = await _context.CAT_NATIONALITIES
                .Where((even) => even.ID_NATIONALITY == id).FirstOrDefaultAsync();

            ResponseDto result = new ResponseDto(ResponseDtoEnum.Success);
            result.response = item;

            return Ok(result);
        }


        // POST api/<ValuesController>
        [HttpPost("Save")]
        public async Task<ActionResult> Save([FromBody] NationalityRegisterDto value)
        {
            try
            {
                var exist = await _context.CAT_NATIONALITIES.AnyAsync(g => g.DESC_NACIONALITY_SP == value.DESC_NACIONALITY_SP
                || g.DESC_NACIONALITY_EN == value.DESC_NACIONALITY_EN);
                if (exist)
                    return Ok(new ResponseDto(ResponseDtoEnum.Duplicated));
                var Event = _mapper.Map<Nationalities>(value);

                await _context.CAT_NATIONALITIES.AddAsync(Event);
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
        public async Task<ActionResult> Update(int id, [FromBody] NationalityRegisterDto data)
        {
            try
            {
                var nationDb = await _context.CAT_NATIONALITIES.AnyAsync(g => g.ID_NATIONALITY == id);
                if (!nationDb)
                    return Ok(new ResponseDto(ResponseDtoEnum.NoData));

                var item = await _context.CAT_NATIONALITIES
                     .Where((even) => even.ID_NATIONALITY == id).FirstOrDefaultAsync();


                item.DESC_NACIONALITY_SP = data.DESC_NACIONALITY_SP;
                item.DESC_NACIONALITY_EN = data.DESC_NACIONALITY_EN;
                item.RESTRICTION = data.RESTRICTION;
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
        public async Task<ActionResult> Delete([FromBody] NationalitiesDto data)
        {
            try
            {
                int id = data.ID_NALCIONALITY;
                var Event = await _context.CAT_NATIONALITIES.FirstOrDefaultAsync(g => g.ID_NATIONALITY.Equals(id));

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
