using MvcProject.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MvcProject
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.RouteExistingFiles = true;

            //action for backend
            routes.MapRoute(
                 "Admin", // Route name
                 "admin/{controller}/{action}/{id}", // URL with parameters
                 new { controller = "Dashboard", action = "Index", id = UrlParameter.Optional }, // Parameter defaults
                 namespaces : new string[] { "MvcProject.Controllers.Admin" }
            );

            //action for front end
            routes.MapRoute(
                  "FrontEnd", // Route name
                  "front/{controller}/{action}/{id}", // URL with parameters
                  new { controller = "Home", action = "Index", id = UrlParameter.Optional }, // Parameter defaults
                  new string[] { "MvcProject.Controllers" }
             );

            //rewrite module
            routes.MapRoute("IUrlRouteHandler", "{*urlRouteHandler}", new string[] { "MvcProject.Controllers" }).RouteHandler = new UrlRouteHandler();

        }

    }
}
