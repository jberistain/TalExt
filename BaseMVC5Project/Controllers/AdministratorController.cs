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

    public class AdministratorController : Controller
    {
        private ComboBoxHelper CB = new ComboBoxHelper();
        private RegistroInvitadoModel registroInvitado;
        private ResponseModel responseObject;
        private HttpManager httpManager = new HttpManager(Constants.WebAPIUrl);
        private CRUDManager crud;

        [PermisoPerfil]
        [Autenticado]
        public async Task<ActionResult> Index(FilterRegisterDto filterParam = null)
        {
            ViewBag.Title = "Home Page";
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

            ViewBag.muestraRegistros = buscarRegistros;
            if(buscarRegistros)
                registrosEncontradosList = await crud.DescargarRegistros(filter);

            model.Registros = registrosEncontradosList;
            return View("Index",model);
        }



        public async Task<ActionResult> CambiarPassword(Login login)
        {
            ViewBag.Title = "CambiarPassword";



            return View("CambiarPassword", login);
        }


        [NoLogin]
        public async Task<ActionResult> GuardaNuevoPassword(Login login)
        {
            crud = new CRUDManager(httpManager);
            ResponseDto resultHttpRequest = new ResponseDto();
            responseObject = new ResponseModel();

            try
            {
                bool cambiaPwd = false;

                if(string.IsNullOrEmpty( login.NewPassword))
                {
                    throw new Exception("El campo Nueva Contraseña y confirmación de Nueva Contraseña deben estar llenos");
                }
                if (string.IsNullOrEmpty(login.ConfirmNewPassword))
                {
                    throw new Exception("El campo Nueva Contraseña y confirmación de Nueva Contraseña deben estar llenos");
                }
                if (string.IsNullOrEmpty(login.Password))
                {
                    throw new Exception("El campo Contraseña debe estar llenos");
                }

                if (string.IsNullOrEmpty(login.Username))
                {
                    throw new Exception("El campo Usuario debe estar llenos");
                }

                if(!login.NewPassword.Equals(login.ConfirmNewPassword))
                {
                    throw new Exception("El campo Contraseña y Nueva Contraseña deben ser iguales");
                }


                var userLogin = await crud.LoginUsuario(login.Username, login.Password);

                if (userLogin.code == 200)
                {
                    UserRegisterUpdatePwdDto obj = new UserRegisterUpdatePwdDto()
                    {
                        ID_USER = userLogin.response.iD_USER,
                        NewPassword = login.NewPassword,
                        MODIFY_BY = userLogin.response.iD_USER
                    };

                    resultHttpRequest = await crud.CambiarPasswordUsuario(obj.ID_USER.ToString(), obj);

                    if (resultHttpRequest != null && !resultHttpRequest.error)
                    {
                        Login model = new Login() { Mensaje = "Contraseña Actualizada" };
                        return View("Login", model);
                    }
                    else
                    {
                        Login model = new Login() { Mensaje = "Ocurrio un error al guardar la contraseña: " + resultHttpRequest.message };
                        return View("Login", model);
                    }
                }
                else
                {
                    throw new Exception("Los datos de Acceso ingresados fueron incorrectos");
                }

            }
            catch (Exception ex)
            {
                Login model = new Login() { Mensaje = "Ocurrio un error en el proceso de Guardar contraseña: " + ex.Message };
                return View("Login", model);
            }
        }


        [NoLogin]
        public async Task<ActionResult> Login(Login login)
        {
            ViewBag.Title = "Login";

            

            return View(login);
        }
        [NoLogin]
        public async Task<ActionResult> Acceder(Login login)
        {
            crud = new CRUDManager(httpManager);
            ResponseDto resultHttpRequest = new ResponseDto();
            responseObject = new ResponseModel();

            try
            {
                //bool usuarioFirmas = ParcheUsuarioFirmas(login);
                //if(usuarioFirmas)
                //{
                //    int userId = 1000;
                //    SessionManager.AddUserToSession(userId.ToString());
                //    CatalogsController catControl = new CatalogsController();
                //    return RedirectToAction("FirmasDocumentos", "Catalogs");
                //    //return await catControl.FirmasDocumentos();
                //}


                resultHttpRequest = await crud.LoginUsuario(login.Username, login.Password);
                
                if(resultHttpRequest == null)
                {
                    Login model = new Login() { Mensaje = "Error al intentar autenticar" };
                    return View("Login", model);
                }

                if (!resultHttpRequest.error)
                {
                    int diasActualizacion = 0;
                    //Comprobar fecha de modificacion de usuario
                    string idStr = "";
                    int idInt = 0;
                    if (resultHttpRequest.response.iD_USER != null)
                        idStr = resultHttpRequest.response.iD_USER.ToString();
                    if(!string.IsNullOrEmpty(idStr))
                        idInt = Convert.ToInt32(idStr);
                    if (idInt == 0)
                    {
                        throw new Exception("Ocurrio un error al obtener la información del usuario");
                    }
                    ResponseDto usuario = await crud.DescargaCatalogoUsuarioPorId(idInt);
                    bool mandarAActualizar = false;
                    DateTime fechaUltimaActualizacionDte = new DateTime();
                    if (usuario.response.modifY_DATE == null)
                    {
                        string fechaInicioVigenciaStr = usuario.response.createD_DATE.ToString();
                        DateTime.TryParse(fechaInicioVigenciaStr, out fechaUltimaActualizacionDte);

                        if(fechaUltimaActualizacionDte != new DateTime())
                        {
                            TimeSpan dSpan = DateTime.Now - fechaUltimaActualizacionDte;
                            diasActualizacion = dSpan.Days;
                            if(diasActualizacion >= 90)
                                mandarAActualizar = true;
                        }
                    }
                    else
                    {
                        string fechaInicioVigenciaStr = usuario.response.modifY_DATE.ToString();
                        DateTime.TryParse(fechaInicioVigenciaStr, out fechaUltimaActualizacionDte);

                        if (fechaUltimaActualizacionDte != new DateTime())
                        {
                            TimeSpan dSpan = DateTime.Now - fechaUltimaActualizacionDte;
                            diasActualizacion = dSpan.Days;
                            if (diasActualizacion >= 90)
                                mandarAActualizar = true;
                        }

                    }
                    if (!mandarAActualizar)
                    {
                        int userId = resultHttpRequest.response.iD_USER;
                        SessionManager.AddUserToSession(userId.ToString());

                        ConsultaModulosPerfil model = new ConsultaModulosPerfil();
                        var menus = await model.ObtenerModulosPorIdUsuario(userId);



                        string menusSerialized = SimpleJson.SerializeObject(menus);
                        SessionManager.AddMenuToSession(menusSerialized);


                        return RedirectToAction("Index", "Administrator");
                        // return await Index(new FilterRegisterDto());
                    }
                    else
                    {
                        login.Mensaje = "Han pasado mas de 90 días y debe cambiar la contraseña";
                        //return Redirect(Url.Content("~/Administrator/CambiarPassword"));
                        return await CambiarPassword(login);
                    }
                }
                else
                {
                    Login model = new Login() { Mensaje = resultHttpRequest.message };
                    return View("Login", model);
                }

            }
            catch (Exception ex)
            {
                return Content("Ocurrio un error en el proceso de Login :(" + ex.Message);
            }

        }

        [NoLogin]
        public async Task<ActionResult> RecuperarPassword(Login login)
        {
            ViewBag.Title = "Recuperar Contraseña";

            return View(login);
        }

        [NoLogin]
        public async Task<ActionResult> IntentaRecuperarPassword(Login login)
        {
            crud = new CRUDManager(httpManager);
            ResponseDto resultHttpRequest = new ResponseDto();
            responseObject = new ResponseModel();

            try
            {
                resultHttpRequest = await crud.RecuperarPassword(login.Username, login.Email);

                if (resultHttpRequest == null)
                {
                    Login model = new Login() { Mensaje = "Error al intentar recuperar contraseña" };
                    return View("RecuperarPassword", model);
                }

                if (!resultHttpRequest.error)
                {
                    Login model = new Login() { Mensaje = resultHttpRequest.message };
                    return View("Login", model);
                }
                else
                {
                    Login model = new Login() { Mensaje = resultHttpRequest.message };
                    return View("RecuperarPassword", model);
                }

            }
            catch (Exception ex)
            {
                return Content("Ocurrio un error en el proceso de Login :( " + ex.Message);
            }

        }


        private bool ParcheUsuarioFirmas(Login login)
        {


            bool result = false;
            if(login.Username.Equals("JMVIVASM") && login.Password.Equals("Zqrss3t#tE2"))
            {
                result = true;
            }
            return result;
        }


        [Autenticado]
        public ActionResult Logout()
        {
            SessionManager.DestroyUserSession();
            return Redirect("~/Administrator/Login");
        }

        [Autenticado]
        public async Task<ActionResult> EditarRegistro(int id )
        {
            try
            {
                string language = "ES";
                RegistroInvitadoModel modelFound = null;

                
                var crud = new CRUDManager(httpManager);


                modelFound = await crud.DescargarRegistroPorId(id);


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