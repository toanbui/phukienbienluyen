using BO;
using Entities.Base;
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
using Entities.Param.FrontEnd;
using Utilities;
using Entities.Entities;

namespace MvcProject.Controllers
{
    //Route : front/Home
    public class HomeController : FrontController
    {
        public ProductBo ProductBo = new ProductBo();
        public AdsPositionBo AdsPositionBo = new AdsPositionBo();
        public AdvertisingBo AdvertisingBo = new AdvertisingBo();
        public ActionResult Index()
        {

            var model = new HomeModel();


            #region Home Slider
            var posId = 1;
            var adsPosParam = new AdsPositionParam() { AdsPositionFilter = new AdsPositionFilter() { Id = posId, Status = (int)Utilities.Constants.RecordStatus.Published} };
            AdsPositionBo.GetById(adsPosParam);

            if (adsPosParam.AdsPosition != null && adsPosParam.AdsPosition.Id > 0)
            {
                var advertisingParam = new AdvertisingParam() { AdvertisingFilter = new AdvertisingFilter() { PositionId = posId, Status = (int)Utilities.Constants.RecordStatus.Published } };
                AdvertisingBo.Search(advertisingParam);

                model.AdvertisingParam = advertisingParam;
            }
            #endregion

            #region ListProduct
            var pagininfo = new EtsPaging { RowStart = 0, PageSize = 12 };
            var param = new ProductParam() { PagingInfo = pagininfo};
            var ProductFilter = new ProductFilter() { Status = (int)Utilities.Constants.RecordStatus.Published };
            param.ProductFilter = ProductFilter;
            ProductBo.FeSearchOrderByPrice(param);
            #endregion

            //#region Article
            //var articleBo = new ArticleBo();
            //var paginNewsinfo = new EtsPaging { RowStart = 0, PageSize = 10 };
            //var paramArticle = new ArticleParam() { PagingInfo = paginNewsinfo , ArticleFilter = new ArticleFilter() { Status = (int)Utilities.Constants.RecordStatus.Published , Invisible = false } };
            //articleBo.Search(paramArticle);
            //#endregion

            model.ProductParam = param;

            #region Meta
            ViewBag.Title = "Phụ kiện biển luyến - Chuyên bán buôn bán sỉ phụ kiện điện thoại số lượng lớn";
            ViewBag.MetaSeo = Utilities.Utils.FillMeta("Phụ kiện biển luyến - Chuyên bán buôn bán sỉ phụ kiện điện thoại số lượng lớn", "", string.Join(", ", new List<string>()));
            ViewBag.OgImage = "";
            ViewBag.OgUrl = "".BuildAbsoluteUrl(Config.DOMAIN);
            ViewBag.Canonical = "".BuildAbsoluteUrl(Config.DOMAIN);
            #endregion

            return View(model);
        }
        [HttpPost]
        public ActionResult AjaxProduct(int pageIndex, int pageSize)
        {
            try
            {
                //var PageInfo = new EtsPaging { RowStart = (pageIndex - 1) * pageSize, PageSize = pageSize };
                var pagininfo = new EtsPaging { RowStart = (pageIndex - 1) * pageSize, PageSize = pageSize };
                var param = new ProductParam() { PagingInfo = pagininfo };
                var ProductFilter = new ProductFilter() { Status = (int)Utilities.Constants.RecordStatus.Published };
                param.ProductFilter = ProductFilter;
                ProductBo.FeSearchOrderByPrice(param);
                if (param.ProductEntitys != null && param.ProductEntitys.Any())
                {
                    return PartialView(param);
                }
                return Json(new { isSuccess = false, mess = "Hết data" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { isSuccess = false, mess = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult CustToolsRegister(string email, string machine)
        {
            try
            {
                var _bo = new CustomerToolsBo();
                var param = new CustomerToolsParam() { CustomerToolsFilter = new CustomerToolsFilter() { Email = email, Status = Utilities.Constants.RecordStatus.Pending.ChangeType<int>(), OrderDateDesc = true } };
                _bo.Search(param);
                if (param.CustomerToolsEntitys != null && param.CustomerToolsEntitys.Any())
                {
                    var info = param.CustomerToolsEntitys.FirstOrDefault();
                    if (info != null && info.Id > 0)
                    {
                        var keygen = machine.CreateMD5();
                        var cust = new CustomerTool() { Email = email, MachineId = machine, Keygen = keygen, Id = info.Id, Status = (int)Utilities.Constants.RecordStatus.Published };
                        param.CustomerTool = cust;
                        _bo.Update(param);
                        return Json(new { isSuccess = true, mess = keygen }, JsonRequestBehavior.AllowGet);
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