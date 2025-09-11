using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Data;
using System.Web.Mvc;
using BaseMVC5Project.Models.Utils;
using MigracionTalentoExtranjero.Models.Administrator;
using MigracionTalentoExtranjero.Models.Enum;
using MigracionTalentoExtranjero.Models.Home;
using MigracionTalentoExtranjero.Models.Utils;
using CommonTools.Csv;
using CommonTools.DTOs.Query;
using MigracionTalentoExtranjero.Models;
using CommonTools.Pdf;
using MigracionTalentoExtranjero.Models.Session;
using CommonTools.DTOs.Register;
using MigracionTalentoExtranjero.Models.Catalogs;
using RestSharp;

namespace MigracionTalentoExtranjero.Controllers
{

    public class AnotherAssistantsAdmonController : Controller
    {
        private ComboBoxHelper CB = new ComboBoxHelper();
        private RegistroInvitadoModel registroInvitado;
        private ResponseModel responseObject;
        private HttpManager httpManager = new HttpManager(Constants.WebAPIUrl);
        private CRUDManager crud;

        // [PermisoPerfil]
        [Autenticado]
        public async Task<ActionResult> Index(FilterRegisterDto filterParam = null)
        {
            
            //Se solicito cambiar empresas por evnto para filtrar por eventos
            ViewBag.CompaniasList = await CB.GetSearchComboBox(CatalogosEnum.CAT_EVENTOS.GetString(), 0, "");

            DashboardModel model = new DashboardModel();

            crud = new CRUDManager(httpManager);

            FilterRegisterDto filter;
            if (filterParam == null)
            {
                filter = new FilterRegisterDto
                {
                    IdEmpresa = 0,
                    FechaInicio = "",
                    FechaFin = "",
                    Busqueda = "",
                    IdEvento = 0,
                };
                
            }
            else
            {
                filter = filterParam;

                //Validaciones de fechas
                DateTime fecha1;
                bool fechaValida = DateTime.TryParse(filter.FechaInicio, out fecha1);
                if(fechaValida)
                {
                    model.Fecha1 = filter.FechaInicio.ToString();
                }
                else
                {
                    filter.FechaInicio = "";
                }
                DateTime fecha2;
                fechaValida = DateTime.TryParse(filter.FechaFin, out fecha2);
                if (fechaValida)
                {
                    model.Fecha2 = filter.FechaFin.ToString();
                }
                else
                {
                    filter.FechaFin = "";
                }

                model.Empresa = filter.IdEvento.ToString();
                
                model.TextoBusqueda = string.IsNullOrEmpty(filter.Busqueda) ? "" : filter.Busqueda.ToString();
                
            }
            List<Registro> registrosEncontradosList = new List<Registro>();
            
            List<ModuloDto> listaModulos = ConsultaModulosPerfil.ObtenerModulosDeSesion();
            bool buscarRegistros = false;
            foreach(var modulo in listaModulos)
            {
                if(!string.IsNullOrEmpty(modulo.DESC_PROCESS_SP) && modulo.DESC_PROCESS_SP.ToUpper().Equals("REGISTROS"))
                {
                    buscarRegistros = true;
                }
            }

            // REGRESAR CUANDO SE CONFIGUREN PERFILES
            buscarRegistros = true;
            ViewBag.muestraRegistros = buscarRegistros;
            if(buscarRegistros)
                registrosEncontradosList = await crud.DescargarRegistrosAnotherAssistants(filter);

            model.Registros = registrosEncontradosList;
            return View("Index",model);
        }


        /// <summary>
        /// Se utiliza para registras nuevos invitados pero para la seccion de administracion donde tendra un formato unico
        /// </summary>
        /// <param name="language"></param>
        /// <param name="modelFound"></param>
        /// <returns></returns>
        public async Task<ActionResult> NuevoRegistro(string language, RegistroInvitadoModel modelFound)
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

                var ListaNacionalidades = await crud.DescargaCatalogosNacionalidad(language);

