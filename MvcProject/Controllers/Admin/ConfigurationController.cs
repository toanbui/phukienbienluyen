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
    [AdminAuthorize(Roles = "Configuration , Manager , Admin")]
    public class ConfigurationController : AdminController
    {
        // GET: Configuration
        private ConfigurationBo _bo = new ConfigurationBo();
        ApplicationDbContext context = new ApplicationDbContext();
        public ActionResult Index()
        {
            ViewBag.Status = Utils.GetStatusList();
            return View();
        }
        public ActionResult AjaxLoadList(GridFilterSetting<ConfigurationEntity> gridFilterSetting, string keysearch , int? status)
        {
            var pagininfo = new EtsPaging { RowStart = gridFilterSetting.iDisplayStart, PageSize = gridFilterSetting.iDisplayLength };
            var param = new ConfigurationParam() { PagingInfo = pagininfo };
            var ConfigurationFilter = new ConfigurationFilter() { keysearch = keysearch, Status = status };
            param.ConfigurationFilter = ConfigurationFilter;
            _bo.Search(param);
            long count = pagininfo.RecordCount;
            return Json(new { aaData = param.ConfigurationEntitys, recordsTotal = count, recordsFiltered = count, amount = 0x2710 }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Create(string Id)
        {
            var _Id = 0;
            var param = new ConfigurationParam();
            if (Int32.TryParse(Id, out _Id))
            {
                param.ConfigurationFilter = new ConfigurationFilter() { Id = _Id };
                _bo.GetById(param);
            }
            else
            {
                param.Configuration = new Configuration();
                ViewBag.Status = Utils.GetStatusList(0);
            }
            return PartialView(param);
        }
        [HttpPost, ValidateInput(false)]
        [AdminAuthorize(Roles = "Developer,Manager")]
        public ActionResult Create(ConfigurationParam modelInput)
        {
            try
            {
                if (modelInput != null && modelInput.Configuration != null)
                {
                    if (modelInput.Configuration.Id > 0)
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
        public ActionResult Delete(int? Id)
        {
            var param = new ConfigurationParam();
            param.ConfigurationFilter = new ConfigurationFilter() { Id = Id ?? 0 };
            _bo.GetById(param);
            var item = param.Configuration;
            if (item == null)
            {
                ViewBag.Error = Resources.Message.Error_NotExit;
                return PartialView(new Configuration());
            }
            else
                return PartialView(item);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AdminAuthorize(Roles = "Developer")]
        public ActionResult Delete(Configuration obj)
        {
            try
            {
                var list = new List<Configuration> { obj };
                var param = new ConfigurationParam { Configurations = list };
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