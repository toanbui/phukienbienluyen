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
    [AdminAuthorize(Roles = "CustomerTools , Manager , Admin")]
    public class CustomerToolsController : AdminController
    {
        // GET: CustomerTools
        private CustomerToolsBo _bo = new CustomerToolsBo();
        ApplicationDbContext context = new ApplicationDbContext();
        public ActionResult Index()
        {
            ViewBag.Status = Utils.GetStatusList();
            return View();
        }
        public ActionResult AjaxLoadList(GridFilterSetting<CustomerToolsEntity> gridFilterSetting, string keysearch , int? status)
        {
            var pagininfo = new EtsPaging { RowStart = gridFilterSetting.iDisplayStart, PageSize = gridFilterSetting.iDisplayLength };
            var param = new CustomerToolsParam() { PagingInfo = pagininfo };
            var CustomerToolsFilter = new CustomerToolsFilter() { keysearch = keysearch, Status = status , OrderDateDesc = true };
            param.CustomerToolsFilter = CustomerToolsFilter;
            _bo.Search(param);
            long count = pagininfo.RecordCount;
            return Json(new { aaData = param.CustomerToolsEntitys, recordsTotal = count, recordsFiltered = count, amount = 0x2710 }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Create(string Id)
        {
            var _Id = 0;
            var param = new CustomerToolsParam();
            if (Int32.TryParse(Id, out _Id))
            {
                param.CustomerToolsFilter = new CustomerToolsFilter() { Id = _Id };
                _bo.GetById(param);
                ViewBag.Status = Utils.GetStatusList(param.CustomerTool.Status);
            }
            else
            {
                param.CustomerTool = new CustomerTool();
                ViewBag.Status = Utils.GetStatusList(0);
            }
            return PartialView(param);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult Create(CustomerToolsParam modelInput)
        {
            try
            {
                if (modelInput != null && modelInput.CustomerTool != null)
                {
                    if (modelInput.CustomerTool.Id > 0)
                    {
                        _bo.Update(modelInput);
                        return Json(new { isSuccess = true, mess = Resources.Message.Msg_UpdateSuccesfull }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        modelInput.CustomerTool.RegisterDate = DateTime.Now;
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
            var param = new CustomerToolsParam();
            param.CustomerToolsFilter = new CustomerToolsFilter() { Id = Id ?? 0 };
            _bo.GetById(param);
            var item = param.CustomerTools;
            if (item == null)
            {
                ViewBag.Error = Resources.Message.Error_NotExit;
                return PartialView(new CustomerTool());
            }
            else
                return PartialView(item);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(CustomerTool obj)
        {
            try
            {
                var list = new List<CustomerTool> { obj };
                var param = new CustomerToolsParam { CustomerTools = list };
                _bo.Delete(param);
                return Json(new { isSuccess = true, mess = Resources.Message.Msg_DeleteSuccesfull }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { isSuccess = false, mess = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult Register(string email, string machine)
        {
            try
            {
                var param = new CustomerToolsParam() { CustomerToolsFilter  = new CustomerToolsFilter() { Email = email, Status = Utilities.Constants.RecordStatus.Pending.ChangeType<int>(), OrderDateDesc = true } };
                _bo.Search(param);
                if (param.CustomerToolsEntitys != null && param.CustomerToolsEntitys.Any())
                {
                    var info = param.CustomerToolsEntitys.FirstOrDefault();
                    if(info != null && info.Id > 0)
                    {
                        var keygen = machine.CreateMD5();
                        var cust = new CustomerTool() { Email = email, MachineId = machine,Keygen = keygen, Id = info.Id , Status = (int)Utilities.Constants.RecordStatus.Published };
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