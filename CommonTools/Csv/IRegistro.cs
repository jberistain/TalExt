using System;
using System.Collections.Generic;
using System.Text;

namespace CommonTools.Csv
{
    public interface IRegistro
    {
        // string Id { get; set; }
        string Nombre { get; set; }
        string Nacionalidad { get; set; }
        string Empresa { get; set; }
        string Pasaporte { get; set; }
        string Actividad { get; set; }
        string FolioYFechaRegistro { get; set; }
        string CorreoElectronico { get; set; }
        string Estatus { get; set; }
    }
}
