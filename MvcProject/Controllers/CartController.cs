using BO;
using Entities.Base;
using Entities.Entities.FrontEnd;
using Entities.Param;
using Entities.Filter;
using MvcProject.Base;
using MvcProject.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Utilities;
using Utilities.Helper;
using Entities.Param.FrontEnd;

namespace MvcProject.Controllers
{
    public class CartController : FrontController
    {
        public ActionResult Index()
        {
            var myCart = CookieStore.GetCookie<CartEntity>(Utilities.Constants.CookieCart);
            if (myCart != null && myCart.ListCart != null && myCart.ListCart.Any())
            {
                var listId = myCart.ListCart.Select(i => i.Id).ToList();
                var bo = new ProductBo();
                var param = new ProductParam() { ProductFilter = new ProductFilter() { Status = (int)Constants.RecordStatus.Published, ListId = listId } };
                bo.GetByListId(param);

                param.TotalProductInCart = 0;

                if (param.ProductInCarts != null && param.ProductInCarts.Any())
                {
                    foreach (var item in param.ProductInCarts)
                    {
                        var prop = myCart.ListCart.Where(i => i.Id == item.Id).FirstOrDefault();
                        if (prop != null && prop.Id > 0)
                        {
                            item.Number = prop.Number;
                            item.TotalPrice = (long)(item.Number * item.Price);
                            param.TotalProductInCart += item.Number;
                            param.TotalPriceInCart += item.TotalPrice;
                        }
                    }
                }
                return View(param);
            }
            return View();
        }
        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateMultiView(ProductParam modelInput)
        {
            try
            {
                //constructor cart
                var param = new ProductParam();
                param.ProductInCarts = new List<ProductInCart>();
                var myCart = new CartEntity();
                myCart.ListCart = modelInput.ProductInCarts;
                param.ProductInCarts = modelInput.ProductInCarts;
                if (myCart.ListCart != null && myCart.ListCart.Any())
                {
                    foreach (var item in myCart.ListCart)
                    {
                        param.TotalProductInCart += item.Number;
                        param.TotalPriceInCart += (long)(item.Price * item.Number);
                        modelInput.TotalPriceInCart = param.TotalPriceInCart;
                    }
                }
                CookieStore.SetCookie(Utilities.Constants.CookieCart, myCart.JsonSerialize(), TimeSpan.FromDays(1000));

                var strTopCart = Utilities.ViewRenderService.RenderView(this, "~/Views/Cart/_TopCart.cshtml", null, true);
                var strTopCartMobile = Utilities.ViewRenderService.RenderView(this, "~/Views/Cart/_TopCartMobile.cshtml", null, true);
                var strCartPage = Utilities.ViewRenderService.RenderView(this, "~/Views/Cart/Index.cshtml", modelInput, true);
                return Json(new { isSuccess = true, mess = "Cập nhật giỏ hàng thành công", topCart = strTopCart , topCartMobile = strTopCartMobile , cartPage = strCartPage }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { isSuccess = false, mess = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult Create(int Id)
        {
            var myCart = CookieStore.GetCookie<CartEntity>(Utilities.Constants.CookieCart);
            if(myCart != null && myCart.ListCart != null && myCart.ListCart.Any())
            {
                var Product = myCart.ListCart.Where(i => i.Id == Id).FirstOrDefault();
                if (Product != null && Product.Id > 0)
                {
                    Product.Number = Product.Number + 1;

                    //update number in cart
                    var index = myCart.ListCart.FindIndex(p => p.Id == Id);
                    myCart.ListCart[index] = Product;
                }
                else
                {
                    //constructor cart
                    Product = new ProductInCart();
                    Product.Id = Id;
                    Product.Number = 1;
                    myCart.ListCart.Add(Product);
                }
            }
            else
            {
                //constructor cart
                myCart = new CartEntity();
                myCart.ListCart = new List<ProductInCart>();
                var Product = new ProductInCart();
                Product.Id = Id;
                Product.Number = 1;
                myCart.ListCart.Add(Product);
            }

            CookieStore.SetCookie(Utilities.Constants.CookieCart, myCart.GetValue().JsonSerialize(), TimeSpan.FromDays(1000));

            var listId = myCart.ListCart.Select(i => i.Id).ToList();
            var bo = new ProductBo();
            var param = new ProductParam() { ProductFilter =  new ProductFilter() { Status = (int)Constants.RecordStatus.Published , ListId = listId } };
            bo.GetByListId(param);

            param.TotalProductInCart = 0;

            if (param.ProductInCarts!= null && param.ProductInCarts.Any())
            {
                foreach (var item in param.ProductInCarts)
                {
                    var prop = myCart.ListCart.Where(i => i.Id == item.Id).FirstOrDefault();
                    if (prop != null && prop.Id > 0)
                    {
                        item.Number = prop.Number;
                        item.TotalPrice = (long)(item.Number * item.Price);
                        param.TotalProductInCart += item.Number;
                        param.TotalPriceInCart += item.TotalPrice;
                    }
                }
            }

            return PartialView(param);
        }
        public ActionResult CreateNumber(int Id , int Number)
        {
            var myCart = CookieStore.GetCookie<CartEntity>(Utilities.Constants.CookieCart);
            if(myCart != null && myCart.ListCart != null && myCart.ListCart.Any())
            {
                var Product = myCart.ListCart.Where(i => i.Id == Id).FirstOrDefault();
                if (Product != null && Product.Id > 0)
                {
                    Product.Number = Product.Number + Number;

                    //update number in cart
                    var index = myCart.ListCart.FindIndex(p => p.Id == Id);
                    myCart.ListCart[index] = Product;
                }
                else
                {
                    //constructor cart
                    Product = new ProductInCart();
                    Product.Id = Id;
                    Product.Number = Number;
                    myCart.ListCart.Add(Product);
                }
            }
            else
            {
                //constructor cart
                myCart = new CartEntity();
                myCart.ListCart = new List<ProductInCart>();
                var Product = new ProductInCart();
                Product.Id = Id;
                Product.Number = Number;
                myCart.ListCart.Add(Product);
            }

            CookieStore.SetCookie(Utilities.Constants.CookieCart, myCart.GetValue().JsonSerialize(), TimeSpan.FromDays(1000));

            var listId = myCart.ListCart.Select(i => i.Id).ToList();
            var bo = new ProductBo();
            var param = new ProductParam() { ProductFilter =  new ProductFilter() { Status = (int)Constants.RecordStatus.Published , ListId = listId } };
            bo.GetByListId(param);

            param.TotalProductInCart = 0;

            if (param.ProductInCarts!= null && param.ProductInCarts.Any())
            {
                foreach (var item in param.ProductInCarts)
                {
                    var prop = myCart.ListCart.Where(i => i.Id == item.Id).FirstOrDefault();
                    if (prop != null && prop.Id > 0)
                    {
                        item.Number = prop.Number;
                        item.TotalPrice = (long)(item.Number * item.Price);
                        param.TotalProductInCart += item.Number;
                        param.TotalPriceInCart += item.TotalPrice;
                    }
                }
            }

            return PartialView("~/Views/Cart/Create.cshtml",param);
        }
        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProductParam modelInput)
        {
            try
            {
                //constructor cart
                var param = new ProductParam();
                param.ProductInCarts = new List<ProductInCart>();
                var myCart = new CartEntity();
                myCart.ListCart = modelInput.ProductInCarts;

                if (myCart.ListCart != null && myCart.ListCart.Any())
                {
                    foreach (var item in myCart.ListCart)
                    {
                        param.TotalProductInCart += item.Number;
                        param.ProductInCarts.Add(new ProductInCart() { Id = item.Id , TotalPrice = (long)(item.Price * item.Number) });
                        param.TotalPriceInCart += (long)(item.Price * item.Number);
                    }
                }
                CookieStore.SetCookie(Utilities.Constants.CookieCart, myCart.JsonSerialize(), TimeSpan.FromDays(1000));
                return Json(new { isSuccess = true, mess = "Cập nhật giỏ hàng thành công" , data = param }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { isSuccess = false, mess = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult Delete(int Id)
        {
            try
            {
                var myCart = CookieStore.GetCookie<CartEntity>(Utilities.Constants.CookieCart);
                if (myCart != null && myCart.ListCart != null && myCart.ListCart.Any())
                {
                    //update cart
                    var index = myCart.ListCart.FindIndex(p => p.Id == Id);
                    myCart.ListCart.RemoveAt(index);

                    CookieStore.SetCookie(Utilities.Constants.CookieCart, myCart.JsonSerialize(), TimeSpan.FromDays(1000));

                    var listId = myCart.ListCart.Select(i => i.Id).ToList();
                    var bo = new ProductBo();
                    var param = new ProductParam() { ProductFilter = new ProductFilter() { Status = (int)Constants.RecordStatus.Published, ListId = listId } };
                    bo.GetByListId(param);

                    param.TotalProductInCart = 0;

                    if (param.ProductInCarts != null && param.ProductInCarts.Any())
                    {
                        foreach (var item in param.ProductInCarts)
                        {
                            var prop = myCart.ListCart.Where(i => i.Id == item.Id).FirstOrDefault();
                            if (prop != null && prop.Id > 0)
                            {
                                item.Number = prop.Number;
                                item.TotalPrice = (long)(item.Number * item.Price);
                                param.TotalProductInCart += item.Number;
                                param.TotalPriceInCart += item.TotalPrice;
                            }
                        }
                    }
                    return PartialView("~/Views/Cart/Create.cshtml",param);
                }
            }
            catch (Exception ex)
            {
                return PartialView();
            }
            return PartialView();
        }
        public ActionResult Remove(int Id)
        {
            try
            {
                var myCart = CookieStore.GetCookie<CartEntity>(Utilities.Constants.CookieCart);
                if (myCart != null && myCart.ListCart != null && myCart.ListCart.Any())
                {
                    //update cart
                    var index = myCart.ListCart.FindIndex(p => p.Id == Id);
                    myCart.ListCart.RemoveAt(index);

                    CookieStore.SetCookie(Utilities.Constants.CookieCart, myCart.JsonSerialize(), TimeSpan.FromDays(1000));

                    var listId = myCart.ListCart.Select(i => i.Id).ToList();
                    var bo = new ProductBo();
                    var param = new ProductParam() { ProductFilter = new ProductFilter() { Status = (int)Constants.RecordStatus.Published, ListId = listId } };
                    bo.GetByListId(param);

                    param.TotalProductInCart = 0;

                    if (param.ProductInCarts != null && param.ProductInCarts.Any())
                    {
                        foreach (var item in param.ProductInCarts)
                        {
                            var prop = myCart.ListCart.Where(i => i.Id == item.Id).FirstOrDefault();
                            if (prop != null && prop.Id > 0)
                            {
                                item.Number = prop.Number;
                                item.TotalPrice = (long)(item.Number * item.Price);
                                param.TotalProductInCart += item.Number;
                                param.TotalPriceInCart += item.TotalPrice;
                            }
                        }
                    }
                    var strTopCart = Utilities.ViewRenderService.RenderView(this, "~/Views/Cart/_TopCart.cshtml", null, true);
                    var strTopCartMobile = Utilities.ViewRenderService.RenderView(this, "~/Views/Cart/_TopCartMobile.cshtml", null, true);
                    //var strCartPage = Utilities.ViewRenderService.RenderView(this, "~/Views/Cart/Index.cshtml", modelInput, true);
                    return Json(new { isSuccess = true, mess = "Cập nhật giỏ hàng thành công", topCart = strTopCart, topCartMobile = strTopCartMobile, cartPage = "" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return PartialView();
            }
            return PartialView();
        }
        public ActionResult UpdateView()
        {
            try
            {
                var myCart = CookieStore.GetCookie<CartEntity>(Utilities.Constants.CookieCart);
                if (myCart != null && myCart.ListCart != null && myCart.ListCart.Any())
                {
                    var listId = myCart.ListCart.Select(i => i.Id).ToList();
                    var bo = new ProductBo();
                    var param = new ProductParam() { ProductFilter = new ProductFilter() { Status = (int)Constants.RecordStatus.Published, ListId = listId } };
                    bo.GetByListId(param);

                    param.TotalProductInCart = 0;

                    if (param.ProductInCarts != null && param.ProductInCarts.Any())
                    {
                        foreach (var item in param.ProductInCarts)
                        {
                            var prop = myCart.ListCart.Where(i => i.Id == item.Id).FirstOrDefault();
                            if (prop != null && prop.Id > 0)
                            {
                                item.Number = prop.Number;
                                item.TotalPrice = (long)(item.Number * item.Price);
                                param.TotalProductInCart += item.Number;
                                param.TotalPriceInCart += item.TotalPrice;
                            }
                        }
                    }
                    var strTopCart = Utilities.ViewRenderService.RenderView(this, "~/Views/Cart/_TopCart.cshtml", null, true);
                    var strTopCartMobile = Utilities.ViewRenderService.RenderView(this, "~/Views/Cart/_TopCartMobile.cshtml", null, true);
                    //var strCartPage = Utilities.ViewRenderService.RenderView(this, "~/Views/Cart/Index.cshtml", modelInput, true);
                    return Json(new { isSuccess = true, mess = "", topCart = strTopCart, topCartMobile = strTopCartMobile, cartPage = "" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return PartialView();
            }
            return PartialView();
        }

        public ActionResult QuickView(int Id)
        {
            var bo = new ProductBo();
            var param = new ProductParam() { ProductFilter = new ProductFilter() { Status = (int)Constants.RecordStatus.Published, Id = Id } };
            bo.GetById(param);

            return PartialView(param);
        }
        public ActionResult CheckOut()
        {
            var model = new CheckOutModel();

            #region Province
            var provinceBo = new ProvinceBo();
            var provinceParam = new ProvinceParam() {  ProvinceFilter = new ProvinceFilter()};
            provinceBo.Search(provinceParam);
            model.ProvinceEntitys = provinceParam.ProvinceEntitys;
            #endregion

            #region Cart Info
            var myCart = CookieStore.GetCookie<CartEntity>(Utilities.Constants.CookieCart);
            if (myCart != null && myCart.ListCart != null && myCart.ListCart.Any())
            {
                var listId = myCart.ListCart.Select(i => i.Id).ToList();
                var bo = new ProductBo();
                var param = new ProductParam() { ProductFilter = new ProductFilter() { Status = (int)Constants.RecordStatus.Published, ListId = listId } };
                bo.GetByListId(param);

                param.TotalProductInCart = 0;

                if (param.ProductInCarts != null && param.ProductInCarts.Any())
                {
                    foreach (var item in param.ProductInCarts)
                    {
                        var prop = myCart.ListCart.Where(i => i.Id == item.Id).FirstOrDefault();
                        if (prop != null && prop.Id > 0)
                        {
                            item.Number = prop.Number;
                            item.TotalPrice = (long)(item.Number * item.Price);
                            param.TotalProductInCart += item.Number;
                            param.TotalPriceInCart += item.TotalPrice;
                        }
                    }
                }
                model.ProductParam = param;
            }
            else
            {
                return Redirect("/");
            }
            #endregion

            return PartialView(model ,false);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CheckOut(CheckOutModel modelInput)
        {
            try
            {
                if (modelInput.ProductParam.ProductInCarts != null && modelInput.ProductParam.ProductInCarts.Any())
                {
                    if (modelInput.ProductParam.TotalPriceInCart < 1000000)
                    {
                        return Json(new { isSuccess = false, mess = "Giá đơn hàng tối thiểu 1 triệu đồng", data = "" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        var query = from n in modelInput.ProductParam.ProductInCarts
                                    select new
                                    {
                                        n.Id,
                                        n.Number
                                    };
                        modelInput.CartOrder.ProductIds = query.ToList().JsonSerialize();
                        modelInput.CartOrder.Approved = false;
                        modelInput.CartOrder.ToltalPrice = modelInput.ProductParam.TotalPriceInCart.ToString();
                        modelInput.CartOrder.Created = DateTime.Now;
                        modelInput.CartOrder.Status = (int)Constants.RecordStatus.Pending;

                        var cartOrderBo = new CartOrderBo();
                        var cartOrderParam = new CartOrderParam() { CartOrder = modelInput.CartOrder };
                        cartOrderBo.Insert(cartOrderParam);

                        CookieStore.RemoveByKey(Utilities.Constants.CookieCart);

                        //Send Mail

                        //Cấu hình
                        var configBo = new ConfigurationBo();
                        var paramConfig = new ConfigurationParam() { ConfigurationFilter = new ConfigurationFilter() };
                        configBo.Search(paramConfig);

                        var email = paramConfig.ConfigurationEntitys.Where(i => i.Id == 1).FirstOrDefault().Value;

                        //EmailHelper.SendOrderMail(modelInput , email);
                        return Json(new { isSuccess = true, mess = "Đặt hàng thành công , nhân viên sẽ gọi cho bạn để chuyển hàng", data = "" }, JsonRequestBehavior.AllowGet);
                    }
                }
                return Json(new { isSuccess = false, mess = "", data = "" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { isSuccess = false, mess = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult ChangeProvince(string code)
        {
            try
            {
                var Bo = new DistrictBo();
                var Param = new DistrictParam() { DistrictFilter = new DistrictFilter() { ProvinceCode = code } };
                Bo.Search(Param);
                return Json(new { isSuccess = true, mess = "", data = Param.DistrictEntitys }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { isSuccess = false, mess = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }

}