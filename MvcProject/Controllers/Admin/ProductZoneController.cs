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
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using Utilities;

namespace MvcProject.Controllers.Admin
{
    [AdminAuthorize(Roles = "Admin , ProductZone")]
    public class ProductZoneController : AdminController
    {
        // GET: ProductZone
        private ProductZoneBo _bo = new ProductZoneBo();
        ApplicationDbContext context = new ApplicationDbContext();
        public ActionResult Index(int? parentId)
        {
            ViewBag.ParentId = parentId ?? 0;
            ViewBag.Status = Utils.GetStatusList();
            return View();
        }
        public ActionResult AjaxLoadList(GridFilterSetting<ProductZoneEntity> gridFilterSetting, string keysearch , int? status , int? parentId)
        {
            var pagininfo = new EtsPaging { RowStart = gridFilterSetting.iDisplayStart, PageSize = gridFilterSetting.iDisplayLength };
            var param = new ProductZoneParam() { PagingInfo = pagininfo };
            var ProductZoneFilter = new ProductZoneFilter() { keysearch = keysearch, Status = status , ParentId = parentId };
            param.ProductZoneFilter = ProductZoneFilter;
            _bo.Search(param);
            if(param.ProductZoneEntitys != null && param.ProductZoneEntitys.Any())
            {
                foreach (var item in param.ProductZoneEntitys)
                {
                    item.Url = string.Format("/danh-muc/{0}-{1}.html", item.Name.ToKoDauAndGach(), item.Id);
                }
            }
            long count = pagininfo.RecordCount;
            return Json(new { aaData = param.ProductZoneEntitys, recordsTotal = count, recordsFiltered = count, amount = 0x2710 }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Create(string Id , string parentId)
        {
            var _Id = 0;
            var _ParentId = 0;
            Int32.TryParse(parentId, out _ParentId);
            var param = new ProductZoneParam();
            if (Int32.TryParse(Id, out _Id))
            {
                param.ProductZoneFilter = new ProductZoneFilter() { Id = _Id };
                _bo.GetById(param);
                ViewBag.Status = Utils.GetStatusList(param.ProductZone.Status);
            }
            else
            {
                
                param.ProductZone = new ProductZone();
                param.ProductZone.ParentId = _ParentId;
                ViewBag.Status = Utils.GetStatusList(0);
            }
            return PartialView(param);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult Create(ProductZoneParam modelInput)
        {
            try
            {
                if (modelInput != null && modelInput.ProductZone != null)
                {
                    if (modelInput.ProductZone.Id > 0)
                    {
                        modelInput.ProductZone.Modified = DateTime.Now;
                        modelInput.ProductZone.ModifiedBy = User.Identity.Name;
                        _bo.Update(modelInput);
                        return Json(new { isSuccess = true, mess = Resources.Message.Msg_UpdateSuccesfull }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        modelInput.ProductZone.Created = DateTime.Now;
                        modelInput.ProductZone.Modified = DateTime.Now;
                        modelInput.ProductZone.CreatedBy = User.Identity.Name;
                        modelInput.ProductZone.ModifiedBy = User.Identity.Name;
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
            var param = new ProductZoneParam();
            param.ProductZoneFilter = new ProductZoneFilter() { Id = Id ?? 0 };
            _bo.GetById(param);
            var item = param.ProductZone;
            if (item == null)
            {
                ViewBag.Error = Resources.Message.Error_NotExit;
                return PartialView(new ProductZone());
            }
            else
                return PartialView(item);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(ProductZone obj)
        {
            try
            {
                var list = new List<ProductZone> { obj };
                var param = new ProductZoneParam { ProductZones = list };
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