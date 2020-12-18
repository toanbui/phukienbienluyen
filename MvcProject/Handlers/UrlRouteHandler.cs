using BO;
using BO.ReadConfigs;
using CacheHelper;
using MvcProject.Controllers;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MvcProject.Handlers
{
    public sealed class UrlRouteHandler : IRouteHandler
    {
        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            var routeData = requestContext.RouteData.Values;
            var context = requestContext.HttpContext;
            var url = requestContext.HttpContext.Request.RawUrl;

            //cache 
            var cacheContent = CacheController.GetFromCacheIIS(url);
            if (!string.IsNullOrEmpty(cacheContent))
            {
                context.Response.Headers.Add("HasCache", "true");
                context.Response.Write(cacheContent);
                context.Response.End();
            }

            var route = UrlHandler.GetRoute(url);

            if (route != null)
            {
                routeData["url"] = route.Url;
                routeData["controller"] = route.Controller;
                routeData["action"] = route.Action;
                routeData["urlRouteHandler"] = route;
                if (route.Querys != null && route.Querys.Any())
                {
                    foreach (var item in route.Querys)
                    {
                        routeData[item.Name] = item.Value;
                    }
                }
            }
            return new MvcHandler(requestContext);
        }
    }
}