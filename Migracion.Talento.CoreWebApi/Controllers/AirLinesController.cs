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
    public class AirLinesController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public AirLinesController(AppDbContext context,IMapper mapper)
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
                if (!await _context.CAT_AIR_LINES.AnyAsync())
                    return Ok(new ResponseDto(ResponseDtoEnum.NoData));

                ResponseDto response = new ResponseDto( ResponseDtoEnum.Success)
                {
                    response = await _context.CAT_AIR_LINES.OrderBy(p => p.DESC_AIR_LINE_SP).ToListAsync(),
                };
                return Ok(response);

            }catch (Exception ex)
            {
                return BadRequest(new ResponseDto(ResponseDtoEnum.Error).message + ex.Message);
            }

            //return new List<AirLines>();
        }

        [HttpGet("GetById")]
        public async Task<ActionResult<ResponseDto>> GetById(int id)
        {
            if (!await _context.CAT_AIR_LINES.AnyAsync(even => even.ID_AIR_LINE == (id)))
                return Ok(new ResponseDto(ResponseDtoEnum.NoData));

            var item = await _context.CAT_AIR_LINES
                .Where((even) => even.ID_AIR_LINE == id).FirstOrDefaultAsync();

            ResponseDto result = new ResponseDto(ResponseDtoEnum.Success);
            result.response = item;

            return Ok(result);
        }


        // POST api/<ValuesController>
        [HttpPost("Save")]
        public async Task<ActionResult> Save([FromBody] AirLineRegisterDto value)
        {
            try
            {
                var exist = await _context.CAT_AIR_LINES.AnyAsync(g => g.DESC_AIR_LINE_SP == value.DESC_AIR_LINE_SP
                || g.DESC_AIR_LINE_EN == value.DESC_AIR_LINE_EN);
                if (exist)
                    return Ok(new ResponseDto(ResponseDtoEnum.Duplicated));
                var Event = _mapper.Map<AirLines>(value);

                await _context.CAT_AIR_LINES.AddAsync(Event);
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
        public async Task<ActionResult> Update(int id, [FromBody] AirLineRegisterDto data)
        {
            try
            {
                var genderDb = await _context.CAT_AIR_LINES.AnyAsync(g => g.ID_AIR_LINE == id);
                if (!genderDb)
                    return Ok(new ResponseDto(ResponseDtoEnum.NoData));

                var item = await _context.CAT_AIR_LINES
                     .Where((even) => even.ID_AIR_LINE == id).FirstOrDefaultAsync();


                item.DESC_AIR_LINE_EN = data.DESC_AIR_LINE_EN;
                item.DESC_AIR_LINE_SP= data.DESC_AIR_LINE_SP;
                item.MODIFY_BY = data.MODIFY_BY;
                item.MODIFY_DATE = data.MODIFY_DATE;

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
        public async Task<ActionResult> Delete([FromBody] AirLinesDto data)
        {
            try
            {
                int id = data.ID_AIR_LINE;
                var Event = await _context.CAT_AIR_LINES.FirstOrDefaultAsync(g => g.ID_AIR_LINE.Equals(id));

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



      
        internal async Task<ResponseDto> SaveIfNotExist(AirLineRegisterDto data)
        {
            try
            {
                ResponseDto res = new ResponseDto(ResponseDtoEnum.Success);
                var exist = await _context.CAT_AIR_LINES.AnyAsync(g => g.DESC_AIR_LINE_SP == data.DESC_AIR_LINE_SP
                || g.DESC_AIR_LINE_EN == data.DESC_AIR_LINE_EN);
                if (exist)
                {
                    var item = await _context.CAT_AIR_LINES
                        .Where((even) => even.DESC_AIR_LINE_SP == data.DESC_AIR_LINE_SP).FirstOrDefaultAsync();

                    res.response = item;
                    return res;
                }
                else
                {

                    var model = _mapper.Map<AirLines>(data);
                    await _context.CAT_AIR_LINES.AddAsync(model);
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
    }
}
