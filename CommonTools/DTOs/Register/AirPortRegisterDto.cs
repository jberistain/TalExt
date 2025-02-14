using System;
using System.Collections.Generic;
using System.Text;

namespace CommonTools.DTOs.Register
{
    public class AirPortRegisterDto
    {
        public string DESC_AIRPORT_SP { get; set; }
        public string DESC_AIRPORT_EN { get; set; }

        public string IATA { get; set; }
        public string LOCATION { get; set; }

        public int CREATED_BY { get; set; }

        public bool ACTIVE { get; set; } = true; 
        public DateTime? CREATED_DATE { get; set; } = DateTime.Now;
        public int? MODIFY_BY { get; set; }
        public DateTime? MODIFY_DATE { get; set; }

    }
}
