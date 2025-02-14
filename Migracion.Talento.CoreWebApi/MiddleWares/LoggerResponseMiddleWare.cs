using Microsoft.Extensions.Logging;
using static System.Net.Mime.MediaTypeNames;

namespace Migracion.Talento.CoreWebApi.MiddleWares
{
    public static class LoggerResponseMiddleWareExtensions
    {

        public static IApplicationBuilder UseLoggerResponse(this IApplicationBuilder app)
        {
            return app.UseMiddleware<LoggerResponseMiddleWare>();
        }
    }

    public class LoggerResponseMiddleWare
    {
        private readonly RequestDelegate Req;
        private readonly ILogger<LoggerResponseMiddleWare> logger;

       

        public LoggerResponseMiddleWare(RequestDelegate req,ILogger<LoggerResponseMiddleWare> logger) {
        
            Req = req;
            this.logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {

            using (var ms = new MemoryStream())
            {
                var cuerpoRespuest = context.Response.Body;
                context.Response.Body = ms;
                await Req(context);
                ms.Seek(0, SeekOrigin.Begin);
                string response = new StreamReader(ms).ReadToEnd();
                ms.Seek(0, SeekOrigin.Begin);
                await ms.CopyToAsync(cuerpoRespuest);
                context.Response.Body = cuerpoRespuest;
                logger.LogInformation(response);
            }
        }


    }
}
