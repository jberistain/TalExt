using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using BaseMVC5Project.Models.Utils;
using MigracionTalentoExtranjero.Models.Enum;
using MigracionTalentoExtranjero.Models.Home;
using MigracionTalentoExtranjero.Models.Utils;
using CommonTools.Pdf;
using System.Net.NetworkInformation;
using System.Web.Helpers;
using CommonTools.Models;
using MigracionTalentoExtranjero.Models;
using iTextSharp.text.pdf;
using CommonTools.DTOs;
using CommonTools.DTOs.Query;
using CommonTools.DTOs.Register;

namespace MigracionTalentoExtranjero.Controllers
{
    public class HomeController : Controller
    {
        private RegistroInvitadoModel registroInvitado;
        private ComboBoxHelper CB = new ComboBoxHelper();
        private ResponseModel responseObject;
        private HttpManager httpManager = new HttpManager(Constants.WebAPIUrl);

        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";
            return View();
        }

        public ActionResult Tutorial()
        {
            return View();
        }



        public ActionResult PassportSearch(string language)
        {
            registroInvitado = new RegistroInvitadoModel();

            ViewBag.Title = "Búsqueda de Pasaporte";
            if (!string.IsNullOrEmpty(language))
                ViewBag.Lenguaje = registroInvitado.ValidateLanguage(language);
            else
                ViewBag.Lenguaje = "ES";
            return View();
        }


        public async Task<ActionResult> InvitationConfirmed(string id = "")
        {
            CRUDManager crud = new CRUDManager(httpManager);


            if (!string.IsNullOrEmpty(id))
            {
                int idInt = Convert.ToInt32(id);
                var registroEncontrado = await crud.ConfirmaCorreoInvitacionPorId(idInt);

                ViewBag.NombreCompleto = $"{registroEncontrado.Nombre} {registroEncontrado.Apellidos}";
            }
            else
                ViewBag.NombreCompleto = "NOT FOUND";
            return View();
        }

        public async Task<ActionResult> EmailConfirmed(string id = "")
        {
            CRUDManager crud = new CRUDManager(httpManager);


            if (!string.IsNullOrEmpty(id))
            {
                int idInt = Convert.ToInt32(id);
                var registroEncontrado = await crud.ConfirmaCorreoRegistroPorId(idInt);

                ViewBag.NombreCompleto = $"{registroEncontrado.Nombre} {registroEncontrado.Apellidos}";
                ViewBag.Codigo = $"{registroEncontrado.CodigoSecreto}";
            }
            else
            {
                ViewBag.NombreCompleto = "NOT FOUND";
                ViewBag.Codigo = $"NOT FOUND";

            }
            return View();
        }



        public ActionResult VisitorSearch()
        {

            return View();
        }

        #region Registro de Invitados

