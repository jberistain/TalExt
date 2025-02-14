using AutoMapper;
using CommonTools.DTOs.Query;
using CommonTools.DTOs.Register;
using CommonTools.Enums;
using CommonTools.Implementation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Migracion.Talento.CoreWebApi.Interfaces;
using Migracion.Talento.Entities.Models;
using Migracion.Talento.WebAPI.DataConnection;

namespace Migracion.Talento.CoreWebApi.Services
{
    public class DocumentsEvents :IDocumentsEvents
    {
        private AppDbContext _context;
        private IMapper _mapper;
        public DocumentsEvents(AppDbContext appDbContext,IMapper mapper)
        {
            _context = appDbContext;
            _mapper = mapper;
        }


        public async Task<ResponseDto> GetAllDocsBySecretCode(string secretCode)
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
                r.message+= e.Message;
                return r;
            }
        }



    }
}
