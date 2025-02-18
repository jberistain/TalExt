using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MigracionTalentoExtranjero.Models.Enum
{
    //public enum HttpHostEnum
    //{
    //    HOST = 1001
    //}
    //public static class HttpHostEnumExtension
    //{
    //    public static string GetString(this HttpHostEnum me)
    //    {
    //        switch (me)
    //        {
    //            case HttpHostEnum.HOST:
    //                return "http://20.242.80.87/talentoextranjerowebapi/api/";
    //            default:
    //                return null;
    //        }
    //    }
    //}

    public enum WebAPIEndPointsEnum
    {
        //ALTAS Y CAMBIOS
        CREATE_NEW_REGISTER = 20001,
        CONFIRMA_CORREO_REGISTRO = 20002,
        ENVIA_CORREOS_INVITACIONES = 20003,
        UPDATE_REGISTER = 20004,
        CONFIRMA_CORREO_INVITACION = 20005,
        RECUPERA_PASSWORD_USUARIO = 20006,

        //CATALOGOS
        CATALOGO_ACTIVITIES = 21001,
        CATALOGO_AIRLINES = 21002,
        CATALOGO_AIRPORTS = 21003,
        CATALOGO_COMPANIES = 21004,
        CATALOGO_CONTRIES = 21005,
        CATALOGO_ESTATES = 21006,
        CATALOGO_EVENTS = 21007,
        CATALOGO_GENDERS = 21008,
        CATALOGO_NATIONALITIES = 21009,
        CATALOGO_PROCESS = 21010,
        CATALOGO_ROLE_BY_COMPANIES = 21011,
        CATALOGO_STATUS = 21012,
        CATALOGO_TIPOS_EVENTOS = 21013,
        CATALOGO_DOCUMENTOS = 21014,
        CATALOGO_USERS = 21015,
        CATALOGO_PDFS = 21016,
        CATALOGO_FIRMAS_DOCUMENTOS = 21017,
        CATALOGO_PERFILES = 21018,
        CATALOGO_CORREOS_PARA_COPIA = 21019,

        // CONSULTAS
        CONSULTA_REGISTRO_POR_PASAPORTE = 22013,

        CONSULTA_REGISTROS_ENCONTRADOS = 23001,
        CONSULTA_REGISTRO_POR_CODIGO_SECRETO = 23002,
        CONSULTA_REGISTRO_POR_ID_REGISTRO = 23003,
        CONSULTA_DOCUMENTOS_POR_CODIGO_SECRETO = 23004,
        CONSULTA_DOCUMENTO_POR_ID = 23005,
        CONSULTA_DOCUMENTOS_POR_TIPO_RESTRICCION = 23006,
        CONSULTA_PDF_POR_ID = 23007,


        CONSULTA_AVISO_PRIVACIDAD = 23008,

        // ELIMINACIONES
        ELIMINAR_REGISTRO_POR_ID = 24001,
    }
    public static class WebAPIEndPointsEnumExtension
    {
        public static string GetString(this WebAPIEndPointsEnum me)
        {
            switch (me)
            {
                case WebAPIEndPointsEnum.CREATE_NEW_REGISTER:
                    return "RegisterEvents/Register";
                case WebAPIEndPointsEnum.CONFIRMA_CORREO_REGISTRO:
                    return "RegisterEvents/ConfirmRegisterEmail";
                case WebAPIEndPointsEnum.ENVIA_CORREOS_INVITACIONES:
                    return "RegisterEvents/SendInvitation";
                case WebAPIEndPointsEnum.UPDATE_REGISTER:
                    return "RegisterEvents/Update";
                case WebAPIEndPointsEnum.CONFIRMA_CORREO_INVITACION:
                    return "RegisterEvents/ConfirmInvitationReceivedEmail";
                case WebAPIEndPointsEnum.CATALOGO_ACTIVITIES:
                    return "Register";
                case WebAPIEndPointsEnum.RECUPERA_PASSWORD_USUARIO:
                    return "Users/RecuperaPasswordPorCorreo";
                case WebAPIEndPointsEnum.CATALOGO_AIRLINES:
                    return "AirLines";
                case WebAPIEndPointsEnum.CATALOGO_AIRPORTS:
                    return "AirPorts";
                case WebAPIEndPointsEnum.CATALOGO_COMPANIES:
                    return "Companies";
                case WebAPIEndPointsEnum.CATALOGO_CONTRIES:
                    return "Countries";
                case WebAPIEndPointsEnum.CATALOGO_ESTATES:
                    return "Estates";
                case WebAPIEndPointsEnum.CATALOGO_EVENTS:
                    return "Events";
                case WebAPIEndPointsEnum.CATALOGO_GENDERS:
                    return "Genders";
                case WebAPIEndPointsEnum.CATALOGO_USERS:
                    return "Users";
                case WebAPIEndPointsEnum.CATALOGO_PDFS:
                    return "Documents";
                case WebAPIEndPointsEnum.CATALOGO_NATIONALITIES:
                    return "Nationalities";
                case WebAPIEndPointsEnum.CATALOGO_PERFILES:
                    return "Roles";
                case WebAPIEndPointsEnum.CATALOGO_TIPOS_EVENTOS:
                    return "TiposEventos";
                case WebAPIEndPointsEnum.CATALOGO_PROCESS:
                    return "Register";
                case WebAPIEndPointsEnum.CATALOGO_ROLE_BY_COMPANIES:
                    return "Register";
                case WebAPIEndPointsEnum.CATALOGO_STATUS:
                    return "Register";
                case WebAPIEndPointsEnum.CATALOGO_DOCUMENTOS:
                    return "RegInvite";
                case WebAPIEndPointsEnum.CONSULTA_REGISTRO_POR_PASAPORTE:
                    return "RegisterEvents/findByPassport";
                case WebAPIEndPointsEnum.CONSULTA_REGISTROS_ENCONTRADOS:
                    return "RegisterEvents/ListaEventos";
                case WebAPIEndPointsEnum.CONSULTA_REGISTRO_POR_CODIGO_SECRETO:
                    return "RegisterEvents/findBySecretCode";
                case WebAPIEndPointsEnum.CONSULTA_REGISTRO_POR_ID_REGISTRO:
                    return "RegisterEvents/findById";
                case WebAPIEndPointsEnum.CONSULTA_DOCUMENTOS_POR_CODIGO_SECRETO:
                    return "RegInvite/GetAllDocsBySecretCode";
                case WebAPIEndPointsEnum.CONSULTA_DOCUMENTO_POR_ID:
                    return "RegInvite/GetById";
                case WebAPIEndPointsEnum.CONSULTA_DOCUMENTOS_POR_TIPO_RESTRICCION:
                    return "RegInvite/GetAllDocsByRestrictedFlag";
                case WebAPIEndPointsEnum.ELIMINAR_REGISTRO_POR_ID:
                    return "RegisterEvents/DeleteById";
                case WebAPIEndPointsEnum.CONSULTA_PDF_POR_ID:
                    return "Documents/GetById";
                case WebAPIEndPointsEnum.CONSULTA_AVISO_PRIVACIDAD:
                    return "AvisoPrivacidad";
                case WebAPIEndPointsEnum.CATALOGO_FIRMAS_DOCUMENTOS:
                    return "RegInvite/GetInfoSignsBlob";
                case WebAPIEndPointsEnum.CATALOGO_CORREOS_PARA_COPIA:
                    return "SendCopyEmails";
                default:
                    return null;
            }
        }
    }

}