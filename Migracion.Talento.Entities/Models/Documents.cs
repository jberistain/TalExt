using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Migracion.Talento.Entities.Models
{
    [PrimaryKey(nameof(ID_INVITE))]
    public class Documents
    {
      public int ID_INVITE { get; set; }
      public int? ID_REG { get; set; }
      public int? ID_EVENT_TYPE { get; set; }
      public string? DESC_SPANISH { get; set; }
      public string? DESC_ENGLISH { get; set; }
      public string? FILE_BLOB { get; set; }
      public DateTime? CREATED_DATE { get; set; }
      public int CREATED_BY { get; set; }
      public DateTime? MODIFY_DATE { get; set; }
      public int? MODIFY_BY { get; set; }
    }
}
