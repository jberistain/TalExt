using System;
using System.Collections.Generic;
using System.Text;

namespace CommonTools.DTOs.Register
{
    public class RoleByCompanyRegisterDto
    {
        public int ID_COMPANY { get; set; }
        public string DESC_ROLE_SP { get; set; }
        public string DESC_ROLE_EN { get; set; }
        public bool ACTIVE { get; set; } = true;
        public DateTime CREATED_DATE { get; set; } = DateTime.Now;
        public int CREATED_BY { get; set; }
    }
}
