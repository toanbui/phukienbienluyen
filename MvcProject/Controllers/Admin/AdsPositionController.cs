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
    [AdminAuthorize(Roles = "AdsPosition , Manager , Admin")]
    public class AdsPositionController : AdminController
    {
        // GET: AdsPosition
        private AdsPositionBo _bo = new AdsPositionBo();
        ApplicationDbContext context = new ApplicationDbContext();
        public ActionResult Index()
        {
            ViewBag.Status = Utils.GetStatusList();
            return View();
        }
        public ActionResult AjaxLoadList(GridFilterSetting<AdsPositionEntity> gridFilterSetting, string keysearch , int? status)
        {
            var pagininfo = new EtsPaging { RowStart = gridFilterSetting.iDisplayStart, PageSize = gridFilterSetting.iDisplayLength };
            var param = new AdsPositionParam() { PagingInfo = pagininfo };
            var AdsPositionFilter = new AdsPositionFilter() { keysearch = keysearch, Status = status };
            param.AdsPositionFilter = AdsPositionFilter;
            _bo.Search(param);
            long count = pagininfo.RecordCount;
            return Json(new { aaData = param.AdsPositionEntitys, recordsTotal = count, recordsFiltered = count, amount = 0x2710 }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Create(string Id)
        {
            var _Id = 0;
            var param = new AdsPositionParam();
            if (Int32.TryParse(Id, out _Id))
            {
                param.AdsPositionFilter = new AdsPositionFilter() { Id = _Id };
                _bo.GetById(param);
                ViewBag.Status = Utils.GetStatusList(param.AdsPosition.Status);
            }
            else
            {
                param.AdsPosition = new AdsPosition();
                ViewBag.Status = Utils.GetStatusList(0);
            }
            return PartialView(param);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult Create(AdsPositionParam modelInput)
        {
            try
            {
                if (modelInput != null && modelInput.AdsPosition != null)
                {
                    if (modelInput.AdsPosition.Id > 0)
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
        [Authorize(Roles = "Developer")]
        public ActionResult Delete(int? Id)
        {
            var param = new AdsPositionParam();
            param.AdsPositionFilter = new AdsPositionFilter() { Id = Id ?? 0 };
            _bo.GetById(param);
            var item = param.AdsPosition;
            if (item == null)
            {
                ViewBag.Error = Resources.Message.Error_NotExit;
                return PartialView(new AdsPosition());
            }
            else
                return PartialView(item);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Developer")]
        public ActionResult Delete(AdsPosition obj)
        {
            try
            {
                var list = new List<AdsPosition> { obj };
                var param = new AdsPositionParam { AdsPositions = list };
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