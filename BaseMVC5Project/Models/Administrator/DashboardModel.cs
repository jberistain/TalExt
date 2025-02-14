using CommonTools.Csv;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigracionTalentoExtranjero.Models.Administrator
{
    public class DashboardModel
    {
        public string Empresa { get; set; }
        public string TextoBusqueda { get; set; }
        public string Fecha1 { get; set; }
        public string Fecha2 { get; set; }
        public string EstatusBuscado { get; set; }

        public List<Registro> Registros { get; set; }

    }
    public class Registro : IRegistro
    {
        public string Id { get; set; }
        public string Nombre { get; set; }
        public string Nacionalidad { get; set; }
        public string Empresa { get; set; }
        public string Pasaporte { get; set; }
        public string Actividad { get; set; }
        public string FolioYFechaRegistro { get; set; }
        public string ClaveSecreta { get; set; }
        public string CorreoElectronico { get; set; }
        public string Estatus { get; set; }
        public string EstatusDesc { get; set; }
        public string Edicion { get; set; }
        public string CssBackground { get
            {
                string cssColor;
                switch(Estatus)
                {
                    case "1":
                        cssColor = "#777777";
                        break;
                    case "2":
                        cssColor = "lightgray";
                        break;
                    case "3":
                        cssColor = "#f0ad4e";
                        break;
                    case "4":
                        cssColor = "#5cb85c";
                        break;
                    case "5":
                        cssColor = "#337ab7";
                        break;
                    default:
                        cssColor = "#777777";
                        break;
                }
                return cssColor;
            } }
        
        public Registro() { }
        public Registro(Registro registro)
        {
            
        }

    }
}
