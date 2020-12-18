using MvcProject.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcProject.Controllers.Admin
{
    public class CacheManagerController : AdminController
    {
        // GET: CacheManager
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Flush()
        {
            try
            {
                CacheHelper.CacheController.FlushAll();
                return Json(new { isSuccess = true, mess = "Xóa cache thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { isSuccess = false, mess = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}