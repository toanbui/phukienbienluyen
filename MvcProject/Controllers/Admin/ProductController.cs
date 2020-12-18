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
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using Utilities;
using Utilities.Helper;

namespace MvcProject.Controllers.Admin
{
    [AdminAuthorize(Roles = "Admin , Product")]
    public class ProductController : AdminController
    {
        // GET: Product
        private ProductBo _bo = new ProductBo();
        ApplicationDbContext context = new ApplicationDbContext();
        public ActionResult Index()
        {
            ViewBag.Status = Utils.GetStatusList();
            return View();
        }
        public ActionResult AjaxLoadList(GridFilterSetting<ProductEntity> gridFilterSetting, string keysearch , int? status)
        {
            var pagininfo = new EtsPaging { RowStart = gridFilterSetting.iDisplayStart, PageSize = gridFilterSetting.iDisplayLength };
            var param = new ProductParam() { PagingInfo = pagininfo };
            var ProductFilter = new ProductFilter() { keysearch = keysearch, Status = status , OrderDateDesc = true };
            param.ProductFilter = ProductFilter;
            _bo.Search(param);
            long count = pagininfo.RecordCount;
            return Json(new { aaData = param.ProductEntitys, recordsTotal = count, recordsFiltered = count, amount = 0x2710 }, JsonRequestBehavior.AllowGet);
        }
        public List<ProductZoneEntity> GetAllZone()
        {
            var param = new ProductZoneParam();
            param.ProductZoneFilter = new ProductZoneFilter() { keysearch = "", Status = null, ParentId = null };
            new ProductZoneBo().Search(param);
            return param.ProductZoneEntitys;
        }
        public List<ProductPropsEntity> GetAllProps()
        {
            var param = new ProductPropsParam();
            param.ProductPropsFilter = new ProductPropsFilter() { keysearch = "", Status = (int)Utilities.Constants.RecordStatus.Published };
            new ProductPropsBo().Search(param);
            return param.ProductPropsEntitys;
        }
        public List<PropsOfProductEntity> GetPropsOfProduct(int productId)
        {
            var param = new PropsOfProductParam();
            param.PropsOfProductFilter = new PropsOfProductFilter() { ProductId = productId };
            new PropsOfProductBo().Search(param);
            return param.PropsOfProductEntitys;
        }
        public ActionResult Create(string Id)
        {
            var _Id = 0;
            var param = new ProductParam();

            ViewBag.Categorys = GetAllZone().OrderBy(i => i.Created);
            ViewBag.Props = GetAllProps().OrderBy(i => i.Created);
            ViewBag.Status = Utils.GetStatusList(0);

            if (Int32.TryParse(Id, out _Id))
            {
                param.ProductFilter = new ProductFilter() { Id = _Id };
                _bo.GetById(param);

                param.PropsOfProductIds = GetPropsOfProduct(_Id).Select(i => i.PropsId ?? 0).ToList();
                param.PropsOfProductEntitys = GetPropsOfProduct(_Id);

                //Extend
                ViewBag.ListAvatar = param.ProductEntity.ListAvatar;
            }
            else
            {
                param.Product = new Product();
                param.PropsOfProductIds = GetPropsOfProduct(_Id).Select(i => i.PropsId ?? 0).ToList();
                ViewBag.Create = true;
                //Extend
                ViewBag.ListAvatar = null;
            }
            return PartialView(param);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult Create(ProductParam modelInput , List<int> hdProps)
        {
            try
            {
                var listPath = UploadHelper.Uploads(Request.Files, "Upload/Product/");
                if (!string.IsNullOrEmpty(listPath.ToStringList()))
                {
                    if (!string.IsNullOrEmpty(modelInput.Product.Avatar))
                    {
                        modelInput.Product.Avatar = string.Format("{0};{1}", modelInput.Product.Avatar, listPath.ToStringList());
                    }
                    else
                    {
                        modelInput.Product.Avatar = listPath.ToStringList();
                    }
                }
                if (modelInput != null && modelInput.Product != null)
                {
                    if (modelInput.Product.Id > 0)
                    {
                        modelInput.Product.Modified = DateTime.Now;
                        modelInput.Product.ModifiedBy = User.Identity.Name;
                        _bo.Update(modelInput);
                        return Json(new { isSuccess = true, mess = Resources.Message.Msg_UpdateSuccesfull }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        modelInput.Product.Modified = DateTime.Now;
                        modelInput.Product.ModifiedBy = User.Identity.Name;
                        modelInput.Product.Created = DateTime.Now;
                        modelInput.Product.CreatedBy = User.Identity.Name;

                        _bo.Insert(modelInput);
                        if (hdProps != null && hdProps.Any() && modelInput.Product.Id > 0)
                        {

                            var _boPropsOfProduct = new PropsOfProductBo();
                            foreach (var item in hdProps)
                            {
                                var PropsOfProduct = new PropsOfProduct() { PropsId = item, ProductId = modelInput.Product.Id };
                                var _paramPropsOfProduct = new PropsOfProductParam() { PropsOfProduct = PropsOfProduct };
                                _boPropsOfProduct.Insert(_paramPropsOfProduct);
                            }

                        }
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
            var param = new ProductParam();
            param.ProductFilter = new ProductFilter() { Id = Id ?? 0 };
            _bo.GetById(param);
            var item = param.Product;
            if (item == null)
            {
                ViewBag.Error = Resources.Message.Error_NotExit;
                return PartialView(new Product());
            }
            else
                return PartialView(item);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Product obj)
        {
            try
            {
                var list = new List<Product> { obj };
                var param = new ProductParam { Products = list };
                _bo.Delete(param);
                return Json(new { isSuccess = true, mess = Resources.Message.Msg_DeleteSuccesfull }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { isSuccess = false, mess = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult DeleteProps(string id, string deleteId)
        {
            var _id = 0;
            var _deleteId = 0;

            if (Int32.TryParse(id, out _id))
            {
                if (Int32.TryParse(deleteId, out _deleteId))
                {
                    var BO = new PropsOfProductBo();
                    var Filter = new PropsOfProductFilter() { ProductId = _id, PropsId = _deleteId };
                    var param = new PropsOfProductParam() { PropsOfProductFilter = Filter };
                    BO.Search(param);
                    if (param.PropsOfProductEntitys != null)
                    {
                        PropsOfProduct item = param.PropsOfProductEntitys.FirstOrDefault();
                        var list = new List<PropsOfProduct> { item };
                        var paramDelete = new PropsOfProductParam { PropsOfProducts = list };
                        BO.Delete(paramDelete);
                        return Json(new { isSuccess = false, mess = Resources.Message.Msg_Successfull }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            return Json(new { isSuccess = false, mess = Resources.Message.Msg_Invalid }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult AddProps(string id, string addId)
        {
            var _id = 0;
            var _addId = 0;

            if (Int32.TryParse(id, out _id))
            {
                if (Int32.TryParse(addId, out _addId))
                {
                    var BO = new PropsOfProductBo();
                    var item = new PropsOfProduct() { ProductId = _id, PropsId = _addId };
                    var param = new PropsOfProductParam() { PropsOfProduct = item };
                    BO.Insert(param);
                    return Json(new { isSuccess = false, mess = Resources.Message.Msg_Successfull }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { isSuccess = false, mess = Resources.Message.Msg_Invalid }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Import()
        {
            return PartialView();
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult Import(FormCollection forminput)
        {
            try
            {
                var filePath = UploadHelper.UpLoadFile(Request.Files["ExcelInput"], "Upload/Document/");
                filePath = Server.MapPath(filePath);

                var dtExcel = Utilities.Helper.ExcelHelper.ReadExcelFile(filePath);

                if (dtExcel != null && dtExcel.Rows.Count > 0)
                {
                    for (int i = 1; i < dtExcel.Rows.Count; i++)
                    {
                        var item = new Product()
                        {
                            Name = dtExcel.Rows[i][0].ToString(),
                            Price = dtExcel.Rows[i][1].ChangeType<int>(),
                            Description = dtExcel.Rows[i][2].ToString(),
                            ZoneId = 1,
                            Created  = DateTime.Now,
                            CreatedBy = User.Identity.Name,
                            Modified = DateTime.Now,
                            ModifiedBy = User.Identity.Name,
                            Status = Utilities.Constants.RecordStatus.Published.ChangeType<int>()
                        };
                        var param = new ProductParam() { Product = item };
                        _bo.Insert(param);
                    }
                    try
                    {
                        System.IO.File.Delete(filePath);
                    }
                    catch { }
                    return Json(new { isSuccess = true, mess = Resources.Message.Msg_AddnewSuccesfull }, JsonRequestBehavior.AllowGet);
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