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
    [AdminAuthorize(Roles = "Admin")]
    public class SysMenuController : AdminController
    {
        // GET: SysMenu
        private SysMenuBo _bo = new SysMenuBo();
        ApplicationDbContext context = new ApplicationDbContext();
        public ActionResult Index()
        {
            ViewBag.Status = Utils.GetStatusList();
            return View();
        }
        public ActionResult AjaxLoadList(GridFilterSetting<SysMenuEntity> gridFilterSetting, string keysearch , int? status)
        {
            var pagininfo = new EtsPaging { RowStart = gridFilterSetting.iDisplayStart, PageSize = gridFilterSetting.iDisplayLength };
            var param = new SysMenuParam() { PagingInfo = pagininfo };
            var SysMenuFilter = new SysMenuFilter() { keysearch = keysearch, Status = status };
            param.SysMenuFilter = SysMenuFilter;
            _bo.Search(param);
            long count = pagininfo.RecordCount;
            return Json(new { aaData = param.SysMenuEntitys, recordsTotal = count, recordsFiltered = count, amount = 0x2710 }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Create(string Id)
        {
            var _Id = 0;
            var param = new SysMenuParam();
            if (Int32.TryParse(Id, out _Id))
            {
                param.SysMenuFilter = new SysMenuFilter() { Id = _Id };
                _bo.GetById(param);
                ViewBag.Status = Utils.GetStatusList(param.SysMenu.Status);
            }
            else
            {
                param.SysMenu = new SysMenu();
                ViewBag.Status = Utils.GetStatusList(0);
            }
            return PartialView(param);
        }
        [HttpPost, ValidateInput(false)]
        [AdminAuthorize(Roles = "Developer")]
        public ActionResult Create(SysMenuParam modelInput)
        {
            try
            {
                if (modelInput != null && modelInput.SysMenu != null)
                {
                    if (!string.IsNullOrEmpty(modelInput.SysMenu.Controller))
                    {
                        //Khởi tạo role khi tạo module mới
                        var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
                        var roleName = modelInput.SysMenu.Controller;

                        if (!roleManager.RoleExists(roleName))
                        {
                            var role = new IdentityRole();
                            role.Name = roleName;
                            roleManager.Create(role);
                        }
                    }

                    if (modelInput.SysMenu.Id > 0)
                    {
                        if(Utilities.Config.GeneratorCode)
                            new GeneratorCode(modelInput.SysMenu.Controller);
                        modelInput.SysMenu.Modified = DateTime.Now;
                        modelInput.SysMenu.ModifiedBy = User.Identity.Name;
                        _bo.Update(modelInput);
                        return Json(new { isSuccess = true, mess = Resources.Message.Msg_UpdateSuccesfull }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        new GeneratorCode(modelInput.SysMenu.Controller);
                        modelInput.SysMenu.Created = DateTime.Now;
                        modelInput.SysMenu.Modified = DateTime.Now;
                        modelInput.SysMenu.CreatedBy = User.Identity.Name;
                        modelInput.SysMenu.ModifiedBy = User.Identity.Name;
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
        public ActionResult Delete(int? Id)
        {
            var param = new SysMenuParam();
            param.SysMenuFilter = new SysMenuFilter() { Id = Id ?? 0 };
            _bo.GetById(param);
            var item = param.SysMenu;
            if (item == null)
            {
                ViewBag.Error = Resources.Message.Error_NotExit;
                return PartialView(new SysMenu());
            }
            else
                return PartialView(item);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AdminAuthorize(Roles = "Developer")]
        public ActionResult Delete(SysMenu obj)
        {
            try
            {
                var list = new List<SysMenu> { obj };
                var param = new SysMenuParam { SysMenus = list };
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