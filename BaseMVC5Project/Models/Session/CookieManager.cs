using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MigracionTalentoExtranjero.Models.Session
{
    public class CookieManager
    {
        public CookieManager()
        {

        }

        public void SetCookieValue(string cookieName, string cookieValue)
        {
            var response = HttpContext.Current.Response;

            HttpCookie mycookie = new HttpCookie(cookieName);
            mycookie.Value = cookieValue;
            mycookie.Expires = DateTime.Now.AddDays(1);

            //Si existe la galleta, se remueve antes de asignarla nuevamente
            HttpCookie myOldCookie = HttpContext.Current.Request.Cookies.Get(cookieName);
            if (myOldCookie != null)
            {
                response.Cookies.Remove(cookieName);
            }
            response.Cookies.Add(mycookie);
        }

        public string GetCookieValue(string cookieName)
        {
            string result = "";
            //Si existe la galleta se retorna el valor
            HttpCookie myOldCookie = HttpContext.Current.Request.Cookies.Get(cookieName);
            if (myOldCookie != null)
            {
                result = myOldCookie.Value;
            }

            return result;
        }

    }
}
