using System;
using System.Collections.Generic;
using System.Text;

namespace CommonTools.DTOs.Register
{
    public class RegEventDto
    {
        public int? ID_REG_EVEN_DATE { get; set; }
        public int ID_GENDER { get; set; }
        public int ID_COUNTRY { get; set; }
        public int ID_NATIONALITY { get; set; }
        public int? ID_ACTIVITY { get; set; }
        public int? ID_AIRPORT { get; set; }
        public int? ID_AIR_LINE { get; set; }
        public int? ID_PROCESS { get; set; }
        public int ID_STATUS { get; set; }
        public int? ID_COMPANY { get; set; }
        public string PASSPORT_NUM { get; set; }
        public DateTime DATE_VIG_INI { get; set; }
        public DateTime DATE_VIG_FIN { get; set; }
        public string PASSPORT_NAME { get; set; }
        public string PASSPORT_LASTNAME { get; set; }
        public string EMAIL { get; set; }
        public string ACTUAL_JOB { get; set; }
        public bool? EXPELLED_MEX { get; set; }
        public string EXPELLED_MEX_DESC { get; set; }
        public bool? CRIMINAL_RECORD_MEX { get; set; }
        public string EVENT_JOB { get; set; }
        public DateTime? DATE_EVENT_INI { get; set; }
        public DateTime? DATE_EVENT_FIN { get; set; }
        public string FLIGHT { get; set; }
        public string FLIGHT_NUMBER { get; set; }
        public string SECRET_CODE { get; set; }
        public string ACTIVITY_COUNTRY { get; set; }
        public string ACTIVITY_MEXICO { get; set; }
        public DateTime? DATE_ARRIVE { get; set; }
        public DateTime? DATE_LEAVE { get; set; }
        public string LANGUAGE { get; set; }
        public bool CHECK_VERIFY { get; set; } = false;
        public bool ACTIVE { get; set; } = true;
        public string NOMBRE_NUEVA_EMPRESA { get; set; }
        public string NOMBRE_NUEVO_AEROPUERTO { get; set; }
        public string NOMBRE_NUEVA_AEROLINEA { get; set; }
        public List<RegEvenStateDateDto> EVENTS { get; set; }
    }
}
