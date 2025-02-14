
using Microsoft.EntityFrameworkCore;

namespace Migracion.Talento.Models
{
    [PrimaryKey(nameof(ID_STATUS))]

    public class Status
    {
        public int ID_STATUS{ get; set; }
        public string? DESC_STATUS_SP { get; set; }
        public string? DESC_STATUS_EN { get; set; }
        public bool ACTIVE { get; set; }
        public DateTime? CREATED_DATE { get; set; }
        public int CREATED_BY { get; set; }
        public DateTime? MODIFY_DATE { get; set; }
        public int? MODIFY_BY { get; set; }
    }
}
