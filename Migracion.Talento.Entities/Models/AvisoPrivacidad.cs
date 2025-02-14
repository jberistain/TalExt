using Microsoft.EntityFrameworkCore;

namespace Migracion.Talento.Models
{
    [PrimaryKey(nameof(ID))]
    public class AvisoPrivacidad
    {
        public int ID { get; set; }
        public string? AVISO_ESP { get; set; }
        public string? AVISO_ENG { get; set; }
    }
}
