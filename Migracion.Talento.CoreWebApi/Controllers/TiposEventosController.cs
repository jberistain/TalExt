using AutoMapper;
using CommonTools.DTOs.Query;
using CommonTools.DTOs.Register;
using CommonTools.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Migracion.Talento.Models;
using Migracion.Talento.WebAPI.DataConnection;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Migracion.Talento.CoreWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TiposEventosController : ControllerBase
    {
        private AppDbContext _context;
        private readonly IMapper _mapper;

        public TiposEventosController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/<CompaniesController>
        [HttpGet]
        public async Task<ActionResult<ResponseDto>> Get()
        {
            if (!await _context.CAT_EVENT_TYPES.AnyAsync())
                return Ok(new ResponseDto(ResponseDtoEnum.NoData));

            ResponseDto response = new ResponseDto(ResponseDtoEnum.Success);
            response.response = await _context.CAT_EVENT_TYPES.OrderBy(p => p.DESC_ACTIVITY_SP).ToListAsync();
            return Ok(response);

        }
        [HttpGet("GetById")]
        public async Task<ActionResult<ResponseDto>> GetById(int id)
        {
            if (!await _context.CAT_EVENT_TYPES.AnyAsync(even => even.ID_EVENT_TYPE == (id)))
                return Ok(new ResponseDto(ResponseDtoEnum.NoData));

            var item = await _context.CAT_EVENT_TYPES
                .Where((even) => even.ID_EVENT_TYPE == id).FirstOrDefaultAsync();

            ResponseDto result = new ResponseDto(ResponseDtoEnum.Success);
            result.response = item;

            return Ok(result);
        }
        // POST api/<>
        [HttpPost("Save")]
        public async Task<ActionResult<ResponseDto>> Save([FromBody] EventTypesRegisterDto data)
        {
            try
            {
                var exist = await _context.CAT_EVENT_TYPES.AnyAsync(g => g.DESC_ACTIVITY_SP == data.DESC_ACTIVITY_SP
                || g.DESC_ACTIVITY_SP == data.DESC_ACTIVITY_SP);
                if (exist)
                    return Ok(new ResponseDto(ResponseDtoEnum.Duplicated));

                var model = _mapper.Map<EventTypes>(data);
                await _context.CAT_EVENT_TYPES.AddAsync(model);
                await _context.SaveChangesAsync();
                return Ok(new ResponseDto(ResponseDtoEnum.Success));
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseDto(ResponseDtoEnum.Error).message + ex.Message);
            }
        }

        // PUT api/<>/5
        [HttpPost("Update")]
        public async Task<ActionResult> Update(int id, [FromBody] EventTypesRegisterDto data)
        {
            try
            {
                var genderDb = await _context.CAT_EVENT_TYPES.AnyAsync(g => g.ID_EVENT_TYPE == id);
                if (!genderDb)
                    return Ok(new ResponseDto(ResponseDtoEnum.NoData));

                var item = await _context.CAT_EVENT_TYPES
                     .Where((even) => even.ID_EVENT_TYPE == id).FirstOrDefaultAsync();


                item.DESC_ACTIVITY_SP = data.DESC_ACTIVITY_SP;
                item.DESC_ACTIVITY_EN = data.DESC_ACTIVITY_EN;
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

        // DELETE api/<CompaniesController>/5
        [HttpPost("Delete")]
        public async Task<ActionResult> Delete([FromBody] EventTypesDto data)
        {
            try
            {
                int id = data.ID_EVENT_TYPE;
                var company = await _context.CAT_EVENT_TYPES.FirstOrDefaultAsync(g => g.ID_EVENT_TYPE.Equals(id));

                if (company == null)
                    return Ok(new ResponseDto(ResponseDtoEnum.NoData));

                var genderDb = await _context.CAT_EVENTS.AnyAsync(g => g.ID_EVENT_TYPE == id);
                if (genderDb)
                    return Ok(new ResponseDto(ResponseDtoEnum.RegisterWithDependency));

                _context.Remove(company);
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
