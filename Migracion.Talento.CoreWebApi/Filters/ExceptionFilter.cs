using Microsoft.AspNetCore.Mvc.Filters;

namespace Migracion.Talento.CoreWebApi.Filters
{
    public class ExceptionFilter:ExceptionFilterAttribute
    {
        public readonly ILogger<ExceptionFilter> Exlogger;

        public ExceptionFilter(ILogger<ExceptionFilter> exlogger)
        {
            Exlogger = exlogger;
        }

        public override void OnException(ExceptionContext context)
        {
            Exlogger.LogError(context.Exception, context.Exception.Message);
            base.OnException(context); 
        }

    }
}