        public async Task<ActionResult> RegistroInvitado(string language, RegistroInvitadoModel modelFound)
        {
            try
            {
                if (modelFound == null)
                    registroInvitado = new RegistroInvitadoModel();
                else
                {
                    registroInvitado = modelFound;
                    if (registroInvitado.Eventos == null)
                        registroInvitado.Eventos = new List<InfoEventoModel>();

                    if (registroInvitado.Eventos.Count == 0)
                        registroInvitado.Eventos.Add(new InfoEventoModel());
                }
                ViewBag.Title = "Formulario de Registro";
                if (!string.IsNullOrEmpty(language))
                {
                    ViewBag.Lenguaje = registroInvitado.ValidateLanguage(language);
                    registroInvitado.IdiomaActual = language;
                }
                else
                {
                    ViewBag.Lenguaje = "ES";
                    registroInvitado.IdiomaActual = "ES";
                    language = "ES";
                }

                var DiaList = await CB.GetSearchComboBox(CatalogosEnum.DIA.GetString(), 0, "");
                ViewBag.DiasList = DiaList;
                var MesList = await CB.GetSearchComboBox(CatalogosEnum.MES.GetString(), 0, "");
                ViewBag.MesList = MesList;
                var AniosList = await CB.GetSearchComboBox(CatalogosEnum.ANIO.GetString(), 0, "");
                ViewBag.AniosList = AniosList;
                var AniosFuturoList = await CB.GetSearchComboBox(CatalogosEnum.CAT_10ANIO_FUTURO.GetString(), 0, "");
                ViewBag.AniosFuturoList = AniosFuturoList;
                var AniosPasadoList = await CB.GetSearchComboBox(CatalogosEnum.CAT_10ANIO_PASADO.GetString(), 0, "");
                ViewBag.AniosPasadoList = AniosPasadoList;

                var aerolineasList = await CB.GetSearchComboBox(CatalogosEnum.CAT_AEROLINEAS.GetString(), 0, "");
                aerolineasList.Add(new SelectListItem() { Text = "OTRA", Value = "OTRA" });
                ViewBag.AerolineasList = aerolineasList;
                if (language.Equals("ES"))
                {
                    ViewBag.GenerosList = await CB.GetSearchComboBox(CatalogosEnum.CAT_GENEROS.GetString(), 0, "");
                }
                else
                {
                    ViewBag.GenerosList = await CB.GetSearchComboBox(CatalogosEnum.CAT_GENEROS_EN.GetString(), 0, "");
                }
                var InmueblesList = await CB.GetSearchComboBox(CatalogosEnum.CAT_INMUEBLES.GetString(), 0, "");
                InmueblesList.Add(new SelectListItem() { Text = "OTRA", Value = "OTRA" });
                ViewBag.InmueblesList = InmueblesList;
                var EventosList = await CB.GetSearchComboBox(CatalogosEnum.CAT_EVENTOS.GetString(), 0, "");
                ViewBag.EventosList = EventosList;


                var ListaAeropuertos = await CB.GetSearchComboBox(CatalogosEnum.CAT_AEROPUERTOS.GetString(), 0, "");
                ListaAeropuertos.Add(new SelectListItem() { Text = "OTRA", Value = "OTRA" });
                ViewBag.AeropuertosList = ListaAeropuertos;
                //ViewBag.NacionalidadesList = await CB.GetSearchComboBox(CatalogosEnum.CAT_NACIONALIDADES.GetString(), 0, "");
                
                //var nacionalidadesList = await CB.GetSearchComboBox(CatalogosEnum.CAT_NACIONALIDADES.GetString(), 0, "");
                CRUDManager crud = new CRUDManager(httpManager);

                var ListaNacionalidades = await crud.DescargaCatalogosNacionalidad();
                
                string htmlNacionalidades = "<select class=\"form-control\" id=\"Nacionalidad\" name=\"Nacionalidad\" required=\"required\" style=\"position:initial\">" +
                    "<option value=\"\"></option>\r\n" 
                    ;
                string nacionalidadExistente = "";
                if(!string.IsNullOrEmpty(modelFound.Nacionalidad))
                {
                    nacionalidadExistente = modelFound.Nacionalidad;
                }
                for(int i=0; i< ListaNacionalidades.Count; i++)
                {
                    string opcionSeleccionada = "";
                    if (!string.IsNullOrEmpty(nacionalidadExistente))
                        if (ListaNacionalidades[i].Id.ToString().Equals(nacionalidadExistente))
                            opcionSeleccionada = "selected=\"selected\"";
                    if (ListaNacionalidades[i].AtributoAdicionalStr1.Equals("SI"))
                    {
                        htmlNacionalidades += $"<option {opcionSeleccionada} style=\"color:red;\" value=\"{ListaNacionalidades[i].Id}\">{ListaNacionalidades[i].Descripcion}</option>";
                    }
                    else
                    {
                        htmlNacionalidades += $"<option {opcionSeleccionada} style value=\"{ListaNacionalidades[i].Id}\">{ListaNacionalidades[i].Descripcion}</option>";
                    }
                }

                htmlNacionalidades += "</select>";
                ViewBag.HTMLNacionalidades = htmlNacionalidades;


                var listaCompanias = await CB.GetSearchComboBox(CatalogosEnum.CAT_COMPANIAS.GetString(), 0, "");
                listaCompanias.Add(new SelectListItem() { Text="OTRA", Value="OTRA"});
                ViewBag.CompaniasList = listaCompanias;
                ViewBag.PaisesList = await CB.GetSearchComboBox(CatalogosEnum.CAT_PAISES.GetString(), 0, "");


                List<SelectListItem> ExpulsadoSiNoList;
                List<SelectListItem> AntecedentesSiNoList;

                if (registroInvitado.IdiomaActual.Equals("ES"))
                {
                    ExpulsadoSiNoList = await CB.GetSearchComboBox(CatalogosEnum.CAT_SI_NO.GetString(), 0, "");
                    AntecedentesSiNoList = await CB.GetSearchComboBox(CatalogosEnum.CAT_SI_NO.GetString(), 0, "");
                }
                else
                {
                    ExpulsadoSiNoList = await CB.GetSearchComboBox(CatalogosEnum.CAT_YES_NO.GetString(), 0, "");
                    AntecedentesSiNoList = await CB.GetSearchComboBox(CatalogosEnum.CAT_YES_NO.GetString(), 0, "");
                }

                ViewBag.ExpulsadoSiNoList = ExpulsadoSiNoList;
                ViewBag.AntecedentesSiNoList = AntecedentesSiNoList;

               

                string opcionesDeComboEventosString = "";
                string opcionActual = "";
                //Generar catalogos con "options"
                foreach(SelectListItem item in EventosList)
                {
                    opcionActual = $"<option value=\"{item.Value}\">{item.Text}</option>";
                    opcionesDeComboEventosString += opcionActual;
                }
                ViewBag.EventosOptionsHTML = opcionesDeComboEventosString;

                string opcionesdeComboInmueblesString = "";
                foreach (SelectListItem item in InmueblesList)
                {
                    opcionActual = $"<option value=\"{item.Value}\">{item.Text}</option>";
                    opcionesdeComboInmueblesString += opcionActual;
                }
                ViewBag.InmueblesOptionsHTML = opcionesdeComboInmueblesString;

                string opcionesdeComboDiaString = "";
                foreach (SelectListItem item in DiaList)
                {
                    opcionActual = $"<option value=\"{item.Value}\">{item.Text}</option>";
                    opcionesdeComboDiaString += opcionActual;
                }
                ViewBag.DiasOptionsHTML = opcionesdeComboDiaString;

                string opcionesdeComboMesesString = "";
                foreach (SelectListItem item in MesList)
                {
                    opcionActual = $"<option value=\"{item.Value}\">{item.Text}</option>";
                    opcionesdeComboMesesString += opcionActual;
                }
                ViewBag.MesOptionsHTML = opcionesdeComboMesesString;

                string opcionesdeComboAniosString = "";
                foreach (SelectListItem item in AniosList)
                {
                    opcionActual = $"<option value=\"{item.Value}\">{item.Text}</option>";
                    opcionesdeComboAniosString += opcionActual;
                }
                ViewBag.AniosOptionsHTML = opcionesdeComboAniosString;


                //Se agrega seccion para descargar el aviso de privacidad a mostrar
                string avisoActual = "";
                var response = await httpManager.GetAsJsonAsync<CommonTools.DTOs.Query.ResponseDto>(WebAPIEndPointsEnum.CONSULTA_AVISO_PRIVACIDAD.GetString());
                if(response.code == 200)
                {
                    if(registroInvitado.IdiomaActual.Equals("ES"))
                    {
                        avisoActual = response.response.avisO_ESP;
                    }
                    else
                    {
                        avisoActual = response.response.avisO_ENG;
                    }
                }

                ViewBag.AvisoPrivacidadActual = avisoActual;

            }
            catch (Exception e)
            { 
                Console.WriteLine(e.ToString());
            }
            return View(registroInvitado);

        }

        

