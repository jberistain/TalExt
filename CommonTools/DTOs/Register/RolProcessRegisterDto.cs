using System;
using System.Collections.Generic;
using System.Text;

namespace CommonTools.DTOs.Register
{
    public class RolProcessRegisterDto
    {
        public int ID_ROLE { get; set; }
        public int? ID_PROCESS { get; set; }
        public bool ACTIVE { get; set; }
        public DateTime CREATED_DATE { get; set; }
        public int CREATED_BY { get; set; }
    }
}
