using Microsoft.EntityFrameworkCore;

namespace Migracion.Talento.Models
{

    [PrimaryKey(nameof(ID_AIR_LINE))]
    public class AirLines
    {
		public int ID_AIR_LINE { get; set; }
        public string? DESC_AIR_LINE_SP { get; set; }
        public string? DESC_AIR_LINE_EN { get; set; }
        public bool ACTIVE { get; set; }
        public DateTime? CREATED_DATE { get; set; }
        public int CREATED_BY { get; set; }
        public DateTime? MODIFY_DATE { get; set; }
        public int? MODIFY_BY { get; set; }
    }
}
