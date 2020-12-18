using MvcProject.App_Start;
using System;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace MvcProject
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            //config for web api
            GlobalConfiguration.Configure(WebApiConfig.Register);

            //disable "X-AspNetMvc-Version" header name
            MvcHandler.DisableMvcResponseHeader = true;

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
        protected void Application_AuthenticateRequest(Object sender, EventArgs e)
        {
            //var a = Context.User;
            //HttpCookie authCookie = Context.Request.Cookies[FormsAuthentication.FormsCookieName];
            //if (authCookie == null || authCookie.Value == "")
            //    return;

            //FormsAuthenticationTicket authTicket;
            //try
            //{
            //    authTicket = FormsAuthentication.Decrypt(authCookie.Value);
            //}
            //catch
            //{
            //    return;
            //}
            //// retrieve roles from UserData
            //string[] roles = authTicket.UserData.Split(';');

            //if (Context.User != null)
            //    Context.User = new GenericPrincipal(Context.User.Identity, roles);
        }
        //protected void Application_PreSendRequestHeaders()
        //{
        //    Response.Headers.Remove("X-Frame-Options");
        //    Response.AddHeader("X-Frame-Options", "AllowAll");

        //}
    }
}
