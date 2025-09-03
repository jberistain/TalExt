using System;
using System.Collections.Generic;
using System.Text;

namespace CommonTools.DTOs.Register
{
    public class RegEvenAnotherAssistantsAdmontDto
    {
        public int ID { get; set; }
        public string PASSPORT_LASTNAME { get; set; }
        public string PASSPORT_NAME { get; set; }
        public string ACTIVITY_MEXICO { get; set; }
        public int ID_NATIONALITY { get; set; }
        public string PASSPORT_NUM { get; set; }
        public bool ACTIVE { get; set; }
        public DateTime CREATED_DATE { get; set; } = DateTime.Now;
        public int CREATED_BY { get; set; }
        public DateTime? MODIFY_DATE { get; set; } = new DateTime(1900, 01, 01);
        public int? MODIFY_BY { get; set; }


    }
}
