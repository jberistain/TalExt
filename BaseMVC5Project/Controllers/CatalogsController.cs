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
using MigracionTalentoExtranjero.Models.Catalogs;
using CommonTools.Models;
using ResponseDto = CommonTools.DTOs.Query.ResponseDto;
using CommonTools.DTOs.Register;
using RestSharp.Extensions;
using MigracionTalentoExtranjero.Models.Session;
using System.Web.UI.WebControls;

namespace MigracionTalentoExtranjero.Controllers
{
    
    [Autenticado]
    public class CatalogsController : Controller
    {
        private ComboBoxHelper CB = new ComboBoxHelper();
        private ResponseModel responseObject;
        private HttpManager httpManager = new HttpManager(Constants.WebAPIUrl);
        private CRUDManager crud;
        
        [PermisoPerfil]
        public async Task<ActionResult> Index()
        {
            ViewBag.Title = "CATÁLOGOS";
           
            return View();
        }

        [PermisoPerfil]
        public async Task<ActionResult> Empresas()
        {
            ViewBag.Title = "EMPRESAS";

            crud = new CRUDManager(httpManager);

            List<CatalogoGeneral> catalogoGeneralList;

            catalogoGeneralList = await crud.DescargaCatalogosEmpresas();

            ViewBag.CatalogList = catalogoGeneralList;

            return View();
        }

        [PermisoPerfil]
        public async Task<ActionResult> Eventos()
        {
            ViewBag.Title = "EVENTOS";
            crud = new CRUDManager(httpManager);

            List<CatalogoGeneral> catalogoGeneralList;

            catalogoGeneralList = await crud.DescargaCatalogosEventos();

            var catalogoEmpresasList = await crud.DescargaCatalogosTipoEventos();
            
            foreach(CatalogoGeneral catalogo in catalogoGeneralList)
            {
                catalogo.AtributoAdicionalStr1 = catalogo.BuscaDescripcionEnLista(catalogo.AtributoAdicionalInt1,catalogoEmpresasList);
            }

            ViewBag.CatalogList = catalogoGeneralList;
            ViewBag.CompaniasList = await CB.GetSearchComboBox(CatalogosEnum.CAT_TIPOS_EVENTOS.GetString(), 0, "");
            ViewBag.InmueblesList = await CB.GetSearchComboBox(CatalogosEnum.CAT_INMUEBLES.GetString(), 0, "");
            return View();
        }


        [PermisoPerfil]
        public async Task<ActionResult> Paises()
        {
            ViewBag.Title = "PAISES";

            crud = new CRUDManager(httpManager);

            List<CatalogoGeneral> catalogoGeneralList;

            catalogoGeneralList = await crud.DescargaCatalogosPaises();

            ViewBag.CatalogList = catalogoGeneralList;
            ViewBag.CatSiNoList = await CB.GetSearchComboBox(CatalogosEnum.CAT_SI_NO.GetString(), 0, "");


            return View();
        }


        [PermisoPerfil]
        public async Task<ActionResult> Nacionalidades()
        {
            ViewBag.Title = "Nacionalidades";

            crud = new CRUDManager(httpManager);

            List<CatalogoGeneral> catalogoGeneralList;

            catalogoGeneralList = await crud.DescargaCatalogosNacionalidad();

            ViewBag.CatalogList = catalogoGeneralList;
            ViewBag.CatSiNoList = await CB.GetSearchComboBox(CatalogosEnum.CAT_SI_NO.GetString(), 0, "");


            return View();
        }


        [PermisoPerfil]
        public async Task<ActionResult> Documentos()
        {
            ViewBag.Title = "DOCUMENTOS";

            crud = new CRUDManager(httpManager);

            List<CatalogoGeneral> catalogoGeneralList;

            ViewBag.ListaTiposEvento = await CB.GetSearchComboBox(CatalogosEnum.CAT_TIPOS_EVENTOS.GetString(), 0, "");

            catalogoGeneralList = await crud.DescargaCatalogosDocumentos();

            ViewBag.CatalogList = catalogoGeneralList;

            return View();
        }


        [PermisoPerfil]
        public async Task<ActionResult> FirmasDocumentos()
        {
            ViewBag.Title = "FIRMAS DE DOCUMENTOS";

            crud = new CRUDManager(httpManager);

            List<CatalogoGeneral> catalogoGeneralList;

            ViewBag.ListaTiposEvento = await CB.GetSearchComboBox(CatalogosEnum.CAT_TIPOS_EVENTOS.GetString(), 0, "");

            catalogoGeneralList = await crud.DescargaCatalogosFirmasDocumentos();

            ViewBag.CatalogList = catalogoGeneralList;

            return View();
            //return View("../Catalogs/FirmasDocumentos", new CatalogoGeneral());
        }

        [PermisoPerfil]
        public async Task<ActionResult> PDFs()
        {
            ViewBag.Title = "DOCUMENTOS PDF";

            crud = new CRUDManager(httpManager);

            List<CatalogoGeneral> catalogoGeneralList;

            ViewBag.ListaTiposEvento = await CB.GetSearchComboBox(CatalogosEnum.CAT_TIPOS_EVENTOS.GetString(), 0, "");

            catalogoGeneralList = await crud.DescargaCatalogosPDF();

            ViewBag.CatalogList = catalogoGeneralList;

            return View();
        }


        [PermisoPerfil]
        public async Task<ActionResult> Aerolineas()
        {
            ViewBag.Title = "Aerolineas";

            crud = new CRUDManager(httpManager);

            List<CatalogoGeneral> catalogoGeneralList;

            catalogoGeneralList = await crud.DescargaCatalogosAerolineas();

            ViewBag.CatalogList = catalogoGeneralList;

            return View();
        }


