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
using Utilities.Helper;

namespace MvcProject.Controllers.Admin
{
    [AdminAuthorize(Roles = "Advertising , Manager , Admin")]
    public class AdvertisingController : AdminController
    {
        // GET: Advertising
        private AdvertisingBo _bo = new AdvertisingBo();
        ApplicationDbContext context = new ApplicationDbContext();
        public ActionResult Index()
        {
            ViewBag.Status = Utils.GetStatusList();
            ViewBag.Positions = GetAllPosition();
            return View();
        }
        public ActionResult AjaxLoadList(GridFilterSetting<AdvertisingEntity> gridFilterSetting, string keysearch , int? status , int? position)
        {
            var pagininfo = new EtsPaging { RowStart = gridFilterSetting.iDisplayStart, PageSize = gridFilterSetting.iDisplayLength };
            var param = new AdvertisingParam() { PagingInfo = pagininfo };
            var AdvertisingFilter = new AdvertisingFilter() { keysearch = keysearch, Status = status  , PositionId = position };
            param.AdvertisingFilter = AdvertisingFilter;
            _bo.Search(param);
            long count = pagininfo.RecordCount;
            return Json(new { aaData = param.AdvertisingEntitys, recordsTotal = count, recordsFiltered = count, amount = 0x2710 }, JsonRequestBehavior.AllowGet);
        }
        public List<AdsPositionEntity> GetAllPosition()
        {
            var positionBo = new AdsPositionBo();
            var param = new AdsPositionParam();
            var AdsPositionFilter = new AdsPositionFilter() { Status = (int)Utilities.Constants.RecordStatus.Published };
            param.AdsPositionFilter = AdsPositionFilter;
            positionBo.Search(param);
            return param.AdsPositionEntitys;
        }
        public ActionResult Create(string Id)
        {
            var _Id = 0;
            var param = new AdvertisingParam();
            if (Int32.TryParse(Id, out _Id))
            {
                param.AdvertisingFilter = new AdvertisingFilter() { Id = _Id };
                _bo.GetById(param);
                ViewBag.Status = Utils.GetStatusList(param.Advertising.Status);
            }
            else
            {
                param.Advertising = new Advertising();
                ViewBag.Status = Utils.GetStatusList(0);
            }
            ViewBag.Positions = GetAllPosition();

            return PartialView(param);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult Create(AdvertisingParam modelInput)
        {
            try
            {
                if (modelInput != null && modelInput.Advertising != null)
                {

                    var Imagelink = UploadHelper.UpLoadFile(Request.Files["AvatarInput"], "Upload/Advertising/");
                    if (!string.IsNullOrEmpty(Imagelink))
                    {
                        modelInput.Advertising.Avatar = Imagelink;
                    }

                    if (modelInput.Advertising.Id > 0)
                    {
                        modelInput.Advertising.Modified = DateTime.Now;
                        modelInput.Advertising.ModifiedBy = User.Identity.Name;
                        _bo.Update(modelInput);
                        return Json(new { isSuccess = true, mess = Resources.Message.Msg_UpdateSuccesfull }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        modelInput.Advertising.Created = DateTime.Now;
                        modelInput.Advertising.Modified = DateTime.Now;
                        modelInput.Advertising.CreatedBy = User.Identity.Name;
                        modelInput.Advertising.ModifiedBy = User.Identity.Name;
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
            var param = new AdvertisingParam();
            param.AdvertisingFilter = new AdvertisingFilter() { Id = Id ?? 0 };
            _bo.GetById(param);
            var item = param.Advertising;
            if (item == null)
            {
                ViewBag.Error = Resources.Message.Error_NotExit;
                return PartialView(new Advertising());
            }
            else
                return PartialView(item);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Advertising obj)
        {
            try
            {
                var list = new List<Advertising> { obj };
                var param = new AdvertisingParam { Advertisings = list };
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