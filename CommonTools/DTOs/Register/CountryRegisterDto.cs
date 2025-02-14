using System;
using System.Collections.Generic;
using System.Text;

namespace CommonTools.DTOs.Register
{
    public class CountryRegisterDto
    {
        public string DESC_COUNTRY_SP { get; set; }
        public string DESC_COUNTRY_EN { get; set; }
        public bool ACTIVE { get; set; } = true;
        public bool RESTRICTION { get; set; }
        public DateTime? CREATED_DATE { get; set; } = DateTime.Now;
        public int CREATED_BY { get; set; }
        public int? MODIFY_BY { get; set; }
        public DateTime? MODIFY_DATE { get; set; }
    }
}
