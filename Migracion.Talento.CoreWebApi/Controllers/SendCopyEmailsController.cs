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
    public class SendCopyEmailsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public SendCopyEmailsController(AppDbContext context,IMapper mapper)
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
                if (!await _context.CAT_SEND_COPY_EMAILS.AnyAsync())
                    return Ok(new ResponseDto(ResponseDtoEnum.NoData));

                ResponseDto response = new ResponseDto( ResponseDtoEnum.Success)
                {
                    response = await _context.CAT_SEND_COPY_EMAILS.OrderBy(p => p.ID).ToListAsync(),
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
            if (!await _context.CAT_SEND_COPY_EMAILS.AnyAsync(even => even.ID == id))
                return Ok(new ResponseDto(ResponseDtoEnum.NoData));

            var item = await _context.CAT_SEND_COPY_EMAILS
                .Where((even) => even.ID == id).FirstOrDefaultAsync();

            ResponseDto result = new ResponseDto(ResponseDtoEnum.Success);
            result.response = item;

            return Ok(result);
        }


        // POST api/<ValuesController>
        [HttpPost("Save")]
        public async Task<ActionResult> Save([FromBody] SendCopyEmailsRegisterDto value)
        {
            try
            {
                var exist = await _context.CAT_SEND_COPY_EMAILS.AnyAsync(g => g.EMAIL == value.EMAIL);
                if (exist)
                    return Ok(new ResponseDto(ResponseDtoEnum.Duplicated));
                var Item = _mapper.Map<SendCopyEmails>(value);
                Item.CREATED_DATE = DateTime.Now;
                await _context.CAT_SEND_COPY_EMAILS.AddAsync(Item);
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
        public async Task<ActionResult> Update(int id, [FromBody] SendCopyEmailsDto data)
        {
            try
            {
                var genderDb = await _context.CAT_SEND_COPY_EMAILS.AnyAsync(g => g.ID == id);
                if (!genderDb)
                    return Ok(new ResponseDto(ResponseDtoEnum.NoData));

                var item = await _context.CAT_SEND_COPY_EMAILS
                     .Where((even) => even.ID == id).FirstOrDefaultAsync();


                item.ACTIVE = data.ACTIVE;
                item.MODIFIED_DATE = DateTime.Now;
                item.MODIFIED_BY = data.MODIFY_BY;
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