        [PermisoPerfil]
        public async Task<ActionResult> Aeropuertos()
        {
            ViewBag.Title = "Aeropuertos";

            crud = new CRUDManager(httpManager);

            List<CatalogoGeneral> catalogoGeneralList;

            catalogoGeneralList = await crud.DescargaCatalogosAeropuertos();

            ViewBag.CatalogList = catalogoGeneralList;

            return View();
        }

        [PermisoPerfil]
        public async Task<ActionResult> Inmuebles()
        {
            ViewBag.Title = "Inmuebles";

            crud = new CRUDManager(httpManager);

            List<CatalogoGeneral> catalogoGeneralList;

            catalogoGeneralList = await crud.DescargaCatalogosInmuebles();

            ViewBag.CatalogList = catalogoGeneralList;


            return View();
        }

        [PermisoPerfil]
        public async Task<ActionResult> Sexos()
        {
            ViewBag.Title = "Sexos";

            crud = new CRUDManager(httpManager);

            List<CatalogoGeneral> catalogoGeneralList;

            catalogoGeneralList = await crud.DescargaCatalogosGeneros();

            ViewBag.CatalogList = catalogoGeneralList;

            return View();
        }

        [PermisoPerfil]
        public async Task<ActionResult> TipoEventos()
        {
            ViewBag.Title = "Tipo Eventos";

            crud = new CRUDManager(httpManager);

            List<CatalogoGeneral> catalogoGeneralList;

            catalogoGeneralList = await crud.DescargaCatalogosTipoEventos();

            ViewBag.CatalogList = catalogoGeneralList;

            return View();
        }

        [PermisoPerfil]
        public async Task<ActionResult> AvisoPrivacidad()
        {
            ViewBag.Title = "Aviso de Privacidad";

            crud = new CRUDManager(httpManager);
            CatalogoGeneral model = new CatalogoGeneral();
            var response = await httpManager.GetAsJsonAsync<CommonTools.DTOs.Query.ResponseDto>(WebAPIEndPointsEnum.CONSULTA_AVISO_PRIVACIDAD.GetString());
            if (response.code == 200)
            {
                model.AtributoAdicionalStr1 = response.response.avisO_ESP;
                model.AtributoAdicionalStr2 = response.response.avisO_ENG;
                model.Id = response.response.id;
            }

            return View(model);
        }

        [PermisoPerfil]
        public async Task<ActionResult> Usuarios()
        {
            ViewBag.Title = "Usuarios";

            crud = new CRUDManager(httpManager);

            List<CatalogoGeneral> catalogoGeneralList;

            catalogoGeneralList = await crud.DescargaCatalogosUsuarios();

            var listaPerfiles = await CB.GetSearchComboBox(CatalogosEnum.CATALOGO_PERFILES.GetString(), 0, "");

            AsignarDescripcionPerfiles(catalogoGeneralList, listaPerfiles);

            ViewBag.ListaPerfiles = listaPerfiles;


            ViewBag.CatalogList = catalogoGeneralList;

            return View();
        }

        private void AsignarDescripcionPerfiles(List<CatalogoGeneral> usuarios, List<SelectListItem> perfiles)
        {
            foreach (var usuario in usuarios)
            {
                // el id de perfil esta en el AtributoAdicionalStr3
                if(usuario.AtributoAdicionalStr3.Equals("0"))
                {
                    usuario.AtributoAdicionalStr3 = "SIN PERFIL";
                }
                else
                {
                    foreach(var perfil in perfiles)
                    {
                        if(perfil.Value.Equals(usuario.AtributoAdicionalStr3))
                        {
                            usuario.AtributoAdicionalStr3 = perfil.Text;
                        }
                    }
                }
            }
        }


        [PermisoPerfil]
        public async Task<ActionResult> Regiones()
        {
            ViewBag.Title = "REGIONES";

            return View();
        }

        public async Task<ActionResult> Perfiles()
        {
            ViewBag.Title = "Perfiles";

            crud = new CRUDManager(httpManager);

            List<CatalogoGeneral> catalogoGeneralList;

            catalogoGeneralList = await crud.DescargaCatalogosPerfiles();

            ViewBag.CatalogList = catalogoGeneralList;

            return View();
        }


