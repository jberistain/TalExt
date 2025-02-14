
using Microsoft.EntityFrameworkCore;

namespace Migracion.Talento.Models
{
    [PrimaryKey(nameof(ID_NATIONALITY))]

    public class Nationalities
    {
		public int ID_NATIONALITY { get; set; }
		public string? DESC_NACIONALITY_SP { get; set; }
		public string? DESC_NACIONALITY_EN { get; set; }
		public bool ACTIVE { get; set; }
        public bool RESTRICTION { get; set; }
        public DateTime? CREATED_DATE { get; set; }
		public int CREATED_BY { get; set; }
		public DateTime? MODIFY_DATE { get; set; }
		public int? MODIFY_BY { get; set; }
    }
}
