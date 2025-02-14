using System;
using System.Collections.Generic;
using System.Text;

namespace CommonTools.Models
{
    public interface IRegEventsDto
    {
        int ID_REG { get; set; }
        int ID_EVENT { get; set; }
        int ID_GENDER { get; set; }
        int ID_COUNTRY { get; set; }
        int ID_NALCIONALITY { get; set; }
        int ID_ACTIVITY { get; set; }
        int ID_AIRPORT { get; set; }
        int ID_AIR_LINE { get; set; }
        int ID_PROCESS { get; set; }
        int ID_STATUS { get; set; }
        string PASSPORT_NUM { get; set; }
        DateTime DATE_VIG_INI { get; set; }
        DateTime DATE_VIG_FIN { get; set; }
        string PASSPORT_NAME { get; set; }
        string PASSPORT_LASTNAME { get; set; }
        string EMAIL { get; set; }
        string ACTUAL_JOB { get; set; }
        byte EXPELLED_MEX { get; set; }
        byte CRIMINAL_RECORD_MEX { get; set; }
        string EVENT_JOB { get; set; }
        DateTime DATE_EVENT_INI { get; set; }
        DateTime DATE_EVENT_FIN { get; set; }
        string FLIGHT { get; set; }
        string FLIGHT_NUMBER { get; set; }
        byte ACTIVE { get; set; }
        DateTime CREATED_DATE { get; set; }
        int CREATED_BY { get; set; }
        DateTime MODIFY_DATE { get; set; }
        int MODIFY_BY { get; set; }

        List<IEventsDto> Events { get; set; }

    }
}
