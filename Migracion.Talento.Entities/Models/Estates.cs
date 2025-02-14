using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Migracion.Talento.Entities.Models
{
    [PrimaryKey(nameof(ID_ESTATE))]
    public  class Estates
    {
        public int ID_ESTATE { get; set; }
        public string? IM_ESTATE { get; set; }
        public string? DESC_ESTATE_SP { get; set; }
        public string? DESC_ESTATE_EN { get; set; }
        public string? ARB_ESTATE { get; set; }
        public int CAPACITY_ESTATE { get; set; }
        public string? TYPE_ESTATE { get; set; }
        public bool ACTIVE { get; set; }
        public DateTime? CREATED_DATE { get; set; }
        public int CREATED_BY { get; set; }
        public DateTime? MODIFY_DATE { get; set; }
        public int? MODIFY_BY { get; set; }
    }
}
