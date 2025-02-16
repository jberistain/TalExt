using AutoMapper;
using CommonTools.DTOs.Query;
using CommonTools.DTOs.Register;
using CommonTools.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Migracion.Talento.Models;
using Migracion.Talento.WebAPI.DataConnection;
using static iTextSharp.text.pdf.AcroFields;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Migracion.Talento.CoreWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompaniesController : ControllerBase
    {
        private AppDbContext _context;
        private readonly IMapper _mapper;

        public CompaniesController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/<CompaniesController>
        [HttpGet]
        public async Task<ActionResult<ResponseDto>> Get()
        {
            if (!await _context.CAT_COMPANIES.AnyAsync())
                return Ok(new ResponseDto(ResponseDtoEnum.NoData));

            ResponseDto response = new ResponseDto(ResponseDtoEnum.Success);
            response.response = await _context.CAT_COMPANIES.OrderBy(p => p.DESC_COMPANY_SP).ToListAsync();
            return Ok(response);

        }
        [HttpGet("GetById")]
        public async Task<ActionResult<ResponseDto>> GetById(int id)
        {
            if (!await _context.CAT_COMPANIES.AnyAsync(even => even.ID_COMPANY == (id)))
                return Ok(new ResponseDto(ResponseDtoEnum.NoData));

            var item = await _context.CAT_COMPANIES
                .Where((even) => even.ID_COMPANY == id).FirstOrDefaultAsync();

            ResponseDto result = new ResponseDto(ResponseDtoEnum.Success);
            result.response = item;

            return Ok(result);
        }
        // POST api/<CompaniesController>
        [HttpPost("Save")]
        public async Task<ActionResult<ResponseDto>> Save([FromBody] CompanyRegisterDto data)
        {
            try
            {
                var exist = await _context.CAT_COMPANIES.AnyAsync(g => g.DESC_COMPANY_SP == data.DESC_COMPANY_SP
                || g.DESC_COMPANY_SP == data.DESC_COMPANY_SP);
                if (exist)
                    return Ok(new ResponseDto(ResponseDtoEnum.Duplicated));

                var model = _mapper.Map<Companies>(data);
                await _context.CAT_COMPANIES.AddAsync(model);
                await _context.SaveChangesAsync();
                return Ok(new ResponseDto(ResponseDtoEnum.Success));
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseDto(ResponseDtoEnum.Error).message + ex.Message);
            }
        }


        internal async Task<ResponseDto> SaveIfNotExist([FromBody] CompanyRegisterDto data)
        {
            try
            {
                ResponseDto res = new ResponseDto(ResponseDtoEnum.Success);
                var exist = await _context.CAT_COMPANIES.AnyAsync(g => g.DESC_COMPANY_SP == data.DESC_COMPANY_SP
                || g.DESC_COMPANY_SP == data.DESC_COMPANY_SP);
                if (exist)
                {

                    var item = await _context.CAT_COMPANIES
                        .Where((even) => even.DESC_COMPANY_SP == data.DESC_COMPANY_SP).FirstOrDefaultAsync();
                   
                    res.response = item;
                    return res;
                }
                else
                {
                    var model = _mapper.Map<Companies>(data);
                    await _context.CAT_COMPANIES.AddAsync(model);
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


        // PUT api/<CompaniesController>/5
        [HttpPost("Update")]
        public async Task<ActionResult> Update(int id, [FromBody] CompanyRegisterDto data)
        {
            try
            {
                var genderDb = await _context.CAT_COMPANIES.AnyAsync(g => g.ID_COMPANY == id);
                if (!genderDb)
                    return Ok(new ResponseDto(ResponseDtoEnum.NoData));

                var item = await _context.CAT_COMPANIES
                     .Where((even) => even.ID_COMPANY == id).FirstOrDefaultAsync();


                item.DESC_COMPANY_SP = data.DESC_COMPANY_SP;
                item.DESC_COMPANY_EN = data.DESC_COMPANY_EN;
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
        public async Task<ActionResult> Delete([FromBody] CompaniesDto data)
        {
            try
            {
                int id = data.ID_COMPANY;
                var company = await _context.CAT_COMPANIES.FirstOrDefaultAsync(g => g.ID_COMPANY.Equals(id));

                if (company == null)
                    return Ok(new ResponseDto(ResponseDtoEnum.NoData));

                var genderDb = await _context.CAT_EVENTS.AnyAsync(g => g.ID_COMPANY == id);
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
