using System;
using System.Collections.Generic;
using System.Text;

namespace CommonTools.DTOs.Register
{
    public class RegEvenStateDateDto
    {

        public int ID_REG_EVEN_DATE { get; set; }
        public int ID_EVENT { get; set; }
        public int ID_ESTATE { get; set; }
        public int ID_REG { get; set; }
        public string DESC_LOCATION { get; set; }
        public DateTime EVENT_DATE { get; set; }
        public DateTime EVENT_DATE_FIN { get; set; }
        public bool ACTIVE { get; set; } = true;
        public DateTime CREATED_DATE { get; set; }= DateTime.Now;
        public int? MODIFY_BY { get; set; }
        public string NOMBRE_NUEVO_INMUEBLE { get; set; }
    }
}
