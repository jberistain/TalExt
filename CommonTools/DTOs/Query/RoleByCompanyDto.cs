using System;
using System.Collections.Generic;
using System.Text;

namespace CommonTools.DTOs.Register
{
    public class RoleByCompanyDto
    {
        public int ID_ROLE { get; set; }
        public int ID_COMPANY { get; set; }
        public string DESC_ROLE_SP { get; set; }
        public string DESC_ROLE_EN { get; set; }
        public bool ACTIVE { get; set; }
        public DateTime? MODIFY_DATE { get; set; }
        public int? MODIFY_BY { get; set; }
    }
}
