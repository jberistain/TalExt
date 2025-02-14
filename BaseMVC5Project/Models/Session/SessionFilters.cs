using MigracionTalentoExtranjero.Models.Catalogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;

namespace MigracionTalentoExtranjero.Models.Session
{
    // Si no estamos logeado, regresamos al login
    public class AutenticadoAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            if (!SessionManager.ExistUserInSession())
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                {
                    controller = "Administrator",
                    action = "Login"
                }));
            }
        }
    }

    // Si estamos logeado ya no podemos acceder a la página de Login
    public class NoLoginAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            if (SessionManager.ExistUserInSession())
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                {
                    controller = "Administrator",
                    action = "Index"
                }));
            }
        }
    }

    // Si se logea el usuario especificado para firmas se redirecciona siempre a esa pag
    public class PermisoPerfilAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            if (!SessionManager.ExistUserInSession())
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                {
                    controller = "Administrator",
                    action = "Login"
                }));
            }
            else
            {
                string actionName = filterContext.ActionDescriptor.ActionName;
                string controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
                var ListaModulos = ConsultaModulosPerfil.ObtenerModulosDeSesion();
                bool esUsuarioParaFirmas = false;

                foreach(var moduloActual in ListaModulos)
                {
                    if (!string.IsNullOrEmpty(moduloActual.DESC_PROCESS_SP) && moduloActual.DESC_PROCESS_SP.ToUpper().Equals("FIRMAS"))
                    {
                        esUsuarioParaFirmas = true;
                        break;
                    }
                }


                if (esUsuarioParaFirmas)
                {
                    if (!actionName.Equals("FirmasDocumentos") || !controllerName.Equals("Catalogs"))
                    {
                        filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                        {
                            controller = "Catalogs",
                            action = "FirmasDocumentos"
                        }));
                    }
                }
                else
                {
                    if (actionName.Equals("FirmasDocumentos") && controllerName.Equals("Catalogs"))
                    {
                        filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                        {
                            controller = "Administrator",
                            action = "Index"
                        }));
                    }
                }
            }
        }
    }
}
