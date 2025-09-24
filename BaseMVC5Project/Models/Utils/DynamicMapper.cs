using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigracionTalentoExtranjero.Models.Utils
{
    public static class DynamicMapper
    {
        public static T ConvertDynamicTo<T>(dynamic source)
        {
            try
            {
                // Serializar a JSON string
                string json = JsonConvert.SerializeObject(source);

                // Deserializar al tipo de destino
                T result = JsonConvert.DeserializeObject<T>(json);

                return result;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("No se pudo convertir el dynamic al tipo especificado.", ex);
            }
        }
    }
}
