
using Microsoft.EntityFrameworkCore;

namespace Migracion.Talento.Models
{
    [PrimaryKey(nameof(ID_EVENT))]
    public class Events
    {
        public int ID_EVENT { get; set; }
        public int? ID_COMPANY { get; set; }
        public int? ID_EVENT_TYPE { get; set; }
        public int? ID_ESTATE { get; set; }
        public string? DESC_LOCATION { get; set; }
        public string DESC_EVENT_SP { get; set; }
        public string DESC_EVENT_EN { get; set; }
        public DateTime? DATE_INI { get; set; }
        public DateTime? DATE_FIN { get; set; }
        public bool ACTIVE { get; set; }
        public DateTime? CREATED_DATE { get; set; }
        public int CREATED_BY { get; set; }
        public DateTime? MODIFY_DATE { get; set; }
        public int? MODIFY_BY { get; set; }
    }
}
