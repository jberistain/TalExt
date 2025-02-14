using Microsoft.EntityFrameworkCore;

namespace Migracion.Talento.Models
{
    [PrimaryKey(nameof(ID_AIRPORT))]
    public class AirPorts
    {
        public int ID_AIRPORT { get; set; }
        public string? DESC_AIRPORT_SP { get; set; }
        public string? DESC_AIRPORT_EN { get; set; }
        public string IATA { get; set; }
        public string LOCATION { get; set; }
        public DateTime? CREATED_DATE { get; set; }
        public int CREATED_BY { get; set; }
        public DateTime? MODIFY_DATE { get; set; }
        public int? MODIFY_BY { get; set; }
    }
}
