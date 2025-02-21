using iTextSharp.text;
using System.Collections.Generic;

namespace CommonTools.Pdf
{
    public interface IReporteInfo
    {
        string NombreInvitado { get; set; }
        string Nacionalidad { get; set; }
        string NumPasaporte { get; set; }
        string PuestoParteStaff { get; set; }
        string FechaEntradaAlPais { get; set; }
        string FechaSalidaAlPais { get; set; }
        string TipoArchivoGenerado { get; set; }

        List<IInfoEvento> InfoEventosList { get; set; }
    }
    public interface IInfoEvento
    {
        string NombreEvento { get; set; }
        string FechaInicioEvento { get; set; } 
        string DiaInicioEvento { get; set; }
        string MesInicioEvento { get; set; }
        string AnioInicioEvento { get; set; }
        string FechaFinEvento { get; set; }
        string DiaFinEvento { get; set; }
        string MesFinEvento { get; set; }
        string AnioFinEvento { get; set; }
        string InmuebleEvento { get; set; }
        string UbicacionInmueble { get; set; }
        string IdEvento { get; set; }
        string IdInmuebleEvento { get; set; }


    }
}
