using System.Web.Mvc;
using System.Web.Routing;

namespace 侍
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //routes.MapRoute(
            //    name: "Default",
            //    url: "{controller}/{action}/{id}",
            //    defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            //);

            routes.MapRoute(name: "Habr Proxy" ,url: "{*proxyUrl}",
                defaults: new { controller = "Proxy", action = "Index", id = UrlParameter.Optional });
        }
    }
}
