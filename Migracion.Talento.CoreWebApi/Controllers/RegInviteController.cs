using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Migracion.Talento.WebAPI.DataConnection;
using Migracion.Talento.WebAPI;
using Migracion.Talento.Models;
using AutoMapper;
using CommonTools.DTOs.Query;
using CommonTools.Enums;
using CommonTools.DTOs.Register;
using Migracion.Talento.Entities.Models;
using System.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Migracion.Talento.CoreWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegInviteController : ControllerBase
    {
        private AppDbContext _context;
        private readonly IMapper _mapper;

        public RegInviteController(AppDbContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        // GET: api/<AirPortsController>
        [HttpGet]
        public async Task<ActionResult<ResponseDto>> Get()
        {
            try
            {
                if (!await _context.REG_INVITE.AnyAsync())
                    return Ok(new ResponseDto(ResponseDtoEnum.NoData));

                ResponseDto response = new ResponseDto(ResponseDtoEnum.Success);
                response.response = await _context.REG_INVITE.ToListAsync();
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
            if (!await _context.REG_INVITE.AnyAsync(even => even.ID_INVITE == (id)))
                return Ok(new ResponseDto(ResponseDtoEnum.NoData));

            var item = await _context.REG_INVITE
                .Where((even) => even.ID_INVITE == id).FirstOrDefaultAsync();

            ResponseDto result = new ResponseDto(ResponseDtoEnum.Success);
            result.response = item;

            return Ok(result);
        }



        [HttpGet("GetInfoSignsBlob")]
        public async Task<ActionResult<ResponseDto>> GetInfoSignsBlob()
        {
            try
            {
                if (!await _context.REG_INVITE.AnyAsync())
                    return Ok(new ResponseDto(ResponseDtoEnum.NoData));

                ResponseDto response = new ResponseDto(ResponseDtoEnum.Success);
                response.response = await _context.REG_INVITE.Where(m=> !string.IsNullOrEmpty(m.DES_TITLE) && m.DES_TITLE.Contains("INVITACION LETTER")).ToListAsync();
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseDto(ResponseDtoEnum.Error).message + ex.Message);
            }


        }



        // POST api/<ValuesController>
        [HttpPost("Save")]
        public async Task<ActionResult> Save([FromBody] RegInviteDto value)
        {
            try
            {
                var exist = await _context.REG_INVITE.AnyAsync(g => g.DES_TITLE == value.DES_TITLE
                || g.DESC_SPANISH == value.DESC_SPANISH);
                if (exist)
                    return Ok(new ResponseDto(ResponseDtoEnum.Duplicated));
                var Event = _mapper.Map<RegInvite>(value);

                await _context.REG_INVITE.AddAsync(Event);
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
        public async Task<ActionResult> Update(int id, [FromBody] RegInviteDto data)
        {
            try
            {
                var genderDb = await _context.REG_INVITE.AnyAsync(g => g.ID_INVITE == id);
                if (!genderDb)
                    return Ok(new ResponseDto(ResponseDtoEnum.NoData));

                var item = await _context.REG_INVITE
                     .Where((even) => even.ID_INVITE == id).FirstOrDefaultAsync();


                item.DES_TITLE = data.DES_TITLE;
                item.DESC_SPANISH = data.DESC_SPANISH;
                item.DESC_ENGLISH = data.DESC_ENGLISH;
                item.SIGN_1 = data.SIGN_1;
                item.FOOT_PAGE = data.FOOT_PAGE;
                item.ID_EVENT_TYPE = data.ID_EVENT_TYPE;
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

        [HttpPost("UpdateSignImage")]
        public async Task<ActionResult<ResponseDto>> UpdateSignImage(int id, [FromBody] RegInviteDto value)
        {
            try
            {
                var documentDb = await _context.REG_INVITE.AnyAsync(g => g.ID_INVITE == id);
                if (!documentDb)
                    return Ok(new ResponseDto(ResponseDtoEnum.NoData));

                var item = await _context.REG_INVITE
                     .Where((even) => even.ID_INVITE == id).FirstOrDefaultAsync();


                item.SIGN_BLOB = value.SIGN_BLOB;

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
        public async Task<ActionResult> Delete([FromBody] InviteDto data)
        {
            try
            {
                int id = data.ID_INVITE;
                var Event = await _context.REG_INVITE.FirstOrDefaultAsync(g => g.ID_INVITE.Equals(id));

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


        [HttpGet("GetAllDocsBySecretCode")]
        public async Task<ActionResult<ResponseDto>> GetAllDocsBySecretCode(string secretCode)
        {
            try
            {
                //BUSCAR EL REGISTRO POR CODIGO SECRETO
                if (!await _context.REG_EVENTS.AnyAsync(even => even.SECRET_CODE.Equals(secretCode)))
                    return new ResponseDto(ResponseDtoEnum.NoData);

                var item = await _context.REG_EVENTS
                    .Where((even) => even.SECRET_CODE.Equals(secretCode)).FirstOrDefaultAsync();


                //BUSCAR LOS REGISTROS DE EVENTOS QUE PERTENECEN A ESE REGISTRO (ID_REG)
                int ID_REG = item.ID_REG;
                if (!await _context.REG_EVENT_ESTATES_DATE.AnyAsync(even => even.ID_REG == ID_REG))
                    return new ResponseDto(ResponseDtoEnum.NoData);

                var registrosEventos = await _context.REG_EVENT_ESTATES_DATE
                    .Where((even) => even.ID_REG == ID_REG).ToListAsync();

                // BUSCAR LOS EVENTOS A LOS QUE PERTENECEN LOS REGISTROS_EVENTOS
                List<int> idsEventosEncontrados = new List<int>();
                foreach (var registro in registrosEventos)
                {
                    if (!idsEventosEncontrados.Contains(registro.ID_EVENT))
                        idsEventosEncontrados.Add(registro.ID_EVENT);
                }

                //4 BUSCAR LOS TIPOS DE EVENTOS A LOS QUE PERTENECEN LOS EVENTOS ENCONTRADOS
                List<int> idsTiposEventosEncontrados = new List<int>();
                foreach (var idEvento in idsEventosEncontrados)
                {
                    if (!await _context.CAT_EVENTS.AnyAsync(even => even.ID_EVENT == idEvento))
                        return new ResponseDto(ResponseDtoEnum.NoData);

                    var currentEvent = await _context.CAT_EVENTS
                        .Where((even) => even.ID_EVENT == idEvento).FirstOrDefaultAsync();

                    if (currentEvent != null && currentEvent.ID_EVENT_TYPE != null
                        && !idsTiposEventosEncontrados.Contains((int)currentEvent.ID_EVENT_TYPE))
                    {
                        idsTiposEventosEncontrados.Add((int)currentEvent.ID_EVENT_TYPE);
                    }
                }

                //BUSCAR LOS DOCUMENTOS QUE APUNTAN A LOS TIPOS DE EVENTOS ENCONTRADOS
                List<RegInvite> Documentos = new List<RegInvite>();
                List<int> idsDocumentosEncontrados = new List<int>();

                ///integra lista de documentos pdf encontrados en la nueva tabla
                List<Documents> documentosPdf = new List<Documents>();
                foreach (var idTipoEvento in idsTiposEventosEncontrados)
                {
                    if (await _context.REG_INVITE.AnyAsync(even => even.ID_EVENT_TYPE == idTipoEvento))
                    {

                        var listDocs = await _context.REG_INVITE
                            .Where((even) => even.ID_EVENT_TYPE == idTipoEvento).ToListAsync();

                        foreach (var currentDoc in listDocs)
                        {
                            if (currentDoc != null && !idsDocumentosEncontrados.Contains(currentDoc.ID_INVITE))
                            {
                                idsDocumentosEncontrados.Add(currentDoc.ID_INVITE);
                                Documentos.Add(currentDoc);
                            }

                        }

                        //new Query to pdf documents

                        var documetsPdf = await _context.REG_DOCUMENTS.Where(docs => docs.ID_EVENT_TYPE == idTipoEvento).ToListAsync();
                        if (documetsPdf != null)
                            documetsPdf.ForEach(documet => {
                                documentosPdf.Add(documet);
                            });

                    }
                }

                //REGRESAR LA INFORMACION DE LOS DOCUMENTOS QUE CORRESPONDEN A ESOS TIPOS DE EVENTOS
                var documentsDto = _mapper.Map<List<InviteDto>>(Documentos);
                var documentsPdf = _mapper.Map<List<DocumentDto>>(documentosPdf);

                ResponseDto result = new ResponseDto(ResponseDtoEnum.Success);
                result.response = documentsDto;
                result.secondResponse = documentsPdf;
                return result;
            }
            catch (Exception e)
            {
                var r = new ResponseDto(ResponseDtoEnum.Error);
                r.message += e.Message;
                return r;
            }
        }


        [HttpGet("GetAllDocsByRestrictedFlag")]
        public async Task<ResponseDto> GetAllDocsByRestrictedFlag(bool isRestricted)
        {
            try
            {
               
                //BUSCAR LOS DOCUMENTOS QUE APUNTAN A LOS TIPOS DE EVENTOS ENCONTRADOS
                List<RegInvite> Documentos = new List<RegInvite>();
                List<int> idsDocumentosEncontrados = new List<int>();
                string busquedaDescCatalogo = "NO RESTRINGIDO";
                if (isRestricted) busquedaDescCatalogo = "RESTRINGIDO";

                if (await _context.CAT_EVENT_TYPES.AnyAsync(even => even.DESC_ACTIVITY_SP == busquedaDescCatalogo))
                {
                    var infoCat = await _context.CAT_EVENT_TYPES
                        .Where(even => even.DESC_ACTIVITY_SP == busquedaDescCatalogo).SingleOrDefaultAsync();

                    if (await _context.REG_INVITE.AnyAsync(even => even.ID_EVENT_TYPE == infoCat.ID_EVENT_TYPE))
                    {
                        var listDocs = await _context.REG_INVITE
                        .Where(even => even.ID_EVENT_TYPE == infoCat.ID_EVENT_TYPE).ToListAsync();

                        foreach (var currentDoc in listDocs)
                        {
                            if (currentDoc != null && !idsDocumentosEncontrados.Contains(currentDoc.ID_INVITE))
                            {
                                idsDocumentosEncontrados.Add(currentDoc.ID_INVITE);
                                Documentos.Add(currentDoc);
                            }
                        }
                    }
                }

                //REGRESAR LA INFORMACION DE LOS DOCUMENTOS QUE CORRESPONDEN A ESOS TIPOS DE EVENTOS
                var documentsDto = _mapper.Map<List<InviteDto>>(Documentos);
                ResponseDto result = new ResponseDto(ResponseDtoEnum.Success);
                result.response = documentsDto;

                return result;
            }
            catch (Exception e)
            {
                return new ResponseDto(ResponseDtoEnum.Error) { message = e.Message };
            }
        }


    }
}
