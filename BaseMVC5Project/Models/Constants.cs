using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigracionTalentoExtranjero.Models
{
    public class Constants
    {
        public static string WebAPIUrl = GetAPIURLS();


        internal static string GetAPIURLS()
        {
            string result = null;

            string key = "HostWebAPI";
            var appSettings = ConfigurationManager.AppSettings;
            result = appSettings[key] ?? "Not Found";
            // result = System.Configuration.ConfigurationManager.AppSettings["HostWebAPI"].ToString();
            

            return result;
        }

    }
}
