using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Utilities;

namespace MvcProject.Base
{
    public class ViewBase : Controller
    {
        public bool _adminView { get; set; }
        private string _layout { get; set; }
        public ViewBase(bool adminView  = false)
        {
            this._adminView = adminView;
            this._layout = _adminView ? "~/Views/Shared/_AdminLayout.cshtml" : "~/Views/Shared/_Layout.cshtml";
        }
        protected new internal ViewResult View()
        {
            return base.View(ControllerContext.BuildViewPath(_adminView), _layout,(object)null);
        }
        protected new internal ViewResult View(object model)
        {
            return base.View(ControllerContext.BuildViewPath(_adminView), _layout, model);
        }
        protected new internal ViewResult View(object model , string layout)
        {
            return base.View(ControllerContext.BuildViewPath(_adminView), layout, model);
        }
        protected new internal ViewResult View(string viewName)
        {
            if (!string.IsNullOrEmpty(viewName) && !viewName.Contains(".cshtml"))
                viewName = ControllerContext.BuildViewPath(viewName, _adminView);

            return base.View(viewName, _layout,(object)null);
        }
        protected new internal ViewResult View(string viewName , string masterView)
        {
            if (!string.IsNullOrEmpty(viewName) && !viewName.Contains(".cshtml"))
                viewName = ControllerContext.BuildViewPath(viewName, _adminView);
            else
                viewName = ControllerContext.BuildViewPath(_adminView);

            return base.View(viewName, masterView, (object)null);
        }
        protected new internal ViewResult View(string viewName, object model)
        {
            if (!string.IsNullOrEmpty(viewName) && !viewName.Contains(".cshtml"))
                viewName = ControllerContext.BuildViewPath(viewName, _adminView);

            return base.View(viewName, _layout, model);
        }
        protected new internal PartialViewResult PartialView()
        {
            return base.PartialView(ControllerContext.BuildViewPath(_adminView), (object)null);
        }
        protected new internal PartialViewResult PartialView(object model)
        {
            return base.PartialView(ControllerContext.BuildViewPath(_adminView), model);
        }
    }
}