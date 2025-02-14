using AutoMapper;
using CommonTools.DTOs;
using CommonTools.DTOs.Query;
using CommonTools.DTOs.Register;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Migracion.Talento.Models;
using Migracion.Talento.WebAPI.DataConnection;
using System.ComponentModel.Design.Serialization;
using CommonTools.Enums;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Migracion.Talento.CoreWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GendersController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public GendersController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/<GendersController>
        [HttpGet]
        public async Task<ActionResult<ResponseDto>> Get()
        {
            try
            {
                if (!await _context.CAT_GENDERS.AnyAsync())
                    return Ok(new ResponseDto(ResponseDtoEnum.NoData));
                        

                ResponseDto response = new ResponseDto(ResponseDtoEnum.Success);
                response.response = await _context.CAT_GENDERS.OrderBy(p => p.DESC_GENDER_SP).ToListAsync();
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
            if (!await _context.CAT_GENDERS.AnyAsync(even => even.ID_GENDER == (id)))
                return Ok(new ResponseDto(ResponseDtoEnum.NoData));

            var item = await _context.CAT_GENDERS
                .Where((even) => even.ID_GENDER == id).FirstOrDefaultAsync();

            ResponseDto result = new ResponseDto(ResponseDtoEnum.Success);
            result.response = item;

            return Ok(result);
        }


        // POST api/<ValuesController>
        [HttpPost("Save")]
        public async Task<ActionResult> Save([FromBody] GenderRegisterDto value)
        {
            try
            {
                var exist = await _context.CAT_GENDERS.AnyAsync(g => g.DESC_GENDER_SP == value.DESC_GENDER_SP
                || g.DESC_GENDER_EN == value.DESC_GENDER_EN);
                if (exist)
                    return Ok(new ResponseDto(ResponseDtoEnum.Duplicated));
                var Event = _mapper.Map<Genders>(value);

                await _context.CAT_GENDERS.AddAsync(Event);
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
        public async Task<ActionResult> Update(int id, [FromBody] GenderRegisterDto data)
        {
            try
            {
                var genderDb = await _context.CAT_GENDERS.AnyAsync(g => g.ID_GENDER == id);
                if (!genderDb)
                    return Ok(new ResponseDto(ResponseDtoEnum.NoData));

                var item = await _context.CAT_GENDERS
                     .Where((even) => even.ID_GENDER == id).FirstOrDefaultAsync();

                item.DESC_GENDER_SP= data.DESC_GENDER_SP;
                item.DESC_GENDER_EN = data.DESC_GENDER_EN;
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
        public async Task<ActionResult> Delete([FromBody] GendersDto data)
        {
            try
            {
                int id = data.ID_GENDER;
                var Event = await _context.CAT_GENDERS.FirstOrDefaultAsync(g => g.ID_GENDER.Equals(id));

                if (Event == null)
                    return Ok(new ResponseDto(ResponseDtoEnum.NoData));


                var genderDb = await _context.REG_EVENTS.AnyAsync(g => g.ID_GENDER == id);
                if (genderDb)
                    return Ok(new ResponseDto(ResponseDtoEnum.RegisterWithDependency));


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
