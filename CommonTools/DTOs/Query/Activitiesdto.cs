using System;
using System.Collections.Generic;
using System.Text;

namespace CommonTools.DTOs.Query
{
    public class Activitiesdto
    {
        public int ID_ACTIVITY { get; set; }
        public string DESC_ACTIVITY_SP { get; set; }
        public string DESC_ACTIVITY_EN { get; set; }
        public bool ACTIVE { get; set; } = true;
        public DateTime? CREATED_DATE { get; set; }
    }
}