        public async Task<JsonResult> CreaNuevoElementoCatalogo(CatalogoGeneral data, HttpPostedFileBase ArchivoPDF)
        {
            responseObject = new ResponseModel();

            crud = new CRUDManager(httpManager);
            ResponseDto resultHttpRequest = new ResponseDto();

            if (!SessionManager.ExistUserInSession())
            {
                responseObject.response = false;
                responseObject.message = "No se una tiene sesión iniciada, por favor vuelva a iniciar sesión.";
                return Json(responseObject);
            }

            int idUser = SessionManager.GetUser();


            switch (data.NombreCatalogo)
            {
                case "EVENTO":
                    resultHttpRequest = await crud.CrearEvento(new EventRegisterDto() { 
                        ID_EVENT_TYPE=Convert.ToInt32(data.AtributoAdicionalStr1), 
                        DESC_EVENT_SP=data.Descripcion, 
                        DESC_EVENT_EN=data.Descripcion, 
                        CREATED_BY = idUser, 
                        DATE_INI=Convert.ToDateTime(data.AtributoAdicional1Dte), 
                        DATE_FIN=Convert.ToDateTime(data.AtributoAdicional2Dte),
                        ID_ESTATE = Convert.ToInt16(data.AtributoAdicionalStr2),
                        DESC_LOCATION = data.AtributoAdicionalStr3
                    });
                    break;

                case "PAIS":
                    resultHttpRequest = await crud.CrearPais(new CountryRegisterDto() { DESC_COUNTRY_SP = data.Descripcion, DESC_COUNTRY_EN = data.Descripcion, CREATED_BY = idUser });
                    break;

                case "EMPRESA":
                    resultHttpRequest = await crud.CrearEmpresa(new CompanyRegisterDto() {DESC_COMPANY_EN = data.Descripcion, DESC_COMPANY_SP= data.Descripcion, LEGAL_REPRESENTATIVE= "LEGAL_REPRESENTATIVE", CREATED_BY= idUser });
                    break;

                case "DOCUMENTO":
                    resultHttpRequest = await crud.CrearDocumento(new RegInviteDto() { DES_TITLE = data.AtributoAdicionalStr1, DESC_SPANISH = data.AtributoAdicionalStr2, DESC_ENGLISH = data.AtributoAdicionalStr3, SIGN_1 = data.AtributoAdicionalStr4, FOOT_PAGE = data.AtributoAdicionalStr5, ID_EVENT_TYPE = Convert.ToInt32(data.AtributoAdicionalStr6), CREATED_BY= idUser });
                    break;

                case "AEROLINEAS":
                    resultHttpRequest = await crud.CrearAerolinea(new AirLineRegisterDto() { DESC_AIR_LINE_SP = data.Descripcion, DESC_AIR_LINE_EN = data.Descripcion, CREATED_BY = idUser });
                    break;

                case "AEROPUERTOS":
                    resultHttpRequest = await crud.CrearAeropuerto(new AirPortRegisterDto() { DESC_AIRPORT_SP = data.Descripcion, DESC_AIRPORT_EN = data.Descripcion, CREATED_BY = idUser });
                    break;

                case "NACIONALIDADES":
                    resultHttpRequest = await crud.CrearNacionalidad(new NationalityRegisterDto() { DESC_NACIONALITY_SP = data.Descripcion, DESC_NACIONALITY_EN = data.Descripcion, CREATED_BY = idUser, RESTRICTION = data.AtributoAdicionalStr1.Equals("SI") ?true:false });
                    break;

                case "INMUEBLES":
                    resultHttpRequest = await crud.CrearInmueble(new EstateRegisterDto() { DESC_ESTATE_SP = data.Descripcion, DESC_ESTATE_EN = data.Descripcion, CREATED_BY = idUser });
                    break;

                case "SEXOS":
                    resultHttpRequest = await crud.CrearGenero(new GenderRegisterDto() { DESC_GENDER_SP = data.Descripcion, DESC_GENDER_EN = data.AtributoAdicionalStr1, CREATED_BY = idUser });
                    break;

                case "TIPOEVENTOS":
                    resultHttpRequest = await crud.CrearTipoEvento(new EventTypesRegisterDto() { DESC_ACTIVITY_SP = data.Descripcion, DESC_ACTIVITY_EN= data.Descripcion, CREATED_BY = idUser });
                    break;

                case "USUARIOS":
                    resultHttpRequest = await crud.CrearUsuario(new UserRegisterDto() { NAME_USER = data.Descripcion, LAST_NAME_USER = data.AtributoAdicionalStr1, CVE_USER = "ADMIN", 
                        USERNAME= data.AtributoAdicionalStr2, PASSWORD_USER = data.AtributoAdicionalStr3, ACTIVE=true, CREATED_BY = idUser, ID_ROLE = Convert.ToInt32(data.AtributoAdicionalStr4),
                        EMAIL_USER = data.AtributoAdicionalStr5
                    });
                    break;


                case "PDF":

                    var streamFile = ArchivoPDF.InputStream;
                    byte[] fileBytes = streamFile.ReadAsBytes();
                    var base64File = Convert.ToBase64String(fileBytes);

                    //Byte[] bytes = File.ReadAllBytes(data.AtributoAdicionalStr3);
                    //String file = Convert.ToBase64String(bytes);
                    resultHttpRequest = await crud.CrearPDF(new DocumentRegisterDto() {FILE_BLOB=base64File, ID_EVENT_TYPE = Convert.ToInt32(data.AtributoAdicionalStr1), DESC_SPANISH = data.AtributoAdicionalStr2, DESC_ENGLISH= data.AtributoAdicionalStr2, CREATED_BY = idUser });
                    break;

               

                default:
                    resultHttpRequest = null;
                    break;
            }

            if(resultHttpRequest == null)
            {
                responseObject.response = false;
                responseObject.message = "No se encontro ningun caso de guardado, enviar la bandera correcta al metodo";
            }
            else
            {
                if (resultHttpRequest.error)
                {
                    responseObject.response = false;
                    responseObject.message = resultHttpRequest.message;
                }
                else
                {
                    responseObject.response = true;
                    responseObject.message = "Guardado exitoso";
                }
            }

            return Json(responseObject);
        }


