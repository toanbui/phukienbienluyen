using BO;
using Entities.Base;
using Entities.Entities;
using Entities.Filter;
using Entities.Param;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using MvcProject.Base;
using MvcProject.Models;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;

namespace MvcProject.Controllers.Admin
{
    [AdminAuthorize(Roles = "Admin")]
    public class AspNetRolesController : AdminController
    {
        ApplicationDbContext context;
        public AspNetRolesController()
        {
            context = new ApplicationDbContext();
        }
        // GET: AspNetRoles
        private AspNetRolesBo _bo = new AspNetRolesBo();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult AjaxLoadList(GridFilterSetting<AspNetRolesEntity> gridFilterSetting, string keysearch, int? status)
        {
            var pagininfo = new EtsPaging { RowStart = gridFilterSetting.iDisplayStart, PageSize = gridFilterSetting.iDisplayLength };
            var param = new AspNetRolesParam() { PagingInfo = pagininfo };
            var AspNetRolesFilter = new AspNetRolesFilter() { keysearch = keysearch, Status = status };
            param.AspNetRolesFilter = AspNetRolesFilter;
            _bo.Search(param);
            long count = pagininfo.RecordCount;
            return Json(new { aaData = param.AspNetRolesEntitys, recordsTotal = count, recordsFiltered = count, amount = 0x2710 }, JsonRequestBehavior.AllowGet);
        }
        //public ActionResult Create(string Id)
        //{
        //    var _Id = 0;
        //    var param = new AspNetRolesParam();
        //    if (Int32.TryParse(Id, out _Id))
        //    {
        //        param.AspNetRolesFilter = new AspNetRolesFilter() { Id = _Id };
        //        _bo.GetById(param);
        //    }
        //    else
        //    {
        //        param.AspNetRole = new AspNetRole();
        //    }
        //    return PartialView(param);
        //}
        //[HttpPost, ValidateInput(false)]
        //public ActionResult Create(AspNetRolesParam modelInput)
        //{
        //    try
        //    {
        //        if (modelInput != null && modelInput.AspNetRoles != null)
        //        {
        //            if (!string.IsNullOrEmpty(modelInput.AspNetRole.Id))
        //            {
        //                _bo.Update(modelInput);
        //                return Json(new { isSuccess = true, mess = Resources.Message.Msg_UpdateSuccesfull }, JsonRequestBehavior.AllowGet);
        //            }
        //            else
        //            {
        //                _bo.Insert(modelInput);
        //                return Json(new { isSuccess = true, mess = Resources.Message.Msg_AddnewSuccesfull }, JsonRequestBehavior.AllowGet);
        //            }

        //        }
        //        return Json(new { isSuccess = false, mess = Resources.Message.Msg_Invalid }, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { isSuccess = false, mess = ex.Message }, JsonRequestBehavior.AllowGet);
        //    }
        //}
        //public ActionResult Delete(int? Id)
        //{
        //    var param = new AspNetRolesParam();
        //    param.AspNetRolesFilter = new AspNetRolesFilter() { Id = Id ?? 0 };
        //    _bo.GetById(param);
        //    var item = param.AspNetRoles;
        //    if (item == null)
        //    {
        //        ViewBag.Error = Resources.Message.Error_NotExit;
        //        return PartialView(new AspNetRole());
        //    }
        //    else
        //        return PartialView(item);
        //}
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Delete(AspNetRole obj)
        //{
        //    try
        //    {
        //        var list = new List<AspNetRole> { obj };
        //        var param = new AspNetRolesParam { AspNetRoles = list };
        //        _bo.Delete(param);
        //        return Json(new { isSuccess = true, mess = Resources.Message.Msg_DeleteSuccesfull }, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { isSuccess = false, mess = ex.Message }, JsonRequestBehavior.AllowGet);
        //    }
        //}
    }
}