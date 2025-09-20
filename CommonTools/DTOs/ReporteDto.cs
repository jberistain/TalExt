using CommonTools.DTOs.Register;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommonTools.DTOs
{
    public class ReporteDto
    {
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }

        List<ReporteDto> ReporteMesDto { get; set; }

        public string Mes { get; set; }
        public string TotalExtranjerosInmvitados { get; set; }
        public string TotalCartasGeneradas { get; set; }

        
        public List<CartasGeneradasVisado> CartasGeneradasVisadoList { get; set; }
        public List<DesgloceExtranjerosPorEvento> DesgloceExtranjerosPorEventoList { get; set; }
        public List<NacionalidadesFrecuentes> NacionalidadesFrecuentesList { get; set; }
        public List<RestringidosFrecuentes> RestringidosFrecuentesList { get; set; }

    }

    public class  CartasGeneradasVisado
    {
        public long TotalCartas { get; set; }
        public string Cartas { get; set; }

    }
    public class DesgloceExtranjerosPorEvento
    {
        public long Total { get; set; }
        public string Evento { get; set; }
    }
    public class NacionalidadesFrecuentes
    {
        public long Total { get; set; }
        public string Nacionalidad { get; set; }

    }
    public class RestringidosFrecuentes
    {
        public long Total { get; set; }
        public string Nacionalidad { get; set; }

    }


    public sealed class AsistentesPorMesDto
    {
        public int Year { get; set; }
        public int Month { get; set; }   // 1..12
        public int TotalAsistentes { get; set; }
    }

}
