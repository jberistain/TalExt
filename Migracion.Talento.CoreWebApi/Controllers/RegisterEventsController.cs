using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Migracion.Talento.CoreWebApi.Interfaces;
using Migracion.Talento.CoreWebApi.Services;
using Migracion.Talento.Entities.Models;
using Migracion.Talento.Models;
using Migracion.Talento.WebAPI.DataConnection;
using CommonTools.DTOs.Query;
using CommonTools.DTOs;
using CommonTools.DTOs.Register;
using CommonTools.Enums;
using static System.Net.WebRequestMethods;
using Microsoft.Extensions.Options;
using static iTextSharp.text.pdf.AcroFields;
using CommonTools.Pdf;
using System.Linq;
using Org.BouncyCastle.Crypto.IO;
using SkiaSharp;
using System.Runtime.Loader;
using CommonTools.Implementation;

namespace Migracion.Talento.CoreWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterEventsController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;
        private readonly IEmailSender _emailSender;
        private readonly ApplicationProperties _configuration;
        private readonly IMapper _mapper;
        private readonly IDocumentsEvents _documentEvents;

        public RegisterEventsController(AppDbContext appDbContext
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


        private List<string> ObtenerListaCorreosParaMandarCopia()
        {
            /* Consultar los emails que deben recibir copia */
            return _appDbContext.CAT_SEND_COPY_EMAILS
                .Where(m => m.ACTIVE == 1)
                .Select(m => m.EMAIL)
                .ToList();
        }

        [HttpPost("ListaEventos")]
        public async Task<ActionResult<ResponseDto>> Get(FilterRegisterDto? filterRegister)
        {
            try
            {
                if (!await _appDbContext.REG_EVENTS.AnyAsync(e => e.ACTIVE))
                    return Ok(new ResponseDto(ResponseDtoEnum.NoData));




                var list = await _appDbContext.REG_EVENTS
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

             List<RegEvenStateDate> ListResult = new List<RegEvenStateDate>();


                //consulta nombres eventos 
                ListResult = await _appDbContext.REG_EVENT_ESTATES_DATE
                   .Include("CAT_EVENTS")
                   .Distinct()
                   .ToListAsync();


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


                    //query by idcompany -- Se solicito cambiar empresa por los que tienen el evento seleccionado, el IdEmpresa en realidad es el IdEvento que se requiere buscar
                    if (filterRegister.IdEvento != 0)
                    {
                        var query = ListResult
                        .Where(reg => reg.ID_EVENT == filterRegister.IdEvento)
                        .ToList();
                        //lista de id de los eventos encontrados
                        var idEvents = query.Select(e => e.ID_REG).ToList();

                        //filtro por ids de tipo evento
                        list =list.Where(ev => idEvents.Contains(ev.ID_REG)).ToList();
                        if (!list.Any())
                            return Ok(new ResponseDto(ResponseDtoEnum.NoData));


                    }

                }

                //mapper de modelo de entrega
                var qry = _mapper.Map<List<QryRegEventDto>>(list);
                qry.ForEach(d=>
                {
                    //Consulta de nombres de los eventos para entrega a front en Dto
                    //d.EVENTS= _mapper.Map<List<RegEvenStateDateDto>>(ListResult.Where(ev=>ev.ID_REG.Equals(d.ID_REG)).ToList());
                    var eventos=ListResult.Where(f => f.ID_REG.Equals(d.ID_REG)).Select(f =>  f.CAT_EVENTS.DESC_EVENT_SP ).ToList();
                    eventos.ForEach(f => d.EVENTOS += f + " \n ");

                    }
                );






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
                if (!await _appDbContext.REG_EVENTS.AnyAsync(even => even.ID_REG == (id)))
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
                    .Where((even) => even.ID_REG == id).FirstOrDefaultAsync();

                var RegEventsDto = _mapper.Map<QryRegEventDto>(item);

                List< RegEvenStateDateDto> listEventsFound = null;
                if (await _appDbContext.REG_EVENT_ESTATES_DATE.AnyAsync(even => even.ID_REG == id))
                {
                    listEventsFound = new List<RegEvenStateDateDto>();

                    var itemsEvents = await _appDbContext.REG_EVENT_ESTATES_DATE
                        .Where((eventos) => eventos.ID_REG == id).ToListAsync();
                    listEventsFound = _mapper.Map<List<RegEvenStateDateDto>>(itemsEvents);

                }
                if (listEventsFound != null)
                    RegEventsDto.EVENTS = listEventsFound;

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


        [HttpPost("Register")]
        public async Task<ActionResult<ResponseDto>> RegisterEvent([FromBody] RegEventDto newEvent)
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

                if (!string.IsNullOrEmpty(newEvent.NOMBRE_NUEVO_AEROPUERTO))
                {
                    AirPortsController airportsController = new AirPortsController(this._appDbContext, this._mapper);
                    AirPortRegisterDto reg = new AirPortRegisterDto();
                    reg.DESC_AIRPORT_EN = newEvent.NOMBRE_NUEVO_AEROPUERTO;
                    reg.DESC_AIRPORT_SP = newEvent.NOMBRE_NUEVO_AEROPUERTO;
                    reg.CREATED_BY = 2;
                    var resp = await airportsController.SaveIfNotExist(reg);
                    idAeropuerto = resp.response.ID_AIRPORT;
                    newEvent.ID_AIRPORT = idAeropuerto;
                }
                if (!string.IsNullOrEmpty(newEvent.NOMBRE_NUEVA_AEROLINEA))
                {
                    AirLinesController airLineController = new AirLinesController(this._appDbContext, this._mapper);
                    AirLineRegisterDto reg = new AirLineRegisterDto();
                    reg.DESC_AIR_LINE_EN = newEvent.NOMBRE_NUEVA_AEROLINEA;
                    reg.DESC_AIR_LINE_SP = newEvent.NOMBRE_NUEVA_AEROLINEA;
                    reg.CREATED_BY = 2;
                    var resp = await airLineController.SaveIfNotExist(reg);
                    newEvent.ID_AIR_LINE = resp.response.ID_AIR_LINE;
                }


                bool antecedentesMexico = false;
                if (newEvent.CRIMINAL_RECORD_MEX != null)
                    antecedentesMexico = (bool)newEvent.CRIMINAL_RECORD_MEX;

                if (!antecedentesMexico)
                    if (newEvent.EXPELLED_MEX != null)
                        antecedentesMexico = (bool)newEvent.EXPELLED_MEX;

                if(antecedentesMexico)
                {
                    newEvent.ID_STATUS = 3;
                }

                string secredCode = RandomString(10);
                newEvent.SECRET_CODE = secredCode;
                var model= _mapper.Map<RegEvents>(newEvent);
                model.CREATED_BY = 2;
                model.CREATED_DATE= DateTime.Now;
                _appDbContext.Add(model);
                var rs = await _appDbContext.SaveChangesAsync();


                //Guardar Los eventos que llegan
                foreach(RegEvenStateDateDto currentEvent in newEvent.EVENTS)
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

                    var modelEvent = _mapper.Map<RegEvenStateDate>(currentEvent);
                    modelEvent.CREATED_BY = 2;
                    modelEvent.CREATED_DATE = DateTime.Now;
                    modelEvent.ID_REG = model.ID_REG;
                    _appDbContext.Add(modelEvent);
                    var rsEvent = await _appDbContext.SaveChangesAsync();
                }


                if (rs == 1)
                {

                    if (!await _appDbContext.CAT_EVENTS.AnyAsync(even => even.ID_EVENT == (newEvent.EVENTS[0].ID_EVENT)))
                        return Ok(new ResponseDto(ResponseDtoEnum.NoData));

                    var item = await _appDbContext.CAT_EVENTS
                        .Where((even) => even.ID_EVENT == (newEvent.EVENTS[0].ID_EVENT)).FirstOrDefaultAsync();


               

                    if (antecedentesMexico)
                    {
                       

                        WelcomeMailTemplate dataTemplate = new WelcomeMailTemplate()
                        {
                            Email = newEvent.EMAIL,
                            Name = $"{newEvent.PASSPORT_NAME} {newEvent.PASSPORT_LASTNAME}",
                            Evento = item.DESC_EVENT_SP,
                            NumSolicitud = model.ID_REG.ToString(),
                            Subject = "OCESA",
                            UrlToConfirm = _configuration.UrlToConfirm + model.ID_REG.ToString(),
                            UrlToConfirm2 = _configuration.UrlToSearchCode,
                            SecretCode = model.SECRET_CODE
                        };

                        bool esLenguajeIngles = model.LANGUAGE != null ? model.LANGUAGE.Equals("EN") : false;
                        if (esLenguajeIngles)
                        {

                            responseDto = await TemplateEmail(dataTemplate, "AvisoAntecedentesEN");
                            if (responseDto.code != (int)ResponseDtoEnum.Success)
                                return Ok(responseDto);
                        }
                        else
                        {
                            responseDto = await TemplateEmail(dataTemplate, "AvisoAntecedentesES");
                            if (responseDto.code != (int)ResponseDtoEnum.Success)
                                return Ok(responseDto);
                        }
                    }
                    else
                    {
                        WelcomeMailTemplate dataTemplate = new WelcomeMailTemplate()
                        {
                            Email = newEvent.EMAIL,
                            Name = $"{newEvent.PASSPORT_NAME} {newEvent.PASSPORT_LASTNAME}",
                            Evento = item.DESC_EVENT_SP,
                            NumSolicitud = model.ID_REG.ToString(),
                            Subject = "OCESA",
                            UrlToConfirm = _configuration.UrlToConfirm + model.ID_REG.ToString(),
                            UrlToConfirm2 = _configuration.UrlToSearchCode,
                            SecretCode = model.SECRET_CODE,
                        };



                        bool esLenguajeIngles = model.LANGUAGE != null ? model.LANGUAGE.Equals("EN") : false;
                        if (esLenguajeIngles)
                        {

                            responseDto = await TemplateEmail(dataTemplate, "Welcome");
                            if (responseDto.code != (int)ResponseDtoEnum.Success)
                                return Ok(responseDto);
                        }
                        else
                        {
                            responseDto = await TemplateEmail(dataTemplate, "WelcomeES");
                            if (responseDto.code != (int)ResponseDtoEnum.Success)
                                return Ok(responseDto);
                        }


                        

                    }

                    object requestObject = new
                    {
                        SECRET_CODE = secredCode
                    };

                

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


        [HttpPost("Update")]
        public async Task<ActionResult<ResponseDto>> UpdateEvent([FromBody] QryRegEventDto newEvent)
        {
            ResponseDto responseDto = new ResponseDto();

            try
            {
                //Eliminar eventos que referencian al registro principal
                //Guardar Los eventos que llegan
                int id;
                foreach (RegEvenStateDateDto currentEvent in newEvent.EVENTS)
                {
                    RegEvenStateDate itemEvent = null;
                    if (currentEvent.ID_REG_EVEN_DATE != null && currentEvent.ID_REG_EVEN_DATE != 0)
                    {
                        id = currentEvent.ID_REG_EVEN_DATE;
                        var regDb = await _appDbContext.REG_EVENT_ESTATES_DATE.AnyAsync(g => g.ID_REG_EVEN_DATE == id);

                        itemEvent = await _appDbContext.REG_EVENT_ESTATES_DATE
                             .Where((even) => even.ID_REG_EVEN_DATE == id).FirstOrDefaultAsync();
                        itemEvent.ID_EVENT = currentEvent.ID_EVENT;
                        itemEvent.ID_ESTATE = currentEvent.ID_ESTATE;
                        itemEvent.DESC_LOCATION = currentEvent.DESC_LOCATION;
                        itemEvent.EVENT_DATE = currentEvent.EVENT_DATE;
                        itemEvent.EVENT_DATE_FIN = currentEvent.EVENT_DATE_FIN;
                        itemEvent.MODIFY_BY = newEvent.MODIFY_BY;
                        itemEvent.MODIFY_DATE = DateTime.Now;
                        itemEvent.ACTIVE = true;
                    }
                    else
                    {
                        itemEvent = new RegEvenStateDate();
                        itemEvent.CREATED_DATE = DateTime.Now;
                        itemEvent.CREATED_BY = newEvent.MODIFY_BY;
                        itemEvent.ID_REG = newEvent.ID_REG;

                        itemEvent.ID_EVENT = currentEvent.ID_EVENT;
                        itemEvent.ID_ESTATE = currentEvent.ID_ESTATE;
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



                id = newEvent.ID_REG;
                var genderDb = await _appDbContext.REG_EVENTS.AnyAsync(g => g.ID_REG == id);
                if (!genderDb)
                    return Ok(new ResponseDto(ResponseDtoEnum.NoData));

                var item = await _appDbContext.REG_EVENTS
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


        [HttpPost("ConfirmRegisterEmail")]
        public async Task<ActionResult<ResponseDto>> ConfirmRegisterEmail([FromBody] QryRegEventDto newEvent)
        {
            ResponseDto responseDto = new ResponseDto();

            try
            {
                int idEvent = newEvent.ID_REG;

                if (!await _appDbContext.REG_EVENTS.AnyAsync(even => even.ID_REG == idEvent))
                    return Ok(new ResponseDto(ResponseDtoEnum.NoData));

                var item = await _appDbContext.REG_EVENTS
                    .Where((even) => even.ID_REG == idEvent).FirstOrDefaultAsync();


                

                item.MODIFY_BY = 2;
                item.MODIFY_DATE = DateTime.Now;
                item.ID_STATUS = 2;
                // _appDbContext.Add(item);
                var rs = await _appDbContext.SaveChangesAsync();

                responseDto.error = false;
                responseDto.response = item;
            }
            catch (Exception ex)
            {
                responseDto.error = true;
                responseDto.message = ex.Message;
            }

            return Ok(responseDto);
        }
        
        
        [HttpPost("ConfirmInvitationReceivedEmail")]
        public async Task<ActionResult<ResponseDto>> ConfirmInvitationReceivedEmail([FromBody] QryRegEventDto newEvent)
        {
            ResponseDto responseDto = new ResponseDto();

            try
            {
                int idEvent = newEvent.ID_REG;

                if (!await _appDbContext.REG_EVENTS.AnyAsync(even => even.ID_REG == idEvent))
                    return Ok(new ResponseDto(ResponseDtoEnum.NoData));

                var item = await _appDbContext.REG_EVENTS
                    .Where((even) => even.ID_REG == idEvent).FirstOrDefaultAsync();


                

                item.MODIFY_BY = 2;
                item.MODIFY_DATE = DateTime.Now;
                item.ID_STATUS = 5;
                // _appDbContext.Add(item);
                var rs = await _appDbContext.SaveChangesAsync();

                responseDto.error = false;
                responseDto.response = item;
            }
            catch (Exception ex)
            {
                responseDto.error = true;
                responseDto.message = ex.Message;
            }

            return Ok(responseDto);
        }


        // GET: RegisterEventsController
        [HttpPost]
        public async Task<ActionResult> SendEmail([FromForm] MailDataDto mailData)
        {
           mailData.Body= "<!DOCTYPE html>\r\n<html lang=\"en\" xmlns=\"http://www.w3.org/1999/xhtml\" xmlns:o=\"urn:schemas-microsoft-com:office:office\">\r\n<head>\r\n  <meta charset=\"UTF-8\">\r\n  <meta name=\"viewport\" content=\"width=device-width,initial-scale=1\">\r\n  <meta name=\"x-apple-disable-message-reformatting\">\r\n  <title></title>\r\n  <!--[if mso]>\r\n  <noscript>\r\n    <xml>\r\n      <o:OfficeDocumentSettings>\r\n        <o:PixelsPerInch>96</o:PixelsPerInch>\r\n      </o:OfficeDocumentSettings>\r\n    </xml>\r\n  </noscript>\r\n  <![endif]-->\r\n  <style>\r\n    table, td, div, h1, p {font-family: Arial, sans-serif;}\r\n  </style>\r\n</head>\r\n<body style=\"margin:0;padding:0;\">\r\n  <table role=\"presentation\" style=\"width:100%;border-collapse:collapse;border:0;border-spacing:0;background:#ffffff;\">\r\n    <tr>\r\n      <td align=\"center\" style=\"padding:0;\">\r\n        <table role=\"presentation\" style=\"width:602px;border-collapse:collapse;border:1px solid #cccccc;border-spacing:0;text-align:left;\">\r\n          <tr>\r\n            <td align=\"center\" style=\"padding:40px 0 30px 0;background:#70bbd9;\">\r\n              <img src=\"https://assets.codepen.io/210284/h1.png\" alt=\"\" width=\"300\" style=\"height:auto;display:block;\" />\r\n            </td>\r\n          </tr>\r\n          <tr>\r\n            <td style=\"padding:36px 30px 42px 30px;\">\r\n              <table role=\"presentation\" style=\"width:100%;border-collapse:collapse;border:0;border-spacing:0;\">\r\n                <tr>\r\n                  <td style=\"padding:0 0 36px 0;color:#153643;\">\r\n                    <h1 style=\"font-size:24px;margin:0 0 20px 0;font-family:Arial,sans-serif;\">Creating Email Magic</h1>\r\n                    <p style=\"margin:0 0 12px 0;font-size:16px;line-height:24px;font-family:Arial,sans-serif;\">Lorem ipsum dolor sit amet, consectetur adipiscing elit. In tempus adipiscing felis, sit amet blandit ipsum volutpat sed. Morbi porttitor, eget accumsan et dictum, nisi libero ultricies ipsum, posuere neque at erat.</p>\r\n                    <p style=\"margin:0;font-size:16px;line-height:24px;font-family:Arial,sans-serif;\"><a href=\"http://www.example.com\" style=\"color:#ee4c50;text-decoration:underline;\">In tempus felis blandit</a></p>\r\n                  </td>\r\n                </tr>\r\n                <tr>\r\n                  <td style=\"padding:0;\">\r\n                    <table role=\"presentation\" style=\"width:100%;border-collapse:collapse;border:0;border-spacing:0;\">\r\n                      <tr>\r\n                        <td style=\"width:260px;padding:0;vertical-align:top;color:#153643;\">\r\n                          <p style=\"margin:0 0 25px 0;font-size:16px;line-height:24px;font-family:Arial,sans-serif;\"><img src=\"https://assets.codepen.io/210284/left.gif\" alt=\"\" width=\"260\" style=\"height:auto;display:block;\" /></p>\r\n                          <p style=\"margin:0 0 12px 0;font-size:16px;line-height:24px;font-family:Arial,sans-serif;\">Lorem ipsum dolor sit amet, consectetur adipiscing elit. In tempus adipiscing felis, sit amet blandit ipsum volutpat sed. Morbi porttitor, eget accumsan dictum, est nisi libero ultricies ipsum, in posuere mauris neque at erat.</p>\r\n                          <p style=\"margin:0;font-size:16px;line-height:24px;font-family:Arial,sans-serif;\"><a href=\"http://www.example.com\" style=\"color:#ee4c50;text-decoration:underline;\">Blandit ipsum volutpat sed</a></p>\r\n                        </td>\r\n                        <td style=\"width:20px;padding:0;font-size:0;line-height:0;\">&nbsp;</td>\r\n                        <td style=\"width:260px;padding:0;vertical-align:top;color:#153643;\">\r\n                          <p style=\"margin:0 0 25px 0;font-size:16px;line-height:24px;font-family:Arial,sans-serif;\"><img src=\"https://assets.codepen.io/210284/right.gif\" alt=\"\" width=\"260\" style=\"height:auto;display:block;\" /></p>\r\n                          <p style=\"margin:0 0 12px 0;font-size:16px;line-height:24px;font-family:Arial,sans-serif;\">Morbi porttitor, eget est accumsan dictum, nisi libero ultricies ipsum, in posuere mauris neque at erat. Lorem ipsum dolor sit amet, consectetur adipiscing elit. In tempus adipiscing felis, sit amet blandit ipsum volutpat sed.</p>\r\n                          <p style=\"margin:0;font-size:16px;line-height:24px;font-family:Arial,sans-serif;\"><a href=\"http://www.example.com\" style=\"color:#ee4c50;text-decoration:underline;\">In tempus felis blandit</a></p>\r\n                        </td>\r\n                      </tr>\r\n                    </table>\r\n                  </td>\r\n                </tr>\r\n              </table>\r\n            </td>\r\n          </tr>\r\n          <tr>\r\n            <td style=\"padding:30px;background:#ee4c50;\">\r\n              <table role=\"presentation\" style=\"width:100%;border-collapse:collapse;border:0;border-spacing:0;font-size:9px;font-family:Arial,sans-serif;\">\r\n                <tr>\r\n                  <td style=\"padding:0;width:50%;\" align=\"left\">\r\n                    <p style=\"margin:0;font-size:14px;line-height:16px;font-family:Arial,sans-serif;color:#ffffff;\">\r\n                      &reg; Someone, Somewhere 2021<br/><a href=\"http://www.example.com\" style=\"color:#ffffff;text-decoration:underline;\">Unsubscribe</a>\r\n                    </p>\r\n                  </td>\r\n                  <td style=\"padding:0;width:50%;\" align=\"right\">\r\n                    <table role=\"presentation\" style=\"border-collapse:collapse;border:0;border-spacing:0;\">\r\n                      <tr>\r\n                        <td style=\"padding:0 0 0 10px;width:38px;\">\r\n                          <a href=\"http://www.twitter.com/\" style=\"color:#ffffff;\"><img src=\"https://assets.codepen.io/210284/tw_1.png\" alt=\"Twitter\" width=\"38\" style=\"height:auto;display:block;border:0;\" /></a>\r\n                        </td>\r\n                        <td style=\"padding:0 0 0 10px;width:38px;\">\r\n                          <a href=\"http://www.facebook.com/\" style=\"color:#ffffff;\"><img src=\"https://assets.codepen.io/210284/fb_1.png\" alt=\"Facebook\" width=\"38\" style=\"height:auto;display:block;border:0;\" /></a>\r\n                        </td>\r\n                      </tr>\r\n                    </table>\r\n                  </td>\r\n                </tr>\r\n              </table>\r\n            </td>\r\n          </tr>\r\n        </table>\r\n      </td>\r\n    </tr>\r\n  </table>\r\n</body>\r\n</html>";
            await _emailSender.SendEmailAsync(mailData);

            return Ok();

        }

        [HttpPost("Sendtemplate")]
        private async Task<ResponseDto> Template(WelcomeMailTemplate welcomeMail)
        {
            try
            {
                // Create MailData object
                MailDataDto mailData = new MailDataDto()
                {
                    To = welcomeMail.Email,
                    Subject = welcomeMail.Subject,
                    Body = _emailSender.GetWelcomeTemplateEmail("Welcome", welcomeMail),
                    ListaEmailsCC = ObtenerListaCorreosParaMandarCopia()
                };
                
                await _emailSender.SendEmailAsync(mailData);



                return new ResponseDto(ResponseDtoEnum.Success);
            }catch(Exception ex)
            {
                var error= new ResponseDto(ResponseDtoEnum.Error);
                error.message += ex.Message;
                return error;
            }
            

        }



        private async Task<ResponseDto> TemplateEmail(WelcomeMailTemplate welcomeMail, string nameTemplate)
        {
            try
            {
                // Create MailData object
                MailDataDto mailData = new MailDataDto()
                {
                    To = welcomeMail.Email,
                    Subject = welcomeMail.Subject,
                    Body = _emailSender.GetWelcomeTemplateEmail(nameTemplate, welcomeMail),
                    ListaEmailsCC = ObtenerListaCorreosParaMandarCopia()
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




        [HttpPost("Attachment")]
        public async Task<ActionResult> SendAttachment([FromForm] MailDataDto mailData)
        {

            return Ok();
        }


        [HttpPost("SendInvitation")]
        public async Task<ActionResult<ResponseDto>> SendInvitation([FromBody] List<int> idsEventos)
        {
            ResponseDto responseDto = new ResponseDto();
            try
            {
                string mensajeRespuesta = "";
                foreach (int id in idsEventos)
                {
                    int idEvent = id;

                    if (!await _appDbContext.REG_EVENTS.AnyAsync(even => even.ID_REG == idEvent))
                        return Ok(new ResponseDto(ResponseDtoEnum.NoData));

                    var item = await _appDbContext.REG_EVENTS
                        .Include("CAT_NATIONALITIES")
                        .Where((even) => even.ID_REG == idEvent).FirstOrDefaultAsync();

                    try
                    {
                        bool mandarCorreo = true;
                        if (item.ID_STATUS == 3 && item.CHECK_VERIFY == false)
                        {
                            mandarCorreo = false;
                        }
                        if (mandarCorreo)
                        {
                            bool esPaisRestringido = true;

                            if (await _appDbContext.CAT_NATIONALITIES.AnyAsync(even => even.ID_NATIONALITY == item.ID_NATIONALITY))
                            {
                                var nacionalidadCat = await _appDbContext.CAT_NATIONALITIES
                                .Where((even) => even.ID_NATIONALITY == item.ID_NATIONALITY).SingleOrDefaultAsync();

                                if (!nacionalidadCat.RESTRICTION)
                                    esPaisRestringido = false;
                            }



                            //incluye nuevos documentos pdf
                            var queryDoctos = await _documentEvents.GetAllDocsBySecretCode(item.SECRET_CODE);
                            var doctos = (List<InviteDto>)queryDoctos.response;
                            var pdfdoctos = (List<DocumentDto>)queryDoctos.secondResponse;

                            RegInviteController regInvite = new RegInviteController(_appDbContext, _mapper);
                            var queryDocumentosRestriccion = await regInvite.GetAllDocsByRestrictedFlag(esPaisRestringido);
                            var doctosRestriccionPais = (List<InviteDto>)queryDocumentosRestriccion.response;

                            if (doctos != null && doctos.Count > 0)
                            {

                                string tipoEventoStr = string.Empty;
                                if (doctos[0].ID_EVENT_TYPE != null)
                                {
                                    if (await _appDbContext.CAT_EVENT_TYPES.AnyAsync(even => even.ID_EVENT_TYPE == doctos[0].ID_EVENT_TYPE))
                                    {
                                        var PrimerTipoEvento = await _appDbContext.CAT_EVENT_TYPES
                                            .Where((even) => even.ID_EVENT_TYPE == doctos[0].ID_EVENT_TYPE).FirstOrDefaultAsync();
                                        if (PrimerTipoEvento != null)
                                            tipoEventoStr = PrimerTipoEvento.DESC_ACTIVITY_SP;
                                    }
                                }


                                IReporteInfo reporteInfo = new ReportInformation()
                                {
                                    NombreInvitado = $"{item.PASSPORT_NAME} {item.PASSPORT_LASTNAME}",
                                    FechaEntradaAlPais = item.DATE_ARRIVE.Value.ToShortDateString(),
                                    FechaSalidaAlPais = item.DATE_LEAVE.Value.ToShortDateString(),
                                    Nacionalidad = item.CAT_NATIONALITIES.DESC_NACIONALITY_SP,
                                    NumPasaporte = item.PASSPORT_NUM,
                                    PuestoParteStaff = item.EVENT_JOB,
                                    TipoArchivoGenerado = tipoEventoStr
                                };


                                List<InfoEventoModel> EventList = new List<InfoEventoModel>();
                                //BUSCAR LOS EVENTOS REGISTRADOS
                                if (await _appDbContext.REG_EVENT_ESTATES_DATE.AnyAsync(even => even.ID_REG == idEvent))
                                {

                                    var RegEventsStatesList = await _appDbContext.REG_EVENT_ESTATES_DATE
                                        .Include("CAT_EVENTS")
                                        .Include("CAT_ESTATES")
                                        .Where((even) => even.ID_REG == idEvent).ToListAsync();
                                    foreach (var currentEventEstate in RegEventsStatesList)
                                    {
                                        EventList.Add(new InfoEventoModel()
                                        {
                                            FechaEvento = currentEventEstate.EVENT_DATE.ToShortDateString(),
                                            InmuebleEvento = currentEventEstate.CAT_ESTATES.DESC_ESTATE_SP,
                                            NombreEvento = currentEventEstate.CAT_EVENTS.DESC_EVENT_SP,
                                            UbicacionInmueble = currentEventEstate.DESC_LOCATION
                                        });
                                    }
                                }
                                reporteInfo.InfoEventosList = EventList.ToList<IInfoEvento>();

                                List<AttachmentFileDto> attachments = new List<AttachmentFileDto>();

                                //Genera Carta invitacion

                                doctos.ForEach(docto =>
                                {
                                    PdfManager reporte = new PdfManager();
                                    var invitacion = reporte.GenerateDocument(docto, reporteInfo);
                                    attachments.Add(invitacion);

                                });

                                doctosRestriccionPais.ForEach(docto =>
                                {
                                    PdfManager reporte = new PdfManager();
                                    var invitacion = reporte.GenerateDocument(docto, reporteInfo);
                                    attachments.Add(invitacion);
                                });

                                //documentos pdf
                                if (pdfdoctos != null)
                                    pdfdoctos.ForEach(pdf =>
                                    {

                                        var attachment = new AttachmentFileDto
                                        {
                                            File = Convert.FromBase64String(pdf.FILE_BLOB),
                                            FileName = pdf.DESC_SPANISH
                                        };
                                        attachments.Add(attachment);
                                    });


                                InvitationMailTemplate dataTemplate = new InvitationMailTemplate()
                                {
                                    Email = item.EMAIL,
                                    Name = $"{item.PASSPORT_NAME} {item.PASSPORT_LASTNAME}",
                                    Evento = item.ID_ACTIVITY.ToString(),
                                    NumSolicitud = item.SECRET_CODE,
                                    Subject = "OCESA",
                                    UrlToConfirm = _configuration.UrlToConfirmReceiveDocumentation + item.ID_REG.ToString(),
                                    attachments = attachments
                                };

                                // En caso de que se tenga expulsion del pais o antecedentes en el pais (mexico) no se deben generar cargas de ingreso, no se manda correo
                                if ((item.CRIMINAL_RECORD_MEX != null
                                    && item.EXPELLED_MEX != null
                                    && (!(bool)item.CRIMINAL_RECORD_MEX
                                    && !(bool)item.EXPELLED_MEX)) || item.CHECK_VERIFY == true)
                                {
                                    bool esLenguajeIngles = item.LANGUAGE != null ? item.LANGUAGE.Equals("EN") : false;
                                    if (!esPaisRestringido)
                                    {
                                        if (esLenguajeIngles)
                                        {
                                            responseDto = await Template(TemplateTypeEnum.NationalityNotRestrictedEN, dataTemplate);
                                        }
                                        else
                                        {
                                            responseDto = await Template(TemplateTypeEnum.NationalityNotRestrictedES, dataTemplate);
                                        }
                                    }
                                    else
                                    {
                                        if (esLenguajeIngles)
                                        {
                                            responseDto = await Template(TemplateTypeEnum.NationalityRestrictedEN, dataTemplate);
                                        }
                                        else
                                        {
                                            responseDto = await Template(TemplateTypeEnum.NationalityRestrictedES, dataTemplate);
                                        }

                                    }
                                    

                                }

                            }

                            item.ID_STATUS = 4;
                            //Actualizar el estatus del registro al siguiente paso 
                            var rs = await _appDbContext.SaveChangesAsync();
                            if (responseDto.code == 200)
                            {
                                responseDto.error = false;
                                responseDto.response = item;
                                mensajeRespuesta += $"<br>El correo del registro {id} con Código {item.SECRET_CODE} se mandó correctamente.";
                            }
                            else
                            {
                                mensajeRespuesta += $"<br>El correo del registro {id} con Código {item.SECRET_CODE} NO logró mandar correctamente. Por favor vuelva a intentarlo.";
                            }

                        }
                        else
                        {
                            mensajeRespuesta += $"<br>El correo del registro {id} con Código {item.SECRET_CODE} no se mandó ya que tiene antecedentes en México.";
                        }
                    }
                    catch(Exception e)
                    {
                        mensajeRespuesta += $"<br>El correo del registro {id} con Código {item.SECRET_CODE} no se mandó ya que hubo un error al procesar el correo: {e.Message}";
                    }
                }
                //desconectar envio de smtp
                await _emailSender.DisconnectSmtpClient();

                if (responseDto.code == (int)ResponseDtoEnum.Success)
                {
                    responseDto.message = mensajeRespuesta;
                    return Ok(responseDto);
                }
                else
                {
                    responseDto.message = mensajeRespuesta;
                    return Ok(responseDto);
                }

            }
            catch (Exception ex)
            {
                responseDto.error = true;
                responseDto.message = ex.Message;
            }

            return Ok(responseDto);
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
                    ListaEmailsCC = ObtenerListaCorreosParaMandarCopia()

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


    }
}
