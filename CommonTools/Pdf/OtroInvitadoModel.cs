using System;
using System.Collections.Generic;
using System.Text;

namespace CommonTools.Pdf
{

    public class OtroInvitadoModel : IOtroInvitadoModel
    {
        public string Id { get; set; }
        public string IdRegistro { get; set; }
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public string ActvidadEnMexico { get; set; }
        public string IdNacionalidad { get; set; }
        public string Nacionalidad { get; set; }
        public string NumPasaporte { get; set; }
        public List<IOtroInvitadoModel> InfoEventosList { get; set; }
    }
  
}
