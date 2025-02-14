using Microsoft.EntityFrameworkCore;
using Migracion.Talento.Entities.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Migracion.Talento.Models
{
    [PrimaryKey(nameof(ID_REG))]

    public class RegEvents
    {
        public int ID_REG { get; set; }
        public int? ID_REG_EVEN_DATE { get; set; }
        public int ID_GENDER { get; set; }
        [ForeignKey(nameof(ID_GENDER))]
        public virtual Genders CAT_GENDERS { get; set; }
        public int ID_COUNTRY { get; set; }   
        [ForeignKey(nameof(ID_COUNTRY))]
        public virtual Countries CAT_COUNTRIES { get; set; }
        public int ID_NATIONALITY { get; set; }
        [ForeignKey(nameof(ID_NATIONALITY))]
        public virtual Nationalities CAT_NATIONALITIES { get; set; }
        public int? ID_ACTIVITY { get; set; }
        [ForeignKey(nameof(ID_ACTIVITY))]
        public virtual Activities CAT_ACTIVITIES { get; set; }
        public int ID_AIRPORT { get; set; }
        [ForeignKey(nameof(ID_AIRPORT))]
        public virtual AirPorts CAT_AIRPORTS { get; set; }
        public int ID_AIR_LINE { get; set; }
        [ForeignKey(nameof(ID_AIR_LINE))]
        public virtual AirLines CAT_AIR_LINES { get; set; }
        public int? ID_PROCESS { get; set; }
        [ForeignKey(nameof(ID_PROCESS))]
        public virtual Process CAT_PROCESS { get; set; }
        public int ID_STATUS { get; set; }
        [ForeignKey(nameof(ID_STATUS))]
        public virtual Status CAT_STATUS { get; set; }
        public int? ID_COMPANY { get; set; }
        [ForeignKey(nameof(ID_COMPANY))]
        public virtual Companies CAT_COMPANIES { get; set; }
        public string PASSPORT_NUM { get; set; }
        public DateTime DATE_VIG_INI { get; set; }
        public DateTime DATE_VIG_FIN { get; set; }
        public string PASSPORT_NAME { get; set; }
        public string PASSPORT_LASTNAME { get; set; }
        [EmailAddress]
        public string EMAIL { get; set; }
        public string? ACTUAL_JOB { get; set; }
        public bool? EXPELLED_MEX { get; set; }
        public string? EXPELLED_MEX_DESC { get; set; }
        public bool? CRIMINAL_RECORD_MEX { get; set; }
        public string? EVENT_JOB { get; set; }
        public DateTime? DATE_EVENT_INI { get; set; }
        public DateTime? DATE_EVENT_FIN { get; set; }
        public string? FLIGHT { get; set; }
        public string? FLIGHT_NUMBER { get; set; }
        public string SECRET_CODE { get; set; }
        public string? ACTIVITY_COUNTRY { get; set; }
        public string? ACTIVITY_MEXICO { get; set; }
        public DateTime? DATE_ARRIVE { get; set; } 
        public DateTime? DATE_LEAVE { get; set; }
        public string? LANGUAGE { get; set; }
        public bool? CHECK_VERIFY { get; set; }
        public bool ACTIVE { get; set; }
        [Range(typeof(DateTime), "1/1/1900", "6/6/2079")]
        public DateTime CREATED_DATE { get; set; } = DateTime.Now;
        public int CREATED_BY { get; set; }
        [Range(typeof(DateTime), "1/1/1900", "6/6/2079")]
        public DateTime? MODIFY_DATE { get; set; } = new DateTime(1900, 01, 01);
        public int? MODIFY_BY { get; set; }


    }
}
