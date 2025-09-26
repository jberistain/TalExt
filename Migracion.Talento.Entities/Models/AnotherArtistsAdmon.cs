using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Migracion.Talento.Models
{
    [PrimaryKey(nameof(ID))]

    public class AnotherArtistsAdmon
    {
        public int ID { get; set; }
        public int ID_REG { get; set; }
        public string PASSPORT_NAME { get; set; }
        public bool ACTIVE { get; set; }
        [Range(typeof(DateTime), "1/1/1900", "6/6/2079")]
        public DateTime CREATED_DATE { get; set; } = DateTime.Now;
        public int CREATED_BY { get; set; }
        [Range(typeof(DateTime), "1/1/1900", "6/6/2079")]
        public DateTime? MODIFY_DATE { get; set; } = new DateTime(1900, 01, 01);
        public int? MODIFY_BY { get; set; }


    }
}
