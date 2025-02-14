using System;
using System.Collections.Generic;
using System.Text;

namespace CommonTools.DTOs.Query
{
    public class ProcessDto
    {
        public int ID_PROCESS { get; set; }
        public string DESC_PROCESS_SP { get; set; }
        public string DESC_PROCESS_EN { get; set; }
        public string URL_PROCESS { get; set; }
        public bool ACTIVE { get; set; }
        public DateTime CREATED_DATE { get; set; }
    }
}
