using AutoMapper;
using CommonTools.DTOs.Query;
using CommonTools.DTOs.Register;
using CommonTools.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Migracion.Talento.Entities.Models;
using Migracion.Talento.Models;
using Migracion.Talento.WebAPI.DataConnection;
using SkiaSharp;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Migracion.Talento.CoreWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstatesController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public EstatesController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/<ValuesController>
        [HttpGet]
        public async Task<ActionResult<ResponseDto>> Get()
        {
            try
            {
                if (!await _context.CAT_ESTATES.AnyAsync())
                    return Ok(new ResponseDto(ResponseDtoEnum.NoData));


                ResponseDto response = new ResponseDto(ResponseDtoEnum.Success);
                response.response = await _context.CAT_ESTATES.OrderBy(p => p.DESC_ESTATE_SP).ToListAsync();
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
            if (!await _context.CAT_ESTATES.AnyAsync(even => even.ID_ESTATE == (id)))
                return Ok(new ResponseDto(ResponseDtoEnum.NoData));

            var item = await _context.CAT_ESTATES
                .Where((even) => even.ID_ESTATE == id).FirstOrDefaultAsync();

            ResponseDto result = new ResponseDto(ResponseDtoEnum.Success);
            result.response = item;

            return Ok(result);
        }


        // POST api/<ValuesController>
        [HttpPost("Save")]
        public async Task<ActionResult> Save([FromBody] EstateRegisterDto value)
        {
            try
            {
                var exist = await _context.CAT_ESTATES.AnyAsync(g => g.DESC_ESTATE_SP == value.DESC_ESTATE_SP
                || g.DESC_ESTATE_EN == value.DESC_ESTATE_EN);
                if (exist)
                    return Ok(new ResponseDto(ResponseDtoEnum.Duplicated));

                if (string.IsNullOrEmpty(value.ARB_ESTATE))
                    value.ARB_ESTATE = "IAT";

                if (string.IsNullOrEmpty(value.IM_ESTATE))
                    value.IM_ESTATE = "IAT";

                if (string.IsNullOrEmpty(value.TYPE_ESTATE))
                    value.TYPE_ESTATE = "IAT";

                var Event = _mapper.Map<Estates>(value);

                await _context.CAT_ESTATES.AddAsync(Event);
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
        public async Task<ActionResult> Update(int id, [FromBody] EstateRegisterDto data)
        {
            try
            {
                var estateDb = await _context.CAT_ESTATES.AnyAsync(g => g.ID_ESTATE == id);
                if (!estateDb)
                    return Ok(new ResponseDto(ResponseDtoEnum.NoData));

                var item = await _context.CAT_ESTATES
                     .Where((even) => even.ID_ESTATE == id).FirstOrDefaultAsync();


                item.DESC_ESTATE_SP = data.DESC_ESTATE_SP;
                item.DESC_ESTATE_EN = data.DESC_ESTATE_EN;
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
        public async Task<ActionResult> Delete([FromBody] EstatesDto data)
        {
            try
            {
                int id = data.ID_ESTATE;
                var Event = await _context.CAT_ESTATES.FirstOrDefaultAsync(g => g.ID_ESTATE.Equals(id));

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

        internal async Task<ResponseDto> SaveIfNotExist(EstateRegisterDto data)
        {
            try
            {
                ResponseDto res = new ResponseDto(ResponseDtoEnum.Success);
                var exist = await _context.CAT_ESTATES.AnyAsync(g => g.DESC_ESTATE_EN == data.DESC_ESTATE_EN
                || g.DESC_ESTATE_SP == data.DESC_ESTATE_SP);
                if (exist)
                {
                    var item = await _context.CAT_ESTATES
                        .Where((even) => even.DESC_ESTATE_SP == data.DESC_ESTATE_SP).FirstOrDefaultAsync();

                    res.response = item;
                    return res;
                }
                else
                {

                    var model = _mapper.Map<Estates>(data);
                    await _context.CAT_ESTATES.AddAsync(model);
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
