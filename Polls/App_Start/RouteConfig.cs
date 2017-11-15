using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Polls
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapMvcAttributeRoutes();
            // routes.MapRoute(
            //    name: "Public",
            //    url: "{action}",
            //    defaults: new { controller = "Public", action = "Index", id = UrlParameter.Optional }
            //);
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Public", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
