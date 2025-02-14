
using Microsoft.EntityFrameworkCore;

namespace Migracion.Talento.Models
{
    [PrimaryKey(nameof(ID_GENDER))]

    public class Genders
    {
		public int ID_GENDER { get; set; }
		public string? DESC_GENDER_SP { get; set; }
        public string? DESC_GENDER_EN { get; set; }
        public bool ACTIVE { get; set; }
        public DateTime? CREATED_DATE { get; set; }
        public int CREATED_BY { get; set; }
        public DateTime? MODIFY_DATE { get; set; }
        public int? MODIFY_BY { get; set; }
    }
}
