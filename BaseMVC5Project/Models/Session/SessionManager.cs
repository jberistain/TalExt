using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;
using System.Web;
using System.Net;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.Ocsp;
using Org.BouncyCastle.Utilities;

namespace MigracionTalentoExtranjero.Models.Session
{
    public class SessionManager
    {
        public static bool ExistUserInSession()
        {
            return HttpContext.Current.User.Identity.IsAuthenticated;
        }
        public static void DestroyUserSession()
        {
            FormsAuthentication.SignOut();
        }
        public static int GetUser()
        {
            int user_id = 0;
            if (HttpContext.Current.User != null && HttpContext.Current.User.Identity is FormsIdentity)
            {
                FormsAuthenticationTicket ticket = ((FormsIdentity)HttpContext.Current.User.Identity).Ticket;
                if (ticket != null)
                {
                    user_id = Convert.ToInt32(ticket.UserData);
                }
            }
            return user_id;
        }
        public static void AddUserToSession(string id)
        {
            bool persist = true;
            var cookie = FormsAuthentication.GetAuthCookie("usuario", persist);

            cookie.Name = FormsAuthentication.FormsCookieName;
            cookie.Expires = DateTime.Now.AddDays(1);

            var ticket = FormsAuthentication.Decrypt(cookie.Value);
            var newTicket = new FormsAuthenticationTicket(ticket.Version, 
                ticket.Name, 
                ticket.IssueDate, 
                cookie.Expires, 
                ticket.IsPersistent, id);

            cookie.Value = FormsAuthentication.Encrypt(newTicket);
            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        public static void AddMenuToSession(string menuSerialized)
        {
            HttpCookie cookie = new HttpCookie("Menu");

            cookie.Value = HttpUtility.UrlEncode( menuSerialized);
            cookie.Expires = DateTime.Now.AddDays(1);
            HttpContext.Current.Response.Cookies.Add(cookie);
        }
        public static string GetMenus()
        {
            string menus = "";
            // Verifica si la cookie existe
            if (HttpContext.Current.Request.Cookies["Menu"] != null)
            {
                string listaSerializada = HttpUtility.UrlDecode(HttpContext.Current.Request.Cookies["Menu"].Value);

                if (!string.IsNullOrEmpty(listaSerializada))
                {
                    menus = listaSerializada;
                }
            }
            return menus;
        }
    }
}
