using Microsoft.EntityFrameworkCore;
using Migracion.Talento.Entities.Models;
using Migracion.Talento.Models;
using Org.BouncyCastle.Asn1.Esf;

namespace Migracion.Talento.WebAPI.DataConnection
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<User>  CAT_USERS { get; set; }
        public DbSet<Companies> CAT_COMPANIES { get; set; }
        public DbSet<AirPorts> CAT_AIRPORTS{ get; set; }
        public DbSet<Activities>CAT_ACTIVITIES { get; set; }
        public DbSet<AirLines> CAT_AIR_LINES { get; set; }
        public DbSet<Countries> CAT_COUNTRIES { get; set; }
        public DbSet<Events> CAT_EVENTS { get; set; }
        public DbSet<Estates> CAT_ESTATES { get; set; }
        public DbSet<Genders> CAT_GENDERS { get; set; }
        public DbSet<Nationalities>CAT_NATIONALITIES { get; set; }  
        public DbSet<Process> CAT_PROCESS { get; set; }
        public DbSet<RegEvents>REG_EVENTS{ get; set; }
        public DbSet<RoleByCompany> CAT_ROLE_BY_COMPANIES { get; set; } 
        public DbSet<Status> CAT_STATUS { get; set; } 
        public DbSet<RegEvenStateDate>REG_EVENT_ESTATES_DATE { get; set; }
        public DbSet<RegInvite> REG_INVITE { get; set; }
        public DbSet<EventTypes> CAT_EVENT_TYPES { get; set; }
        public DbSet<Documents> REG_DOCUMENTS { get; set; } 
        public DbSet<AvisoPrivacidad> CAT_AVISO_PRIVACIDAD { get; set; } 
        public DbSet<RoleProcess> ROL_PROCESS { get; set; }

    }
}
