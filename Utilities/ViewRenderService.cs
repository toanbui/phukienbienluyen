using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Utilities
{
    public static class ViewRenderService
    {
        public static string RenderView(Controller controller, string viewName, object model , bool partial = false)
        {
            controller.ViewData.Model = model;
            var viewResult = new ViewEngineResult(new List<string>());
            if (partial)
                viewResult = ViewEngines.Engines.FindPartialView(controller.ControllerContext, viewName);
            else
                viewResult = ViewEngines.Engines.FindView(controller.ControllerContext, viewName, "_Layout");

            if (viewResult.View != null)
            {
                using (var sw = new StringWriter())
                {
                    var viewContext = new ViewContext(controller.ControllerContext, viewResult.View, controller.ViewData, controller.TempData, sw);
                    viewResult.View.Render(viewContext, sw);
                    viewResult.ViewEngine.ReleaseView(controller.ControllerContext, viewResult.View);
                    return sw.GetStringBuilder().ToString();
                }
            }
            else
                return "";
        }
    }
}
