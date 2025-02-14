using System;
using System.Collections.Generic;
using System.Text;

namespace CommonTools.DTOs.Register
{
    public class UserRegisterDto
    {
        public int ID_USER { get; set; }
        public int? ID_ROLE { get; set; }
        public int? ID_COMPANY { get; set; }
        public string CVE_USER { get; set; }
        public string USERNAME { get; set; }
        public string PASSWORD_USER { get; set; }
        public string NAME_USER { get; set; }
        public string LAST_NAME_USER { get; set; }
        public string EMAIL_USER { get; set; }
        public bool ACTIVE { get; set; }=true;
        public DateTime CREATED_DATE { get; set; } = DateTime.Now;

        public int CREATED_BY { get; set; }
        public int? MODIFY_BY { get; set; }
        public DateTime? MODIFY_DATE { get; set; }


    }

    public class UserRegisterUpdatePwdDto : UserRegisterDto
    {
        public string NewPassword { get; set; }
    }

}
