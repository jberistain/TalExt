

using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Migracion.Talento.Models
{
    [PrimaryKey(nameof(ID_EVENT_TYPE))]
    public class EventTypes
	{
		public int ID_EVENT_TYPE { get; set; }
        public string DESC_ACTIVITY_SP { get; set; }
        public string DESC_ACTIVITY_EN { get; set; }
        public bool ACTIVE { get; set; }
        public DateTime CREATED_DATE { get; set; }
        public int CREATED_BY { get; set; }
        public DateTime? MODIFY_DATE { get; set; }
        public int? MODIFY_BY { get; set; }
    }
}