        public async Task<JsonResult> ActualizarElementoCatalogo(CatalogoGeneral data, HttpPostedFileBase ArchivoPDF, HttpPostedFileBase ArchivoImagen)
        {
            responseObject = new ResponseModel();

            crud = new CRUDManager(httpManager);
            ResponseDto resultHttpRequest = new ResponseDto();

            if (!SessionManager.ExistUserInSession())
            {
                responseObject.response = false;
                responseObject.message = "No se una tiene sesión iniciada, por favor vuelva a iniciar sesión.";
                return Json(responseObject);
            }

            int idUser = SessionManager.GetUser();

            switch (data.NombreCatalogo)
            {
                case "EVENTO":
                    resultHttpRequest = await crud.ActualizarEvento(data.Id, new EventRegisterDto()
                    {
                        ID_EVENT_TYPE = Convert.ToInt32(data.AtributoAdicionalStr1),
                        DESC_EVENT_SP = data.Descripcion,
                        DESC_EVENT_EN = data.Descripcion,
                        DATE_INI = Convert.ToDateTime(data.AtributoAdicional1Dte),
                        DATE_FIN = Convert.ToDateTime(data.AtributoAdicional2Dte),
                        ID_ESTATE = Convert.ToInt16(data.AtributoAdicionalStr2),
                        DESC_LOCATION = data.AtributoAdicionalStr3,
                        MODIFY_BY = idUser
                    }) ;
                    break;

                case "PAIS":
                    resultHttpRequest = await crud.ActualizarPais(data.Id, new CountryRegisterDto() { DESC_COUNTRY_SP = data.Descripcion, DESC_COUNTRY_EN = data.Descripcion, RESTRICTION = false,  MODIFY_BY=idUser});
                    break;

                case "EMPRESA":
                    resultHttpRequest = await crud.ActualizarEmpresa(data.Id, new CompanyRegisterDto() { DESC_COMPANY_EN = data.Descripcion, DESC_COMPANY_SP = data.Descripcion, MODIFY_BY = idUser });
                    break;
                case "DOCUMENTO":
                    resultHttpRequest = await crud.ActualizarDocumento(data.Id, new RegInviteDto() { DES_TITLE=data.AtributoAdicionalStr1, DESC_SPANISH = data.AtributoAdicionalStr2, DESC_ENGLISH = data.AtributoAdicionalStr3, SIGN_1 = data.AtributoAdicionalStr4, FOOT_PAGE = data.AtributoAdicionalStr5, ID_EVENT_TYPE=Convert.ToInt32(data.AtributoAdicionalStr6), MODIFY_BY = idUser });
                    break;
                case "AEROLINEAS":
                    resultHttpRequest = await crud.ActualizarAerolinea(data.Id, new AirLineRegisterDto() { DESC_AIR_LINE_SP=data.Descripcion, DESC_AIR_LINE_EN = data.Descripcion, MODIFY_BY = idUser });
                    break;
                case "AEROPUERTOS":
                    resultHttpRequest = await crud.ActualizarAeropuerto(data.Id, new AirPortRegisterDto() { DESC_AIRPORT_SP = data.Descripcion, DESC_AIRPORT_EN = data.Descripcion, MODIFY_BY = idUser });
                    break; 
                case "NACIONALIDADES":
                    resultHttpRequest = await crud.ActualizarNacionalidad(data.Id, new NationalityRegisterDto() { DESC_NACIONALITY_SP = data.Descripcion, DESC_NACIONALITY_EN = data.Descripcion, RESTRICTION = data.AtributoAdicionalStr1.Equals("SI") ? true : false, MODIFY_BY = idUser });
                    break;
                case "INMUEBLES":
                    resultHttpRequest = await crud.ActualizarInmueble(data.Id, new EstateRegisterDto() { DESC_ESTATE_SP = data.Descripcion, DESC_ESTATE_EN = data.Descripcion, MODIFY_BY = idUser });
                    break;
                case "SEXOS":
                    resultHttpRequest = await crud.ActualizarGenero(data.Id, new GenderRegisterDto() { DESC_GENDER_SP = data.Descripcion, DESC_GENDER_EN = data.AtributoAdicionalStr1, MODIFY_BY = idUser });
                    break;
                case "TIPOEVENTOS":
                    resultHttpRequest = await crud.ActualizarTipoEvento(data.Id, new EventTypesRegisterDto() { DESC_ACTIVITY_EN = data.Descripcion, DESC_ACTIVITY_SP = data.Descripcion, MODIFY_BY = idUser });
                    break;

                case "USUARIOS":
                    resultHttpRequest = await crud.ActualizarUsuario(data.Id, new UserRegisterDto() { NAME_USER = data.Descripcion, CVE_USER = "ADMIN", 
                        USERNAME = data.AtributoAdicionalStr2, PASSWORD_USER = data.AtributoAdicionalStr3, LAST_NAME_USER = data.AtributoAdicionalStr1, 
                        MODIFY_BY = idUser, ID_ROLE = Convert.ToInt32(data.AtributoAdicionalStr4), EMAIL_USER= data.AtributoAdicionalStr5
                    });
                    break;


                case "PDF":

                    var streamFile = ArchivoPDF.InputStream;
                    byte[] fileBytes = streamFile.ReadAsBytes();
                    var base64File = Convert.ToBase64String(fileBytes);


                    resultHttpRequest = await crud.ActualizarPDF(data.Id, new DocumentRegisterDto() { FILE_BLOB = base64File, ID_EVENT_TYPE = Convert.ToInt32(data.AtributoAdicionalStr1), DESC_SPANISH = data.AtributoAdicionalStr2, DESC_ENGLISH = data.AtributoAdicionalStr2, CREATED_BY = 1, MODIFY_BY = idUser });
                    break;

                case "IMAGENFIRMA":
                    var streamFileImg = ArchivoImagen.InputStream;
                    byte[] fileBytesImg = streamFileImg.ReadAsBytes();
                    var base64FileImg = Convert.ToBase64String(fileBytesImg);

                    resultHttpRequest = await crud.ActualizarImagenFirma(data.Id, new RegInviteDto() { SIGN_BLOB = base64FileImg, MODIFY_BY = idUser });
                    break;

                case "AVISOPRIVACIDAD":
                    resultHttpRequest = await crud.ActualizarAvisoPrivacidad(data.Id, new AvisoPrivacidadDto() { AVISO_ESP = data.AtributoAdicionalStr1, AVISO_ENG = data.AtributoAdicionalStr2 });
                    break;
                default:
                    resultHttpRequest = null;
                    break;
            }

            if (resultHttpRequest == null)
            {
                responseObject.response = false;
                responseObject.message = "No se encontro ningun caso de guardado, enviar la bandera correcta al metodo";
            }
            else
            {
                if (resultHttpRequest.error)
                {
                    responseObject.response = false;
                    responseObject.message = resultHttpRequest.message;
                }
                else
                {
                    responseObject.response = true;
                    responseObject.message = "Guardado exitoso";
                }
            }

            return Json(responseObject);
        }

