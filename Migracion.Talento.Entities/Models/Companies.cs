

using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Migracion.Talento.Models
{
    [PrimaryKey(nameof(ID_COMPANY))]
    public class Companies
	{
		public int ID_COMPANY { get; set; }
        public string? DESC_COMPANY_SP { get; set; }
        public string? DESC_COMPANY_EN { get; set; }
        public string? LEGAL_REPRESENTATIVE { get; set; }
        public bool ACTIVE { get; set; }
        public DateTime? CREATED_DATE { get; set; }
        public int CREATED_BY { get; set; }
        public DateTime? MODIFY_DATE { get; set; }
        public int? MODIFY_BY { get; set; }
    }
}
