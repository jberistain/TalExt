using System;
using System.Collections.Generic;
using System.Text;

namespace CommonTools.Pdf
{

    public interface IOtroInvitadoModel
    {
         string IdRegistro { get; set; }
         string Nombre { get; set; }
         string Apellidos { get; set; }
         string ActvidadEnMexico { get; set; }
         int IdNacionalidad { get; set; }
         string NumPasaporte { get; set; }

         List<IOtroInvitadoModel> InfoEventosList { get; set; }
    }
  
}