        public async Task<JsonResult> EliminarElementoCatalogo(CatalogoGeneral data)
        {
            responseObject = new ResponseModel();

            crud = new CRUDManager(httpManager);
            ResponseDto resultHttpRequest = new ResponseDto();
            string mensajeRespuesta = "Borrado exitoso";
            switch (data.NombreCatalogo)
            {
                case "EVENTO":
                    resultHttpRequest = await crud.EliminarEvento(new EventsDto() { ID_EVENT = data.Id});
                    break;
                case "PAIS":
                    resultHttpRequest = await crud.EliminarPais(new CountriesDto() { ID_COUNTRY = data.Id });
                    break;
                case "EMPRESA":
                    resultHttpRequest = await crud.EliminarEmpresa(new CompaniesDto() { ID_COMPANY = data.Id });
                    break;
                case "DOCUMENTO":
                    resultHttpRequest = await crud.EliminarDocumento(new InviteDto() { ID_INVITE = data.Id });
                    break;
                case "AEROLINEAS":
                    resultHttpRequest = await crud.EliminarAerolinea(new AirLinesDto() { ID_AIR_LINE = data.Id });
                    break;
                case "AEROPUERTOS":
                    resultHttpRequest = await crud.EliminarAeropuerto(new AirPortsDto() { ID_AIRPORT = data.Id });
                    break;
                case "NACIONALIDADES":
                    resultHttpRequest = await crud.EliminarNacionalidad(new NationalitiesDto() { ID_NALCIONALITY = data.Id });
                    break;
                case "INMUEBLES":
                    resultHttpRequest = await crud.EliminarInmueble(new EstatesDto() { ID_ESTATE = data.Id });
                    break;
                case "SEXOS":
                    resultHttpRequest = await crud.EliminarGenero(new GendersDto() { ID_GENDER = data.Id });
                    break;
                case "TIPOEVENTOS":
                    resultHttpRequest = await crud.EliminarTipoEvento(new EventTypesDto() { ID_EVENT_TYPE = data.Id });
                    break;
                case "USUARIOS":
                    resultHttpRequest = await crud.EliminarUsuario(new UserDto() { ID_USER = data.Id });
                    mensajeRespuesta = "Desactivación exitosa";
                    break;
                case "PERFILES":
                    resultHttpRequest = await crud.EliminarPerfil(new RoleByCompanyDto() { ID_ROLE = data.Id });
                    break;
                case "PDF":
                    resultHttpRequest = await crud.EliminarPdf(new DocumentDto() { ID_INVITE = data.Id });
                    break;
                default:
                    resultHttpRequest = null;
                    break;
            }

            if (resultHttpRequest == null)
            {
                responseObject.response = false;
                responseObject.message = "No se encontro ningun caso de guardado, enviar la bandera correcta al metodo";
            }
            else
            {
                if (resultHttpRequest.error)
                {
                    responseObject.response = false;
                    responseObject.message = resultHttpRequest.message;
                }
                else
                {
                    responseObject.response = true;
                    responseObject.message = mensajeRespuesta;
                }
            }

            return Json(responseObject);
        }


        public async Task<JsonResult> ActivarUsuario(CatalogoGeneral data)
        {
            responseObject = new ResponseModel();

            crud = new CRUDManager(httpManager);
            ResponseDto resultHttpRequest = new ResponseDto();
            resultHttpRequest = await crud.ActivarUsuario(new UserDto() { ID_USER = data.Id });

            if (resultHttpRequest == null)
            {
                responseObject.response = false;
                responseObject.message = "Error al Activar usuario";
            }
            else
            {
                if (resultHttpRequest.error)
                {
                    responseObject.response = false;
                    responseObject.message = resultHttpRequest.message;
                }
                else
                {
                    responseObject.response = true;
                    responseObject.message = "Activación exitosa";
                }
            }

            return Json(responseObject);
        }