                string htmlNacionalidades = "<select class=\"form-control\" id=\"Nacionalidad\" name=\"Nacionalidad\" required=\"required\" style=\"position:initial\">" +
                    "<option value=\"\"></option>\r\n"
                    ;
                string nacionalidadExistente = "";
                if (!string.IsNullOrEmpty(modelFound.Nacionalidad))
                {
                    nacionalidadExistente = modelFound.Nacionalidad;
                }
                for (int i = 0; i < ListaNacionalidades.Count; i++)
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
                listaCompanias.Add(new SelectListItem() { Text = "OTRA", Value = "OTRA" });
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
                foreach (SelectListItem item in EventosList)
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
                if (response.code == 200)
                {
                    if (registroInvitado.IdiomaActual.Equals("ES"))
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


        /// <summary>
        /// Se usa para guardar la informacion de los nuevos registros de invitados
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
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

                    var response = await httpManager.PostAsJsonAsync<Object, CommonTools.DTOs.Query.ResponseDto>(requestObject, WebAPIEndPointsEnum.CREATE_NEW_REGISTER_ANOTHER_ASSISTANTS.GetString());

                    if (response != null && response.code == 200)
                    {
                        //Setear respuesta
                        responseObject.response = true;
                        responseObject.message = "Se guardó con éxito el registro.";
                        responseObject.result = new ResultadoRegistroModel()
                        {
                            NombreCompleto = $"{model.Nombre} {model.Apellidos}",
                            Codigo = response.response.secreT_CODE,
                            Url = Url.Action("EditarRegistro", "AnotherAssistantsAdmon", new { id = response.response.idinvitacion })
                        };
                    }
                    else
                    {
                        responseObject.message = response.message != null ? response.message : "No se pudo guardar, error en respuesta de API";
                        responseObject.response = false;
                    }

                }
            }
            catch (Exception ex)
            {
                responseObject.response = false;
                responseObject.message = ex.Message;
            }

