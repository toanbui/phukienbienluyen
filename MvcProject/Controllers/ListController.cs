using BO;
using Entities.Base;
using Entities.Filter;
using Entities.Param;
using Entities.Param.FrontEnd;
using MvcProject.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Utilities;

namespace MvcProject.Controllers
{
    public class ListController : Controller
    {
        // GET: List
        //[Authorize(Roles = "Admin, Super User")]
        public ProductBo ProductBo = new ProductBo();
        public AdsPositionBo AdsPositionBo = new AdsPositionBo();
        public AdvertisingBo AdvertisingBo = new AdvertisingBo();
        public ActionResult Index(int catId)
        {

            var model = new HomeModel();
           
            #region ListProduct
            var pagininfo = new EtsPaging { RowStart = 0, PageSize = 12 };
            var param = new ProductParam() { PagingInfo = pagininfo };
            var ProductFilter = new ProductFilter() { Status = (int)Utilities.Constants.RecordStatus.Published  , ZoneId = catId};
            param.ProductFilter = ProductFilter;
            ProductBo.FeSearchOrderByPrice(param);
            #endregion

            model.ProductParam = param;

            #region Zone
            var zoneBo = new ProductZoneBo();
            var zoneParam = new ProductZoneParam();
            var zoneFilter = new ProductZoneFilter() { Id = catId };
            zoneParam.ProductZoneFilter = zoneFilter;
            zoneBo.GetById(zoneParam);
            ViewBag.ZoneName = zoneParam.ProductZone.Name;
            #endregion


            #region Meta
            ViewBag.Title = "Phụ kiện biển luyến - Chuyên sỉ phụ kiện";
            ViewBag.MetaSeo = Utilities.Utils.FillMeta("Phụ kiện biển luyến - Chuyên sỉ phụ kiện", "", string.Join(", ", new List<string>()));
            ViewBag.OgImage = "";
            ViewBag.OgUrl = "".BuildAbsoluteUrl(Config.DOMAIN);
            ViewBag.Canonical = "".BuildAbsoluteUrl(Config.DOMAIN);
            ViewBag.ZoneId = catId;
            #endregion

            return View(model);
        }
        [HttpPost]
        public ActionResult AjaxProduct(int pageIndex, int pageSize , int zoneId)
        {
            try
            {
                var pagininfo = new EtsPaging { RowStart = (pageIndex - 1) * pageSize, PageSize = pageSize };
                var param = new ProductParam() { PagingInfo = pagininfo };
                var ProductFilter = new ProductFilter() { Status = (int)Utilities.Constants.RecordStatus.Published , ZoneId = zoneId };
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
    }
}