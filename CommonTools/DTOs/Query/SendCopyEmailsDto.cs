using System;

namespace CommonTools.DTOs.Query
{
    public class SendCopyEmailsDto
    {
        public int ID { get; set; }
        public string EMAIL { get; set; }
        public int ACTIVE { get; set; }
        public int MODIFY_BY { get; set; }
    }
}
