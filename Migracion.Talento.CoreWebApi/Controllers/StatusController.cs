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
    public class StatusController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public StatusController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/<StatusController>
        [HttpGet]
        public async Task<ActionResult<ResponseDto>> Get()
        {
            try
            {
                if (!await _context.CAT_STATUS.AnyAsync())
                    return Ok(new ResponseDto(ResponseDtoEnum.NoData));


                ResponseDto response = new ResponseDto(ResponseDtoEnum.Success);
                response.response = await _context.CAT_STATUS.ToListAsync();
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseDto(ResponseDtoEnum.Error).message + ex.Message);
            }

        }

        // POST api/<StatusController>
        [HttpPost]
        public async Task<ActionResult<ResponseDto>> Save([FromBody] StatusRegisterDto data)
        {
            try
            {
                var exist = await _context.CAT_STATUS.AnyAsync(g => g.DESC_STATUS_SP == data.DESC_STATUS_SP
                || g.DESC_STATUS_EN == data.DESC_STATUS_EN);
                if (exist)
                    return Ok(new ResponseDto(ResponseDtoEnum.Duplicated));

                var model = _mapper.Map<Status>(data);
                await _context.CAT_STATUS.AddAsync(model);
                await _context.SaveChangesAsync();
                return Ok(new ResponseDto(ResponseDtoEnum.Success));
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseDto(ResponseDtoEnum.Error).message + ex.Message);
            }
        }

        // PUT api/<StatusController>/5
        [HttpPost("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] StatusRegisterDto data)
        {
            try
            {
                var StatusDb = await _context.CAT_STATUS.AnyAsync(g => g.ID_STATUS == id);
                if (!StatusDb)
                    return Ok(new ResponseDto(ResponseDtoEnum.NoData));

                var model = _mapper.Map<Status>(data);
                model.ID_STATUS = id;

                _context.Update(model);
                await _context.SaveChangesAsync();

                return Ok(new ResponseDto(ResponseDtoEnum.Success));
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseDto(ResponseDtoEnum.Error).message + ex.Message);
            }
        }

        // DELETE api/<StatusController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var Status = await _context.CAT_STATUS.FirstOrDefaultAsync(g => g.ID_STATUS.Equals(id));

                if (Status == null)
                    return Ok(new ResponseDto(ResponseDtoEnum.NoData));

                _context.Remove(Status);
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
