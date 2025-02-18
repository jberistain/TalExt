using CommonTools.DTOs.Query;
using CommonTools.DTOs.Register;
using CommonTools.Pdf;
using MigracionTalentoExtranjero.Models.Administrator;
using MigracionTalentoExtranjero.Models.Catalogs;
using MigracionTalentoExtranjero.Models.Enum;
using MigracionTalentoExtranjero.Models.Home;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigracionTalentoExtranjero.Models.Utils
{
    public class CRUDManager
    {
        private HttpManager http;
        private CatalogsManager catalogs;
        public CRUDManager(HttpManager http) { 
            this.http = http;
        }


        public async Task<List<Registro>> DescargarRegistros(FilterRegisterDto filter)
        {
            List<Registro> result = new List<Registro>();

            ResponseDto responseHttp = await http.PostAsJsonAsync<FilterRegisterDto, ResponseDto>(filter, WebAPIEndPointsEnum.CONSULTA_REGISTROS_ENCONTRADOS.GetString());
            if (!responseHttp.error)
            {
                foreach (var currentValue in responseHttp.response)
                {
                    string marcarEspecial = "";
                    if (currentValue.checK_VERIFY == true)
                        marcarEspecial = "**";
                    result.Add(new Registro()
                    {
                        Id = currentValue.iD_REG,
                        //Empresa = currentValue.iD_AIR_LINE,
                        Empresa = currentValue.eventos,
                        
                        CorreoElectronico = currentValue.email,
                        Estatus = currentValue.iD_STATUS,
                        EstatusDesc = currentValue.caT_STATUS.desC_STATUS_SP,
                        FolioYFechaRegistro = $"{currentValue.secreT_CODE}\n{Convert.ToDateTime( currentValue.createD_DATE).ToString("dd/MM/yyyy")}",
                        Nacionalidad = currentValue.caT_NATIONALITIES.desC_NACIONALITY_SP,
                        Nombre = $"{marcarEspecial} {currentValue.passporT_NAME} {currentValue.passporT_LASTNAME}",
                        Pasaporte = currentValue.passporT_NUM,
                        Actividad = currentValue.activitY_MEXICO,
                        ClaveSecreta = currentValue.secreT_CODE,
                        
                    });
                }
            }
            return result;
        }

        public async Task<RegistroInvitadoModel> DescargarRegistroPorId(int id)
        {
            RegistroInvitadoModel result = new RegistroInvitadoModel();

            Dictionary<string, string> requestParams = new Dictionary<string, string>();
            requestParams.Add("id", id.ToString());

            ResponseDto responseHttp = await http.GetAsJsonAsync<ResponseDto>(WebAPIEndPointsEnum.CONSULTA_REGISTRO_POR_ID_REGISTRO.GetString(), requestParams);
            if (!responseHttp.error)
            {
                    DateTime FechaInicioVigencia = Convert.ToDateTime( responseHttp.response.datE_VIG_INI);
                    DateTime FechaFinVigencia = Convert.ToDateTime( responseHttp.response.datE_EVENT_FIN);

                DateTime FechaLlegada = Convert.ToDateTime(responseHttp.response.datE_ARRIVE);
                DateTime FechaSalida = Convert.ToDateTime(responseHttp.response.datE_LEAVE);

                string expulsadoDeMexicoStr = responseHttp.response.expelleD_MEX == true ? "SI" : "NO";
                string antecedentesPenalesMexico = responseHttp.response.criminaL_RECORD_MEX == true ? "SI" : "NO";

                result = new RegistroInvitadoModel()
                    {
                        Id = responseHttp.response.iD_REG,
                        Sexo = responseHttp.response.iD_GENDER,
                        PaisNacimiento = responseHttp.response.iD_COUNTRY,
                        Nacionalidad = responseHttp.response.iD_NATIONALITY,
                        
                        AeropuertoLlegada = responseHttp.response.iD_AIRPORT,
                        Aerolinea = responseHttp.response.iD_AIR_LINE,
                        NumeroPasaporte = responseHttp.response.passporT_NUM,
                        DiaExpPas = FechaInicioVigencia.ToString("dd"),
                        MesExpPas = FechaInicioVigencia.ToString("MM"),
                        AnioExpPas = FechaInicioVigencia.ToString("yyyy"),
                        DiaVenPas = FechaFinVigencia.ToString("dd"),
                        MesVenPas = FechaFinVigencia.ToString("MM"),
                        AnioVenPas = FechaFinVigencia.ToString("yyyy"),
                        Nombre = responseHttp.response.passporT_NAME,
                        Apellidos = responseHttp.response.passporT_LASTNAME,
                        Correo = responseHttp.response.email,
                        ActividadPaisResidencia = responseHttp.response.activitY_COUNTRY,
                        FueExpulsadoDeMexico = expulsadoDeMexicoStr,
                        AntecedentesPenalesEnMexico = antecedentesPenalesMexico,
                        ActividadEnMexico = responseHttp.response.activitY_MEXICO,
                        NumeroVuelo = responseHttp.response.flighT_NUMBER,
                        CodigoSecreto = responseHttp.response.secreT_CODE,
                    DiaEntrada = FechaLlegada.ToString("dd"),
                    MesEntrada = FechaLlegada.ToString("MM"),
                    AnioEntrada = FechaLlegada.ToString("yyyy"),
                    DiaSalida = FechaSalida.ToString("dd"),
                    MesSalida = FechaSalida.ToString("MM"),
                    AnioSalida = FechaSalida.ToString("yyyy"),
                    Empresa = responseHttp.response.iD_COMPANY,
                    ConfirmacionCorreo = responseHttp.response.email,
                    ExplicacionAntecedentesExpulsion = responseHttp.response.expelleD_MEX_DESC,
                    CHECK_VERIFY = responseHttp.response.checK_VERIFY,
                    
                    //Eventos = responseHttp.response.events
                };
                List<InfoEventoModel> Eventos = new List<InfoEventoModel>();
                if (responseHttp.response.events != null)
                {
                    foreach (var currentValue in responseHttp.response.events)
                    {
                        DateTime FechaInicio;
                        string auxiliarFechaStr = currentValue.evenT_DATE.ToString();
                        bool esFechaInicioValida = DateTime.TryParse(auxiliarFechaStr, out FechaInicio);
                        DateTime FechaFin;
                        auxiliarFechaStr = currentValue.evenT_DATE_FIN.ToString();
                        bool esFechaFinValida = DateTime.TryParse(auxiliarFechaStr, out FechaFin);
                        if (esFechaInicioValida && esFechaFinValida)
                        {
                            Eventos.Add(new CommonTools.Pdf.InfoEventoModel()
                            {
                                Id = currentValue.iD_REG_EVEN_DATE,
                                IdEvento = currentValue.iD_EVENT,
                                IdInmuebleEvento = currentValue.iD_ESTATE,
                                NombreEvento = currentValue.iD_EVENT,
                                InmuebleEvento= currentValue.iD_ESTATE,
                                UbicacionInmueble = currentValue.desC_LOCATION,
                                DiaInicioEvento = FechaInicio.ToString("dd"),
                                MesInicioEvento = FechaInicio.ToString("MM"),
                                AnioInicioEvento = FechaInicio.ToString("yyyy"),
                                DiaFinEvento = FechaFin.ToString("dd"),
                                MesFinEvento = FechaFin.ToString("MM"),
                                AnioFinEvento = FechaFin.ToString("yyyy"),
                            });
                        }
                    }
                }
                if (Eventos.Count == 0)
                    result.Eventos = new List<InfoEventoModel>() { new InfoEventoModel() };
                else
                    result.Eventos = Eventos;
            }
            return result;
        }

        public async Task<RegistroInvitadoModel> DescargarRegistroPorCodigoSecreto(string secretCode)
        {
            RegistroInvitadoModel result = null;

            Dictionary<string, string> requestParams = new Dictionary<string, string>();
            requestParams.Add("secretCode", secretCode);

            ResponseDto responseHttp = await http.GetAsJsonAsync<ResponseDto>(WebAPIEndPointsEnum.CONSULTA_REGISTRO_POR_CODIGO_SECRETO.GetString(), requestParams);
            if (!responseHttp.error)
            {
                DateTime FechaInicioVigencia = Convert.ToDateTime(responseHttp.response.datE_VIG_INI);
                DateTime FechaFinVigencia = Convert.ToDateTime(responseHttp.response.datE_VIG_INI);

                result = new RegistroInvitadoModel()
                {
                    Id = responseHttp.response.iD_REG,
                    Sexo = responseHttp.response.iD_GENDER,
                    PaisNacimiento = responseHttp.response.iD_COUNTRY,
                    Nacionalidad = responseHttp.response.iD_NATIONALITY,

                    AeropuertoLlegada = responseHttp.response.iD_AIRPORT,
                    Aerolinea = responseHttp.response.iD_AIR_LINE,
                    NumeroPasaporte = responseHttp.response.passporT_NUM,
                    DiaExpPas = FechaInicioVigencia.ToString("dd"),
                    MesExpPas = FechaInicioVigencia.ToString("MM"),
                    AnioExpPas = FechaInicioVigencia.ToString("yyyy"),
                    DiaVenPas = FechaFinVigencia.ToString("dd"),
                    MesVenPas = FechaFinVigencia.ToString("MM"),
                    AnioVenPas = FechaFinVigencia.ToString("yyyy"),
                    Nombre = responseHttp.response.passporT_NAME,
                    Apellidos = responseHttp.response.passporT_LASTNAME,
                    Correo = responseHttp.response.email,
                    ActividadPaisResidencia = responseHttp.response.actuaL_JOB,
                    FueExpulsadoDeMexico = responseHttp.response.expelleD_MEX,
                    AntecedentesPenalesEnMexico = responseHttp.response.criminaL_RECORD_MEX,
                    ActividadEnMexico = responseHttp.response.evenT_JOB,
                    NumeroVuelo = responseHttp.response.flighT_NUMBER,
                    CodigoSecreto = responseHttp.response.secreT_CODE,
                    PaisNacimientoDesc=responseHttp.response.caT_COUNTRIES.desC_COUNTRY_SP,

                    IdStatusActual = responseHttp.response.iD_STATUS
                };

            }
            return result;
        }
           public async Task<List<InviteDto>> DescargarDocumentosPorCodigoSecreto(string secretCode)
        {
            

            Dictionary<string, string> requestParams = new Dictionary<string, string>();
            requestParams.Add("secretCode", secretCode);

            ResponseDto responseHttp = await http.GetAsJsonAsync<ResponseDto>(WebAPIEndPointsEnum.CONSULTA_DOCUMENTOS_POR_CODIGO_SECRETO.GetString(), requestParams);
            if (!responseHttp.error)
            {
                if (responseHttp.response != null)
                {
                    List<InviteDto> resultList = new List<InviteDto>();
                    foreach (var doc in responseHttp.response)
                    {
                        resultList.Add(new InviteDto()
                        {
                            ID_INVITE = doc.iD_INVITE,
                            ID_REG = doc.iD_REG,
                            ID_EVENT_TYPE = doc.iD_EVENT_TYPE,
                            DES_TITLE = doc.deS_TITLE,
                            DESC_SPANISH = doc.desC_SPANISH,
                            DESC_ENGLISH = doc.desC_ENGLISH,
                            SIGN_1 = doc.sigN_1,
                            SIGN_2 = doc.sigN_2,
                            SIGN_3 = doc.sigN_3,
                            SIGN_4 = doc.sigN_4,
                            FOOT_PAGE = doc.fooT_PAGE,
                            FILE_NAME = doc.filE_NAME,
                            INVITE_XML = doc.invitE_XML,
                            ACTIVE = doc.active
                        });
                    }
                    return resultList;
                }
                else
                    return null;
            }
            return null;
        }


        public async Task<List<DocumentDto>> DescargarDocumentosPDFPorCodigoSecreto(string secretCode)
        {


            Dictionary<string, string> requestParams = new Dictionary<string, string>();
            requestParams.Add("secretCode", secretCode);

            ResponseDto responseHttp = await http.GetAsJsonAsync<ResponseDto>(WebAPIEndPointsEnum.CONSULTA_DOCUMENTOS_POR_CODIGO_SECRETO.GetString(), requestParams);
            if (!responseHttp.error)
            {
                if (responseHttp.secondResponse != null)
                {
                    List<DocumentDto> resultList = new List<DocumentDto>();
                    foreach (var doc in responseHttp.secondResponse)
                    {
                        resultList.Add(new DocumentDto()
                        {
                            ID_INVITE = doc.iD_INVITE,
                            NAME = doc.name,
                            ID_REG = doc.iD_REG,
                            ID_EVENT_TYPE = doc.iD_EVENT_TYPE,
                            DESC_SPANISH = doc.desC_SPANISH,
                            DESC_ENGLISH = doc.desC_ENGLISH,
                            FILE_BLOB = doc.filE_BLOB
                        });
                    }
                    return resultList;
                }
                else
                    return null;
            }
            return null;
        }


        public async Task<List<InviteDto>> GetAllDocsByRestrictedFlag(bool esRestringido)
        {


            Dictionary<string, string> requestParams = new Dictionary<string, string>();
            requestParams.Add("isRestricted", esRestringido.ToString());

            ResponseDto responseHttp = await http.GetAsJsonAsync<ResponseDto>(WebAPIEndPointsEnum.CONSULTA_DOCUMENTOS_POR_TIPO_RESTRICCION.GetString(), requestParams);
            if (!responseHttp.error)
            {
                if (responseHttp.response != null)
                {
                    List<InviteDto> resultList = new List<InviteDto>();
                    foreach (var doc in responseHttp.response)
                    {
                        resultList.Add(new InviteDto()
                        {
                            ID_INVITE = doc.iD_INVITE,
                            ID_REG = doc.iD_REG,
                            ID_EVENT_TYPE = doc.iD_EVENT_TYPE,
                            DES_TITLE = doc.deS_TITLE,
                            DESC_SPANISH = doc.desC_SPANISH,
                            DESC_ENGLISH = doc.desC_ENGLISH,
                            SIGN_1 = doc.sigN_1,
                            SIGN_2 = doc.sigN_2,
                            SIGN_3 = doc.sigN_3,
                            SIGN_4 = doc.sigN_4,
                            FOOT_PAGE = doc.fooT_PAGE,
                            FILE_NAME = doc.filE_NAME,
                            INVITE_XML = doc.invitE_XML,
                            ACTIVE = doc.active
                        });
                    }
                    return resultList;
                }
                else
                    return null;
            }
            return null;
        }



        public async Task<InviteDto> DescargarDocumentoPorId(string idDocumento)
        {
            Dictionary<string, string> requestParams = new Dictionary<string, string>();
            requestParams.Add("id", idDocumento);

            ResponseDto responseHttp = await http.GetAsJsonAsync<ResponseDto>(WebAPIEndPointsEnum.CONSULTA_DOCUMENTO_POR_ID.GetString(), requestParams);

            InviteDto result = null;
            if (!responseHttp.error)
            {
                if (responseHttp.response != null)
                {
                    var doc = responseHttp.response;
                    result = new InviteDto()
                    {
                        ID_INVITE = doc.iD_INVITE,
                        ID_REG = doc.iD_REG,
                        ID_EVENT_TYPE = doc.iD_EVENT_TYPE,
                        DES_TITLE = doc.deS_TITLE,
                        DESC_SPANISH = doc.desC_SPANISH,
                        DESC_ENGLISH = doc.desC_ENGLISH,
                        SIGN_1 = doc.sigN_1,
                        SIGN_2 = doc.sigN_2,
                        SIGN_3 = doc.sigN_3,
                        SIGN_4 = doc.sigN_4,
                        FOOT_PAGE = doc.fooT_PAGE,
                        FILE_NAME = doc.filE_NAME,
                        INVITE_XML = doc.invitE_XML,
                        ACTIVE = doc.active,
                        SIGN_BLOB = doc.sigN_BLOB,
                    };
                }
            }
            return result;
        }



        public async Task<DocumentDto> DescargarPDFPorId(string idDocumento)
        {
            Dictionary<string, string> requestParams = new Dictionary<string, string>();
            requestParams.Add("id", idDocumento);

            ResponseDto responseHttp = await http.GetAsJsonAsync<ResponseDto>(WebAPIEndPointsEnum.CONSULTA_PDF_POR_ID.GetString(), requestParams);

            DocumentDto result = null;
            if (!responseHttp.error)
            {
                if (responseHttp.response != null)
                {
                    var doc = responseHttp.response;
                    result = new DocumentDto()
                    {
                        ID_INVITE = doc.iD_INVITE,
                        NAME = doc.name,
                        ID_REG = doc.iD_REG,
                        ID_EVENT_TYPE = doc.iD_EVENT_TYPE,
                        DESC_SPANISH = doc.desC_SPANISH,
                        DESC_ENGLISH = doc.desC_ENGLISH,
                        FILE_BLOB = doc.filE_BLOB
                    };
                }
            }
            return result;
        }




        public async Task<ResponseModel> EliminarRegistroPorId(int id)
        {

            ResponseModel result = new ResponseModel();
            object requestBody = new
            {
                ID_REG = id
            };

            ResponseDto responseHttp = await http.PostAsJsonAsync<object, ResponseDto>(requestBody, WebAPIEndPointsEnum.ELIMINAR_REGISTRO_POR_ID.GetString());
            
            if(responseHttp != null )
            {
                if (!responseHttp.error)
                {
                    result.response = true;
                    result.message = "Elemento eliminado correctamente";
                }
                else
                {
                    result.response = false;
                    result.message = responseHttp.message;
                }
            }
            else
            {
                result.response = false;
                result.message = "Error al consumir el servicio de eliminación";
            }

            return result;
        }


        public async Task<RegistroInvitadoModel> ConfirmaCorreoRegistroPorId(int id)
        {
            RegistroInvitadoModel result = new RegistroInvitadoModel();

            object requestBody = new {
                ID_REG = id
            };
            ResponseDto responseHttp = await http.PostAsJsonAsync<object, ResponseDto>(requestBody, WebAPIEndPointsEnum.CONFIRMA_CORREO_REGISTRO.GetString());
            if (!responseHttp.error)
            {
                result = new RegistroInvitadoModel()
                {
                    Id = responseHttp.response.iD_REG,
                    Nombre = responseHttp.response.passporT_NAME,
                    Apellidos = responseHttp.response.passporT_LASTNAME,

                };

            }
            return result;
        }

        public async Task<RegistroInvitadoModel> ConfirmaCorreoInvitacionPorId(int id)
        {
            RegistroInvitadoModel result = new RegistroInvitadoModel();

            object requestBody = new
            {
                ID_REG = id
            };
            ResponseDto responseHttp = await http.PostAsJsonAsync<object, ResponseDto>(requestBody, WebAPIEndPointsEnum.CONFIRMA_CORREO_INVITACION.GetString());
            if (!responseHttp.error)
            {
                result = new RegistroInvitadoModel()
                {
                    Id = responseHttp.response.iD_REG,
                    Nombre = responseHttp.response.passporT_NAME,
                    Apellidos = responseHttp.response.passporT_LASTNAME,

                };

            }
            return result;
        }

        public async Task<ResponseDto> EnviarCorreosInvitacionesPorIds(List<int> ids)
        {
            ResponseDto result = new ResponseDto();

            result = await http.PostAsJsonAsync<List<int>, ResponseDto>(ids, WebAPIEndPointsEnum.ENVIA_CORREOS_INVITACIONES.GetString());
            return result;
        }


        public async Task<ResponseDto> RecuperarPassword(string username, string email)
        {
            UserRegisterDto requestBody = new UserRegisterDto() { USERNAME = username, EMAIL_USER = email};
            ResponseDto responseHttp = await http.PostAsJsonAsync<object, ResponseDto>(requestBody, WebAPIEndPointsEnum.RECUPERA_PASSWORD_USUARIO.GetString());
            return responseHttp;
        }

        #region CATALOGOS

        #region EMPRESAS
        public async Task<List<CatalogoGeneral>> DescargaCatalogosEmpresas()
        {
            List<CatalogoGeneral> result = new List<CatalogoGeneral>();
            catalogs = new CatalogsManager(http);
            var catalogList = await catalogs.DescargarCatalogoCompanias();
            foreach(CompaniesDto company in catalogList)
            {
                result.Add(new CatalogoGeneral() { Id=company.ID_COMPANY, Descripcion=company.DESC_COMPANY_SP});
            }

            return result;
        }

        public async Task<ResponseDto> DescargaCatalogoEmpresaPorId(int id)
        {
            ResponseDto result;
            result = await http.GetAsJsonAsync<ResponseDto>($"Companies/GetById?id={id}");
            return result;
        }

        public async Task<ResponseDto> ActualizarEmpresa(int id, CompanyRegisterDto data)
        {
            ResponseDto result;

            result = await http.PostAsJsonAsync<object, ResponseDto>(data, $"Companies/Update?id={id}");
            
            return result;
        }

        public async Task<ResponseDto> CrearEmpresa(CompanyRegisterDto data)
        {
            ResponseDto result;

            result = await http.PostAsJsonAsync<object, ResponseDto>(data, $"Companies/Save");

            return result;
        }
        public async Task<ResponseDto> EliminarEmpresa(CompaniesDto data)
        {
            ResponseDto result;

            result = await http.PostAsJsonAsync<object, ResponseDto>(data, $"Companies/Delete");

            return result;
        }
        #endregion

        #region EVENTOS
        public async Task<List<CatalogoGeneral>> DescargaCatalogosEventos()
        {
            List<CatalogoGeneral> result = new List<CatalogoGeneral>();
            catalogs = new CatalogsManager(http);
            var catalogList = await catalogs.DescargarCatalogoEventos();
            foreach (EventsDto item in catalogList)
            {
                result.Add(new CatalogoGeneral() { Id = item.ID_EVENT, Descripcion = item.DESC_EVENT_SP, AtributoAdicionalInt1= item.ID_EVENT_TYPE });
            }

            return result;
        }
        public async Task<ResponseDto> DescargaCatalogoEventoPorId(int id)
        {
            ResponseDto result;
            result = await http.GetAsJsonAsync<ResponseDto>($"Events/GetById?id={id}");
            return result;
        }

        public async Task<ResponseDto> ActualizarEvento(int id, EventRegisterDto data)
        {
            ResponseDto result;

            result = await http.PostAsJsonAsync<object, ResponseDto>(data, $"Events/Update?id={id}");

            return result;
        }

        public async Task<ResponseDto> CrearEvento(EventRegisterDto data)
        {
            ResponseDto result;

            result = await http.PostAsJsonAsync<object, ResponseDto>(data, $"Events/Save");

            return result;
        }
        public async Task<ResponseDto> EliminarEvento(EventsDto data)
        {
            ResponseDto result;

            result = await http.PostAsJsonAsync<object, ResponseDto>(data, $"Events/Delete");

            return result;
        }
        #endregion

        #region PAISES
        public async Task<List<CatalogoGeneral>> DescargaCatalogosPaises()
        {
            List<CatalogoGeneral> result = new List<CatalogoGeneral>();
            catalogs = new CatalogsManager(http);
            var catalogList = await catalogs.DescargarCatalogoPaises();
            foreach (CountriesDto item in catalogList)
            {
                result.Add(new CatalogoGeneral()
                {
                    Id = item.ID_COUNTRY,
                    Descripcion = item.DESC_COUNTRY_SP,
                    AtributoAdicionalStr1 = item.RESTRICTION == true ? "SI" : "NO"
                });
            }

            return result;
        }
        public async Task<ResponseDto> DescargaCatalogoPaisPorId(int id)
        {
            ResponseDto result;
            result = await http.GetAsJsonAsync<ResponseDto>($"Countries/GetById?id={id}");
            return result;
        }

        public async Task<ResponseDto> ActualizarPais(int id, CountryRegisterDto data)
        {
            ResponseDto result;

            result = await http.PostAsJsonAsync<object, ResponseDto>(data, $"Countries/Update?id={id}");

            return result;
        }

        public async Task<ResponseDto> CrearPais(CountryRegisterDto data)
        {
            ResponseDto result;

            result = await http.PostAsJsonAsync<object, ResponseDto>(data, $"Countries/Save");

            return result;
        }
        public async Task<ResponseDto> EliminarPais(CountriesDto data)
        {
            ResponseDto result;

            result = await http.PostAsJsonAsync<object, ResponseDto>(data, $"Countries/Delete");

            return result;
        }
        #endregion

        #region TIPOS EVENTOS
        public async Task<List<CatalogoGeneral>> DescargaCatalogosTipoEventos()
        {
            List<CatalogoGeneral> result = new List<CatalogoGeneral>();
            catalogs = new CatalogsManager(http);
            var catalogList = await catalogs.DescargarCatalogoTiposEventos();
            foreach (var item in catalogList)
            {
                result.Add(new CatalogoGeneral()
                {
                    Id = item.ID_EVENT_TYPE,
                    Descripcion = item.DESC_ACTIVITY_SP,

                });
            }

            return result;
        }
        public async Task<ResponseDto> DescargaCatalogoTipoEventoPorId(int id)
        {
            ResponseDto result;
            result = await http.GetAsJsonAsync<ResponseDto>($"TiposEventos/GetById?id={id}");
            return result;
        }

        public async Task<ResponseDto> ActualizarTipoEvento(int id, EventTypesRegisterDto data)
        {
            ResponseDto result;

            result = await http.PostAsJsonAsync<object, ResponseDto>(data, $"TiposEventos/Update?id={id}");

            return result;
        }

        public async Task<ResponseDto> CrearTipoEvento(EventTypesRegisterDto data)
        {
            ResponseDto result;

            result = await http.PostAsJsonAsync<object, ResponseDto>(data, $"TiposEventos/Save");

            return result;
        }
        public async Task<ResponseDto> EliminarTipoEvento(EventTypesDto data)
        {
            ResponseDto result;

            result = await http.PostAsJsonAsync<object, ResponseDto>(data, $"TiposEventos/Delete");

            return result;
        }
        #endregion

        #region DOCUMENTOS
        public async Task<List<CatalogoGeneral>> DescargaCatalogosDocumentos()
        {
            List<CatalogoGeneral> result = new List<CatalogoGeneral>();
            catalogs = new CatalogsManager(http);
            var catalogList = await catalogs.DescargarCatalogoDocumentos();
            foreach (InviteDto item in catalogList)
            {
                result.Add(new CatalogoGeneral()
                {
                    Id = item.ID_INVITE,
                    AtributoAdicionalInt1 = (int)item.ID_EVENT_TYPE,
                    AtributoAdicionalStr1 = item.DES_TITLE,
                    AtributoAdicionalStr2 = item.DESC_SPANISH,
                    AtributoAdicionalStr3 = item.DESC_ENGLISH,
                    AtributoAdicionalStr4 = item.SIGN_1,
                    AtributoAdicionalStr5 = item.SIGN_2,
                    AtributoAdicionalStr6 = item.SIGN_3,
                    AtributoAdicionalStr7 = item.SIGN_4,
                    AtributoAdicionalStr8 = item.FOOT_PAGE,
                });
            }

            return result;
        }
        public async Task<List<CatalogoGeneral>> DescargaCatalogosFirmasDocumentos()
        {
            List<CatalogoGeneral> result = new List<CatalogoGeneral>();
            catalogs = new CatalogsManager(http);
            var catalogList = await catalogs.DescargarCatalogoFirmasDocumentos();
            foreach (InviteDto item in catalogList)
            {
                result.Add(new CatalogoGeneral()
                {
                    Id = item.ID_INVITE,
                    AtributoAdicionalInt1 = (int)item.ID_EVENT_TYPE,
                    AtributoAdicionalStr1 = item.DES_TITLE,
                    AtributoAdicionalStr2 = item.DESC_SPANISH,
                    AtributoAdicionalStr3 = item.DESC_ENGLISH,
                    AtributoAdicionalStr4 = item.SIGN_1,
                    AtributoAdicionalStr5 = item.SIGN_2,
                    AtributoAdicionalStr6 = item.SIGN_3,
                    AtributoAdicionalStr7 = item.SIGN_4,
                    AtributoAdicionalStr8 = item.FOOT_PAGE,
                });
            }

            return result;
        }


        public async Task<ResponseDto> DescargaCatalogoDocumentoPorId(int id)
        {
            ResponseDto result;
            result = await http.GetAsJsonAsync<ResponseDto>($"RegInvite/GetById?id={id}");
            return result;
        }


        public async Task<ResponseDto> CrearDocumento(RegInviteDto data)
        {
            ResponseDto result;

            result = await http.PostAsJsonAsync<object, ResponseDto>(data, $"RegInvite/Save");

            return result;
        }

        public async Task<ResponseDto> ActualizarDocumento(int id, RegInviteDto data)
        {
            ResponseDto result;

            result = await http.PostAsJsonAsync<object, ResponseDto>(data, $"RegInvite/Update?id={id}");

            return result;
        }
        public async Task<ResponseDto> EliminarDocumento(InviteDto data)
        {
            ResponseDto result;

            result = await http.PostAsJsonAsync<object, ResponseDto>(data, $"RegInvite/Delete");

            return result;
        }

        public async Task<ResponseDto> ActualizarImagenFirma(int id, RegInviteDto data)
        {
            ResponseDto result;

            result = await http.PostAsJsonAsync<object, ResponseDto>(data, $"RegInvite/UpdateSignImage?id={id}");

            return result;
        }

        #endregion


        public async Task<ResponseDto> ActualizarAvisoPrivacidad(int id, AvisoPrivacidadDto data)
        {
            ResponseDto result;

            result = await http.PostAsJsonAsync<object, ResponseDto>(data, $"AvisoPrivacidad/Update?id={id}");

            return result;
        }


        #region DOCUMENTOS PDF
        public async Task<List<CatalogoGeneral>> DescargaCatalogosPDF()
        {
            List<CatalogoGeneral> result = new List<CatalogoGeneral>();
            catalogs = new CatalogsManager(http);
            var catalogList = await catalogs.DescargarCatalogoPDFs();
            foreach (DocumentDto item in catalogList)
            {
                result.Add(new CatalogoGeneral()
                {
                    Id = item.ID_INVITE,
                    AtributoAdicionalStr1 = item.ID_EVENT_TYPE.ToString(),
                    AtributoAdicionalStr2 = item.DESC_SPANISH,
                });
            }

            return result;
        }


        public async Task<ResponseDto> DescargaCatalogoPDFPorId(int id)
        {
            ResponseDto result;
            result = await http.GetAsJsonAsync<ResponseDto>($"Documents/GetById?id={id}");
            return result;
        }


        public async Task<ResponseDto> CrearPDF(DocumentRegisterDto data)
        {
            ResponseDto result;

            result = await http.PostAsJsonAsync<object, ResponseDto>(data, $"Documents/Save");

            return result;
        }

        public async Task<ResponseDto> ActualizarPDF(int id, DocumentRegisterDto data)
        {
            ResponseDto result;

            result = await http.PostAsJsonAsync<object, ResponseDto>(data, $"Documents/Update?id={id}");

            return result;
        }

        public async Task<ResponseDto> EliminarPdf(DocumentDto data)
        {
            ResponseDto result;

            result = await http.PostAsJsonAsync<object, ResponseDto>(data, $"Documents/Delete");

            return result;
        }
        #endregion




        #region AEROLINEAS
        public async Task<List<CatalogoGeneral>> DescargaCatalogosAerolineas()
        {
            List<CatalogoGeneral> result = new List<CatalogoGeneral>();
            catalogs = new CatalogsManager(http);
            var catalogList = await catalogs.DescargarCatalogoAerolineas();
            foreach (AirLinesDto item in catalogList)
            {
                result.Add(new CatalogoGeneral() { Id = item.ID_AIR_LINE, Descripcion = item.DESC_AIR_LINE_SP});
            }

            return result;
        }
        public async Task<ResponseDto> DescargaCatalogoAerolineaPorId(int id)
        {
            ResponseDto result;
            result = await http.GetAsJsonAsync<ResponseDto>($"Airlines/GetById?id={id}");
            return result;
        }

        public async Task<ResponseDto> ActualizarAerolinea(int id, AirLineRegisterDto data)
        {
            ResponseDto result;

            result = await http.PostAsJsonAsync<object, ResponseDto>(data, $"Airlines/Update?id={id}");

            return result;
        }

        public async Task<ResponseDto> CrearAerolinea(AirLineRegisterDto data)
        {
            ResponseDto result;

            result = await http.PostAsJsonAsync<object, ResponseDto>(data, $"Airlines/Save");

            return result;
        }
        public async Task<ResponseDto> EliminarAerolinea(AirLinesDto data)
        {
            ResponseDto result;

            result = await http.PostAsJsonAsync<object, ResponseDto>(data, $"Airlines/Delete");

            return result;
        }

        #endregion

        #region AEROPUERTOS
        public async Task<List<CatalogoGeneral>> DescargaCatalogosAeropuertos()
        {
            List<CatalogoGeneral> result = new List<CatalogoGeneral>();
            catalogs = new CatalogsManager(http);
            var catalogList = await catalogs.DescargarCatalogoAeropuertos();
            foreach (AirPortsDto item in catalogList)
            {
                result.Add(new CatalogoGeneral() { Id = item.ID_AIRPORT, Descripcion = item.DESC_AIRPORT_SP });
            }

            return result;
        }
        public async Task<ResponseDto> DescargaCatalogoAeropuertoPorId(int id)
        {
            ResponseDto result;
            result = await http.GetAsJsonAsync<ResponseDto>($"Airports/GetById?id={id}");
            return result;
        }

        public async Task<ResponseDto> ActualizarAeropuerto(int id, AirPortRegisterDto data)
        {
            ResponseDto result;

            result = await http.PostAsJsonAsync<object, ResponseDto>(data, $"Airports/Update?id={id}");

            return result;
        }

        public async Task<ResponseDto> CrearAeropuerto(AirPortRegisterDto data)
        {
            ResponseDto result;

            result = await http.PostAsJsonAsync<object, ResponseDto>(data, $"Airports/Save");

            return result;
        }
        public async Task<ResponseDto> EliminarAeropuerto(AirPortsDto data)
        {
            ResponseDto result;

            result = await http.PostAsJsonAsync<object, ResponseDto>(data, $"AirPorts/Delete");

            return result;
        }

        #endregion

        #region NACIONALIDAD
        public async Task<List<CatalogoGeneral>> DescargaCatalogosNacionalidad()
        {
            List<CatalogoGeneral> result = new List<CatalogoGeneral>();
            catalogs = new CatalogsManager(http);
            var catalogList = await catalogs.DescargarCatalogoNacionalidades();
            foreach (NationalitiesDto item in catalogList)
            {
                result.Add(new CatalogoGeneral() { Id = item.ID_NALCIONALITY, Descripcion = item.DESC_NACIONALITY_SP,
                    AtributoAdicionalStr1 = item.RESTRICTION == true ? "SI" : "NO"
                });
            }

            return result;
        }
        public async Task<ResponseDto> DescargaCatalogoNacionalidadPorId(int id)
        {
            ResponseDto result;
            result = await http.GetAsJsonAsync<ResponseDto>($"Nationalities/GetById?id={id}");
            return result;
        }

        public async Task<ResponseDto> ActualizarNacionalidad(int id, NationalityRegisterDto data)
        {
            ResponseDto result;

            result = await http.PostAsJsonAsync<object, ResponseDto>(data, $"Nationalities/Update?id={id}");

            return result;
        }

        public async Task<ResponseDto> CrearNacionalidad(NationalityRegisterDto data)
        {
            ResponseDto result;

            result = await http.PostAsJsonAsync<object, ResponseDto>(data, $"Nationalities/Save");

            return result;
        }
        public async Task<ResponseDto> EliminarNacionalidad(NationalitiesDto data)
        {
            ResponseDto result;

            result = await http.PostAsJsonAsync<object, ResponseDto>(data, $"Nationalities/Delete");

            return result;
        }

        #endregion

        #region INMUEBLES
        public async Task<List<CatalogoGeneral>> DescargaCatalogosInmuebles()
        {
            List<CatalogoGeneral> result = new List<CatalogoGeneral>();
            catalogs = new CatalogsManager(http);
            var catalogList = await catalogs.DescargarCatalogoInmuebles();
            foreach (EstatesDto item in catalogList)
            {
                result.Add(new CatalogoGeneral() { Id = item.ID_ESTATE, Descripcion = item.DESC_ESTATE_SP });
            }

            return result;
        }
        public async Task<ResponseDto> DescargaCatalogoInmueblePorId(int id)
        {
            ResponseDto result;
            result = await http.GetAsJsonAsync<ResponseDto>($"Estates/GetById?id={id}");
            return result;
        }

        public async Task<ResponseDto> ActualizarInmueble(int id, EstateRegisterDto data)
        {
            ResponseDto result;

            result = await http.PostAsJsonAsync<object, ResponseDto>(data, $"Estates/Update?id={id}");

            return result;
        }

        public async Task<ResponseDto> CrearInmueble(EstateRegisterDto data)
        {
            ResponseDto result;

            result = await http.PostAsJsonAsync<object, ResponseDto>(data, $"Estates/Save");

            return result;
        }
        public async Task<ResponseDto> EliminarInmueble(EstatesDto data)
        {
            ResponseDto result;

            result = await http.PostAsJsonAsync<object, ResponseDto>(data, $"Estates/Delete");

            return result;
        }

        #endregion

        #region USUARIOS
        //Consumo de los servicios
        public async Task<List<CatalogoGeneral>> DescargaCatalogosUsuarios()
        {
            List<CatalogoGeneral> result = new List<CatalogoGeneral>();
            catalogs = new CatalogsManager(http);
            var catalogList = await catalogs.DescargarCatalogoUsuarios();
            foreach (UserDto item in catalogList)
            {
                int idRol = item.ID_ROLE == null ? 0 : (int)item.ID_ROLE;
                string usuarioActivoStr = item.ACTIVE.ToString().ToUpper().Equals("TRUE") ? "SI" : "NO";
                result.Add(new CatalogoGeneral() { Id = item.ID_USER, Descripcion = item.NAME_USER + " " + item.LAST_NAME_USER, AtributoAdicionalStr1=item.USERNAME, AtributoAdicionalStr2=usuarioActivoStr, AtributoAdicionalStr3= idRol.ToString()});
            }

            return result;
        }

        public async Task<ResponseDto> DescargaCatalogoUsuarioPorId(int id)
        {
            ResponseDto result;
            result = await http.GetAsJsonAsync<ResponseDto>($"Users/GetById?id={id}");
            return result;
        }

        public async Task<ResponseDto> LoginUsuario(string usuario, string password)
        {
            ResponseDto result;
            UserRegisterDto userRegister = new UserRegisterDto() { USERNAME=usuario, PASSWORD_USER=password};
            result = await http.PostAsJsonAsync<UserRegisterDto, ResponseDto>(userRegister,$"Users/Login");
            return result;
        }

        public async Task<ResponseDto> ActualizarUsuario(int id, UserRegisterDto data)
        {
            ResponseDto result;

            result = await http.PostAsJsonAsync<object, ResponseDto>(data, $"Users/Update?id={id}");

            return result;
        }


        public async Task<ResponseDto> CambiarPasswordUsuario(string id, UserRegisterUpdatePwdDto data)
        {
            ResponseDto result;
            result = await http.PostAsJsonAsync<object, ResponseDto>(data, $"Users/UpdatePassword?id={id}"); 
            return result;
        }



        public async Task<ResponseDto> CrearUsuario(UserRegisterDto data)
        {
            ResponseDto result;

            result = await http.PostAsJsonAsync<object, ResponseDto>(data, $"Users/Save");

            return result;
        }

        public async Task<ResponseDto> EliminarUsuario(UserDto data)
        {
            ResponseDto result;

            result = await http.PostAsJsonAsync<object, ResponseDto>(data, $"Users/Delete");

            return result;
        }

        public async Task<ResponseDto> ActivarUsuario(UserDto data)
        {
            ResponseDto result;

            result = await http.PostAsJsonAsync<object, ResponseDto>(data, $"Users/Reactive");

            return result;
        }

        #endregion

        #region PERFILES
        public async Task<List<CatalogoGeneral>> DescargaCatalogosPerfiles()
        {
            List<CatalogoGeneral> result = new List<CatalogoGeneral>();
            catalogs = new CatalogsManager(http);
            var catalogList = await catalogs.DescargarCatalogoPerfiles();
            foreach (RoleByCompanyDto item in catalogList)
            {
                result.Add(new CatalogoGeneral() { Id = item.ID_ROLE, Descripcion = $"{item.DESC_ROLE_SP}" });
            }

            return result;
        }
        public async Task<ResponseDto> DescargaCatalogoPerfilesPorId(int id)
        {
            ResponseDto result;
            result = await http.GetAsJsonAsync<ResponseDto>($"Roles/GetById?id={id}");
            return result;
        }
        public async Task<ResponseDto> DescargaCatalogoModulosPorIdRol(int id)
        {
            ResponseDto result;
            result = await http.GetAsJsonAsync<ResponseDto>($"RolesProcess/GetProcessByIdRol?idRol={id}");
            return result;
        }
        public async Task<ResponseDto> DescargaCatalogoModulosPorIdUsuario(int id)
        {
            ResponseDto result;
            result = await http.GetAsJsonAsync<ResponseDto>($"RolesProcess/GetProcessByIdUsuario?id={id}");
            return result;
        }

        public async Task<ResponseDto> DescargaCatalogoModulos()
        {
            ResponseDto result;
            result = await http.GetAsJsonAsync<ResponseDto>($"Process");
            return result;
        }

        public async Task<ResponseDto> ActualizarPerfil(int id, CatalogoPerfilesDto data)
        {
            ResponseDto result;

            result = await http.PostAsJsonAsync<object, ResponseDto>(data, $"Roles/UpdateRolesAndPermissions?id={id}");

            return result;
        }

        public async Task<ResponseDto> CrearPerfil(CatalogoPerfilesDto data)
        {
            ResponseDto result;

            result = await http.PostAsJsonAsync<object, ResponseDto>(data, $"Roles/SaveRolesAndPermissions");

            return result;
        }
        public async Task<ResponseDto> EliminarPerfil(RoleByCompanyDto data)
        {
            ResponseDto result;

            result = await http.PostAsJsonAsync<object, ResponseDto>(data, $"Roles/Delete");

            return result;
        }
        #endregion


        #region GENERO
        public async Task<List<CatalogoGeneral>> DescargaCatalogosGeneros()
        {
            List<CatalogoGeneral> result = new List<CatalogoGeneral>();
            catalogs = new CatalogsManager(http);
            var catalogList = await catalogs.DescargarCatalogoGeneros();
            foreach (GendersDto item in catalogList)
            {
                result.Add(new CatalogoGeneral() { Id = item.ID_GENDER, Descripcion = $"{item.DESC_GENDER_SP} - {item.DESC_GENDER_EN}"  });
            }

            return result;
        }
        public async Task<ResponseDto> DescargaCatalogoGeneroPorId(int id)
        {
            ResponseDto result;
            result = await http.GetAsJsonAsync<ResponseDto>($"Genders/GetById?id={id}");
            return result;
        }

        public async Task<ResponseDto> ActualizarGenero(int id, GenderRegisterDto data)
        {
            ResponseDto result;

            result = await http.PostAsJsonAsync<object, ResponseDto>(data, $"Genders/Update?id={id}");

            return result;
        }

        public async Task<ResponseDto> CrearGenero(GenderRegisterDto data)
        {
            ResponseDto result;

            result = await http.PostAsJsonAsync<object, ResponseDto>(data, $"Genders/Save");

            return result;
        }
        public async Task<ResponseDto> EliminarGenero(GendersDto data)
        {
            ResponseDto result;

            result = await http.PostAsJsonAsync<object, ResponseDto>(data, $"Genders/Delete");

            return result;
        }

        #endregion

        #region CORREOS PARA ENVIAR COPIA
        public async Task<List<CatalogoGeneral>> DescargaCatalogoCorreosParaCopia()
        {
            List<CatalogoGeneral> result = new List<CatalogoGeneral>();
            catalogs = new CatalogsManager(http);
            var catalogList = await catalogs.DescargarCatalogoCorreosParaCopia();
            foreach (SendCopyEmailsDto item in catalogList)
            {
                result.Add(new CatalogoGeneral() { Id = item.ID, Descripcion = $"{item.EMAIL}", Activo=Convert.ToBoolean(item.ACTIVE) });
            }

            return result;
        }
        public async Task<ResponseDto> CrearCorreoParaCopia(SendCopyEmailsRegisterDto data)
        {
            ResponseDto result;

            result = await http.PostAsJsonAsync<object, ResponseDto>(data, $"SendCopyEmails/Save");

            return result;
        }
        public async Task<ResponseDto> ActualizarCorreoParaCopia(int id, SendCopyEmailsRegisterDto data)
        {
            ResponseDto result;

            result = await http.PostAsJsonAsync<object, ResponseDto>(data, $"SendCopyEmails/Update?id={id}");

            return result;
        }

        #endregion

        #endregion
    }
}