        public async Task<JsonResult> ObtenerElementoCatalogo(CatalogoGeneral data)
        {
            responseObject = new ResponseModel();
            try
            {
                crud = new CRUDManager(httpManager);
                ResponseDto resultHttpRequest = new ResponseDto();
                CatalogoGeneral catalogResponse = new CatalogoGeneral();
                switch (data.NombreCatalogo)
                {
                    case "EVENTO":
                        resultHttpRequest = await crud.DescargaCatalogoEventoPorId(data.Id);
                        if (resultHttpRequest != null && !resultHttpRequest.error)
                        {
                            catalogResponse.Id = resultHttpRequest.response.iD_EVENT;
                            catalogResponse.AtributoAdicionalStr1 = resultHttpRequest.response.iD_EVENT_TYPE.ToString();
                            catalogResponse.Descripcion = resultHttpRequest.response.desC_EVENT_SP;
                            if (resultHttpRequest.response.datE_INI != null && DateTime.TryParse(resultHttpRequest.response.datE_INI.ToString(), out DateTime salida1))
                            {
                                catalogResponse.AtributoAdicional1Dte = resultHttpRequest.response.datE_INI.ToString();
                            }
                            if (resultHttpRequest.response.datE_FIN != null && DateTime.TryParse(resultHttpRequest.response.datE_FIN.ToString(), out DateTime salida2))
                            {
                                catalogResponse.AtributoAdicional2Dte = resultHttpRequest.response.datE_FIN.ToString();
                            }

                            catalogResponse.AtributoAdicionalStr2 = resultHttpRequest.response.iD_ESTATE;
                            catalogResponse.AtributoAdicionalStr2 = string.IsNullOrEmpty(catalogResponse.AtributoAdicionalStr2) ? "0" : catalogResponse.AtributoAdicionalStr2;

                            catalogResponse.AtributoAdicionalStr3 = resultHttpRequest.response.desC_LOCATION;
                            catalogResponse.AtributoAdicionalStr3 = string.IsNullOrEmpty(catalogResponse.AtributoAdicionalStr3) ? "" : catalogResponse.AtributoAdicionalStr3;
                        }
                        break;

                    case "PAIS":
                        resultHttpRequest = await crud.DescargaCatalogoPaisPorId(data.Id);
                        if (resultHttpRequest != null && !resultHttpRequest.error)
                        {
                            catalogResponse.Id = resultHttpRequest.response.iD_COUNTRY;
                            catalogResponse.Descripcion = resultHttpRequest.response.desC_COUNTRY_SP;
                            //catalogResponse.AtributoAdicionalStr1 = resultHttpRequest.response.restriction == true ? "SI" : "NO";
                        }
                        break;

                    case "EMPRESA":
                        resultHttpRequest = await crud.DescargaCatalogoEmpresaPorId(data.Id);
                        if (resultHttpRequest != null && !resultHttpRequest.error)
                        {
                            catalogResponse.Id = resultHttpRequest.response.iD_COMPANY;
                            catalogResponse.Descripcion = resultHttpRequest.response.desC_COMPANY_SP;
                        }
                        break;

                    case "DOCUMENTO":
                        resultHttpRequest = await crud.DescargaCatalogoDocumentoPorId(data.Id);
                        if (resultHttpRequest != null && !resultHttpRequest.error)
                        {
                            catalogResponse.Id = resultHttpRequest.response.iD_INVITE;
                            catalogResponse.AtributoAdicionalInt1 = resultHttpRequest.response.iD_EVENT_TYPE;
                            catalogResponse.AtributoAdicionalStr1 = resultHttpRequest.response.deS_TITLE;
                            catalogResponse.AtributoAdicionalStr2 = resultHttpRequest.response.desC_SPANISH;
                            catalogResponse.AtributoAdicionalStr3  = resultHttpRequest.response.desC_ENGLISH;
                            catalogResponse.AtributoAdicionalStr4  = resultHttpRequest.response.sigN_1;
                            catalogResponse.AtributoAdicionalStr5  = resultHttpRequest.response.fooT_PAGE;
                            catalogResponse.AtributoAdicionalStr6  = resultHttpRequest.response.iD_EVENT_TYPE != null ? resultHttpRequest.response.iD_EVENT_TYPE.ToString() : 0;
                        }
                        break; 

                    case "AEROLINEAS":
                        resultHttpRequest = await crud.DescargaCatalogoAerolineaPorId(data.Id);
                        if (resultHttpRequest != null && !resultHttpRequest.error)
                        {
                            catalogResponse.Id = resultHttpRequest.response.iD_AIR_LINE;
                            // catalogResponse.AtributoAdicionalInt1 = resultHttpRequest.response.desC_AIR_LINE_SP;
                            catalogResponse.Descripcion = resultHttpRequest.response.desC_AIR_LINE_SP;
                        }
                        break;

                    case "AEROPUERTOS":
                        resultHttpRequest = await crud.DescargaCatalogoAeropuertoPorId(data.Id);
                        if (resultHttpRequest != null && !resultHttpRequest.error)
                        {
                            catalogResponse.Id = resultHttpRequest.response.iD_AIRPORT;
                            // catalogResponse.AtributoAdicionalInt1 = resultHttpRequest.response.desC_AIR_LINE_SP;
                            catalogResponse.Descripcion = resultHttpRequest.response.desC_AIRPORT_SP;
                        }
                        break;

                    case "NACIONALIDADES":
                        resultHttpRequest = await crud.DescargaCatalogoNacionalidadPorId(data.Id);
                        if (resultHttpRequest != null && !resultHttpRequest.error)
                        {
                            catalogResponse.Id = resultHttpRequest.response.iD_NATIONALITY;
                            // catalogResponse.AtributoAdicionalInt1 = resultHttpRequest.response.desC_AIR_LINE_SP;
                            catalogResponse.Descripcion = resultHttpRequest.response.desC_NACIONALITY_SP;
                            catalogResponse.AtributoAdicionalStr1 = resultHttpRequest.response.restriction == true ? "SI" : "NO";
                        }
                        break;

                    case "INMUEBLES":
                        resultHttpRequest = await crud.DescargaCatalogoInmueblePorId(data.Id);
                        if (resultHttpRequest != null && !resultHttpRequest.error)
                        {
                            catalogResponse.Id = resultHttpRequest.response.iD_ESTATE;
                            // catalogResponse.AtributoAdicionalInt1 = resultHttpRequest.response.desC_AIR_LINE_SP;
                            catalogResponse.Descripcion = resultHttpRequest.response.desC_ESTATE_SP;
                        }
                        break;

                    case "SEXOS":
                        resultHttpRequest = await crud.DescargaCatalogoGeneroPorId(data.Id);
                        if (resultHttpRequest != null && !resultHttpRequest.error)
                        {
                            catalogResponse.Id = resultHttpRequest.response.iD_GENDER;
                            // catalogResponse.AtributoAdicionalInt1 = resultHttpRequest.response.desC_AIR_LINE_SP;
                            catalogResponse.Descripcion = resultHttpRequest.response.desC_GENDER_SP;
                            catalogResponse.AtributoAdicionalStr1 = resultHttpRequest.response.desC_GENDER_EN;
                        }
                        break;
                    case "USUARIOS":
                        resultHttpRequest = await crud.DescargaCatalogoUsuarioPorId(data.Id);
                        if (resultHttpRequest != null && !resultHttpRequest.error)
                        {
                            catalogResponse.Id = resultHttpRequest.response.iD_USER;
                            // catalogResponse.AtributoAdicionalInt1 = resultHttpRequest.response.desC_AIR_LINE_SP;
                            catalogResponse.Descripcion = resultHttpRequest.response.namE_USER;
                            catalogResponse.AtributoAdicionalStr1 = resultHttpRequest.response.lasT_NAME_USER;
                            catalogResponse.AtributoAdicionalStr2 = resultHttpRequest.response.username;
                            catalogResponse.AtributoAdicionalStr3 = resultHttpRequest.response.passworD_USER;
                            catalogResponse.AtributoAdicionalStr4 = resultHttpRequest.response.iD_ROLE;
                            catalogResponse.AtributoAdicionalStr5 = resultHttpRequest.response.emaiL_USER;
                        }
                        break;
                    case "PERFILES":

                        return await ObtenerDatosPerfiles(data);


                        break;
                    case "TIPOEVENTOS":
                        resultHttpRequest = await crud.DescargaCatalogoTipoEventoPorId(data.Id);
                        if (resultHttpRequest != null && !resultHttpRequest.error)
                        {
                            catalogResponse.Id = resultHttpRequest.response.iD_EVENT_TYPE;
                            // catalogResponse.AtributoAdicionalInt1 = resultHttpRequest.response.desC_AIR_LINE_SP;
                            catalogResponse.Descripcion = resultHttpRequest.response.desC_ACTIVITY_SP;
                        }
                        break;
                    case "PDF":
                        resultHttpRequest = await crud.DescargaCatalogoPDFPorId(data.Id);
                        if (resultHttpRequest != null && !resultHttpRequest.error)
                        {
                            catalogResponse.Id = resultHttpRequest.response.iD_INVITE;
                            // catalogResponse.AtributoAdicionalInt1 = resultHttpRequest.response.desC_AIR_LINE_SP;
                            catalogResponse.AtributoAdicionalStr1 = resultHttpRequest.response.iD_EVENT_TYPE;
                            catalogResponse.AtributoAdicionalStr2 = resultHttpRequest.response.desC_SPANISH;
                        }
                        break;
                    case "IMAGENFIRMA":
                        resultHttpRequest = await crud.DescargaCatalogoDocumentoPorId(data.Id);
                        if (resultHttpRequest != null && !resultHttpRequest.error)
                        {
                            catalogResponse.Id = resultHttpRequest.response.iD_INVITE;
                            catalogResponse.AtributoAdicionalStr1 = resultHttpRequest.response.deS_TITLE;
                            catalogResponse.AtributoAdicionalStr4 = resultHttpRequest.response.sigN_1;
                            catalogResponse.AtributoAdicionalStr6 = resultHttpRequest.response.iD_EVENT_TYPE != null ? resultHttpRequest.response.iD_EVENT_TYPE.ToString() : 0;
                            catalogResponse.AtributoAdicionalStr7 = resultHttpRequest.response.sigN_BLOB;

                        }
                        break;
                    default:
                        resultHttpRequest = null;
                        break;
                }

                if (resultHttpRequest == null)
                {
                    responseObject.response = false;
                    responseObject.message = "No se encontro ningun caso de guardado, enviar la bandera correcta al metodo";
                }
                else
                {
                    if (resultHttpRequest.error)
                    {
                        responseObject.response = false;
                        responseObject.message = resultHttpRequest.message;
                    }
                    else
                    {
                        responseObject.response = true;
                        responseObject.message = "Consulta exitosa";
                        responseObject.result = catalogResponse;
                    }
                }
            }
            catch(Exception e)
            {
                responseObject = new ResponseModel();
                responseObject.response = false;
                responseObject.message = e.Message;
                
            }

            return Json(responseObject);
        }


