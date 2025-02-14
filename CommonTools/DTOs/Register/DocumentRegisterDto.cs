using System;
using System.Collections.Generic;
using System.Text;

namespace CommonTools.DTOs.Register
{
    public class DocumentRegisterDto
    {
        public string NAME { get; set; }
        public int? ID_REG { get; set; }
        public int? ID_EVENT_TYPE { get; set; }
        public string DESC_SPANISH { get; set; }
        public string DESC_ENGLISH { get; set; }
        public string FILE_BLOB { get; set; }
        public DateTime? CREATED_DATE { get; set; } = DateTime.Now;
        public int CREATED_BY { get; set; } = 2;
        public DateTime? MODIFY_DATE { get; set; }
        public int? MODIFY_BY { get; set; }
    }
}
