using BO;
using Entities.Base;
using Entities.Entities;
using Entities.Entities.FrontEnd;
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
    [AdminAuthorize(Roles = "CartOrder , Manager , Admin")]
    public class CartOrderController : AdminController
    {
        // GET: CartOrder
        private CartOrderBo _bo = new CartOrderBo();
        ApplicationDbContext context = new ApplicationDbContext();
        public ActionResult Index()
        {
            ViewBag.Status = Utils.GetStatusList();
            return View();
        }
        public ActionResult AjaxLoadList(GridFilterSetting<CartOrderEntity> gridFilterSetting, string keysearch , int? status)
        {
            var pagininfo = new EtsPaging { RowStart = gridFilterSetting.iDisplayStart, PageSize = gridFilterSetting.iDisplayLength };
            var param = new CartOrderParam() { PagingInfo = pagininfo };
            var CartOrderFilter = new CartOrderFilter() { keysearch = keysearch, Status = status };
            param.CartOrderFilter = CartOrderFilter;
            _bo.Search(param);
            long count = pagininfo.RecordCount;
            return Json(new { aaData = param.CartOrderEntitys, recordsTotal = count, recordsFiltered = count, amount = 0x2710 }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Create(string Id)
        {
            var _Id = 0;
            var param = new CartOrderParam();
            if (Int32.TryParse(Id, out _Id))
            {
                param.CartOrderFilter = new CartOrderFilter() { Id = _Id };
                _bo.GetById(param);
                ViewBag.Status = Utils.GetStatusList(param.CartOrder.Status);
            }
            else
            {
                param.CartOrder = new CartOrder();
                ViewBag.Status = Utils.GetStatusList(0);
            }
            return PartialView(param);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult Create(CartOrderParam modelInput)
        {
            try
            {
                if (modelInput != null && modelInput.CartOrder != null)
                {
                    if (modelInput.CartOrder.Id > 0)
                    {
                        modelInput.CartOrder.Modified = DateTime.Now;
                        modelInput.CartOrder.ModifiedBy = User.Identity.Name;
                        _bo.Update(modelInput);
                        return Json(new { isSuccess = true, mess = Resources.Message.Msg_UpdateSuccesfull }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        modelInput.CartOrder.Created = DateTime.Now;
                        modelInput.CartOrder.Modified = DateTime.Now;
                        modelInput.CartOrder.ModifiedBy = User.Identity.Name;
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
            var param = new CartOrderParam();
            param.CartOrderFilter = new CartOrderFilter() { Id = Id ?? 0 };
            _bo.GetById(param);
            var item = param.CartOrder;
            if (item == null)
            {
                ViewBag.Error = Resources.Message.Error_NotExit;
                return PartialView(new CartOrder());
            }
            else
                return PartialView(item);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(CartOrder obj)
        {
            try
            {
                var list = new List<CartOrder> { obj };
                var param = new CartOrderParam { CartOrders = list };
                _bo.Delete(param);
                return Json(new { isSuccess = true, mess = Resources.Message.Msg_DeleteSuccesfull }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { isSuccess = false, mess = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult ReView(string Id)
        {
            var _Id = 0;
            var param = new CartOrderParam();
            if (Int32.TryParse(Id, out _Id))
            {
                param.CartOrderFilter = new CartOrderFilter() { Id = _Id };
                _bo.GetById(param);
                if (param.CartOrderEntity != null)
                {
                    var myCart = param.CartOrderEntity.ProductIds.JsonDeserialize<List<ProductInCart>>();
                    var listId = myCart.Select(i => i.Id).ToList();
                    var bo = new ProductBo();
                    var paramProduct = new ProductParam() { ProductFilter = new ProductFilter() { Status = (int)Utilities.Constants.RecordStatus.Published, ListId = listId } };
                    bo.GetByListId(paramProduct);

                    if (paramProduct.ProductInCarts != null && paramProduct.ProductInCarts.Any())
                    {
                        foreach (var item in paramProduct.ProductInCarts)
                        {
                            var prop = myCart.Where(i => i.Id == item.Id).FirstOrDefault();
                            if (prop != null && prop.Id > 0)
                            {
                                item.Number = prop.Number;
                                item.TotalPrice = (long)(item.Number * item.Price);
                            }
                        }
                    }
                    param.ProductInCarts = paramProduct.ProductInCarts;
                }
                ViewBag.Status = Utils.GetStatusList(param.CartOrder.Status);
            }
            else
            {
                param.CartOrder = new CartOrder();
                ViewBag.Status = Utils.GetStatusList(0);
            }
            return PartialView(param);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult ReView(CartOrderParam modelInput)
        {
            try
            {
                if (modelInput != null && modelInput.CartOrder != null)
                {
                    if (modelInput.CartOrder.Id > 0)
                    {
                        modelInput.CartOrder.ApprovedBy = User.Identity.Name;
                        _bo.Approve(modelInput);
                        return Json(new { isSuccess = true, mess = Resources.Message.Msg_UpdateSuccesfull }, JsonRequestBehavior.AllowGet);
                    }
                }
                return Json(new { isSuccess = false, mess = Resources.Message.Msg_Invalid }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { isSuccess = false, mess = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}