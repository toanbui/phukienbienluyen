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
    [AdminAuthorize(Roles = "Admin, ProductProps")]
    public class ProductPropsController : AdminController
    {
        // GET: ProductProp
        private ProductPropsBo _bo = new ProductPropsBo();
        ApplicationDbContext context = new ApplicationDbContext();
        public ActionResult Index()
        {
            ViewBag.Status = Utils.GetStatusList();
            return View();
        }
        public ActionResult AjaxLoadList(GridFilterSetting<ProductPropsEntity> gridFilterSetting, string keysearch , int? status)
        {
            var pagininfo = new EtsPaging { RowStart = gridFilterSetting.iDisplayStart, PageSize = gridFilterSetting.iDisplayLength };
            var param = new ProductPropsParam() { PagingInfo = pagininfo };
            var ProductPropsFilter = new ProductPropsFilter() { keysearch = keysearch, Status = status };
            param.ProductPropsFilter = ProductPropsFilter;
            _bo.Search(param);
            long count = pagininfo.RecordCount;
            return Json(new { aaData = param.ProductPropsEntitys, recordsTotal = count, recordsFiltered = count, amount = 0x2710 }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Create(string Id)
        {
            var _Id = 0;
            var param = new ProductPropsParam();
            if (Int32.TryParse(Id, out _Id))
            {
                param.ProductPropsFilter = new ProductPropsFilter() { Id = _Id };
                _bo.GetById(param);
                ViewBag.Status = Utils.GetStatusList(param.ProductProps.Status);
            }
            else
            {
                param.ProductProps = new ProductProp();
                ViewBag.Status = Utils.GetStatusList(0);
            }
            return PartialView(param);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult Create(ProductPropsParam modelInput)
        {
            try
            {
                if (modelInput != null && modelInput.ProductProps != null)
                {
                    if (modelInput.ProductProps.Id > 0)
                    {
                        modelInput.ProductProps.Modified = DateTime.Now;
                        modelInput.ProductProps.ModifiedBy = User.Identity.Name;
                        _bo.Update(modelInput);
                        return Json(new { isSuccess = true, mess = Resources.Message.Msg_UpdateSuccesfull }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        modelInput.ProductProps.Created = DateTime.Now;
                        modelInput.ProductProps.Modified = DateTime.Now;
                        modelInput.ProductProps.CreatedBy = User.Identity.Name;
                        modelInput.ProductProps.ModifiedBy = User.Identity.Name;
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
            var param = new ProductPropsParam();
            param.ProductPropsFilter = new ProductPropsFilter() { Id = Id ?? 0 };
            _bo.GetById(param);
            var item = param.ProductProps;
            if (item == null)
            {
                ViewBag.Error = Resources.Message.Error_NotExit;
                return PartialView(new ProductProp());
            }
            else
                return PartialView(item);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(ProductProp obj)
        {
            try
            {
                var list = new List<ProductProp> { obj };
                var param = new ProductPropsParam { ProductPropss = list };
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