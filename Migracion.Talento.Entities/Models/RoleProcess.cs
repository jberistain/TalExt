
using Microsoft.EntityFrameworkCore;

namespace Migracion.Talento.Models
{
    [PrimaryKey(nameof(ID_ROL_PROCCESS))]

    public class RoleProcess
    {
        public int ID_ROL_PROCCESS { get; set; }
        public int ID_ROLE { get; set; }
        public int ID_PROCESS { get; set; }
        public bool ACTIVE { get; set; }
        public DateTime CREATED_DATE { get; set; }
        public int CREATED_BY { get; set; }
        public DateTime? MODIFY_DATE { get; set; }
        public int? MODIFY_BY { get; set; }
    }
}
