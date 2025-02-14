
using Microsoft.EntityFrameworkCore;

namespace Migracion.Talento.Models
{
    [PrimaryKey(nameof(ID_ROLE))]

    public class RoleByCompany
    {
        public int ID_ROLE { get; set; }
        public int ID_COMPANY { get; set; }
        public string DESC_ROLE_SP { get; set; }
        public string DESC_ROLE_EN { get; set; }
        public bool ACTIVE { get; set; }
        public DateTime CREATED_DATE { get; set; }
        public int? CREATED_BY { get; set; }
        public DateTime? MODIFY_DATE { get; set; }
        public int? MODIFY_BY { get; set; }
    }
}
