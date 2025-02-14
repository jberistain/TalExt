using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonTools.Csv
{
    public class CsvManager
    {
        public CsvManager() { }


        public string GeneraCVSBusquedaActual(List<IRegistro> registrosEncontrados)
        {

            var sb = new StringBuilder();

            string line = "";
            int i = 0;
            int final = registrosEncontrados.Count();
            foreach (IRegistro rep in registrosEncontrados)
            {
                var properties = typeof(IRegistro).GetProperties();
                foreach (var property in properties)
                {
                    string nameproperty = property.Name;
                    string typeProperty = property.PropertyType.ToString();
                    //El SP siempre retorna != null los valores que se usara, entonces si es nulo es qu eno se debe usar ese valor en el reporte
                    var valueColumn = property.GetValue(rep);
                    //Si el valor no es nulo, se agrega al reporte
                    if (valueColumn != null)
                    {
                        string value = property.GetValue(rep).ToString();
                        if (string.IsNullOrEmpty(value))
                            value = "";

                        if (i == 0)
                            line += $"\"{value.Replace("\"", "\"\"")}\"";
                        else
                            line += "," + $"\"{value.Replace("\"", "\"\"")}\"";
                    }
                    i++;
                }

                sb.AppendLine(line);
                line = "";
                i = 0;
            }
            
           return sb.ToString();
        }
    }
}
