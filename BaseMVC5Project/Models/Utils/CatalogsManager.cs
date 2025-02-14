using CommonTools.DTOs;
using CommonTools.DTOs.Query;
using CommonTools.DTOs.Register;
using MigracionTalentoExtranjero.Models.Enum;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.IsisMtt.X509;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigracionTalentoExtranjero.Models.Utils
{
    public class CatalogsManager
    {
        private HttpManager http;
        public CatalogsManager(HttpManager _http) {
            this.http = _http;
        }

        public async Task<List<AirLinesDto>> DescargarCatalogoAerolineas()
        {
            List<AirLinesDto> result = new List<AirLinesDto>();

            var responseHttp = await http.GetAsJsonAsync<ResponseDto>(WebAPIEndPointsEnum.CATALOGO_AIRLINES.GetString());
            if(!responseHttp.error)
            {
                foreach (var currentValue in responseHttp.response)
                {
                    string currentPosition = currentValue.ToString();
                    result.Add(new AirLinesDto() { 
                        ID_AIR_LINE = currentValue.iD_AIR_LINE, 
                        DESC_AIR_LINE_EN = currentValue.desC_AIR_LINE_EN, 
                        DESC_AIR_LINE_SP= currentValue.desC_AIR_LINE_SP 
                    });
                }
            }
            return result;
        }

        public async Task<List<NationalitiesDto>> DescargarCatalogoNacionalidades()
        {
            List<NationalitiesDto> result = new List<NationalitiesDto>();

            var responseHttp = await http.GetAsJsonAsync<ResponseDto>(WebAPIEndPointsEnum.CATALOGO_NATIONALITIES.GetString());
            if (!responseHttp.error)
            {
                foreach (var currentValue in responseHttp.response)
                {
                    string currentPosition = currentValue.ToString();
                    result.Add(new NationalitiesDto()
                    {
                        ID_NALCIONALITY = currentValue.iD_NATIONALITY,
                        DESC_NACIONALITY_SP = currentValue.desC_NACIONALITY_SP,
                        RESTRICTION = currentValue.restriction,
                    });
                }
            }
            return result;
        }

        public async Task<List<AirPortsDto>> DescargarCatalogoAeropuertos()
        {
            List<AirPortsDto> result = new List<AirPortsDto>();

            var responseHttp = await http.GetAsJsonAsync<ResponseDto>(WebAPIEndPointsEnum.CATALOGO_AIRPORTS.GetString());
            if (!responseHttp.error)
            {
                foreach (var currentValue in responseHttp.response)
                {
                    string currentPosition = currentValue.ToString();
                    result.Add(new AirPortsDto()
                    {
                        ID_AIRPORT = currentValue.iD_AIRPORT,
                        DESC_AIRPORT_SP = currentValue.desC_AIRPORT_SP,
                    });
                }
            }
            return result;
        }

        public async Task<List<CompaniesDto>> DescargarCatalogoCompanias()
        {
            List<CompaniesDto> result = new List<CompaniesDto>();

            var responseHttp = await http.GetAsJsonAsync<ResponseDto>(WebAPIEndPointsEnum.CATALOGO_COMPANIES.GetString());
            if (!responseHttp.error)
            {
                foreach (var currentValue in responseHttp.response)
                {
                    string currentPosition = currentValue.ToString();
                    result.Add(new CompaniesDto()
                    {
                        ID_COMPANY= currentValue.iD_COMPANY,
                        DESC_COMPANY_SP= currentValue.desC_COMPANY_SP,
                    });
                }
            }
            return result;
        }

        public async Task<List<EstatesDto>> DescargarCatalogoInmuebles()
        {
            List<EstatesDto> result = new List<EstatesDto>();

            var responseHttp = await http.GetAsJsonAsync<ResponseDto>(WebAPIEndPointsEnum.CATALOGO_ESTATES.GetString());
            if (!responseHttp.error)
            {
                foreach (var currentValue in responseHttp.response)
                {
                    string currentPosition = currentValue.ToString();
                    result.Add(new EstatesDto()
                    {
                        ID_ESTATE= currentValue.iD_ESTATE,
                        DESC_ESTATE_SP= currentValue.desC_ESTATE_SP,
                    });
                }
            }
            return result;
        }

        public async Task<List<EventsDto>> DescargarCatalogoEventos()
        {
            List<EventsDto> result = new List<EventsDto>();

            var responseHttp = await http.GetAsJsonAsync<ResponseDto>(WebAPIEndPointsEnum.CATALOGO_EVENTS.GetString());
            if (!responseHttp.error)
            {
                foreach (var currentValue in responseHttp.response)
                {
                    string currentPosition = currentValue.ToString();
                    result.Add(new EventsDto()
                    {
                        ID_EVENT = currentValue.iD_EVENT,
                        DESC_EVENT_SP = currentValue.desC_EVENT_SP,
                        ID_COMPANY = currentValue.iD_COMPANY,
                        ID_EVENT_TYPE = currentValue.iD_EVENT_TYPE,
                    });
                }
            }
            return result;
        }

        public async Task<List<GendersDto>> DescargarCatalogoGeneros()
        {
            List<GendersDto> result = new List<GendersDto>();

            var responseHttp = await http.GetAsJsonAsync<ResponseDto>(WebAPIEndPointsEnum.CATALOGO_GENDERS.GetString());
            if (!responseHttp.error)
            {
                foreach (var currentValue in responseHttp.response)
                {
                    string currentPosition = currentValue.ToString();
                    result.Add(new GendersDto()
                    {
                        ID_GENDER = currentValue.iD_GENDER,
                        DESC_GENDER_SP = currentValue.desC_GENDER_SP,
                        DESC_GENDER_EN = currentValue.desC_GENDER_EN,
                    });
                }
            }
            return result;
        }
        public async Task<List<GendersDto>> DescargarCatalogoGenerosEN()
        {
            List<GendersDto> result = new List<GendersDto>();

            var responseHttp = await http.GetAsJsonAsync<ResponseDto>(WebAPIEndPointsEnum.CATALOGO_GENDERS.GetString());
            if (!responseHttp.error)
            {
                foreach (var currentValue in responseHttp.response)
                {
                    string currentPosition = currentValue.ToString();
                    result.Add(new GendersDto()
                    {
                        ID_GENDER = currentValue.iD_GENDER,
                        DESC_GENDER_SP = currentValue.desC_GENDER_EN,
                    });
                }
            }
            return result;
        }

        public async Task<List<RoleByCompanyDto>> DescargarCatalogoPerfiles()
        {
            List<RoleByCompanyDto> result = new List<RoleByCompanyDto>();

            var responseHttp = await http.GetAsJsonAsync<ResponseDto>(WebAPIEndPointsEnum.CATALOGO_PERFILES.GetString());
            if (!responseHttp.error)
            {
                foreach (var currentValue in responseHttp.response)
                {
                    string currentPosition = currentValue.ToString();
                    result.Add(new RoleByCompanyDto()
                    {
                        ID_ROLE = currentValue.iD_ROLE,
                        DESC_ROLE_SP = currentValue.desC_ROLE_SP,
                        DESC_ROLE_EN = currentValue.desC_ROLE_EN,
                        ACTIVE = currentValue.active
                    });
                }
            }
            return result;
        }

        public async Task<List<CountriesDto>> DescargarCatalogoPaises()
        {
            List<CountriesDto> result = new List<CountriesDto>();

            var responseHttp = await http.GetAsJsonAsync<ResponseDto>(WebAPIEndPointsEnum.CATALOGO_CONTRIES.GetString());
            if (!responseHttp.error)
            {
                foreach (var currentValue in responseHttp.response)
                {
                    string currentPosition = currentValue.ToString();
                    result.Add(new CountriesDto()
                    {
                        ID_COUNTRY = currentValue.iD_COUNTRY,
                        DESC_COUNTRY_SP = currentValue.desC_COUNTRY_SP,
                        RESTRICTION = currentValue.restriction,
                    });
                }
            }
            return result;
        }

        public async Task<List<UserDto>> DescargarCatalogoUsuarios()
        {
            List<UserDto> result = new List<UserDto>();

            var responseHttp = await http.GetAsJsonAsync<ResponseDto>(WebAPIEndPointsEnum.CATALOGO_USERS.GetString());
            if (!responseHttp.error)
            {
                foreach (var currentValue in responseHttp.response)
                {
                    string currentPosition = currentValue.ToString();
                    result.Add(new UserDto()
                    {
                        ID_USER = currentValue.iD_USER,
                        CVE_USER = currentValue.cvE_USER,
                        NAME_USER = currentValue.namE_USER,
                        USERNAME = currentValue.username,
                        ID_ROLE = currentValue.iD_ROLE,
                        LAST_NAME_USER = currentValue.lasT_NAME_USER,
                        ACTIVE = currentValue.active,
                    });
                }
            }
            return result;
        }



        public async Task<List<InviteDto>> DescargarCatalogoDocumentos()
        {
            List<InviteDto> result = new List<InviteDto>();

            var responseHttp = await http.GetAsJsonAsync<ResponseDto>(WebAPIEndPointsEnum.CATALOGO_DOCUMENTOS.GetString());
            if (!responseHttp.error)
            {
                foreach (var currentValue in responseHttp.response)
                {
                    string currentPosition = currentValue.ToString();
                    result.Add(new InviteDto()
                    {
                        ID_INVITE = currentValue.iD_INVITE,
                        ID_EVENT_TYPE = currentValue.iD_EVENT_TYPE,
                        DES_TITLE = currentValue.deS_TITLE,
                        DESC_SPANISH = currentValue.desC_SPANISH,
                        DESC_ENGLISH= currentValue.desC_ENGLISH,
                        SIGN_1= currentValue.sigN_1,
                        SIGN_2= currentValue.sigN_2,
                        SIGN_3= currentValue.sigN_3,
                        SIGN_4= currentValue.sigN_4,
                        FOOT_PAGE= currentValue.fooT_PAGE,
                    });
                }
            }
            return result;
        }
        public async Task<List<InviteDto>> DescargarCatalogoFirmasDocumentos()
        {
            List<InviteDto> result = new List<InviteDto>();

            var responseHttp = await http.GetAsJsonAsync<ResponseDto>(WebAPIEndPointsEnum.CATALOGO_FIRMAS_DOCUMENTOS.GetString());
            if (!responseHttp.error)
            {
                foreach (var currentValue in responseHttp.response)
                {
                    string currentPosition = currentValue.ToString();
                    result.Add(new InviteDto()
                    {
                        ID_INVITE = currentValue.iD_INVITE,
                        ID_EVENT_TYPE = currentValue.iD_EVENT_TYPE,
                        DES_TITLE = currentValue.deS_TITLE,
                        DESC_SPANISH = currentValue.desC_SPANISH,
                        DESC_ENGLISH= currentValue.desC_ENGLISH,
                        SIGN_1= currentValue.sigN_1,
                        SIGN_2= currentValue.sigN_2,
                        SIGN_3= currentValue.sigN_3,
                        SIGN_4= currentValue.sigN_4,
                        FOOT_PAGE= currentValue.fooT_PAGE,
                    });
                }
            }
            return result;
        }


        public async Task<List<EventTypesDto>> DescargarCatalogoTiposEventos()
        {
            List<EventTypesDto> result = new List<EventTypesDto>();

            var responseHttp = await http.GetAsJsonAsync<ResponseDto>(WebAPIEndPointsEnum.CATALOGO_TIPOS_EVENTOS.GetString());
            if (!responseHttp.error)
            {
                foreach (var currentValue in responseHttp.response)
                {
                    string currentPosition = currentValue.ToString();
                    result.Add(new EventTypesDto()
                    {
                        ID_EVENT_TYPE = currentValue.iD_EVENT_TYPE,
                        DESC_ACTIVITY_SP = currentValue.desC_ACTIVITY_SP,
                        DESC_ACTIVITY_EN = currentValue.desC_ACTIVITY_EN,
                    });
                }
            }
            return result;
        }



        public async Task<List<RoleByCompanyDto>> DescargarCatalogoRoles()
        {
            List<RoleByCompanyDto> result = new List<RoleByCompanyDto>();

            var responseHttp = await http.GetAsJsonAsync<ResponseDto>(WebAPIEndPointsEnum.CATALOGO_PERFILES.GetString());
            if (!responseHttp.error)
            {
                foreach (var currentValue in responseHttp.response)
                {
                    string currentPosition = currentValue.ToString();
                    result.Add(new RoleByCompanyDto()
                    {
                        ID_ROLE = currentValue.iD_ROLE,
                        DESC_ROLE_SP = currentValue.desC_ROLE_SP,
                        DESC_ROLE_EN = currentValue.desC_ROLE_EN,
                    });
                }
            }
            return result;
        }


        public async Task<List<DocumentDto>> DescargarCatalogoPDFs()
        {
            List<DocumentDto> result = new List<DocumentDto>();

            var responseHttp = await http.GetAsJsonAsync<ResponseDto>(WebAPIEndPointsEnum.CATALOGO_PDFS.GetString());
            if (!responseHttp.error)
            {
                foreach (var currentValue in responseHttp.response)
                {
                    result.Add(new DocumentDto()
                    {
                        ID_INVITE = currentValue.iD_INVITE,
                        NAME = currentValue.name,
                        ID_REG = currentValue.iD_REG,
                        ID_EVENT_TYPE = currentValue.iD_EVENT_TYPE,
                        DESC_SPANISH = currentValue.desC_SPANISH,
                        DESC_ENGLISH = currentValue.desC_ENGLISH,
                        // FILE_BLOB = currentValue.filE_BLOB
                    });
                }
            }
            return result;
        }



    }

}
