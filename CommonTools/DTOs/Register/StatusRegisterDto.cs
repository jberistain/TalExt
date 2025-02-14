using System;
using System.Collections.Generic;
using System.Text;

namespace CommonTools.DTOs.Register
{
    public class StatusRegisterDto
    {
        public string DESC_STATUS_SP { get; set; }
        public string DESC_STATUS_EN { get; set; }
        public bool ACTIVE { get; set; } = true;
        public DateTime CREATED_DATE { get; set; }= DateTime.Now;

    }
}
