using System;
using System.Collections.Generic;
using System.Text;

namespace CommonTools.DTOs.Query
{
    public class FilterRegisterDto
    {
        public int IdEmpresa { get; set; }
        public string FechaInicio { get;set; }
        public string FechaFin { get; set; }
        public string Busqueda { get; set; }
        public int EstatusBuscado { get; set; }
        public int IdEvento { get;set; }

    }
}
