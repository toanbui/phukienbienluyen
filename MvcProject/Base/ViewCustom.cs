using BO;
using CacheHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Utilities;

namespace MvcProject.Base
{
    public class ViewCustom : ActionResult
    {
        public Controller _controller { get; set; }
        public bool _partial { get; set; }
        public object _model { get; set; }
        public string _action { get; set; }
        public string _controllerName { get; set; }
        public bool _allowComment { get; set; }
        public bool _allowCache { get; set; }
        public bool _adminView { get; set; }
        private string comment = "";
        public ViewCustom(Controller controller, object model = null,string action = "",string controllerName = "", bool partial = false , bool allowComment = true , bool allowCache = true, bool adminView = false)
        {
            this._controller = controller;
            this._model = model;
            this._partial = partial;
            this._action = action;
            this._controllerName = controllerName;
            this._allowComment = allowComment;
            this._allowCache = allowCache;
            this._adminView = adminView;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            var viewPath = "";

            if (!string.IsNullOrEmpty(_action) && !string.IsNullOrEmpty(_controllerName))
                viewPath = viewPath.BuildViewPath(_controllerName,_action,_adminView);
            else if(!string.IsNullOrEmpty(_action))
                viewPath = context.BuildViewPath(_action, _adminView);
            else
                viewPath = context.BuildViewPath(_adminView);

            string result = Utilities.ViewRenderService.RenderView(_controller, viewPath, _model, _partial);
            var url = context.HttpContext.Request.RawUrl;
            if (!string.IsNullOrEmpty(result) && this._allowCache)
            {
                comment = _allowComment
                ? string.Format("<!--{0}: {1}-->", "time", DateTime.Now)
                : string.Empty;
                CacheController.SaveToCacheIIS(url, result + comment, 60 * 24 * 7);
            }
            HttpResponseBase response = context.HttpContext.Response;
            response.ContentType = "text/html; charset=utf-8";
            response.Write(result);
        }
    }
}