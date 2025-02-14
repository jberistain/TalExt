using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using CommonTools.DTOs;
using CommonTools.DTOs.Query;
using CommonTools.DTOs.Register;
using MigracionTalentoExtranjero.Models;
using MigracionTalentoExtranjero.Models.Enum;
using MigracionTalentoExtranjero.Models.Home;
using MigracionTalentoExtranjero.Models.Utils;

namespace BaseMVC5Project.Models.Utils
{
    public class ComboBoxHelper
    {

        HttpManager http = new HttpManager(Constants.WebAPIUrl);
        CatalogsManager catalogsRequest;

        private const string CAT_DIA = "DIA";
        private const string CAT_MES = "MES";
        private const string CAT_ANIO = "ANIO";
        private const string CAT_10ANIO_FUTURO = "CAT_10ANIO_FUTURO";
        private const string CAT_10ANIO_PASADO = "CAT_10ANIO_PASADO";
        private const string CAT_AEROLINEAS = "AEROLINEAS";
        private const string CAT_NACIONALIDADES = "NACIONALIDADES";
        private const string CAT_AEROPUERTOS = "AEROPUERTOS";
        private const string CAT_COMPANIAS = "COMPANIAS";
        private const string CAT_INMUEBLES = "INMUEBLES";
        private const string CAT_EVENTOS = "EVENTOS";
        private const string CAT_GENEROS = "GENEROS";
        private const string CAT_GENEROS_EN = "CAT_GENEROS_EN";
        private const string CAT_PAISES = "PAISES";
        private const string CAT_SI_NO = "CAT_SI_NO";
        private const string CAT_TIPOS_EVENTOS = "TIPOSEVENTOS";
        private const string CAT_YES_NO = "CAT_YES_NO";
        private const string CATALOGO_PERFILES = "CATALOGO_PERFILES";
        public async Task< List<SelectListItem>> GetSearchComboBox(string catalogSearch, int idSearchInt, string idSearchStr)
        {
            List<SelectListItem> result = new List<SelectListItem>();
            result.Add(new SelectListItem
            {
                Text = "",
                Value = ""
            });
            switch (catalogSearch)
            {
                case CAT_SI_NO:                    
                    //Obtener las opciones del catalogo almacen
                    List<string> siNo = RegistroInvitadoModel.CatSiNo();
                    foreach (string opt in siNo)
                    {
                        result.Add(new SelectListItem
                        {
                            Text = opt,
                            Value = opt
                        });
                    }
                    break;
                case CAT_YES_NO:                    
                    //Obtener las opciones del catalogo almacen
                    List<string> yesNo = RegistroInvitadoModel.CatYesNo();
                    foreach (string opt in yesNo)
                    {
                        result.Add(new SelectListItem
                        {
                            Text = opt,
                            Value = opt
                        });
                    }
                    break;
                case CAT_DIA:                    
                    //Obtener las opciones del catalogo almacen
                    List<string> dias = RegistroInvitadoModel.CatDias();
                    foreach (string opt in dias)
                    {
                        result.Add(new SelectListItem
                        {
                            Text = opt,
                            Value = opt
                        });
                    }
                    break;
                case CAT_MES:                    
                    //Obtener las opciones del catalogo almacen
                    List<string> meses = RegistroInvitadoModel.CatMes();
                    foreach (string opt in meses)
                    {
                        result.Add(new SelectListItem
                        {
                            Text = opt,
                            Value = opt
                        });
                    }
                    break;
                case CAT_ANIO:                    
                    //Obtener las opciones del catalogo almacen
                    List<string> anios = RegistroInvitadoModel.CatAnios();
                    foreach (string opt in anios)
                    {
                        result.Add(new SelectListItem
                        {
                            Text = opt,
                            Value = opt
                        });
                    }
                    break;
                case CAT_10ANIO_FUTURO:                    
                    //Obtener las opciones del catalogo almacen
                    List<string> futuro10anios = RegistroInvitadoModel.Cat10AniosFuturo();
                    foreach (string opt in futuro10anios)
                    {
                        result.Add(new SelectListItem
                        {
                            Text = opt,
                            Value = opt
                        });
                    }
                    break;
                case CAT_10ANIO_PASADO:                    
                    //Obtener las opciones del catalogo almacen
                    List<string> pasado10anios = RegistroInvitadoModel.Cat10AniosPasado();
                    foreach (string opt in pasado10anios)
                    {
                        result.Add(new SelectListItem
                        {
                            Text = opt,
                            Value = opt
                        });
                    }
                    break;

                case CAT_AEROLINEAS:
                    catalogsRequest = new CatalogsManager(http);
                    List<AirLinesDto> catAerolineasList = await catalogsRequest.DescargarCatalogoAerolineas();

                    foreach (AirLinesDto opt in catAerolineasList)
                    {
                        result.Add(new SelectListItem
                        {
                            Text = opt.DESC_AIR_LINE_EN,
                            Value = opt.ID_AIR_LINE.ToString()
                        });
                    }
                    break;
                case CAT_NACIONALIDADES:
                    catalogsRequest = new CatalogsManager(http);
                    List<NationalitiesDto> catList = await catalogsRequest.DescargarCatalogoNacionalidades();

                    foreach (NationalitiesDto opt in catList)
                    {
                        result.Add(new SelectListItem
                        {
                            Text = opt.DESC_NACIONALITY_SP,
                            Value = opt.ID_NALCIONALITY.ToString()
                        });
                    }
                    break;
                case CAT_AEROPUERTOS:
                    catalogsRequest = new CatalogsManager(http);
                    List<AirPortsDto> catAeropuertosList = await catalogsRequest.DescargarCatalogoAeropuertos();

                    foreach (AirPortsDto opt in catAeropuertosList)
                    {
                        result.Add(new SelectListItem
                        {
                            Text = opt.DESC_AIRPORT_SP,
                            Value = opt.ID_AIRPORT.ToString()
                        });
                    }
                    break;
                case CAT_COMPANIAS:
                    catalogsRequest = new CatalogsManager(http);
                    List<CompaniesDto> catCompaniasList = await catalogsRequest.DescargarCatalogoCompanias();

                    foreach (CompaniesDto opt in catCompaniasList)
                    {
                        result.Add(new SelectListItem
                        {
                            Text = opt.DESC_COMPANY_SP,
                            Value = opt.ID_COMPANY.ToString()
                        });
                    }
                    break;
                case CAT_INMUEBLES:
                    catalogsRequest = new CatalogsManager(http);
                    List<EstatesDto> catInmueblesList = await catalogsRequest.DescargarCatalogoInmuebles();

                    foreach (EstatesDto opt in catInmueblesList)
                    {
                        result.Add(new SelectListItem
                        {
                            Text = opt.DESC_ESTATE_SP,
                            Value = opt.ID_ESTATE.ToString()
                        });
                    }
                    break;
                case CAT_EVENTOS:
                    catalogsRequest = new CatalogsManager(http);
                    List<EventsDto> catEventosList = await catalogsRequest.DescargarCatalogoEventos();

                    foreach (EventsDto opt in catEventosList)
                    {
                        result.Add(new SelectListItem
                        {
                            Text = opt.DESC_EVENT_SP,
                            Value = opt.ID_EVENT.ToString()
                        });
                    }
                    break;
                case CAT_GENEROS:
                    catalogsRequest = new CatalogsManager(http);
                    List<GendersDto> catGenerosList = await catalogsRequest.DescargarCatalogoGeneros();

                    foreach (GendersDto opt in catGenerosList)
                    {
                        result.Add(new SelectListItem
                        {
                            Text = opt.DESC_GENDER_SP,
                            Value = opt.ID_GENDER.ToString()
                        });
                    }
                    break;
                case CAT_GENEROS_EN:
                    catalogsRequest = new CatalogsManager(http);
                    List<GendersDto> catGenerosENList = await catalogsRequest.DescargarCatalogoGenerosEN();

                    foreach (GendersDto opt in catGenerosENList)
                    {
                        result.Add(new SelectListItem
                        {
                            Text = opt.DESC_GENDER_SP,
                            Value = opt.ID_GENDER.ToString()
                        });
                    }
                    break;
                case CAT_PAISES:
                    catalogsRequest = new CatalogsManager(http);
                    List<CountriesDto> catPaisesList = await catalogsRequest.DescargarCatalogoPaises();

                    foreach (CountriesDto opt in catPaisesList)
                    {
                        result.Add(new SelectListItem
                        {
                            Text = opt.DESC_COUNTRY_SP,
                            Value = opt.ID_COUNTRY.ToString()
                        });
                    }
                    break;
                case CAT_TIPOS_EVENTOS:
                    catalogsRequest = new CatalogsManager(http);
                    List<EventTypesDto> catTiposEventosList = await catalogsRequest.DescargarCatalogoTiposEventos();

                    foreach (var opt in catTiposEventosList)
                    {
                        result.Add(new SelectListItem
                        {
                            Text = opt.DESC_ACTIVITY_SP,
                            Value = opt.ID_EVENT_TYPE.ToString()
                        });
                    }
                    break;
                case CATALOGO_PERFILES:
                    catalogsRequest = new CatalogsManager(http);
                    List<RoleByCompanyDto> catPerfilesList = await catalogsRequest.DescargarCatalogoRoles();

                    foreach (var opt in catPerfilesList)
                    {
                        result.Add(new SelectListItem
                        {
                            Text = opt.DESC_ROLE_SP,
                            Value = opt.ID_ROLE.ToString()
                        });
                    }
                    break;
            }
            //Validar escenario en el que no se encontraron registros
            if (result.Count <= 1)
            {
                string lsOpcionList = "SIN OPCIONES";

                result.Add(new SelectListItem
                {
                    Text = lsOpcionList, //"-- NO HAY OPCIONES --",
                    Value = "0"
                });
            }

            return result;
        }

    }
}