        private async Task<JsonResult> ObtenerDatosPerfiles(CatalogoGeneral data)
        {
            CatalogoPerfilesDto catalogResponse = new CatalogoPerfilesDto();
            var resultHttpRequest3 = await crud.DescargaCatalogoModulos();
            // Descargar todos los modulos que existen
            List<ProcessCatalog> processCatalogList = new List<ProcessCatalog>();
            if (resultHttpRequest3 != null && !resultHttpRequest3.error)
            {

                foreach (var currentValue in resultHttpRequest3.response)
                {
                    ProcessCatalog processCatalog = new ProcessCatalog();
                    processCatalog.DESC_PROCESS_SP = currentValue.desC_PROCESS_SP;
                    processCatalog.DESC_PROCESS_EN = currentValue.desC_PROCESS_EN;
                    processCatalog.ID_PROCESS = currentValue.iD_PROCESS;
                    processCatalogList.Add(processCatalog);
                }
            }

            if (data != null && data.Id != 0)
            {
                var resultHttpRequest = await crud.DescargaCatalogoPerfilesPorId(data.Id);
                var resultHttpRequest2 = await crud.DescargaCatalogoModulosPorIdRol(data.Id);


                if (resultHttpRequest != null && !resultHttpRequest.error)
                {
                    catalogResponse.IdRol = resultHttpRequest.response.iD_ROLE;
                    // catalogResponse.AtributoAdicionalInt1 = resultHttpRequest.response.desC_AIR_LINE_SP;
                    catalogResponse.Descripcion = resultHttpRequest.response.desC_ROLE_SP;
                }

               

                // Descargar los modulos que tiene asignados el rol para activarlos en frontend
                if (resultHttpRequest2 != null && !resultHttpRequest2.error)
                {
                    foreach (var currentValue in resultHttpRequest2.response)
                    {
                        foreach (ProcessCatalog processCatalog in processCatalogList)
                        {
                            if (currentValue.iD_PROCESS == processCatalog.ID_PROCESS)
                            {
                                processCatalog.moduloActivo = true;
                                break;
                            }
                        }
                    }
                }
            }

            catalogResponse.HtmlTablaModulos = $"<table class=\"table table-condensed\" id=\"registerFound\">" +
                $"<tr>" +
                $"<th>MÓDULO</th><th>ACTIVAR</th>" +
                $"</tr>";
            string filasTabla = "";
            string estaMarcado;
            int indiceElemento = 0;
            foreach (var processCatalog in processCatalogList)
            {
                estaMarcado = "";
                if (processCatalog.moduloActivo)
                    estaMarcado = "checked=\"checked\"";
                filasTabla += $"<tr><td style =\"vertical-align:middle;\">{processCatalog.DESC_PROCESS_SP}<input type='hidden' id=\"Modulos_{indiceElemento}__ID_PROCESS\" name=\"Modulos[{indiceElemento}].ID_PROCESS\" value='{processCatalog.ID_PROCESS}' /></td>" +
                $"<td style=\"vertical-align:middle;\">" +
                $"<input type=\"checkbox\" {estaMarcado} data-val=\"true\" data-val-required=\"The Tea field is required.\" id=\"Modulos_{indiceElemento}__moduloActivo\" name=\"Modulos[{indiceElemento}].moduloActivo\"  value=\"true\" />" +
                $"<input name=\"Modulos[{indiceElemento}].moduloActivo\" type=\"hidden\" value=\"false\" />" +
                $"<input type='hidden' id=\"Modulos_{indiceElemento}__DESC_PROCESS_SP\" name=\"Modulos[{indiceElemento}].DESC_PROCESS_SP\" value='{processCatalog.DESC_PROCESS_SP}' />" +
                $"<input type='hidden' id=\"Modulos_{indiceElemento}__DESC_PROCESS_EN\" name=\"Modulos[{indiceElemento}].DESC_PROCESS_EN\" value='{processCatalog.DESC_PROCESS_EN}' />" +
                $"<input type='hidden' id=\"Modulos_{indiceElemento}__ACTIVE\" name=\"Modulos[{indiceElemento}].ACTIVE\" value='{processCatalog.ACTIVE}' />" +
                $"<input type='hidden' id=\"Modulos_{indiceElemento}__CREATED_DATE\" name=\"Modulos[{indiceElemento}].CREATED_DATE\" value='{processCatalog.CREATED_DATE}' />" +
                $"</td></tr>";
                indiceElemento++;
            }

            catalogResponse.HtmlTablaModulos += filasTabla;
            catalogResponse.HtmlTablaModulos += $"</table>";


            catalogResponse.Modulos = processCatalogList;

            responseObject = new ResponseModel();
            responseObject.response = true;
            responseObject.message = "Consulta exitosa";
            responseObject.result = catalogResponse;

            return Json(responseObject);

        }


