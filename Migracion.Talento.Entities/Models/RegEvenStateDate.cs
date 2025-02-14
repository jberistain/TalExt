using Microsoft.EntityFrameworkCore;
using Migracion.Talento.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Migracion.Talento.Entities.Models
{
    [PrimaryKey(nameof(ID_REG_EVEN_DATE))]
    public  class RegEvenStateDate
    {
        public int ID_REG_EVEN_DATE { get; set; }

        public int ID_EVENT { get; set; }

        [ForeignKey(nameof(ID_EVENT))]
        public virtual Events CAT_EVENTS { get; set; }
        public int ID_ESTATE { get; set; }
        [ForeignKey(nameof(ID_ESTATE))]
        public virtual Estates CAT_ESTATES { get; set; }


        public int ID_REG { get; set; }
        [ForeignKey(nameof(ID_REG))]

        public string? DESC_LOCATION { get; set; }

        [Range(typeof(DateTime), "1/1/1900", "6/6/2079")]
        public DateTime EVENT_DATE { get; set; }

        [Range(typeof(DateTime), "1/1/1900", "6/6/2079")]
        public DateTime EVENT_DATE_FIN { get; set; }
        public bool ACTIVE { get; set; }

        [Range(typeof(DateTime), "1/1/1900", "6/6/2079")]
        public DateTime? CREATED_DATE{ get; set; }
        public int CREATED_BY{ get; set; }
        [ForeignKey(nameof(CREATED_BY))]

        [Range(typeof(DateTime), "1/1/1900", "6/6/2079")]
        public DateTime? MODIFY_DATE { get; set; }
        public int? MODIFY_BY { get; set; }


    }
}
