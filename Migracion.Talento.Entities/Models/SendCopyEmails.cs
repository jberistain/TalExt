using Microsoft.EntityFrameworkCore;

namespace Migracion.Talento.Models
{
    [PrimaryKey(nameof(ID))]
    public class SendCopyEmails
    {
        public int ID { get; set; }
        public string EMAIL { get; set; }
        public int ACTIVE { get; set; }
        public DateTime CREATED_DATE { get; set; }
        public int CREATED_BY { get; set; }
        public DateTime? MODIFIED_DATE { get; set; }
        public int? MODIFIED_BY { get; set; }
    }
}
