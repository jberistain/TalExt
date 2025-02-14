using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Migracion.Talento.Entities.Models
{
    [PrimaryKey(nameof(ID_INVITE))]
    public class RegInvite
    {
        public int ID_INVITE { get; set; }
        public int? ID_REG { get; set; }
        public int? ID_EVENT_TYPE { get; set; }
        public string? DES_TITLE { get; set; }
        public string? DESC_SPANISH { get; set; }
        public string? DESC_ENGLISH { get; set; }
        public string? SIGN_1 { get; set; }
        public string? SIGN_2 { get; set; }
        public string? SIGN_3 { get; set; }
        public string? SIGN_4 { get; set; }
        public string? FOOT_PAGE { get; set; }
        public string? FILE_NAME { get; set; }
        public string? INVITE_XML { get; set; }
        public string? SIGN_BLOB { get; set; }
        public bool ACTIVE { get; set; }
        public DateTime CREATED_DATE { get; set; }
        public int CREATED_BY { get; set; }
        public DateTime? MODIFY_DATE { get; set; }
        public int? MODIFY_BY { get; set; }
    }
}