        public async Task<JsonResult> ActualizarCatalogoPerfile(CatalogoPerfilesDto data)
        {
            responseObject = new ResponseModel();

            crud = new CRUDManager(httpManager);
            ResponseDto resultHttpRequest = new ResponseDto();

            if (!SessionManager.ExistUserInSession())
            {
                responseObject.response = false;
                responseObject.message = "No se una tiene sesión iniciada, por favor vuelva a iniciar sesión.";
                return Json(responseObject);
            }

            int idUser = SessionManager.GetUser();

            resultHttpRequest = await crud.ActualizarPerfil(data.IdRol, new CatalogoPerfilesDto() { Descripcion = data.Descripcion, IdRol = data.IdRol, MODIFY_BY = idUser, Modulos=data.Modulos });

            if (resultHttpRequest == null)
            {
                responseObject.response = false;
                responseObject.message = "No se encontro ningun caso de guardado, enviar la bandera correcta al metodo";
            }
            else
            {
                if (resultHttpRequest.error)
                {
                    responseObject.response = false;
                    responseObject.message = resultHttpRequest.message;
                }
                else
                {
                    responseObject.response = true;
                    responseObject.message = "Guardado exitoso";
                }
            }

            return Json(responseObject);
        }

        public async Task<JsonResult> CreaCatalogoPerfile(CatalogoPerfilesDto data)
        {
            responseObject = new ResponseModel();

            crud = new CRUDManager(httpManager);
            ResponseDto resultHttpRequest = new ResponseDto();

            if (!SessionManager.ExistUserInSession())
            {
                responseObject.response = false;
                responseObject.message = "No se una tiene sesión iniciada, por favor vuelva a iniciar sesión.";
                return Json(responseObject);
            }

            int idUser = SessionManager.GetUser();

            resultHttpRequest = await crud.CrearPerfil( new CatalogoPerfilesDto() { Descripcion = data.Descripcion, IdRol = data.IdRol, CREATED_BY = idUser, Modulos = data.Modulos });

            if (resultHttpRequest == null)
            {
                responseObject.response = false;
                responseObject.message = "No se encontro ningun caso de guardado, enviar la bandera correcta al metodo";
            }
            else
            {
                if (resultHttpRequest.error)
                {
                    responseObject.response = false;
                    responseObject.message = resultHttpRequest.message;
                }
                else
                {
                    responseObject.response = true;
                    responseObject.message = "Guardado exitoso";
                }
            }

            return Json(responseObject);
        }
    }
}