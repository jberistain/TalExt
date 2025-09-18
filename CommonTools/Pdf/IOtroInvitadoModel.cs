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
         string IdNacionalidad { get; set; }
         string Nacionalidad { get; set; }
         string NumPasaporte { get; set; }

         List<IOtroInvitadoModel> InfoEventosList { get; set; }
    }
  
}
