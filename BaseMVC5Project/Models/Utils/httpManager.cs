using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using RestSharp;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace MigracionTalentoExtranjero.Models.Utils
{
    public  class HttpManager
    {

        private readonly HttpClient httpRestClient;
        public HttpManager(string host) 
        {
            if (httpRestClient == null)
            {
                httpRestClient = new HttpClient();
                httpRestClient.BaseAddress = new Uri(host);
            }
        }

        /// <summary>
        /// Se Cre metodo para consumir servicios con POST
        /// </summary>
        /// <typeparam name="TOne">Es el objeto que se enviara en el Request</typeparam>
        /// <typeparam name="TTwo">Es el formato de Objeto que se recibira como Response de la peticion</typeparam>
        /// <param name="item"></param>
        /// <param name="endPoint"></param>
        /// <returns></returns>
        public async Task<TTwo> PostAsJsonAsync<TOne, TTwo>(TOne item, string endPoint) where TOne:class where TTwo : class
        {
            string serializeObject = SimpleJson.SerializeObject(item);

            StringContent stringContent = new StringContent(serializeObject, System.Text.Encoding.UTF8, "application/json");
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage
            {
                Content = stringContent
            };
            httpRequestMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            httpRequestMessage.Method = HttpMethod.Post;
            httpRequestMessage.RequestUri = new Uri($"{this.httpRestClient.BaseAddress}{endPoint}");

            TTwo result = null;
            using (var response = await this.httpRestClient.SendAsync(httpRequestMessage))
            {
                string responseString = await response.Content.ReadAsStringAsync();
                result = Newtonsoft.Json.JsonConvert.DeserializeObject<TTwo>(responseString);
            }
            return result;

        }
        public async Task<T> GetAsJsonAsync<T>(string endPoint, Dictionary<string,string> requestParamsList = null) where T : class
        {
            string paramsToAdd = string.Empty;
            if(requestParamsList != null)
            {
                bool firstParameter = true;
                foreach(var parameter in requestParamsList)
                {
                    if (firstParameter)
                    {
                        paramsToAdd = $"?{parameter.Key}={parameter.Value}";
                        firstParameter= false ;
                    }
                    else
                        paramsToAdd = $"&{parameter.Key}={parameter.Value}";
                }
            }
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage
            {
            };
            // httpRequestMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            httpRequestMessage.Method = HttpMethod.Get;
            httpRequestMessage.RequestUri = new Uri($"{httpRestClient.BaseAddress}{endPoint}{paramsToAdd}");
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(ValidateServerCertificate);
            T result = null;
            using (var response = await httpRestClient.SendAsync(httpRequestMessage))
            {
                string responseString = await response.Content.ReadAsStringAsync();
                result = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(responseString);
            }
            return result;

        }

        public static bool ValidateServerCertificate(Object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }
    }
    
}
