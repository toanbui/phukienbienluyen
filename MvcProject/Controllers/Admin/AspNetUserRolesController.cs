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
using System.Text.RegularExpressions;
using System.Web.Mvc;
using Utilities;

namespace MvcProject.Controllers.Admin
{
    [AdminAuthorize(Roles = "AspNetUserRoles")]
    public class AspNetUserRolesController : AdminController
    {
        // GET: AspNetUserRoles
        private AspNetUserRolesBo _bo = new AspNetUserRolesBo();
        ApplicationDbContext context = new ApplicationDbContext();
        public ActionResult Index()
        {
            ViewBag.Status = Utils.GetStatusList();
            return View();
        }
        public ActionResult AjaxLoadList(GridFilterSetting<AspNetUserRolesEntity> gridFilterSetting, string keysearch , int? status)
        {
            var pagininfo = new EtsPaging { RowStart = gridFilterSetting.iDisplayStart, PageSize = gridFilterSetting.iDisplayLength };
            var param = new AspNetUserRolesParam() { PagingInfo = pagininfo };
            var AspNetUserRolesFilter = new AspNetUserRolesFilter() { keysearch = keysearch };
            param.AspNetUserRolesFilter = AspNetUserRolesFilter;
            _bo.Search(param);
            long count = pagininfo.RecordCount;
            return Json(new { aaData = param.AspNetUserRolesEntitys, recordsTotal = count, recordsFiltered = count, amount = 0x2710 }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Create(string Id)
        {
            var _Id = 0;
            var param = new AspNetUserRolesParam();
            if (Int32.TryParse(Id, out _Id))
            {
                param.AspNetUserRolesFilter = new AspNetUserRolesFilter();
                _bo.GetById(param);
            }
            else
            {
                param.AspNetUserRoles = new AspNetUserRole();
                ViewBag.Status = Utils.GetStatusList(0);
            }
            return PartialView(param);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult Create(AspNetUserRolesParam modelInput)
        {
            try
            {
                if (modelInput != null && modelInput.AspNetUserRoles != null)
                {
                    if (!string.IsNullOrEmpty(modelInput.AspNetUserRoles.RoleId))
                    {
                        _bo.Update(modelInput);
                        return Json(new { isSuccess = true, mess = Resources.Message.Msg_UpdateSuccesfull }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        _bo.Insert(modelInput);
                        return Json(new { isSuccess = true, mess = Resources.Message.Msg_AddnewSuccesfull }, JsonRequestBehavior.AllowGet);
                    }
                }
                return Json(new { isSuccess = false, mess = Resources.Message.Msg_Invalid }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { isSuccess = false, mess = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult Delete(string Id)
        {
            var param = new AspNetUserRolesParam();
            param.AspNetUserRolesFilter = new AspNetUserRolesFilter();
            _bo.GetById(param);
            var item = param.AspNetUserRoles;
            if (item == null)
            {
                ViewBag.Error = Resources.Message.Error_NotExit;
                return PartialView(new AspNetUserRole());
            }
            else
                return PartialView(item);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(AspNetUserRole obj)
        {
            try
            {
                var list = new List<AspNetUserRole> { obj };
                var param = new AspNetUserRolesParam { AspNetUserRoless = list };
                _bo.Delete(param);
                return Json(new { isSuccess = true, mess = Resources.Message.Msg_DeleteSuccesfull }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { isSuccess = false, mess = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}