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
    public class CountriesController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public CountriesController(AppDbContext context,IMapper mapper)
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
                var countries=await _context.CAT_COUNTRIES.AnyAsync();

                if (!countries)
                    return Ok(new ResponseDto(ResponseDtoEnum.NoData));

                ResponseDto response = new ResponseDto(ResponseDtoEnum.Success);
                response.response = await _context.CAT_COUNTRIES.OrderBy(p=> p.DESC_COUNTRY_SP).ToListAsync();
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
            if (!await _context.CAT_COUNTRIES.AnyAsync(even => even.ID_COUNTRY == (id)))
                return Ok(new ResponseDto(ResponseDtoEnum.NoData));

            var item = await _context.CAT_COUNTRIES
                .Where((even) => even.ID_COUNTRY == id).FirstOrDefaultAsync();

            ResponseDto result = new ResponseDto(ResponseDtoEnum.Success);
            result.response = item;

            return Ok(result);
        }

        // POST api/<ValuesController>
        [HttpPost("Save")]
        public async Task<ActionResult> Save([FromBody] CountryRegisterDto value)
        {
            try
            {
                var exist = await _context.CAT_COUNTRIES.AnyAsync(g => g.DESC_COUNTRY_SP == value.DESC_COUNTRY_SP
               || g.DESC_COUNTRY_EN == value.DESC_COUNTRY_EN);
                if (exist)
                    return Ok(new ResponseDto(ResponseDtoEnum.Duplicated));


                var country = _mapper.Map<Countries>(value);

                await _context.CAT_COUNTRIES.AddAsync(country);
                await _context.SaveChangesAsync();
                return Ok(new ResponseDto(ResponseDtoEnum.Success));
            }
            catch (Exception ex) 
            {
                return BadRequest(new ResponseDto(ResponseDtoEnum.Error).message + ex.Message);
            }
        }



        // PUT api/<CountriesController>/5
        [HttpPost("Update")]
        public async Task<ActionResult> Update(int id, [FromBody] CountryRegisterDto data)
        {
            try
            {
                var genderDb = await _context.CAT_COUNTRIES.AnyAsync(g => g.ID_COUNTRY == id);
                if (!genderDb)
                    return Ok(new ResponseDto(ResponseDtoEnum.NoData));

                var item = await _context.CAT_COUNTRIES
                     .Where((even) => even.ID_COUNTRY == id).FirstOrDefaultAsync();


                item.DESC_COUNTRY_SP = data.DESC_COUNTRY_SP;
                item.DESC_COUNTRY_EN = data.DESC_COUNTRY_EN;
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

        // DELETE api/<CountriesController>/5
        [HttpPost("Delete")]
        public async Task<ActionResult> Delete([FromBody] CountriesDto data)
        {
            try
            {
                int id = data.ID_COUNTRY;
                var Country = await _context.CAT_COUNTRIES.FirstOrDefaultAsync(g => g.ID_COUNTRY.Equals(id));

                if (Country == null)
                    return Ok(new ResponseDto(ResponseDtoEnum.NoData));

                _context.Remove(Country);
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
