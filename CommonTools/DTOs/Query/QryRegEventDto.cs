using CommonTools.DTOs.Register;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace CommonTools.DTOs.Query
{
    public class QryRegEventDto
    {
        public int ID_REG { get; set; }
        public int? ID_REG_EVEN_DATE { get; set; }
        public int ID_GENDER { get; set; }

        public GendersDto CAT_GENDERS { get; set; }
        public int ID_COUNTRY { get; set; }

        public  CountriesDto CAT_COUNTRIES { get; set; }
        public int ID_NATIONALITY { get; set; }

        public NationalitiesDto CAT_NATIONALITIES { get; set; }
        public int? ID_ACTIVITY { get; set; }
       
        public  Activitiesdto CAT_ACTIVITIES { get; set; }
        public int? ID_AIRPORT { get; set; }
        
        public  AirPortsDto CAT_AIRPORTS { get; set; }
        public int? ID_AIR_LINE { get; set; }
       
        public  AirLinesDto CAT_AIR_LINES { get; set; }
        public int? ID_PROCESS { get; set; }
        
        public  ProcessDto CAT_PROCESS { get; set; }
        public int ID_STATUS { get; set; }
       
        public  StatusDto CAT_STATUS { get; set; }

        public int ID_COMPANY { get; set; }
        public virtual CompaniesDto CAT_COMPANIES { get; set; }
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
        public string LANGUAGE { get; set; }
        public bool CHECK_VERIFY { get; set; }
        public bool ACTIVE { get; set; }
        public DateTime? DATE_ARRIVE { get; set; }
        public DateTime? DATE_LEAVE { get; set; }
        public DateTime? MODIFY_DATE { get; set; }
        public DateTime CREATED_DATE { get; set; }
        public int MODIFY_BY { get; set; }
        public List<RegEvenStateDateDto> EVENTS { get; set; }

        public string EVENTOS { get; set; }

    }
}
