using System;
using System.Collections.Generic;
using System.Text;

namespace CommonTools.DTOs.Query
{
    public class StatusDto
    {
        public int ID_STATUS { get; set; }
        public string DESC_STATUS_SP { get; set; }
        public string DESC_STATUS_EN { get; set; }
        public bool ACTIVE { get; set; }
        public DateTime CREATED_DATE { get; set; }
    }
}
