using System;
using System.Collections.Generic;
using System.Text;

namespace CommonTools.DTOs.Register
{
    public class EventRegisterDto
    {
        public int ID_COMPANY { get; set; }
        public int ID_EVENT_TYPE { get; set; }
        public int? ID_ESTATE { get; set; }
        public string DESC_LOCATION { get; set; }
        public string DESC_EVENT_SP { get; set; }
        public string DESC_EVENT_EN { get; set; }
        public string EMAIL1 { get; set; }
        public string EMAIL2 { get; set; }
        public DateTime DATE_INI { get; set; }
        public DateTime DATE_FIN { get; set; }
        public bool ACTIVE { get; set; } = true;
        public DateTime CREATED_DATE { get; set; } = DateTime.Now;
        public int CREATED_BY { get; set; }
        public int? MODIFY_BY { get; set; }

    }
}
