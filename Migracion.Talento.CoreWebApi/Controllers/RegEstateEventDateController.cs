using AutoMapper;
using CommonTools.DTOs.Query;
using CommonTools.DTOs.Register;
using CommonTools.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Migracion.Talento.WebAPI.DataConnection;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Migracion.Talento.CoreWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegEstateEventDateController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public RegEstateEventDateController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        // GET: api/<REGEVENTESTATESDATE>
        [HttpGet]
        public async Task<ActionResult<ResponseDto>> Get()
        {
            try
            {
                var reg = await _context.REG_EVENT_ESTATES_DATE.AnyAsync(e => e.ACTIVE);

                if (!reg)
                    return Ok(new ResponseDto(ResponseDtoEnum.NoData));

                var regmodel= await _context.REG_EVENT_ESTATES_DATE
                    .Include("CAT_EVENTS")
                    .Include("CAT_ESTATES")
                    .ToListAsync();
                 
                ResponseDto response = new ResponseDto(ResponseDtoEnum.Success);
                response.response = regmodel;
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseDto(ResponseDtoEnum.Error).message + ex.Message);
            }

        }



        // POST api/<REGEVENTESTATESDATE>
        [HttpPost]
        public async Task<ActionResult> Save([FromBody] RegEvenStateDateDto value)
        {
            try
            {
               // var exist = await _context.REG_EVENT_ESTATES_DATE.AnyAsync(g => g.des == value.DESC_PROCESS_SP
               //|| g.DESC_PROCESS_SP == value.DESC_PROCESS_SP);
               // if (exist)
               //     return Ok(new ResponseDto(ResponseDtoEnum.Duplicated));


               // var Process = _mapper.Map<Process>(value);

               // await _context.REG_EVENT_ESTATES_DATE.AddAsync(Process);
               // await _context.SaveChangesAsync();
                return Ok(new ResponseDto(ResponseDtoEnum.Success));
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseDto(ResponseDtoEnum.Error).message + ex.Message);
            }
        }



        // PUT api/<REGEVENTESTATESDATE>/5
        [HttpPost("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] RegEvenStateDateDto data)
        {
            try
            {
                //var ProcessDb = await _context.REG_EVENT_ESTATES_DATE.AnyAsync(g => g.ID_EVENT == id);
                //if (!ProcessDb)
                //    return Ok(new ResponseDto(ResponseDtoEnum.NoData));

                //var model = _mapper.Map<Process>(data);
                //model.ID_PROCESS = id;

                //_context.Update(model);
                //await _context.SaveChangesAsync();

                return Ok(new ResponseDto(ResponseDtoEnum.Success));
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseDto(ResponseDtoEnum.Error).message + ex.Message);
            }
        }

        // DELETE api/<REGEVENTESTATESDATE>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                //var Process = await _context.REG_EVENT_ESTATES_DATE.FirstOrDefaultAsync(g => g.ID_PROCESS.Equals(id));

                //if (Process == null)
                //    return Ok(new ResponseDto(ResponseDtoEnum.NoData));

                //_context.Remove(Process);
                //await _context.SaveChangesAsync();
                return Ok(new ResponseDto(ResponseDtoEnum.Success));
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseDto(ResponseDtoEnum.Error).message + ex.Message);
            }
        }
    }
}
