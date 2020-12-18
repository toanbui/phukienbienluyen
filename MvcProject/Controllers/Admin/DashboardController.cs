using MvcProject.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace MvcProject.Controllers.Admin
{
    [AdminAuthorize(Roles = "Admin , Manager , DashBoard")]
    public class DashboardController : AdminController
    {
        // GET: Dashboard
        public ActionResult Index()
        {
            //DirectoryInfo d = new DirectoryInfo(@"E:\Working\MvcProject\MvcProject\Scripts\Admin");//Assuming Test is your Folder
            //FileInfo[] Files = d.GetFiles("*.js"); //Getting Text files
            //string str = "";
            //StringBuilder sb = new StringBuilder();
            //foreach (FileInfo file in Files)
            //{
            //    sb.AppendLine("<script src=\"~/Scripts/Admin/{1}\"></script>".Replace("{1}",file.Name));
            //}
            //var a = sb.ToString();


            //DirectoryInfo d2 = new DirectoryInfo(@"E:\Working\MvcProject\MvcProject\Content\Admin");//Assuming Test is your Folder
            //FileInfo[] Files2 = d2.GetFiles("*.css"); //Getting Text files
            //StringBuilder sb2 = new StringBuilder();
            //foreach (FileInfo file in Files2)
            //{
            //    sb2.AppendLine("<link rel=\"stylesheet\" type=\"text/css\" href=\"~/Content/Admin/{1}\">".Replace("{1}", file.Name));
            //}
            //var b = sb2.ToString();

            return View();
        }
    }
}