            return Json(responseObject);
        }



        [Autenticado]
        public async Task<ActionResult> EditarRegistro(int id )
        {
            try
            {
                string language = "ES";
                RegistroInvitadoOtrosAsisModel modelFound = null;

                
                var crud = new CRUDManager(httpManager);


                modelFound = await crud.DescargarRegistroOtroAsisPorId(id);


                if (modelFound == null)
                    registroInvitado = new RegistroInvitadoOtrosAsisModel();
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
                ViewBag.GenerosList = await CB.GetSearchComboBox(CatalogosEnum.CAT_GENEROS.GetString(), 0, "");


                var ListaAeropuertos = await CB.GetSearchComboBox(CatalogosEnum.CAT_AEROPUERTOS.GetString(), 0, "");
                ListaAeropuertos.Add(new SelectListItem() { Text = "OTRA", Value = "OTRA" });
                ViewBag.AeropuertosList = ListaAeropuertos;
                //ViewBag.NacionalidadesList = await CB.GetSearchComboBox(CatalogosEnum.CAT_NACIONALIDADES.GetString(), 0, "");

                var nacionalidadesList = await CB.GetSearchComboBox(CatalogosEnum.CAT_NACIONALIDADES.GetString(), 0, "");
                
                var ListaNacionalidades = await crud.DescargaCatalogosNacionalidad();

                string htmlNacionalidades = "<select class=\"form-control\" id=\"Nacionalidad\" name=\"Nacionalidad\" required=\"required\" style=\"position:initial\">" +
                    "<option value=\"\"></option>\r\n"
                    ;
                for (int i = 0; i < ListaNacionalidades.Count; i++)
                {
                    string opcionSeleccionada = "";
                    if (ListaNacionalidades[i].Id.ToString().Equals(registroInvitado.Nacionalidad))
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
                listaCompanias.Add(new SelectListItem() { Text = "OTRA", Value = "OTRA" });
                ViewBag.CompaniasList = listaCompanias;
                ViewBag.PaisesList = await CB.GetSearchComboBox(CatalogosEnum.CAT_PAISES.GetString(), 0, "");
                registroInvitado.SelectListEvents = new List<List<SelectListItem>>();
                registroInvitado.SelectListInmuebles = new List<List<SelectListItem>>();
                registroInvitado.SelectListDiaInicioEvento = new List<List<SelectListItem>>();
                registroInvitado.SelectListMesInicioEvento = new List<List<SelectListItem>>();
                registroInvitado.SelectListAnioInicioEvento = new List<List<SelectListItem>>();
                registroInvitado.SelectListDiaFinEvento = new List<List<SelectListItem>>();
                registroInvitado.SelectListMesFinEvento = new List<List<SelectListItem>>();
                registroInvitado.SelectListAnioFinEvento = new List<List<SelectListItem>>();
                foreach (var item in registroInvitado.Eventos)
                {
                    registroInvitado.SelectListEvents.Add(registroInvitado.SetSelectedItem(item.NombreEvento, await CB.GetSearchComboBox(CatalogosEnum.CAT_EVENTOS.GetString(), 0, "")));
                    registroInvitado.SelectListInmuebles.Add(registroInvitado.SetSelectedItem(item.InmuebleEvento, await CB.GetSearchComboBox(CatalogosEnum.CAT_INMUEBLES.GetString(), 0, "")));

                    registroInvitado.SelectListDiaInicioEvento.Add(registroInvitado.SetSelectedItem(item.DiaInicioEvento, await CB.GetSearchComboBox(CatalogosEnum.DIA.GetString(), 0, "")));
                    registroInvitado.SelectListMesInicioEvento.Add(registroInvitado.SetSelectedItem(item.MesInicioEvento, await CB.GetSearchComboBox(CatalogosEnum.MES.GetString(), 0, "")));
                    registroInvitado.SelectListAnioInicioEvento.Add(registroInvitado.SetSelectedItem(item.AnioInicioEvento, await CB.GetSearchComboBox(CatalogosEnum.ANIO.GetString(), 0, "")));

                    registroInvitado.SelectListDiaFinEvento.Add(registroInvitado.SetSelectedItem(item.DiaFinEvento, await CB.GetSearchComboBox(CatalogosEnum.DIA.GetString(), 0, "")));
                    registroInvitado.SelectListMesFinEvento.Add(registroInvitado.SetSelectedItem(item.MesFinEvento, await CB.GetSearchComboBox(CatalogosEnum.MES.GetString(), 0, "")));
                    registroInvitado.SelectListAnioFinEvento.Add(registroInvitado.SetSelectedItem(item.AnioFinEvento, await CB.GetSearchComboBox(CatalogosEnum.ANIO.GetString(), 0, "")));
                }

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
                var EventosList = await CB.GetSearchComboBox(CatalogosEnum.CAT_EVENTOS.GetString(), 0, "");
                foreach (SelectListItem item in EventosList)
                {
                    opcionActual = $"<option value=\"{item.Value}\">{item.Text}</option>";
                    opcionesDeComboEventosString += opcionActual;
                }
                ViewBag.EventosOptionsHTML = opcionesDeComboEventosString;

                string opcionesdeComboInmueblesString = "";
                var InmueblesList = await CB.GetSearchComboBox(CatalogosEnum.CAT_INMUEBLES.GetString(), 0, "");
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
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return View(registroInvitado);

        }



        [Autenticado]
        public async Task<JsonResult> ActualizarRegistroInvidato(RegistroInvitadoModel model)
        {
            responseObject = new ResponseModel();

            // Se validan los campos requeridos del modelo entrante
            string camposRequeridosVacios = model.ValidaCamposRequeridos();
            if (!string.IsNullOrEmpty(camposRequeridosVacios))
            {
                responseObject.message = "Debe llenar los campos requeridos: " + camposRequeridosVacios;
                responseObject.response = false;
            }
            else
            {

                ResponseDto response = null;

                if (SessionManager.ExistUserInSession())
                {
                    int idUser = SessionManager.GetUser();
                    bool expulsadoMexico = false;
                    if (!string.IsNullOrEmpty(model.FueExpulsadoDeMexico))
                        expulsadoMexico = model.FueExpulsadoDeMexico.Equals("SI") ? true : false;

                    bool antecedentesEnMexico = false;
                    if (!string.IsNullOrEmpty(model.AntecedentesPenalesEnMexico))
                        antecedentesEnMexico = model.AntecedentesPenalesEnMexico.Equals("SI") ? true : false;
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
                                int idCurrentEvent = currentEvent.Id == null ? 0 : int.Parse(currentEvent.Id);
                                Events.Add(new
                                {
                                    ID_REG_EVEN_DATE = idCurrentEvent,
                                    ID_EVENT = currentEvent.NombreEvento,//Se deja este nombre para enviar el ID del combobox
                                    ID_ESTATE = currentEvent.InmuebleEvento,
                                    DESC_LOCATION = currentEvent.UbicacionInmueble,
                                    EVENT_DATE = fechaInicio,
                                    EVENT_DATE_FIN = fechaFin,
                                }) ;
                            }
                        }

                    }

                    DateTime fechaInicioVigenciaPasaporte = new DateTime(Convert.ToInt32(model.AnioExpPas), Convert.ToInt32(model.MesExpPas), Convert.ToInt32(model.DiaExpPas));
                    DateTime fechaExpiracionPasaporte = new DateTime(Convert.ToInt32(model.AnioVenPas), Convert.ToInt32(model.MesVenPas), Convert.ToInt32(model.DiaVenPas));

                    DateTime fechaLlegada = new DateTime(Convert.ToInt32(model.AnioEntrada), Convert.ToInt32(model.MesEntrada), Convert.ToInt32(model.DiaEntrada));
                    DateTime fechaSalida = new DateTime(Convert.ToInt32(model.AnioSalida), Convert.ToInt32(model.MesSalida), Convert.ToInt32(model.DiaSalida));

                    //Consumir servicio de guardado
                    object requestObject = new
                    {
                        ID_REG = model.Id,
                        ID_REG_EVEN_DATE = 1,
                        ID_GENDER = model.Sexo,
                        ID_COUNTRY = model.PaisNacimiento,
                        ID_NATIONALITY = model.Nacionalidad,
                        ID_ACTIVITY = 1,
                        ID_AIRPORT = model.AeropuertoLlegada,
                        ID_AIR_LINE = model.Aerolinea,
                        ID_PROCESS = 1,
                        ID_STATUS = 1,
                        ID_COMPANY = model.Empresa,
                        PASSPORT_NUM = model.NumeroPasaporte,
                        DATE_VIG_INI = fechaInicioVigenciaPasaporte,
                        DATE_VIG_FIN = fechaExpiracionPasaporte,
                        PASSPORT_NAME = model.Nombre,
                        PASSPORT_LASTNAME = model.Apellidos,
                        EMAIL = model.Correo,
                        ACTUAL_JOB = model.ActividadEnMexico,
                        EXPELLED_MEX = expulsadoMexico,
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
                        MODIFY_BY = idUser,
                        CHECK_VERIFY = model.CHECK_VERIFY,
                        Events = Events
                    };

                    response = await httpManager.PostAsJsonAsync<Object, ResponseDto>(requestObject, WebAPIEndPointsEnum.UPDATE_REGISTER.GetString());
                }



                if (response != null)
                {
                    if (response.code == 200)
                    {
                        //Setear respuesta
                        responseObject.response = true;
                        responseObject.message = "Se guardó con éxito el registro.";
                        responseObject.href = Url.Content("~/Administrator/Index");
                    }
                    else
                    {
                        responseObject.message = response.message;
                        responseObject.response = false;
                    }
                }

            }

            return Json(responseObject);
        }

        [Autenticado]
        public async Task GeneraCsv(string empresa, string fechaInicio, string fechaFin, string busqueda, string idEvento)
        {
            crud = new CRUDManager(httpManager);
            FilterRegisterDto filter = new FilterRegisterDto();
            //Validaciones de fechas
            DateTime fecha1;
            bool fechaValida = DateTime.TryParse(fechaInicio, out fecha1);
            if (fechaValida)
            {
                filter.FechaInicio = fechaInicio.ToString();
            }
            else
            {
                filter.FechaInicio = "";
            }
            DateTime fecha2;
            fechaValida = DateTime.TryParse(fechaFin, out fecha2);
            if (fechaValida)
            {
                filter.FechaFin = fechaFin.ToString();
            }
            else
            {
                filter.FechaFin = "";
            }

            filter.IdEmpresa = string.IsNullOrEmpty(empresa) ? 0 : Convert.ToInt32(empresa);
            filter.IdEvento = string.IsNullOrEmpty(idEvento) ? 0 : Convert.ToInt32(idEvento);

            filter.Busqueda = string.IsNullOrEmpty(busqueda) ? "" : busqueda.ToString();

            var registrosEncontradosList = await crud.DescargarRegistros(filter);



            CsvManager csv= new CsvManager();
            List<Registro> registrosBuscados = new List<Registro>();
            registrosBuscados.Add(new Registro() { Actividad="ACTIVIDAD", Empresa="EVENTO", CorreoElectronico="CORREO",Estatus="ESTATUS",FolioYFechaRegistro="FOLIO Y FECHA DE REGISTRO", Nacionalidad="NACIONALIDAD", Nombre="NOMBRE", Pasaporte="PASAPORTE"});

            foreach(var registro in registrosEncontradosList)
            {
                registrosBuscados.Add(new Registro() { 
                    Actividad = string.IsNullOrEmpty(registro.Actividad)?"": registro.Actividad, 
                    Empresa = string.IsNullOrEmpty(registro.Empresa) ? "" : registro.Empresa,
                    CorreoElectronico = string.IsNullOrEmpty(registro.CorreoElectronico) ? "" : registro.CorreoElectronico,
                    Estatus = string.IsNullOrEmpty(registro.EstatusDesc) ? "" : registro.EstatusDesc,
                    FolioYFechaRegistro = string.IsNullOrEmpty(registro.FolioYFechaRegistro) ? "" : registro.FolioYFechaRegistro, 
                    Nacionalidad = string.IsNullOrEmpty(registro.Nacionalidad) ? "" : registro.Nacionalidad, 
                    Nombre = string.IsNullOrEmpty(registro.Nombre) ? "" : registro.Nombre , 
                    Pasaporte = string.IsNullOrEmpty(registro.Pasaporte) ? "" : registro.Pasaporte
                });
            }

            List<IRegistro> registrosEncontrados = registrosBuscados.ToList<IRegistro>();
            string docString = csv.GeneraCVSBusquedaActual(registrosEncontrados);
            string nombreArchivo = $"Registros_{DateTime.Now.ToString()}";
            string attachment = $"attachment; filename={nombreArchivo}.csv";

            System.Web.HttpContext.Current.Response.Clear();
            System.Web.HttpContext.Current.Response.ClearHeaders();
            System.Web.HttpContext.Current.Response.ClearContent();
            System.Web.HttpContext.Current.Response.AddHeader("content-disposition", attachment);
            System.Web.HttpContext.Current.Response.ContentType = "text/csv";
            System.Web.HttpContext.Current.Response.AddHeader("Pragma", "public");
            System.Web.HttpContext.Current.Response.ContentEncoding = Encoding.Default;
            //Mandar archivo a guardar al cliente
            System.Web.HttpContext.Current.Response.Write(docString);
        }

        [Autenticado]
        public async Task<JsonResult> EnviarInvitaciones(List<string> idRegistroList)
        {
            responseObject = new ResponseModel();

            if (idRegistroList != null)
            {
                List<int> ids = new List<int>();
                foreach (string id in idRegistroList)
                {
                    if (!string.IsNullOrEmpty(id))
                        ids.Add(Convert.ToInt32(id));
                }


                CRUDManager crud = new CRUDManager(httpManager);
                var resultadoEnvios = await crud.EnviarCorreosInvitacionesPorIds(ids);

                if (resultadoEnvios.code == 200)
                {
                    responseObject.response = true;
                    responseObject.message = resultadoEnvios.message;
                }
                else
                {
                    responseObject.response = false;
                    responseObject.message = resultadoEnvios.message;
                }
            }
            else
            {
                responseObject.response = false;
                responseObject.message = "No se marcó ningún elemento para enviar invitación";
            }


            return Json(responseObject);
        }

        [Autenticado]
        public JsonResult FiltrarRegistros(DashboardModel filtros)
        {
            responseObject = new ResponseModel();


            responseObject.response = true;

            List<Registro> registrosEncontrados = new List<Registro>();
            registrosEncontrados.Add(new Registro() { Empresa = "Empresa nueva busqueda", Estatus="3" });
            responseObject.result = registrosEncontrados;

            return Json(responseObject);
        }

        [Autenticado]
        public async Task<JsonResult> ObtenerInformacionRegistroEvento(int id)
        {
            responseObject = new ResponseModel();
            try
            {
                if (crud == null)
                    crud = new CRUDManager(httpManager);

                
                var registro = await crud.DescargarRegistroPorId(id);
                responseObject.response= true;
                responseObject.result = registro;
            }
            catch(Exception ex)
            { 
                responseObject.response=false;
                responseObject.message = $"Error al obtener el evento: {ex.Message}";
            }

            return Json(responseObject);
        }

        [Autenticado]
        public async Task<JsonResult> EliminarRegistroEvento(int id)
        {
            responseObject = new ResponseModel();
            try
            {
                if (crud == null)
                    crud = new CRUDManager(httpManager);


                responseObject = await crud.EliminarRegistroPorId(id);
            }
            catch (Exception ex)
            {
                responseObject.response = false;
                responseObject.message = $"Error al obtener el evento: {ex.Message}";
            }

            return Json(responseObject);
        }



    }
}