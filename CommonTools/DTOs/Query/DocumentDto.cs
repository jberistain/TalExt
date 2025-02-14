using System;
using System.Collections.Generic;
using System.Text;

namespace CommonTools.DTOs.Query
{
    public class DocumentDto
    {
        public int ID_INVITE { get; set; }
        public string NAME { get; set; }
        public int? ID_REG { get; set; }
        public int? ID_EVENT_TYPE { get; set; }
        public string DESC_SPANISH { get; set; }
        public string DESC_ENGLISH { get; set; }
        public string FILE_BLOB { get; set; }
    }
}
