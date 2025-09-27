using AutoMapper;
using CommonTools.DTOs;
using CommonTools.DTOs.Query;
using CommonTools.DTOs.Register;
using CommonTools.Enums;
using CommonTools.Pdf;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Migracion.Talento.CoreWebApi.Interfaces;
using Migracion.Talento.Entities.Models;
using Migracion.Talento.Models;
using Migracion.Talento.WebAPI.DataConnection;
using SkiaSharp;
using System.Globalization;
using System.Text;
using static iTextSharp.text.pdf.AcroFields;

namespace Migracion.Talento.CoreWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnotherAssistantsController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;
        private readonly IEmailSender _emailSender;
        private readonly ApplicationProperties _configuration;
        private readonly IMapper _mapper;
        private readonly IDocumentsEvents _documentEvents;

        private List<string> ListaEmailsCC = new List<string>();
        public AnotherAssistantsController(AppDbContext appDbContext
            , IEmailSender emailSender
            ,IOptions<ApplicationProperties> configuration
            , IMapper mapper
            ,IDocumentsEvents documentsEvents)
        {
            _appDbContext = appDbContext;
            _emailSender = emailSender;
            _configuration = configuration.Value;
            _mapper = mapper;
            _documentEvents = documentsEvents;
        }


        [HttpPost("ListaEventos")]
        public async Task<ActionResult<ResponseDto>> Get(FilterRegisterDto? filterRegister)
        {
            try
            {
                if (!await _appDbContext.REG_EVENTS_ADMON.AnyAsync(e => e.ACTIVE))
                    return Ok(new ResponseDto(ResponseDtoEnum.NoData));


                var list = await _appDbContext.REG_EVENTS_ADMON
                    .Include("CAT_ACTIVITIES")
                    .Include("CAT_GENDERS")
                    .Include("CAT_COUNTRIES")
                    .Include("CAT_NATIONALITIES")
                    .Include("CAT_PROCESS")
                    .Include("CAT_STATUS")
                    .Include("CAT_AIR_LINES")
                    .Include("CAT_AIRPORTS")
                    .Include("CAT_COMPANIES")
                    .Where(e => e.ACTIVE).ToListAsync();


                if (filterRegister != null)
                {
                    //query by date  
                    if (!string.IsNullOrEmpty(filterRegister.FechaInicio)
                        && !string.IsNullOrEmpty(filterRegister.FechaFin))
                    {
                        DateTime ShortDateIni = Convert.ToDateTime(DateTime.Parse(filterRegister.FechaInicio).ToString());
                        DateTime ShortDateFin = Convert.ToDateTime(DateTime.Parse(filterRegister.FechaFin).ToString());

                        list = list.Where(lista => lista.CREATED_DATE.Date >= ShortDateIni.Date
                            && lista.CREATED_DATE.Date <= ShortDateFin.Date).ToList();
                    }
                    if (!string.IsNullOrEmpty(filterRegister.Busqueda))
                        list = list.Where(lista => lista.PASSPORT_NAME.ToUpper().Contains(filterRegister.Busqueda.ToUpper()) || lista.PASSPORT_LASTNAME.ToUpper().Contains(filterRegister.Busqueda.ToUpper())).ToList();

                    if (filterRegister.EstatusBuscado != 0)
                        list = list.Where(lista => lista.ID_STATUS == filterRegister.EstatusBuscado).ToList();

                }

                //mapper de modelo de entrega
                var qry = _mapper.Map<List<QryRegEventAdmonDto>>(list);

                ResponseDto result = new ResponseDto(ResponseDtoEnum.Success);
                result.response = qry;
                return Ok(result);
            }
            catch (Exception e)
            {
                var er = new ResponseDto(ResponseDtoEnum.NoData);
                er.message = e.Message;
                return Ok(er);
            }
        }



        [HttpGet("findByPassport")]
        public async Task<ActionResult<ResponseDto>> FindPassportByNumber(string passportNumber)
        {
            if (!await _appDbContext.REG_EVENTS.AnyAsync(even => even.PASSPORT_NUM.Equals(passportNumber)))
                return Ok(new ResponseDto(ResponseDtoEnum.NoData));

            var item = await _appDbContext.REG_EVENTS
                .Include("CAT_ACTIVITIES")
                .Include("CAT_GENDERS")
                .Include("CAT_COUNTRIES")
                .Include("CAT_NATIONALITIES")
                .Include("CAT_PROCESS")
                .Include("CAT_STATUS")
                .Include("CAT_AIR_LINES")
                .Include("CAT_AIRPORTS")
                .Include("CAT_COMPANIES")
                .Where((even) => even.PASSPORT_NUM.Equals(passportNumber)).FirstOrDefaultAsync();

            ResponseDto result = new ResponseDto(ResponseDtoEnum.Success);
            result.response= item;

            return Ok(result);
        }

        [HttpGet("findBySecretCode")]
        public async Task<ActionResult<ResponseDto>> FindBySecretCode(string secretCode)
        {
            if (!await _appDbContext.REG_EVENTS.AnyAsync(even => even.SECRET_CODE.Equals(secretCode)))
                return Ok(new ResponseDto(ResponseDtoEnum.NoData));

            var item = await _appDbContext.REG_EVENTS
                .Include("CAT_ACTIVITIES")
                .Include("CAT_GENDERS")
                .Include("CAT_COUNTRIES")
                .Include("CAT_NATIONALITIES")
                .Include("CAT_PROCESS")
                .Include("CAT_STATUS")
                .Include("CAT_AIR_LINES")
                .Include("CAT_AIRPORTS")
                .Include("CAT_COMPANIES")
                .Where((even) => even.SECRET_CODE.Equals(secretCode)).FirstOrDefaultAsync();

            ResponseDto result = new ResponseDto(ResponseDtoEnum.Success);
            result.response = item;

            return Ok(result);
        }

        [HttpGet("findById")]
        public async Task<ActionResult<ResponseDto>> FindById(int id)
        {
            try
            {
                if (!await _appDbContext.REG_EVENTS_ADMON.AnyAsync(even => even.ID_REG == (id)))
                    return Ok(new ResponseDto(ResponseDtoEnum.NoData));

                var item = await _appDbContext.REG_EVENTS_ADMON
                    .Include("CAT_ACTIVITIES")
                    .Include("CAT_GENDERS")
                    .Include("CAT_COUNTRIES")
                    .Include("CAT_NATIONALITIES")
                    .Include("CAT_PROCESS")
                    .Include("CAT_STATUS")
                    .Include("CAT_AIR_LINES")
                    .Include("CAT_AIRPORTS")
                    .Include("CAT_COMPANIES")
                    .Where((even) => even.ID_REG == id).FirstOrDefaultAsync();

                var RegEventsDto = _mapper.Map<QryRegEventAdmonDto>(item);

                List< RegEvenStateDateAdmonDto> listEventsFound = null;
                if (await _appDbContext.REG_EVENT_ESTATES_DATE_ADMON.AnyAsync(even => even.ID_REG == id))
                {
                    listEventsFound = new List<RegEvenStateDateAdmonDto>();

                    var itemsEvents = await _appDbContext.REG_EVENT_ESTATES_DATE_ADMON
                        .Where((eventos) => eventos.ID_REG == id).ToListAsync();
                    listEventsFound = _mapper.Map<List<RegEvenStateDateAdmonDto>>(itemsEvents);

                }
                if (listEventsFound != null)
                    RegEventsDto.EVENTS = listEventsFound;


                List<AnotherAssistantDto> listAnotherAssistantsFound = null;
                if (await _appDbContext.ANOTHER_ASSISTANTS_ADMON.AnyAsync(even => even.ID_REG == id))
                {
                    listAnotherAssistantsFound = new List<AnotherAssistantDto>();

                    var itemsEvents = await _appDbContext.ANOTHER_ASSISTANTS_ADMON
                        .Where((eventos) => eventos.ID_REG == id).ToListAsync();
                    listAnotherAssistantsFound = _mapper.Map<List<AnotherAssistantDto>>(itemsEvents);

                }
                if (listAnotherAssistantsFound != null)
                    RegEventsDto.ANOTHER_ASSISTANTS_ADMON_LIST = listAnotherAssistantsFound;



                List<AnotherArtistDto> listAnotherArtistsFound = null;
                if (await _appDbContext.ANOTHER_ARTISTS_ADMON.AnyAsync(even => even.ID_REG == id))
                {
                    listAnotherArtistsFound = new List<AnotherArtistDto>();

                    var itemsEvents = await _appDbContext.ANOTHER_ARTISTS_ADMON
                        .Where((eventos) => eventos.ID_REG == id).ToListAsync();
                    listAnotherArtistsFound = _mapper.Map<List<AnotherArtistDto>>(itemsEvents);
                }
                if (listAnotherArtistsFound != null)
                    RegEventsDto.ANOTHER_ARTISTS_ADMON_LIST = listAnotherArtistsFound;

                ResponseDto result = new ResponseDto(ResponseDtoEnum.Success);
                result.response = RegEventsDto;

                return Ok(result);
            }
            catch(Exception e)
            {
                return Ok(e.Message);
            }
        }



        private Random random = new Random();
        private string RandomString(int length)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }


        [HttpPost("CreateNewInvitation")]
        public async Task<ActionResult<ResponseDto>> RegisterEvent([FromBody] RegEventAnotherAssistantDto newEvent)
        {
            ResponseDto responseDto = new ResponseDto();

            try
            {
                int idAeropuerto;
                int idEmpresa;
                if (!string.IsNullOrEmpty(newEvent.NOMBRE_NUEVA_EMPRESA))
                {
                    CompaniesController companiesController = new CompaniesController(this._appDbContext, this._mapper);
                    CompanyRegisterDto reg = new CompanyRegisterDto();
                    reg.DESC_COMPANY_EN = newEvent.NOMBRE_NUEVA_EMPRESA;
                    reg.DESC_COMPANY_SP = newEvent.NOMBRE_NUEVA_EMPRESA;
                    reg.LEGAL_REPRESENTATIVE = "LEGAL_REPRESENTATIVE";
                    reg.CREATED_BY = 2;
                    var resp = await companiesController.SaveIfNotExist(reg);
                    idEmpresa = resp.response.ID_COMPANY;
                    newEvent.ID_COMPANY = idEmpresa;
                }


                bool antecedentesMexico = false;
                if (newEvent.CRIMINAL_RECORD_MEX != null)
                    antecedentesMexico = (bool)newEvent.CRIMINAL_RECORD_MEX;

                if (!antecedentesMexico)
                    if (newEvent.EXPELLED_MEX != null)
                        antecedentesMexico = (bool)newEvent.EXPELLED_MEX;

                if (antecedentesMexico)
                {
                    newEvent.ID_STATUS = 3;
                }

                string secredCode = RandomString(10);
                newEvent.SECRET_CODE = secredCode;
                var model = _mapper.Map<RegEventsAdmon>(newEvent);
                model.CREATED_BY = 2;
                model.CREATED_DATE = DateTime.Now;
                _appDbContext.Add(model);
                var rs = await _appDbContext.SaveChangesAsync();

                /* Guardar los eventos */
                foreach (RegEvenStateDateDto currentEvent in newEvent.EVENTS)
                {
                    //Si se crea un nuevo inmueble se tiene que actualizar el valor
                    if (!string.IsNullOrEmpty(currentEvent.NOMBRE_NUEVO_INMUEBLE))
                    {
                        EstatesController estateController = new EstatesController(this._appDbContext, this._mapper);
                        EstateRegisterDto reg = new EstateRegisterDto();
                        reg.DESC_ESTATE_EN = currentEvent.NOMBRE_NUEVO_INMUEBLE;
                        reg.DESC_ESTATE_SP = currentEvent.NOMBRE_NUEVO_INMUEBLE;
                        reg.CREATED_BY = 2;
                        var resp = await estateController.SaveIfNotExist(reg);
                        currentEvent.ID_ESTATE = resp.response.ID_ESTATE;
                    }

                    var modelEvent = _mapper.Map<RegEvenStateDateAdmon>(currentEvent);
                    modelEvent.CREATED_BY = 2;
                    modelEvent.CREATED_DATE = DateTime.Now;
                    modelEvent.ID_REG = model.ID_REG;
                    _appDbContext.Add(modelEvent);
                    var rsEvent = await _appDbContext.SaveChangesAsync();
                }

                /* Guardar los otros asistentes */
                if (newEvent.ANOTHER_ASSISTANTS != null)
                {
                    foreach (AnotherAssistantDto currentElement in newEvent.ANOTHER_ASSISTANTS)
                    {
                        var modelEvent = _mapper.Map<AnotherAssistantsAdmon>(currentElement);
                        modelEvent.PASSPORT_LASTNAME = currentElement.PASSPORT_LASTNAME;
                        modelEvent.PASSPORT_NAME = currentElement.PASSPORT_NAME;
                        modelEvent.ACTIVITY_MEXICO = currentElement.ACTIVITY_MEXICO;
                        modelEvent.ID_NATIONALITY = currentElement.ID_NATIONALITY;
                        modelEvent.PASSPORT_NUM = currentElement.PASSPORT_NUM;
                        modelEvent.ACTIVE = true;
                        modelEvent.CREATED_DATE = DateTime.Now;
                        modelEvent.CREATED_BY = 2;
                        modelEvent.ID_REG = model.ID_REG;
                        _appDbContext.Add(modelEvent);
                        var rsEvent = await _appDbContext.SaveChangesAsync();
                    }
                }


                /* Guardar los otros artistas */
                if (newEvent.ANOTHER_ARTISTS != null)
                {
                    foreach (AnotherArtistDto currentElement in newEvent.ANOTHER_ARTISTS)
                    {
                        var modelEvent = _mapper.Map<AnotherArtistsAdmon>(currentElement);
                        modelEvent.PASSPORT_NAME = currentElement.PASSPORT_NAME;
                        modelEvent.ACTIVE = true;
                        modelEvent.CREATED_DATE = DateTime.Now;
                        modelEvent.CREATED_BY = 2;
                        modelEvent.ID_REG = model.ID_REG;
                        _appDbContext.Add(modelEvent);
                        var rsEvent = await _appDbContext.SaveChangesAsync();
                    }
                }

                if (rs == 1)
                {
                    object requestObject = new
                    {
                        SECRET_CODE = secredCode,
                        MENSAJE = "Guardado exitoso",
                        idinvitacion= model.ID_REG
                    }
                ;

                    responseDto.error = false;
                    responseDto.code = 200;
                    responseDto.response = requestObject;
                }
                else
                {
                    responseDto.error = true;
                    responseDto.code = 400;
                }
            }
            catch (Exception ex)
            {
                responseDto.error = true;
                responseDto.message = ex.Message;
            }

            return Ok(responseDto);
        }


        [HttpPost("UpdateInvitation")]
        public async Task<ActionResult<ResponseDto>> UpdateInvitation([FromBody] QryRegEventAdmonDto newEvent)
        {
            ResponseDto responseDto = new ResponseDto();

            try
            {
                //Eliminar eventos que referencian al registro principal
                //Guardar Los eventos que llegan
                int id;
                foreach (RegEvenStateDateAdmonDto currentEvent in newEvent.EVENTS)
                {
                    RegEvenStateDateAdmon itemEvent = null;
                    if (currentEvent.ID_REG_EVEN_DATE != 0)
                    {
                        id = currentEvent.ID_REG_EVEN_DATE;
                        var regDb = await _appDbContext.REG_EVENT_ESTATES_DATE_ADMON.AnyAsync(g => g.ID_REG_EVEN_DATE == id);

                        itemEvent = await _appDbContext.REG_EVENT_ESTATES_DATE_ADMON
                             .Where((even) => even.ID_REG_EVEN_DATE == id).FirstAsync();
                        itemEvent.ID_EVENT = (int)currentEvent.ID_EVENT;
                        itemEvent.ID_ESTATE = (int)currentEvent.ID_ESTATE;
                        itemEvent.DESC_LOCATION = currentEvent.DESC_LOCATION;
                        itemEvent.EVENT_DATE = currentEvent.EVENT_DATE;
                        itemEvent.EVENT_DATE_FIN = currentEvent.EVENT_DATE_FIN;
                        itemEvent.MODIFY_BY = newEvent.MODIFY_BY;
                        itemEvent.MODIFY_DATE = DateTime.Now;
                        itemEvent.ACTIVE = true;
                    }
                    else
                    {
                        itemEvent = new RegEvenStateDateAdmon();
                        itemEvent.CREATED_DATE = DateTime.Now;
                        itemEvent.CREATED_BY = newEvent.MODIFY_BY;
                        itemEvent.ID_REG = newEvent.ID_REG;

                        itemEvent.ID_EVENT = (int)currentEvent.ID_EVENT;
                        itemEvent.ID_ESTATE = (int)currentEvent.ID_ESTATE;
                        itemEvent.DESC_LOCATION = currentEvent.DESC_LOCATION;
                        itemEvent.EVENT_DATE = currentEvent.EVENT_DATE;
                        itemEvent.EVENT_DATE_FIN = currentEvent.EVENT_DATE_FIN;
                        itemEvent.MODIFY_BY = currentEvent.MODIFY_BY;
                        itemEvent.MODIFY_DATE = DateTime.Now;
                        itemEvent.ACTIVE = true;

                        await _appDbContext.AddAsync(itemEvent);
                    }

                    var rsEvent = await _appDbContext.SaveChangesAsync();
                }

                if (newEvent.ANOTHER_ASSISTANTS_ADMON_LIST != null) {
                    foreach (AnotherAssistantDto currentEvent in newEvent.ANOTHER_ASSISTANTS_ADMON_LIST)
                    {
                        AnotherAssistantsAdmon itemEvent = null;
                        if (currentEvent.ID != 0)
                        {
                            id = currentEvent.ID;
                            itemEvent = await _appDbContext.ANOTHER_ASSISTANTS_ADMON
                                 .Where((even) => even.ID == id).FirstAsync();
                            itemEvent.PASSPORT_LASTNAME = currentEvent.PASSPORT_LASTNAME;
                            itemEvent.PASSPORT_NAME = currentEvent.PASSPORT_NAME;
                            itemEvent.ACTIVITY_MEXICO = currentEvent.ACTIVITY_MEXICO;
                            itemEvent.ID_NATIONALITY = currentEvent.ID_NATIONALITY;
                            itemEvent.PASSPORT_NUM = currentEvent.PASSPORT_NUM;
                            itemEvent.MODIFY_BY = newEvent.MODIFY_BY;
                            itemEvent.MODIFY_DATE = DateTime.Now;
                            itemEvent.ACTIVE = true;
                        }
                        else
                        {
                            itemEvent = new AnotherAssistantsAdmon();
                            itemEvent.CREATED_DATE = DateTime.Now;
                            itemEvent.CREATED_BY = newEvent.MODIFY_BY;
                            itemEvent.ID_REG = newEvent.ID_REG;

                            itemEvent.PASSPORT_LASTNAME = currentEvent.PASSPORT_LASTNAME;
                            itemEvent.PASSPORT_NAME = currentEvent.PASSPORT_NAME;
                            itemEvent.ACTIVITY_MEXICO = currentEvent.ACTIVITY_MEXICO;
                            itemEvent.ID_NATIONALITY = currentEvent.ID_NATIONALITY;
                            itemEvent.PASSPORT_NUM = currentEvent.PASSPORT_NUM;
                            itemEvent.MODIFY_BY = newEvent.MODIFY_BY;
                            itemEvent.MODIFY_DATE = DateTime.Now;
                            itemEvent.ACTIVE = true;

                            await _appDbContext.AddAsync(itemEvent);
                        }

                        var rsEvent = await _appDbContext.SaveChangesAsync();
                    }
                }

                /* Actualizar lista de otros artistas invitados */
                if (newEvent.ANOTHER_ARTISTS_ADMON_LIST != null)
                {
                    foreach (AnotherArtistDto currentEvent in newEvent.ANOTHER_ARTISTS_ADMON_LIST)
                    {
                        AnotherArtistsAdmon itemEvent;
                        if (currentEvent.ID != 0)
                        {
                            id = currentEvent.ID;
                            itemEvent = await _appDbContext.ANOTHER_ARTISTS_ADMON
                                 .Where((even) => even.ID == currentEvent.ID).FirstAsync();
                            itemEvent.PASSPORT_NAME = currentEvent.PASSPORT_NAME;
                            itemEvent.MODIFY_BY = newEvent.MODIFY_BY;
                            itemEvent.MODIFY_DATE = DateTime.Now;
                            itemEvent.ACTIVE = true;
                        }
                        else
                        {
                            itemEvent = new AnotherArtistsAdmon();
                            itemEvent.CREATED_DATE = DateTime.Now;
                            itemEvent.CREATED_BY = newEvent.MODIFY_BY;
                            itemEvent.ID_REG = newEvent.ID_REG;

                            itemEvent.PASSPORT_NAME = currentEvent.PASSPORT_NAME;
                            itemEvent.MODIFY_BY = newEvent.MODIFY_BY;
                            itemEvent.MODIFY_DATE = DateTime.Now;
                            itemEvent.ACTIVE = true;

                            await _appDbContext.AddAsync(itemEvent);
                        }

                        var rsEvent = await _appDbContext.SaveChangesAsync();
                    }
                }

                id = newEvent.ID_REG;
                var genderDb = await _appDbContext.REG_EVENTS_ADMON.AnyAsync(g => g.ID_REG == id);
                if (!genderDb)
                    return Ok(new ResponseDto(ResponseDtoEnum.NoData));

                var item = await _appDbContext.REG_EVENTS_ADMON
                     .Where((even) => even.ID_REG == id).FirstOrDefaultAsync();


                item.ID_GENDER = newEvent.ID_GENDER;
                item.ID_COUNTRY = newEvent.ID_COUNTRY;
                item.ID_NATIONALITY = newEvent.ID_NATIONALITY;
                item.ID_AIRPORT = newEvent.ID_AIRPORT;
                item.ID_AIR_LINE = newEvent.ID_AIR_LINE;
                item.ID_COMPANY = newEvent.ID_COMPANY;
                item.PASSPORT_NUM = newEvent.PASSPORT_NUM;
                item.DATE_VIG_INI = newEvent.DATE_VIG_INI;
                item.DATE_VIG_FIN = newEvent.DATE_VIG_FIN;
                item.PASSPORT_NAME = newEvent.PASSPORT_NAME;
                item.PASSPORT_LASTNAME = newEvent.PASSPORT_LASTNAME;
                item.EMAIL = newEvent.EMAIL;
                item.ACTUAL_JOB = newEvent.ACTUAL_JOB;
                // item.EXPELLED_MEX = newEvent.EXPELLED_MEX;
                // item.CRIMINAL_RECORD_MEX = newEvent.CRIMINAL_RECORD_MEX;
                item.DATE_EVENT_INI = newEvent.DATE_EVENT_INI;
                item.DATE_EVENT_FIN = newEvent.DATE_EVENT_FIN;
                item.FLIGHT = newEvent.FLIGHT;
                item.FLIGHT_NUMBER = newEvent.FLIGHT_NUMBER;
                item.ACTIVITY_COUNTRY = newEvent.ACTIVITY_COUNTRY;
                item.ACTIVITY_MEXICO = newEvent.ACTIVITY_MEXICO;
                item.DATE_ARRIVE = newEvent.DATE_ARRIVE;
                item.DATE_LEAVE = newEvent.DATE_LEAVE;
                item.CHECK_VERIFY = newEvent.CHECK_VERIFY;
                item.MODIFY_DATE = DateTime.Now;
                item.MODIFY_BY = newEvent.MODIFY_BY;

                // _appDbContext.Add(model);
                var rs = await _appDbContext.SaveChangesAsync();

                responseDto.code = 200;
                responseDto.message = "Registro actualizado correctamente";
                responseDto.error = false;


            }
            catch (Exception ex)
            {
                responseDto.error = true;
                responseDto.message = ex.Message;
            }

            return Ok(responseDto);
        }


        public static string RemoveDiacritics(string input)
        {
            if (string.IsNullOrEmpty(input)) return input;
            var normalized = input.Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder(normalized.Length);
            foreach (var c in normalized)
            {
                var uc = CharUnicodeInfo.GetUnicodeCategory(c);
                if (uc != UnicodeCategory.NonSpacingMark &&
                    uc != UnicodeCategory.SpacingCombiningMark &&
                    uc != UnicodeCategory.EnclosingMark)
                {
                    sb.Append(c);
                }
            }
            return sb.ToString().Normalize(NormalizationForm.FormC);
        }

        [HttpPost("UpdateAssistants")]
        public async Task<ActionResult<ResponseDto>> UpdateAssistants([FromBody] QryRegEventAdmonDto newEvent)
        {
            ResponseDto responseDto = new ResponseDto();

            try
            {
                int idRegEvent = newEvent.ID_REG;
                int numRegsAfectados = 0;
                if (newEvent?.ANOTHER_ASSISTANTS_ADMON_LIST is { Count: > 0 } list)
                {
                    // 1) Normaliza y prepara llaves
                    var passports = list
                        .Where(x => !string.IsNullOrWhiteSpace(x.PASSPORT_NUM))
                        .Select(x => x.PASSPORT_NUM.Trim())
                        .Distinct(StringComparer.OrdinalIgnoreCase)
                        .ToList();

                    // 2) Trae de una sola vez los existentes para este evento
                    var existingMap = await _appDbContext.ANOTHER_ASSISTANTS_ADMON
                        .Where(a => a.ID_REG == idRegEvent && passports.Contains(a.PASSPORT_NUM))
                        .ToDictionaryAsync(a => a.PASSPORT_NUM, StringComparer.OrdinalIgnoreCase);

                    // 3) Upsert en memoria
                    foreach (var dto in list)
                    {
                        var pass = dto.PASSPORT_NUM?.Trim();
                        if (string.IsNullOrEmpty(pass)) continue; // pasar al siguiente porque el pasaporte no puede ser nulo o vacío

                        /* Validar que tenga existencia la nacionalidad */
                        if (string.IsNullOrEmpty(dto.NATIONALITY) || !await _appDbContext.CAT_NATIONALITIES.AnyAsync( m => m.DESC_NACIONALITY_EN.ToUpper().Equals(dto.NATIONALITY.ToUpper()) || m.DESC_NACIONALITY_SP.ToUpper().Equals(dto.NATIONALITY.ToUpper())))
                        {
                            continue;
                        }

                        var currentNationality = await _appDbContext.CAT_NATIONALITIES
                            .Where(m => m.DESC_NACIONALITY_EN.ToUpper().Equals(dto.NATIONALITY.ToUpper()) || m.DESC_NACIONALITY_SP.ToUpper().Equals(dto.NATIONALITY.ToUpper())).FirstAsync();

                        if (existingMap.TryGetValue(pass, out var assistant))
                        {
                            // Update
                            assistant.PASSPORT_LASTNAME = dto.PASSPORT_LASTNAME;
                            assistant.PASSPORT_NAME = dto.PASSPORT_NAME;
                            assistant.ACTIVITY_MEXICO = dto.ACTIVITY_MEXICO;
                            assistant.ID_NATIONALITY = currentNationality.ID_NATIONALITY;
                        }
                        else
                        {
                            // Insert
                            var newAssistant = new AnotherAssistantsAdmon
                            {
                                ID_REG = idRegEvent,          
                                PASSPORT_LASTNAME = dto.PASSPORT_LASTNAME,
                                PASSPORT_NAME = dto.PASSPORT_NAME,
                                ACTIVITY_MEXICO = dto.ACTIVITY_MEXICO,
                                ID_NATIONALITY = currentNationality.ID_NATIONALITY,
                                PASSPORT_NUM = pass,
                                ACTIVE = true,
                                CREATED_BY=2
                            };
                            await _appDbContext.AddAsync(newAssistant);
                        }
                        numRegsAfectados++;
                    }

                    // 4) Un solo SaveChanges (o por lotes si la lista es enorme)
                    await _appDbContext.SaveChangesAsync();

                    responseDto.code = 200;
                    responseDto.message = $"Registros ingresados correctamente. Se actualizaron {numRegsAfectados} registros";
                    responseDto.error = false;
                }
                else
                {
                    responseDto.code = 400;
                    responseDto.message = "No se encontraron registros en el archivo";
                    responseDto.error = false;
                }
            }
            catch (Exception ex)
            {
                responseDto.error = true;
                responseDto.message = ex.Message;
            }

            return Ok(responseDto);
        }

        private async Task<ResponseDto> GeneraInvitacionPDF(QryRegEventDto regEvent) {
            ResponseDto responseDto = new ResponseDto();

            var regInvite = await _appDbContext.REG_EVENTS_ADMON
                .Include(r => r.CAT_NATIONALITIES)
                .Where(r => r.ID_REG == regEvent.ID_REG)
                .FirstOrDefaultAsync();

            IReporteInfo reporteInfo = new ReportInformation()
            {
                NombreInvitado = $"{regInvite.PASSPORT_NAME} {regInvite.PASSPORT_LASTNAME}",
                FechaEntradaAlPais = regInvite.DATE_ARRIVE.Value.ToString("dd/MM/yyyy"),
                FechaSalidaAlPais = regInvite.DATE_LEAVE.Value.ToString("dd/MM/yyyy"),
                NacionalidadEsp = regInvite.CAT_NATIONALITIES.DESC_NACIONALITY_SP,
                NacionalidadIng = regInvite.CAT_NATIONALITIES.DESC_NACIONALITY_EN,
                NumPasaporte = regInvite.PASSPORT_NUM,
                PuestoParteStaff = regInvite.ACTUAL_JOB,
                TipoArchivoGenerado = string.Empty
            };

            int idEvento = 0;
            List<InfoEventoModel> EventList = new List<InfoEventoModel>();
            //BUSCAR LOS EVENTOS REGISTRADOS
            if (await _appDbContext.REG_EVENT_ESTATES_DATE_ADMON.AnyAsync(even => even.ID_REG == regEvent.ID_REG))
            {

                var RegEventsStatesList = await _appDbContext.REG_EVENT_ESTATES_DATE_ADMON
                    .Include("CAT_EVENTS")
                    .Include("CAT_ESTATES")
                    .Where((even) => even.ID_REG == regEvent.ID_REG).ToListAsync();
                foreach (var currentEventEstate in RegEventsStatesList)
                {
                    EventList.Add(new InfoEventoModel()
                    {
                        FechaInicioEvento = currentEventEstate.EVENT_DATE.ToString("dd/MM/yyyy"),
                        FechaFinEvento = currentEventEstate.EVENT_DATE_FIN.ToString("dd/MM/yyyy"),
                        InmuebleEvento = currentEventEstate.CAT_ESTATES.DESC_ESTATE_SP,
                        NombreEvento = currentEventEstate.CAT_EVENTS.DESC_EVENT_SP,
                        UbicacionInmueble = currentEventEstate.DESC_LOCATION
                    });
                    if (idEvento == 0)
                    {
                        idEvento = currentEventEstate.ID_EVENT != null ? (int)currentEventEstate.ID_EVENT : 0;
                    }
                }
            }

            reporteInfo.InfoEventosList = EventList.ToList<IInfoEvento>();

            /* Buscar lista de otros asistentes */
            List<OtroInvitadoModel> OtrosInvitadosList = new List<OtroInvitadoModel>();
            OtrosInvitadosList.Add(new OtroInvitadoModel()
            {
                Apellidos = regInvite.PASSPORT_LASTNAME,
                Nombre = regInvite.PASSPORT_NAME,
                ActvidadEnMexico = regInvite.ACTIVITY_MEXICO,
                IdNacionalidad = regInvite.ID_NATIONALITY.ToString(),
                Nacionalidad = regInvite.LANGUAGE.Equals("EN") ? regInvite.CAT_NATIONALITIES.DESC_NACIONALITY_EN : regInvite.CAT_NATIONALITIES.DESC_NACIONALITY_SP,
                NumPasaporte = regInvite.PASSPORT_NUM
            });
            if (await _appDbContext.ANOTHER_ASSISTANTS_ADMON.AnyAsync(even => even.ID_REG == regEvent.ID_REG))
            {
                var AnotherAssistants = await _appDbContext.ANOTHER_ASSISTANTS_ADMON
                    .Include("CAT_NATIONALITIES")
                    .Where((even) => even.ID_REG == regEvent.ID_REG).ToListAsync();
                foreach (var currentElement in AnotherAssistants)
                {
                    OtrosInvitadosList.Add(new OtroInvitadoModel()
                    {
                        Apellidos = currentElement.PASSPORT_LASTNAME,
                        Nombre = currentElement.PASSPORT_NAME,
                        ActvidadEnMexico = currentElement.ACTIVITY_MEXICO,
                        IdNacionalidad = currentElement.ID_NATIONALITY.ToString(),
                        Nacionalidad = regEvent.LANGUAGE.Equals("EN") ? currentElement.CAT_NATIONALITIES.DESC_NACIONALITY_EN : currentElement.CAT_NATIONALITIES.DESC_NACIONALITY_SP,
                        NumPasaporte = currentElement.PASSPORT_NUM
                    });
                }
            }
            List<IOtroInvitadoModel> AnotherAssistantsList = OtrosInvitadosList.ToList<IOtroInvitadoModel>();

            /* Buscar los artistas */
            List<OtroInvitadoModel> OtrosArtistas = new List<OtroInvitadoModel>();
            //OtrosArtistas.Add(new OtroInvitadoModel()
            //{
            //    Apellidos = regInvite.PASSPORT_LASTNAME,
            //    Nombre = regInvite.PASSPORT_NAME,
            //    ActvidadEnMexico = regInvite.ACTIVITY_MEXICO,
            //    IdNacionalidad = regInvite.ID_NATIONALITY.ToString(),
            //    Nacionalidad = regInvite.LANGUAGE.Equals("EN") ? regInvite.CAT_NATIONALITIES.DESC_NACIONALITY_EN : regInvite.CAT_NATIONALITIES.DESC_NACIONALITY_SP,
            //    NumPasaporte = regInvite.PASSPORT_NUM
            //});

            if (await _appDbContext.ANOTHER_ARTISTS_ADMON.AnyAsync(even => even.ID_REG == regEvent.ID_REG))
            {
                var AnotherAssistants = await _appDbContext.ANOTHER_ARTISTS_ADMON
                    .Where((even) => even.ID_REG == regEvent.ID_REG).ToListAsync();
                foreach (var currentElement in AnotherAssistants)
                {
                    OtrosArtistas.Add(new OtroInvitadoModel()
                    {
                        Nombre = currentElement.PASSPORT_NAME,
                    });
                }
            }
            List<IOtroInvitadoModel> AnotherArtistsList = OtrosArtistas.ToList<IOtroInvitadoModel>();

            // Buscar documento a partir del primer evento encontrado
            /* El usuario definio que solo se iban  a seleccionar eventos que fueran de la misma empresa */
            var firstEventFoud = await _appDbContext.CAT_EVENTS.Where(doc => doc.ID_EVENT == idEvento).FirstAsync();
            /* Si ningun evento tiene un documento asignado se manda error */
            if (firstEventFoud.ID_EVENT_TYPE_ADMON == null || firstEventFoud.ID_EVENT_TYPE_ADMON == 0)
            {
                responseDto.error = true;
                responseDto.code = 400;
                responseDto.message = "No se encontro un evento que tenga asignada una empresa para generar la invitación. Pida al administrador que asigne una empresa en el catálogo de Eventos para el evento seleccionado";
                return responseDto;
            }
            var firstEventType = await _appDbContext.CAT_EVENT_TYPES.Where(doc => doc.ID_EVENT_TYPE == firstEventFoud.ID_EVENT_TYPE_ADMON).FirstOrDefaultAsync();

            /* Se gennera un solo documento, se obtiene el primero que se encuentre activo */
            RegInviteAdmon esquemaInvitacion = await _appDbContext.REG_INVITE_ADMON.Where(doc => doc.ID_EVENT_TYPE == firstEventType.ID_EVENT_TYPE && doc.ACTIVE == true).FirstOrDefaultAsync();
            InviteDto esquemaInvitacionDto = _mapper.Map<InviteDto>(esquemaInvitacion);


            //Genera Carta invitacion
            PdfManager reporte = new PdfManager();
            var invitacion = reporte.GenerateOtherAssistantsDocument(esquemaInvitacionDto, reporteInfo, AnotherAssistantsList, AnotherArtistsList, regEvent.LANGUAGE);
            invitacion.FileName = regEvent.ID_REG.ToString()+".pdf";



            responseDto.error = false;
            responseDto.code = 200;
            responseDto.response = Convert.ToBase64String(invitacion.File);
            responseDto.message = invitacion.FileName;
            return responseDto;
        }

        [HttpPost("GetInvitationPDF")]
        public async Task<ActionResult<ResponseDto>> GetInvitationPDF([FromBody] QryRegEventDto regEvent)
        {
            return await GeneraInvitacionPDF(regEvent);
        }



        [HttpPost("DownloadInvitationsAnotherAssistants")]
        public async Task<ActionResult<ResponseDto>> DownloadInvitationsAnotherAssistants([FromBody] List<string> regEvent)
        {
            ResponseDto responseDto = new ResponseDto();
            List<ResponseDto> ListaDocumentos = new List<ResponseDto>();
            foreach (string idStr in regEvent)
            { 

                int ID_REG = Convert.ToInt32(idStr);
                
                /* invocar al otro metodo que genera el pdf */   
                var documentoEsp = await GeneraInvitacionPDF(new QryRegEventDto() { ID_REG = ID_REG, LANGUAGE = "ES" });
                documentoEsp.message = "ES"+ documentoEsp.message;
                ListaDocumentos.Add(documentoEsp);
                var documentoIng = await GeneraInvitacionPDF(new QryRegEventDto() { ID_REG = ID_REG, LANGUAGE = "EN" });
                documentoIng.message = "EN"+ documentoIng.message;
                ListaDocumentos.Add(documentoIng);

            }

            responseDto.error = false;
            responseDto.code = 200;
            responseDto.response = ListaDocumentos;
            return responseDto;
        }



        [HttpPost("DeleteById")]
        public async Task<ActionResult> DeleteById([FromBody] QryRegEventDto data)
        {
            try
            {
                int id = data.ID_REG;
                var result = new ResponseDto(ResponseDtoEnum.Success);
                //Eliminar registros 
                if (await _appDbContext.REG_EVENT_ESTATES_DATE.AnyAsync(even => even.ID_REG == id))
                {
                    var itemsEvents = await _appDbContext.REG_EVENT_ESTATES_DATE
                        .Where((eventos) => eventos.ID_REG == id).ToListAsync();
                    foreach (var item in itemsEvents)
                    {
                        _appDbContext.Remove(item);
                        await _appDbContext.SaveChangesAsync();
                    }
                }

                if(await _appDbContext.REG_EVENTS.AnyAsync(even => even.ID_REG == id))
                {
                    var itemEvent = await _appDbContext.REG_EVENTS
                        .Where((eventos) => eventos.ID_REG == id).FirstOrDefaultAsync();

                    if(itemEvent != null)
                    {
                        _appDbContext.Remove(itemEvent);
                        await _appDbContext.SaveChangesAsync();
                    }
                }
                result.message = "El registro fue eliminado exitosamente";
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseDto(ResponseDtoEnum.Error).message + ex.Message);
            }
        }





        [HttpPost("Attachment")]
        public async Task<ActionResult> SendAttachment([FromForm] MailDataDto mailData)
        {

            return Ok();
        }


        private async Task<ResponseDto> Template(TemplateTypeEnum templateType, InvitationMailTemplate invitationMail)
        {
            try
            {


                // Create MailData object
                MailDataDto mailData = new MailDataDto()
                {
                    To = invitationMail.Email,
                    Subject = invitationMail.Subject,
                    Body = _emailSender.GetWelcomeTemplateEmail(templateType.ToString(), invitationMail),
                    Attachments = invitationMail.attachments,
                    EmailsCC = ListaEmailsCC
                };

                await _emailSender.SendEmailAsync(mailData);

                return new ResponseDto(ResponseDtoEnum.Success);
            }
            catch (Exception ex)
            {
                var error = new ResponseDto(ResponseDtoEnum.Error);
                error.message += ex.Message;
                return error;
            }


        }

        #region ADMIN CATALOGO DOCUMENTOS
        [HttpGet("GetActiveDocuments")]
        public async Task<ActionResult<ResponseDto>> GetActiveDocuments()
        {
            try
            {
                var documents = await _appDbContext.REG_INVITE_ADMON.AnyAsync();

                if (!documents)
                    return Ok(new ResponseDto(ResponseDtoEnum.NoData));

                ResponseDto response = new ResponseDto(ResponseDtoEnum.Success);

                var listDocumen = await _appDbContext.REG_INVITE_ADMON.OrderBy(p => p.ID_INVITE).ToListAsync();
                var map = _mapper.Map<List<InviteDto>>(listDocumen);
                response.response = map;
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseDto(ResponseDtoEnum.Error).message + ex.Message);
            }

        }

        // GET: api/<DocumentsController>
        [HttpGet("GetDocumentById")]
        public async Task<ActionResult<ResponseDto>> GetDocumentById(int id)
        {
            try
            {
                if (!await _appDbContext.REG_INVITE_ADMON.AnyAsync(i => i.ID_INVITE == id))
                    return Ok(new ResponseDto(ResponseDtoEnum.NoData));

                ResponseDto response = new ResponseDto(ResponseDtoEnum.Success);

                var Documen = await _appDbContext.REG_INVITE_ADMON.FirstAsync(i => i.ID_INVITE == id);
                var map = _mapper.Map<InviteDto>(Documen);
                response.response = map;
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseDto(ResponseDtoEnum.Error).message + ex.Message);
            }

        }
        // POST api/<DocumentsController>
        [HttpPost("SaveDocument")]
        public async Task<ActionResult<ResponseDto>> SaveDocument([FromBody] RegInviteDto value)
        {
            try
            {
                var exist = await _appDbContext.REG_INVITE_ADMON.AnyAsync(g => g.DESC_SPANISH == value.DESC_SPANISH
                    && g.ID_EVENT_TYPE == value.ID_EVENT_TYPE);
                if (exist)
                    return Ok(new ResponseDto(ResponseDtoEnum.Duplicated));

                var document = _mapper.Map<RegInviteAdmon>(value);

                await _appDbContext.REG_INVITE_ADMON.AddAsync(document);
                await _appDbContext.SaveChangesAsync();
                return Ok(new ResponseDto(ResponseDtoEnum.Success));
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseDto(ResponseDtoEnum.Error).message + ex.Message);
            }
        }

        // PUT api/<DocumentsController>/5
        [HttpPost("UpdateDocument")]
        public async Task<ActionResult<ResponseDto>> UpdateDocument(int id, [FromBody] RegInviteDto value)
        {
            try
            {
                var documentDb = await _appDbContext.REG_INVITE_ADMON.AnyAsync(g => g.ID_INVITE == id);
                if (!documentDb)
                    return Ok(new ResponseDto(ResponseDtoEnum.NoData));

                var item = await _appDbContext.REG_INVITE_ADMON
                     .Where((even) => even.ID_INVITE == id).FirstOrDefaultAsync();


                item.ID_REG = value.ID_REG;
                item.ID_EVENT_TYPE = value.ID_EVENT_TYPE;
                item.DES_TITLE = value.DES_TITLE;
                item.DESC_SPANISH = value.DESC_SPANISH;
                item.DESC_ENGLISH = value.DESC_ENGLISH;
                item.SIGN_1 = value.SIGN_1;
                item.SIGN_2 = value.SIGN_2;
                item.SIGN_3 = value.SIGN_3;
                item.SIGN_4 = value.SIGN_4;
                item.FOOT_PAGE = value.FOOT_PAGE;
                item.FILE_NAME= value.FILE_NAME;
                item.SIGN_BLOB = value.SIGN_BLOB;
                item.MODIFY_BY = value.MODIFY_BY;
                item.MODIFY_DATE = DateTime.Now;

                await _appDbContext.SaveChangesAsync();

                return Ok(new ResponseDto(ResponseDtoEnum.Success));
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseDto(ResponseDtoEnum.Error).message + ex.Message);
            }
        }

        // DELETE api/<DocumentsController>/5
        [HttpPost("DeleteDocument")]
        public async Task<ActionResult<ResponseDto>> DeleteDocument([FromBody] InviteDto data)
        {
            try
            {
                int id = data.ID_INVITE;
                var document = await _appDbContext.REG_INVITE_ADMON.FirstOrDefaultAsync(g => g.ID_INVITE.Equals(id));

                if (document == null)
                    return Ok(new ResponseDto(ResponseDtoEnum.NoData));

                _appDbContext.Remove(document);
                await _appDbContext.SaveChangesAsync();
                return Ok(new ResponseDto(ResponseDtoEnum.Success));
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseDto(ResponseDtoEnum.Error).message + ex.Message);
            }
        }
        #endregion

        #region REPORTE

        [HttpPost("DownloadReportAnotherassistants")]
        public async Task<ActionResult<ResponseDto>> DownloadReportAnotherassistants([FromBody] ReporteDto model)
        {
            try
            {
                ResponseDto response = new ResponseDto(ResponseDtoEnum.Success);
                DateTime fechaInicio = model.FechaInicio;
                DateTime fechaFin = model.FechaFin;
                var endExclusive = fechaFin.Date.AddDays(1);

                // 1) Trae de DB ya filtrado y ordenado (AsNoTracking para performance en lecturas)
                var items = await _appDbContext.REG_EVENTS_ADMON
                    .AsNoTracking()
                    .Where(x => x.CREATED_DATE >= fechaInicio && x.CREATED_DATE < endExclusive)
                    .OrderBy(x => x.CREATED_BY)
                    .Include(x => x.CAT_NATIONALITIES)
                    .ToListAsync();

                /* Grupos */
                var bucketsByMonth = items
                .GroupBy(i => new { i.CREATED_DATE.Year, i.CREATED_DATE.Month })
                .OrderBy(g => g.Key.Year).ThenBy(g => g.Key.Month)
                .ToList();

                List<ReporteDto> reporteList = new List<ReporteDto>();

                foreach (var grupo in bucketsByMonth) 
                {
                    ReporteDto reporte = new ReporteDto();

                    var listaIDs = grupo.Select(g => g.ID_REG).ToList();    


                    // Totales de asistentes invitados por mes
                    var totalInvitados = _appDbContext.ANOTHER_ASSISTANTS_ADMON
                      .Where(a => listaIDs.Contains(a.ID_REG))
                      .Count();

                    // Obten NAcionalidades
                    var nacionalidades = grupo
                     .GroupBy(i => i.CAT_NATIONALITIES.DESC_NACIONALITY_SP) // agrupar por nombre del catálogo
                     .Select(g => new NacionalidadesFrecuentes
                     {
                         Nacionalidad = g.Key,
                         Total = g.Count()
                     })
                     .OrderBy(x => x.Nacionalidad)
                     .ToList();


                    // Obten NAcionalidades
                    var nacionalidadesRestringidas = grupo
                     .Where(i => i.CAT_NATIONALITIES.RESTRICTION) // filtra solo los restringidos
                     .GroupBy(i => i.CAT_NATIONALITIES.DESC_NACIONALITY_SP) // agrupar por nombre del catálogo
                     .Select(g => new RestringidosFrecuentes
                     {
                         Nacionalidad = g.Key,
                         Total = g.Count()
                     })
                     .OrderBy(x => x.Nacionalidad)
                     .ToList();


                    reporte.TotalExtranjerosInmvitados = (grupo.Count() + totalInvitados).ToString();
                    reporte.TotalCartasGeneradas = grupo.Count().ToString();
                    reporte.NacionalidadesFrecuentesList = nacionalidades;
                    reporte.RestringidosFrecuentesList = nacionalidadesRestringidas;
                    reporteList.Add(reporte);
                }
                response.error = false;
                response.message= "Consulta realizada correctamente";
                response.response = reporteList;
                return response;
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseDto(ResponseDtoEnum.Error).message + ex.Message);
            }
        }

        #endregion

        #region Obtener permisos de usuario



        [HttpGet("GetAccessByIdUser")]
        public async Task<ActionResult<ResponseDto>> GetAccessByIdUser(int id)
        {
            ResponseDto result;
            try
            {
                if (!await _appDbContext.CAT_USERS.AnyAsync(even => even.ID_USER == id))
                    return Ok(new ResponseDto(ResponseDtoEnum.NoData));

                var Usuario = await _appDbContext.CAT_USERS
                    .Where(even => even.ID_USER == id).FirstAsync();


                if (Usuario == null || Usuario.ID_ROLE == null || Usuario.ID_ROLE == 0)
                    return Ok(new ResponseDto(ResponseDtoEnum.NoData) { message = "El usuario no tiene se tiene un rol asignado" });


                int idRol = (int)Usuario.ID_ROLE;

                if (!await _appDbContext.CAT_ROLE_BY_COMPANIES.AnyAsync(even => even.ID_ROLE == idRol))
                    return Ok(new ResponseDto(ResponseDtoEnum.NoData));

                var item = await _appDbContext.CAT_ROLE_BY_COMPANIES
                    .Where((even) => even.ID_ROLE == idRol).FirstAsync();

                bool tieneAcceso = false;
                if(item.ACCESS_ANOTHER_ASSISTANTS != null)
                {
                    if(item.ACCESS_ANOTHER_ASSISTANTS == 1) 
                    {
                        tieneAcceso = true;
                    }
                }

                result = new ResponseDto(ResponseDtoEnum.Success);
                result.response = tieneAcceso;
                return Ok(result);
            }
            catch (Exception ex)
            {
                result = new ResponseDto(ResponseDtoEnum.Error);
                result.message = ex.Message;
                return BadRequest(result);
            }
        }


        #endregion


        #region Catalogo otros artistas por evento

        [HttpPost("GetArtistsByIdRegEvent")]
        public async Task<ActionResult<ResponseDto>> GetArtistsByIdEvent(int id)
        {
            /* Agregar la consulta de los registros de otros artistas en caso de que existan, esto para el nuevo modulo */
            if (!await _appDbContext.ANOTHER_ARTISTS_ADMON.AnyAsync(even => even.ID_REG == (id)))
            {
                return Ok(new ResponseDto(ResponseDtoEnum.NoData));
            }
            var item = await _appDbContext.ANOTHER_ARTISTS_ADMON.Where(even => even.ID_REG == (id)).ToListAsync();
            ResponseDto result = new ResponseDto(ResponseDtoEnum.Success);
            result.response = item;

            return Ok(result);
        }

        [HttpPost("SaveArtistForRegEvent")]
        public async Task<ActionResult> SaveArtistForEvent([FromBody] AnotherArtistDto value)
        {
            try
            {
                // En nationality llega el numero del catalogo seleccionado
                var model = _mapper.Map<AnotherArtistsAdmon>(value);
                model.CREATED_BY = 2;
                model.CREATED_DATE = DateTime.Now;
                model.ACTIVE = true;
                _appDbContext.ANOTHER_ARTISTS_ADMON.Add(model);
                var rsEvent = await _appDbContext.SaveChangesAsync();

                return Ok(new ResponseDto(ResponseDtoEnum.Success));
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseDto(ResponseDtoEnum.Error).message + ex.Message);
            }

        }


        [HttpPost("UpdateArtistForRegEvent")]
        public async Task<ActionResult> UpdateArtistForEvent([FromBody] AnotherArtistDto value)
        {
            try
            {
                AnotherArtistsAdmon itemArtist = await _appDbContext.ANOTHER_ARTISTS_ADMON
                                 .Where((even) => even.ID == value.ID).FirstAsync();
                itemArtist.PASSPORT_NAME = value.PASSPORT_NAME;

                _appDbContext.Add(itemArtist);
                var rsEvent = await _appDbContext.SaveChangesAsync();

                return Ok(new ResponseDto(ResponseDtoEnum.Success));
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseDto(ResponseDtoEnum.Error).message + ex.Message);
            }

        }

        [HttpPost("DeleteArtistForRegEvent")]
        public async Task<ActionResult> Delete([FromBody] AnotherArtistDto data)
        {
            try
            {
                int id = data.ID;
                var Event = await _appDbContext.ANOTHER_ARTISTS_ADMON.FirstOrDefaultAsync(g => g.ID_REG.Equals(id));

                if (Event == null)
                    return Ok(new ResponseDto(ResponseDtoEnum.NoData));

                _appDbContext.Remove(Event);
                await _appDbContext.SaveChangesAsync();
                return Ok(new ResponseDto(ResponseDtoEnum.Success));
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseDto(ResponseDtoEnum.Error).message + ex.Message);
            }
        }
        #endregion

    }
}
