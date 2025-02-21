using System;
using System.Collections.Generic;
using System.Text;

namespace CommonTools.Pdf
{

    public class ReportInformation : IReporteInfo
    {
        public string NombreInvitado { get; set; }
        public string Nacionalidad { get; set; }
        public string NumPasaporte { get; set; }
        public string PuestoParteStaff { get; set; }
        public string FechaEntradaAlPais { get; set; }
        public string FechaSalidaAlPais { get; set; }
        public string TipoArchivoGenerado { get; set; }

        public List<IInfoEvento> InfoEventosList { get; set; }
    }
  

    public class InfoEventoModel : IInfoEvento
    {
        public string Id { get; set; }
        public string NombreEvento { get; set; } = string.Empty;
        public string FechaInicioEvento { get; set; } = string.Empty;
        public string DiaInicioEvento { get; set; } = string.Empty;
        public string MesInicioEvento { get; set; } = string.Empty;
        public string AnioInicioEvento { get; set; } = string.Empty;
        public string FechaFinEvento { get; set; } = string.Empty;
        public string DiaFinEvento { get; set; } = string.Empty;
        public string MesFinEvento { get; set; } = string.Empty;
        public string AnioFinEvento { get; set; } = string.Empty;


        public string InmuebleEvento { get; set; } = string.Empty;
        public string NuevoInmuebleEvento { get; set; } = string.Empty;
        public string UbicacionInmueble { get; set; } = string.Empty;
        public string IdEvento { get; set; } = string.Empty;
        public string IdInmuebleEvento { get; set; } = string.Empty;

    }
}
