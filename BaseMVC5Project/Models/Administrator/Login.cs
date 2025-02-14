using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigracionTalentoExtranjero.Models.Administrator
{
    public class Login
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string ConfirmNewPassword { get; set; }
        public string NewPassword { get; set; }
        public string Email { get; set; }
        public string Mensaje { get; set; }=string.Empty;

    }
}
