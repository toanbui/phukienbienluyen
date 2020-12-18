using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Utilities;

namespace MvcProject.Base
{
    public class FrontController : ViewBase
    {
        public FrontController() : base(false)
        {

        }
        protected new ActionResult View()
        {
            return new ViewCustom(this);
        }
        protected new ActionResult View(object model)
        {
            return new ViewCustom(this , model : model);
        }
        protected new ActionResult View(bool allowCache = true)
        {
            return new ViewCustom(this , allowCache : allowCache);
        }
        protected new ActionResult View(object model, bool allowCache = true)
        {
            return new ViewCustom(this, model: model , allowCache : allowCache);
        }
        protected new ActionResult PartialView()
        {
            return new ViewCustom(this, partial : true);
        }
        protected new ActionResult PartialView(object model)
        {
            return new ViewCustom(this, partial : true , model : model);
        }
        protected new ActionResult PartialView(bool allowCache = true)
        {
            return new ViewCustom(this, partial: true, allowCache: allowCache);
        }
        protected new ActionResult PartialView(object model , bool allowCache = true)
        {
            return new ViewCustom(this, partial : true , model : model , allowCache : allowCache);
        }

    }
}