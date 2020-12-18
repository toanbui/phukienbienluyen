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
    [AdminAuthorize(Roles = "LiveChatHistory , Manager , Admin")]
    public class LiveChatHistoryController : AdminController
    {
        // GET: LiveChatHistory
        private LiveChatHistoryBo _bo = new LiveChatHistoryBo();
        ApplicationDbContext context = new ApplicationDbContext();
        public ActionResult Index()
        {
            ViewBag.Status = Utils.GetStatusList();
            return View();
        }
        public ActionResult AjaxLoadList(GridFilterSetting<LiveChatHistoryEntity> gridFilterSetting, string keysearch , int? status)
        {
            var pagininfo = new EtsPaging { RowStart = gridFilterSetting.iDisplayStart, PageSize = gridFilterSetting.iDisplayLength };
            var param = new LiveChatHistoryParam() { PagingInfo = pagininfo };
            var LiveChatHistoryFilter = new LiveChatHistoryFilter() { keysearch = keysearch, Status = status };
            param.LiveChatHistoryFilter = LiveChatHistoryFilter;
            _bo.Search(param);
            long count = pagininfo.RecordCount;
            return Json(new { aaData = param.LiveChatHistoryEntitys, recordsTotal = count, recordsFiltered = count, amount = 0x2710 }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Create(string Id)
        {
            var _Id = 0;
            var param = new LiveChatHistoryParam();
            if (Int32.TryParse(Id, out _Id))
            {
                param.LiveChatHistoryFilter = new LiveChatHistoryFilter() { Id = _Id };
                _bo.GetById(param);
            }
            else
            {
                param.LiveChatHistory = new LiveChatHistory();
                ViewBag.Status = Utils.GetStatusList(0);
            }
            return PartialView(param);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult Create(LiveChatHistoryParam modelInput)
        {
            try
            {
                if (modelInput != null && modelInput.LiveChatHistory != null)
                {
                    if (modelInput.LiveChatHistory.Id > 0)
                    {
                        _bo.Update(modelInput);
                        return Json(new { isSuccess = true, mess = Resources.Message.Msg_UpdateSuccesfull }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        modelInput.LiveChatHistory.Created = DateTime.Now;
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
            var param = new LiveChatHistoryParam();
            param.LiveChatHistoryFilter = new LiveChatHistoryFilter() { Id = Id ?? 0 };
            _bo.GetById(param);
            var item = param.LiveChatHistory;
            if (item == null)
            {
                ViewBag.Error = Resources.Message.Error_NotExit;
                return PartialView(new LiveChatHistory());
            }
            else
                return PartialView(item);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(LiveChatHistory obj)
        {
            try
            {
                var list = new List<LiveChatHistory> { obj };
                var param = new LiveChatHistoryParam { LiveChatHistorys = list };
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