        public async Task<JsonResult> GuardaRegistroInvidato(RegistroInvitadoModel model)
        {
            responseObject = new ResponseModel();
            try
            {

                // Se validan los campos requeridos del modelo entrante
                string camposRequeridosVacios = model.ValidaCamposRequeridos();
                if (!string.IsNullOrEmpty(camposRequeridosVacios))
                {
                    responseObject.message = "Debe llenar los campos requeridos: " + camposRequeridosVacios;
                    responseObject.response = false;
                }
                else
                {
                    bool expulsadoMexico = false;
                    if (!string.IsNullOrEmpty(model.FueExpulsadoDeMexico))
                        expulsadoMexico = (model.FueExpulsadoDeMexico.Equals("SI") || model.FueExpulsadoDeMexico.Equals("YES")) ? true : false;

                    bool antecedentesEnMexico = false;
                    if (!string.IsNullOrEmpty(model.AntecedentesPenalesEnMexico))
                        antecedentesEnMexico = (model.AntecedentesPenalesEnMexico.Equals("SI") || model.AntecedentesPenalesEnMexico.Equals("YES")) ? true : false;
                    List<object> Events = null;
                    if (model.Eventos != null && model.Eventos.Count > 0)
                    {
                        Events = new List<object>();
                        foreach (InfoEventoModel currentEvent in model.Eventos)
                        {
                            if (!string.IsNullOrEmpty(currentEvent.AnioInicioEvento) &&
                                !string.IsNullOrEmpty(currentEvent.MesInicioEvento) &&
                                !string.IsNullOrEmpty(currentEvent.DiaInicioEvento) &&
                                !string.IsNullOrEmpty(currentEvent.AnioFinEvento) &&
                                !string.IsNullOrEmpty(currentEvent.MesFinEvento) &&
                                !string.IsNullOrEmpty(currentEvent.DiaFinEvento) &&
                                !string.IsNullOrEmpty(currentEvent.NombreEvento) &&
                                !string.IsNullOrEmpty(currentEvent.InmuebleEvento) &&
                                !string.IsNullOrEmpty(currentEvent.UbicacionInmueble)
                                )
                            {
                                DateTime fechaInicio = new DateTime(Convert.ToInt32(currentEvent.AnioInicioEvento), Convert.ToInt32(currentEvent.MesInicioEvento), Convert.ToInt32(currentEvent.DiaInicioEvento));
                                DateTime fechaFin = new DateTime(Convert.ToInt32(currentEvent.AnioFinEvento), Convert.ToInt32(currentEvent.MesFinEvento), Convert.ToInt32(currentEvent.DiaFinEvento));
                                Events.Add(new
                                {
                                    ID_EVENT = currentEvent.NombreEvento,//Se deja este nombre para enviar el ID del combobox
                                    ID_ESTATE = currentEvent.InmuebleEvento.Equals("OTRA") ? "0" : currentEvent.InmuebleEvento,
                                    DESC_LOCATION = currentEvent.UbicacionInmueble,
                                    EVENT_DATE = fechaInicio,
                                    EVENT_DATE_FIN = fechaFin,
                                    NOMBRE_NUEVO_INMUEBLE = currentEvent.NuevoInmuebleEvento
                                });
                            }
                        }

                    }

                    DateTime fechaInicioVigenciaPasaporte = new DateTime(Convert.ToInt32(model.AnioExpPas), Convert.ToInt32(model.MesExpPas), Convert.ToInt32(model.DiaExpPas));
                    DateTime fechaExpiracionPasaporte = new DateTime(Convert.ToInt32(model.AnioVenPas), Convert.ToInt32(model.MesVenPas), Convert.ToInt32(model.DiaVenPas));

                    DateTime fechaLlegada = new DateTime(Convert.ToInt32(model.AnioEntrada), Convert.ToInt32(model.MesEntrada), Convert.ToInt32(model.DiaEntrada));
                    DateTime fechaSalida = new DateTime(Convert.ToInt32(model.AnioSalida), Convert.ToInt32(model.MesSalida), Convert.ToInt32(model.DiaSalida));

                    if (string.IsNullOrEmpty(model.AeropuertoLlegada))
                        model.AeropuertoLlegada = "0";
                    
                    if (string.IsNullOrEmpty(model.Aerolinea))
                        model.Aerolinea = "0";

                    //Consumir servicio de guardado
                    object requestObject = new
                    {
                        ID_REG_EVEN_DATE = 1,
                        ID_GENDER = model.Sexo,
                        ID_COUNTRY = model.PaisNacimiento,
                        ID_NATIONALITY = model.Nacionalidad,
                        ID_ACTIVITY = 1,
                        ID_AIRPORT = model.AeropuertoLlegada.Equals("OTRA") ? "0" : model.AeropuertoLlegada,
                        ID_AIR_LINE = model.Aerolinea.Equals("OTRA") ? "0" : model.Aerolinea,
                        ID_PROCESS = 1,
                        ID_STATUS = 1,
                        ID_COMPANY = model.Empresa.Equals("OTRA") ? "0" : model.Empresa,
                        PASSPORT_NUM = model.NumeroPasaporte,
                        DATE_VIG_INI = fechaInicioVigenciaPasaporte,
                        DATE_VIG_FIN = fechaExpiracionPasaporte,
                        PASSPORT_NAME = model.Nombre,
                        PASSPORT_LASTNAME = model.Apellidos,
                        EMAIL = model.Correo,
                        ACTUAL_JOB = model.ActividadEnMexico,
                        EXPELLED_MEX = expulsadoMexico,
                        EXPELLED_MEX_DESC = model.ExplicacionAntecedentesExpulsion,
                        CRIMINAL_RECORD_MEX = antecedentesEnMexico,
                        EVENT_JOB = "",
                        DATE_EVENT_INI = DateTime.Now,
                        DATE_EVENT_FIN = DateTime.Now,
                        FLIGHT = model.NumeroVuelo,
                        FLIGHT_NUMBER = model.NumeroVuelo,
                        SECRET_CODE = "",
                        ACTIVITY_COUNTRY = model.ActividadPaisResidencia,
                        ACTIVITY_MEXICO = model.ActividadEnMexico,
                        DATE_ARRIVE = fechaLlegada,
                        DATE_LEAVE = fechaSalida,
                        CREATED_BY = 2,
                        NOMBRE_NUEVA_EMPRESA = model.NuevaEmpresa,
                        NOMBRE_NUEVO_AEROPUERTO = model.NuevoAeropuerto,
                        NOMBRE_NUEVA_AEROLINEA = model.NuevaAerolinea,
                        LANGUAGE = model.IdiomaActual,
                        Events = Events
                    };

                    var response = await httpManager.PostAsJsonAsync<Object, CommonTools.DTOs.Query.ResponseDto>(requestObject, WebAPIEndPointsEnum.CREATE_NEW_REGISTER.GetString());

                    if (response != null)
                    {
                        if (response.code == 200)
                        {
                            //Setear respuesta
                            responseObject.response = true;
                            responseObject.message = "Se guardó con éxito el registro.";
                            responseObject.href = Url.Content("~/Home/ResultadoRegistro");
                            responseObject.result = new ResultadoRegistroModel()
                            {
                                NombreCompleto = $"{model.Nombre} {model.Apellidos}",
                                Codigo = response.response.secreT_CODE,
                                Url = ""
                            };
                        }
                        else
                        {
                            responseObject.message = response.message;
                            responseObject.response = false;
                        }
                    }

                }
            }
            catch(Exception ex)
            {
                responseObject.response=false;
                responseObject.message=ex.Message;
            }
            
            return Json(responseObject);
        }

