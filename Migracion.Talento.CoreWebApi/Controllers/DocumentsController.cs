using AutoMapper;
using CommonTools.DTOs.Query;
using CommonTools.DTOs.Register;
using CommonTools.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Migracion.Talento.Entities.Models;
using Migracion.Talento.Models;
using Migracion.Talento.WebAPI.DataConnection;
using System.Reflection.Metadata;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Migracion.Talento.CoreWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public DocumentsController(AppDbContext context, IMapper mapper) {
            _context = context;
            _mapper = mapper;
        }
        // GET: api/<DocumentsController>
        [HttpGet]
        public async Task<ActionResult<ResponseDto>> Get()
        {
            try
            {
                var documents = await _context.REG_DOCUMENTS.AnyAsync();

                if (!documents)
                    return Ok(new ResponseDto(ResponseDtoEnum.NoData));

                ResponseDto response = new ResponseDto(ResponseDtoEnum.Success);

                var listDocumen=await _context.REG_DOCUMENTS.OrderBy(p => p.DESC_SPANISH).ToListAsync();
                var map = _mapper.Map<List<DocumentDto>>(listDocumen);
                response.response = map;
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseDto(ResponseDtoEnum.Error).message + ex.Message);
            }

        }

        // GET: api/<DocumentsController>
        [HttpGet("GetById")]
        public async Task<ActionResult<ResponseDto>> Get(int id)
        {
            try
            {
                var documents = await _context.REG_DOCUMENTS.AnyAsync(i => i.ID_INVITE == id);

                if (!documents)
                    return Ok(new ResponseDto(ResponseDtoEnum.NoData));

                ResponseDto response = new ResponseDto(ResponseDtoEnum.Success);


                var Documen = await _context.REG_DOCUMENTS.FirstOrDefaultAsync(i => i.ID_INVITE == id);
                var map = _mapper.Map<DocumentDto>(Documen);
                response.response= map;
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseDto(ResponseDtoEnum.Error).message + ex.Message);
            }

        }
        // POST api/<DocumentsController>
        [HttpPost("Save")]
        public async Task<ActionResult<ResponseDto>> Post([FromBody] DocumentRegisterDto value)
        {
            try
            {
                var exist = await _context.REG_DOCUMENTS.AnyAsync(g => g.DESC_SPANISH == value.DESC_SPANISH 
               && g.ID_EVENT_TYPE == value.ID_EVENT_TYPE);
                if (exist)
                    return Ok(new ResponseDto(ResponseDtoEnum.Duplicated));


                var document = _mapper.Map<Documents>(value);

                await _context.REG_DOCUMENTS.AddAsync(document);
                await _context.SaveChangesAsync();
                return Ok(new ResponseDto(ResponseDtoEnum.Success));
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseDto(ResponseDtoEnum.Error).message + ex.Message);
            }
        }

        // PUT api/<DocumentsController>/5
        [HttpPost("Update")]
        public async Task<ActionResult<ResponseDto>> update(int id, [FromBody] DocumentRegisterDto value)
        {
            try
            {
                var documentDb = await _context.REG_DOCUMENTS.AnyAsync(g => g.ID_INVITE== id);
                if (!documentDb)
                    return Ok(new ResponseDto(ResponseDtoEnum.NoData));

                var item = await _context.REG_DOCUMENTS
                     .Where((even) => even.ID_INVITE == id).FirstOrDefaultAsync();


                item.DESC_SPANISH = value.DESC_SPANISH;
                item.DESC_ENGLISH= value.DESC_ENGLISH;
                item.ID_EVENT_TYPE = value.ID_EVENT_TYPE;
                item.ID_REG = value.ID_REG;
                item.FILE_BLOB = value.FILE_BLOB;
                item.MODIFY_BY = value.MODIFY_BY;
                item.MODIFY_DATE = DateTime.Now;

                await _context.SaveChangesAsync();

                return Ok(new ResponseDto(ResponseDtoEnum.Success));
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseDto(ResponseDtoEnum.Error).message + ex.Message);
            }
        }

        // DELETE api/<DocumentsController>/5
        [HttpPost("delete")]
        public async Task<ActionResult<ResponseDto>> Delete([FromBody] DocumentDto data)
        {

            try
            {
                int id = data.ID_INVITE;
                var document = await _context.REG_DOCUMENTS.FirstOrDefaultAsync(g => g.ID_INVITE.Equals(id));

                if (document == null)
                    return Ok(new ResponseDto(ResponseDtoEnum.NoData));

                _context.Remove(document);
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
