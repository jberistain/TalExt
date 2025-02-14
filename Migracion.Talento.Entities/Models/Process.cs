using Microsoft.EntityFrameworkCore;

namespace Migracion.Talento.Models
{
    [PrimaryKey(nameof(ID_PROCESS))]

    public class Process
    {
		public int ID_PROCESS { get; set; }
        public string? DESC_PROCESS_SP { get; set; }
        public string? DESC_PROCESS_EN { get; set; }
        public string? URL_PROCESS { get; set; }
        public bool ACTIVE { get; set; }
        public DateTime? CREATED_DATE { get; set; }
        public int CREATED_BY { get; set; }
        public DateTime? MODIFY_DATE { get; set; }
        public int? MODIFY_BY { get; set; }
    }
}
