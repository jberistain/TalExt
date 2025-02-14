using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MigracionTalentoExtranjero.Models.Enum
{
    public enum LanguageEnum
    {
        ES = 1001,
        EN = 1002
    }
    public static class LanguageEnumExtension
    {
        public static string GetString(this LanguageEnum me)
        {
            switch (me)
            {
                case LanguageEnum.ES:
                    return "ES";
                case LanguageEnum.EN:
                    return "EN";
                default:
                    return null;
            }
        }
    }


    public enum CatalogosEnum
    {
        DIA = 1101,
        MES = 1102,
        ANIO= 1103,
        CAT_AEROLINEAS = 1104,
        CAT_NACIONALIDADES = 1105,
        CAT_AEROPUERTOS = 1106,
        CAT_COMPANIAS = 1107,
        CAT_INMUEBLES = 1108,
        CAT_EVENTOS = 1109,
        CAT_GENEROS = 1110,
        CAT_PAISES = 1111,
        CAT_SI_NO = 1112,
        CAT_TIPOS_EVENTOS = 1113,
        CAT_10ANIO_FUTURO = 1114,
        CAT_10ANIO_PASADO = 1115,
        CAT_YES_NO = 1116,
        CAT_GENEROS_EN = 1117,
        CATALOGO_PERFILES = 1118,

    }

    public static class CatalogosEnumExtension
    {
        public static string GetString(this CatalogosEnum me)
        {
            switch (me)
            {
                case CatalogosEnum.DIA:
                    return "DIA";
                case CatalogosEnum.MES:
                    return "MES";
                case CatalogosEnum.ANIO:
                    return "ANIO";
                case CatalogosEnum.CAT_10ANIO_FUTURO:
                    return "CAT_10ANIO_FUTURO";
                case CatalogosEnum.CAT_10ANIO_PASADO:
                    return "CAT_10ANIO_PASADO";
                case CatalogosEnum.CAT_AEROLINEAS:
                    return "AEROLINEAS";
                case CatalogosEnum.CAT_NACIONALIDADES:
                    return "NACIONALIDADES";
                case CatalogosEnum.CAT_AEROPUERTOS:
                    return "AEROPUERTOS";
                case CatalogosEnum.CAT_COMPANIAS:
                    return "COMPANIAS";
                case CatalogosEnum.CAT_INMUEBLES:
                    return "INMUEBLES";
                case CatalogosEnum.CAT_EVENTOS:
                    return "EVENTOS";
                case CatalogosEnum.CAT_GENEROS:
                    return "GENEROS";
                case CatalogosEnum.CAT_PAISES:
                    return "PAISES";
                case CatalogosEnum.CAT_SI_NO:
                    return "CAT_SI_NO";
                case CatalogosEnum.CAT_TIPOS_EVENTOS:
                    return "TIPOSEVENTOS";
                case CatalogosEnum.CAT_YES_NO:
                    return "CAT_YES_NO";
                case CatalogosEnum.CAT_GENEROS_EN:
                    return "CAT_GENEROS_EN";
                case CatalogosEnum.CATALOGO_PERFILES:
                    return "CATALOGO_PERFILES";
                default:
                    return null;
            }
        }
    }


}