        public ActionResult ResultadoRegistro(ResultadoRegistroModel model)
        {
            ViewBag.Title = "Your request has been registered / Tu solicitud ha sido registrada";

            model.Url = "http://ocesarrsads.com";


            return View(model);
        }



        //Busqueda de registro en pantalla de visitante
        public async Task<JsonResult> BuscaRegistro(string codigo)
        {
            responseObject= new ResponseModel();
            try
            {
                responseObject.response = false;
                responseObject.message = $"The code {codigo} is incorrect / El código {codigo} es incorrecto";


                CRUDManager crud = new CRUDManager(httpManager);

                var resultadoConsulta = await crud.DescargarRegistroPorCodigoSecreto(codigo);


                if (resultadoConsulta != null)
                {
                    if (resultadoConsulta.IdStatusActual >= 4)
                    {
                        var documentosList = await crud.DescargarDocumentosPorCodigoSecreto(codigo);

                        var catNacionalidad = await crud.DescargaCatalogoNacionalidadPorId(Convert.ToInt32(resultadoConsulta.Nacionalidad));

                        bool paisRestringido = catNacionalidad.response.restriction;
                        var documentosTipoPais = await crud.GetAllDocsByRestrictedFlag(paisRestringido);

                        var DocumentosPDF = await crud.DescargarDocumentosPDFPorCodigoSecreto(codigo);

                        var resultObject = new VisitorSearchModel()
                        {
                            Mensaje = $"The code \"{resultadoConsulta.CodigoSecreto}\" is correct / El código \"{codigo}\" es correcto",
                            Codigo = resultadoConsulta.CodigoSecreto,
                            Nombre = $"{resultadoConsulta.Nombre} {resultadoConsulta.Apellidos}",
                            Pais = $"{resultadoConsulta.PaisNacimientoDesc}",
                            IdReg = resultadoConsulta.Id.ToString()
                        };

                        foreach (var documento in documentosList)
                        {
                            Dictionary<string, string> elemento = new Dictionary<string, string>();
                            elemento.Add("NombreArchivo", documento.FILE_NAME);
                            elemento.Add("IdentificadorDocumento", documento.ID_INVITE.ToString());
                            elemento.Add("TipoArchivo", "INVITACION");
                            resultObject.ListaBotonesDocumentos.Add(elemento);
                        }

                        foreach (var documento in documentosTipoPais)
                        {
                            Dictionary<string, string> elemento = new Dictionary<string, string>();
                            elemento.Add("NombreArchivo", documento.FILE_NAME);
                            elemento.Add("IdentificadorDocumento", documento.ID_INVITE.ToString());
                            elemento.Add("TipoArchivo", "INVITACION");
                            resultObject.ListaBotonesDocumentos.Add(elemento);
                        }

                        foreach (var documento in DocumentosPDF)
                        {
                            Dictionary<string, string> elemento = new Dictionary<string, string>();
                            elemento.Add("NombreArchivo", documento.DESC_SPANISH);
                            elemento.Add("IdentificadorDocumento", documento.ID_INVITE.ToString());
                            elemento.Add("TipoArchivo", "PDF");
                            resultObject.ListaBotonesDocumentos.Add(elemento);
                        }



                        responseObject.result = resultObject;


                        responseObject.response = true;
                    }
                    else
                    {
                        responseObject.response = false;
                        responseObject.message = "El registro se encuentra en proceso de validación aún  / the registration is still in the validation process";
                    }
                }

            }
            catch (Exception ex)
            {
                responseObject.response=false;
                responseObject.message=ex.Message;
            }
            return Json(responseObject);
        }
         public async Task<JsonResult> BuscarPasaporte(string pasaporte)
        {
            responseObject= new ResponseModel();
            responseObject.response = false;

            try
            {


                Dictionary<string, string> requestParams = new Dictionary<string, string>();
                requestParams.Add("passportNumber", pasaporte);

                var response = await httpManager.GetAsJsonAsync<CommonTools.DTOs.Query.ResponseDto>(WebAPIEndPointsEnum.CONSULTA_REGISTRO_POR_PASAPORTE.GetString(), requestParams);

                if (response.code == 200)
                {
                    DateTime fechaInicioVigenciaPassDte = new DateTime();

                    if (response.response.datE_VIG_INI != null)
                    {
                        string fechaInicioVigenciaStr = response.response.datE_VIG_INI.ToString();
                        DateTime.TryParse(fechaInicioVigenciaStr, out fechaInicioVigenciaPassDte);
                    }

                    DateTime fechaFinVigenciaPassDte = new DateTime();
                    if (response.response.datE_VIG_FIN != null)
                    {
                        string fechaFinVigenciaStr = response.response.datE_VIG_FIN.ToString();
                        DateTime.TryParse(fechaFinVigenciaStr, out fechaFinVigenciaPassDte);
                    }


                    List<InfoEventoModel> listaEventos = new List<InfoEventoModel>();
                    listaEventos.Add(new InfoEventoModel());

                    responseObject.result = new RegistroInvitadoModel()
                    {
                        AceptaPoliticaPrivacidad = false,
                        Sexo = response.response.iD_GENDER,
                        PaisNacimiento = response.response.iD_COUNTRY,
                        Nacionalidad = response.response.iD_NATIONALITY,
                        NumeroPasaporte = response.response.passporT_NUM,
                        Apellidos = response.response.passporT_LASTNAME,
                        Nombre = response.response.passporT_NAME,
                        Correo = response.response.email,
                        DiaExpPas = fechaInicioVigenciaPassDte.ToString("dd"),
                        MesExpPas = fechaInicioVigenciaPassDte.ToString("MM"),
                        AnioExpPas = fechaInicioVigenciaPassDte.ToString("yyyy"),
                        DiaVenPas = fechaFinVigenciaPassDte.ToString("dd"),
                        MesVenPas = fechaFinVigenciaPassDte.ToString("MM"),
                        AnioVenPas = fechaFinVigenciaPassDte.ToString("yyyy"),

                        ActividadEnMexico = response.response.activitY_MEXICO,
                        ActividadPaisResidencia = response.response.activitY_COUNTRY,
                        Aerolinea = "",
                        AeropuertoLlegada = "",
                        AnioEntrada = "",
                        AnioSalida = "",
                        AntecedentesPenalesEnMexico = "",
                        ConfirmacionCorreo = "",
                        DiaEntrada = "",
                        DiaSalida = "",
                        Empresa = response.response.iD_COMPANY,
                        FueExpulsadoDeMexico = "",
                        MesEntrada = "",
                        MesSalida = "",
                        NumeroVuelo = "",

                        Eventos = listaEventos
                    };

                    responseObject.response = true;
                }


                responseObject.href = Url.Content("~/Home/RegistroInvitado");
            }
            catch(Exception e )
            {
                responseObject.response = false;
                responseObject.message = e.Message;
            }
            return Json(responseObject);
        }


