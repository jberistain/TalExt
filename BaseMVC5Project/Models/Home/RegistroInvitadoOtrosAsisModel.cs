using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using BaseMVC5Project.Models.Utils;
using CommonTools.Pdf;
using MigracionTalentoExtranjero.Models.Enum;
using MigracionTalentoExtranjero.Models.Utils;
using Org.BouncyCastle.Crypto.Operators;

namespace MigracionTalentoExtranjero.Models.Home
{
    public class RegistroInvitadoOtrosAsisModel : RegistroInvitadoModel
    {
        public List<OtroInvitadoModel> OtrosAsistentes { get; set; } = new List<OtroInvitadoModel>();
        public List<string> OtrosAsistentesNacionalidadesComboBox { get; set; } = new List<string>();
        public List<OtroInvitadoModel> ArtistasInvitados { get; set; } = new List<OtroInvitadoModel>();
        public string IdiomaInvitacion { get; set; } 
        public int IdEvento { get; set; } 
    }
}