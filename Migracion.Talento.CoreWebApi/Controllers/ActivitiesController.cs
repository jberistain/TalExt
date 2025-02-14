using AutoMapper;
using CommonTools.DTOs.Query;
using CommonTools.DTOs.Register;
using CommonTools.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Migracion.Talento.Models;
using Migracion.Talento.WebAPI.DataConnection;

namespace Migracion.Talento.CoreWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActivitiesController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public ActivitiesController(AppDbContext context, IMapper mapper)
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
                if (!await _context.CAT_ACTIVITIES.AnyAsync())
                    return Ok(new ResponseDto(ResponseDtoEnum.NoData));

                ResponseDto response = new ResponseDto(ResponseDtoEnum.Success)
                {
                    response = await _context.CAT_ACTIVITIES.ToListAsync(),
                };
                return Ok(response);

            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseDto(ResponseDtoEnum.Error).message + ex.Message);
            }

            //return new List<Activities>();
        }

        [HttpPost]
        public async Task<ActionResult<ResponseDto>> Saveactivity([FromBody] ActivityRegisterDto activity)
        {
            try
            {
                var exist = await _context.CAT_ACTIVITIES.AnyAsync(
                a => a.DESC_ACTIVITY_EN.Equals(activity.DESC_ACTIVITY_EN)
                || a.DESC_ACTIVITY_SP.Equals(activity.DESC_ACTIVITY_SP));
                if (exist)
                    return Ok(new ResponseDto(ResponseDtoEnum.Duplicated));

                var _activity = _mapper.Map<Activities>(activity);
                await _context.AddAsync(_activity);
                await _context.SaveChangesAsync();

                return Ok(new ResponseDto(ResponseDtoEnum.Success));
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseDto(ResponseDtoEnum.Error).message + ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name=""></param>
        /// <returns></returns>
        [HttpPost("{id}")]
        public async Task<ActionResult> Updateactivity(int id, [FromBody] ActivityRegisterDto activity)
        {
            try
            {
                var airDb = await _context.CAT_ACTIVITIES.AnyAsync(g => g.ID_ACTIVITY == id);
                if (!airDb)
                    return Ok(new ResponseDto(ResponseDtoEnum.NoData));
                var model = _mapper.Map<Activities>(activity);
                model.ID_ACTIVITY = id;
                _context.Update(model);
                await _context.SaveChangesAsync();
                return Ok(new ResponseDto(ResponseDtoEnum.Success));
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseDto(ResponseDtoEnum.Error).message + ex.Message);
            }

        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Deleteactivity(int id)
        {
            try
            {
                var activity = await _context.CAT_ACTIVITIES.AnyAsync(e => e.ID_ACTIVITY.Equals(id));
                if (!activity)
                    return Ok(new ResponseDto(ResponseDtoEnum.NoData));

                _context.Remove(activity);
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
