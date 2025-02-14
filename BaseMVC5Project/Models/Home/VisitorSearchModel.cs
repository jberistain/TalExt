using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigracionTalentoExtranjero.Models.Home
{
    public class VisitorSearchModel
    {

        public string IdReg { get; set; }
        public string Codigo { get; set; }
        public string Mensaje { get; set; }
        public string Nombre { get; set; }
        public string Pais { get; set; }
        public string Detalle { get; set; }
        public string Empresa { get; set; }
        public string TiempoTranscurrido { get; set; }

        public List<Dictionary<string, string>> ListaBotonesDocumentos { get; set; } = new List<Dictionary<string, string>>();


        public VisitorSearchModel() { }


    }
}