        #endregion


        #region Genera reportes
        public async Task<ActionResult> GeneraDocumento(string idDocumento, string idRegistro, string tipoArchivo)
        {


            CRUDManager crud = new CRUDManager(httpManager);

            if (tipoArchivo.Equals("INVITACION"))
            {
                var resultadoConsulta = await crud.DescargarRegistroPorId(Convert.ToInt32(idRegistro));


                var infoEvento = await crud.DescargarDocumentoPorId(idDocumento);


                var catNacionalidad = await crud.DescargaCatalogoNacionalidadPorId(Convert.ToInt32(resultadoConsulta.Nacionalidad));

                IReporteInfo reporteInfo = new ReportInformation()
                {
                    NombreInvitado = $"{resultadoConsulta.Nombre} {resultadoConsulta.Apellidos}",
                    FechaEntradaAlPais = $"{resultadoConsulta.AnioEntrada}/{resultadoConsulta.MesEntrada}/{resultadoConsulta.DiaEntrada}",
                    FechaSalidaAlPais = $"{resultadoConsulta.AnioSalida}/{resultadoConsulta.MesSalida}/{resultadoConsulta.DiaSalida}",
                    Nacionalidad = $"{catNacionalidad.response.desC_NACIONALITY_SP}",
                    NumPasaporte = $"{resultadoConsulta.NumeroPasaporte}",
                    PuestoParteStaff = $"{resultadoConsulta.ActividadEnMexico}"
                };
                List<InfoEventoModel> EventList = new List<InfoEventoModel>();
                foreach (var eventoInfo in resultadoConsulta.Eventos)
                {

                    var catInmuebleActual = await crud.DescargaCatalogoInmueblePorId(Convert.ToInt32(eventoInfo.InmuebleEvento));
                    var catEventoActual = await crud.DescargaCatalogoEventoPorId(Convert.ToInt32(eventoInfo.NombreEvento));

                    EventList.Add(new InfoEventoModel()
                    {
                        /* Las fechas ya llegan con el toShortDateString*/
                        FechaInicioEvento = eventoInfo.FechaInicioEvento,
                        FechaFinEvento = eventoInfo.FechaFinEvento,
                        InmuebleEvento = $"{catInmuebleActual.response.desC_ESTATE_SP}",
                        NombreEvento = $"{catEventoActual.response.desC_EVENT_SP}",
                        UbicacionInmueble = $"{eventoInfo.UbicacionInmueble}"
                    });
                }
                reporteInfo.InfoEventosList = EventList.ToList<IInfoEvento>();
                PdfManager reporte = new PdfManager();
                var bytesBuffer = reporte.GenerateDocument(infoEvento, reporteInfo);

                return File(bytesBuffer.File, "application/pdf", bytesBuffer.FileName);
            }
            else
            {

                var infoEvento = await crud.DescargarPDFPorId(idDocumento);

                var bytesBuffer = new AttachmentFileDto
                {
                    File = Convert.FromBase64String(infoEvento.FILE_BLOB),
                    FileName = infoEvento.DESC_SPANISH
                };

                return File(bytesBuffer.File, "application/pdf", bytesBuffer.FileName);
            }
        }
        #endregion


        public async Task<JsonResult> ObtenerInfoEventoPorId(string idEvento)
        {
            responseObject = new ResponseModel();
            CRUDManager crud = new CRUDManager(httpManager);

            CommonTools.DTOs.Query.ResponseDto infoEvento = await crud.DescargaCatalogoEventoPorId(Convert.ToInt32(idEvento));

            if (infoEvento.code == 200)
            {
                EventsDto evento = new EventsDto()
                {
                    DATE_INI = infoEvento.response.datE_INI,
                    DATE_FIN = infoEvento.response.datE_FIN,
                    DESC_EVENT_SP = infoEvento.response.desC_EVENT_SP,
                    ID_ESTATE = infoEvento.response.iD_ESTATE,
                    DESC_LOCATION = infoEvento.response.desC_LOCATION
                };

                evento.ID_ESTATE = evento.ID_ESTATE == null ? 0 : evento.ID_ESTATE;
                evento.DESC_LOCATION = string.IsNullOrEmpty(evento.DESC_LOCATION) ? "" : evento.DESC_LOCATION;



                responseObject.result = evento;
            }

            return Json(responseObject);

        }
    }
}