using System;

namespace CommonTools.DTOs.Register
{
    public class SendCopyEmailsRegisterDto
    {
        public int ID { get; set; }
        public string EMAIL { get; set; }
        public int ACTIVE { get; set; }
        public DateTime CREATED_DATE { get; set; }
        public int CREATED_BY { get; set; }
        public int MODIFY_BY { get; set; }
    }
}
