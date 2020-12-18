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
    [AdminAuthorize(Roles = "UserInLiveChat , Manager , Admin")]
    public class UserInLiveChatController : AdminController
    {
        // GET: UserInLiveChat
        private UserInLiveChatBo _bo = new UserInLiveChatBo();
        ApplicationDbContext context = new ApplicationDbContext();
        public ActionResult Index()
        {
            ViewBag.Status = Utils.GetStatusList();
            return View();
        }
        public ActionResult AjaxLoadList(GridFilterSetting<UserInLiveChatEntity> gridFilterSetting, string keysearch , int? status)
        {
            var pagininfo = new EtsPaging { RowStart = gridFilterSetting.iDisplayStart, PageSize = gridFilterSetting.iDisplayLength };
            var param = new UserInLiveChatParam() { PagingInfo = pagininfo };
            var UserInLiveChatFilter = new UserInLiveChatFilter() { keysearch = keysearch, Status = status , OrderDateDesc = true };
            param.UserInLiveChatFilter = UserInLiveChatFilter;
            _bo.Search(param);
            long count = pagininfo.RecordCount;
            return Json(new { aaData = param.UserInLiveChatEntitys, recordsTotal = count, recordsFiltered = count, amount = 0x2710 }, JsonRequestBehavior.AllowGet);
        }
        public List<AspNetUsersEntity> GetAllUser()
        {
            var bo = new AspNetUsersBo();
            var param = new AspNetUsersParam();
            var AspNetUsersFilter = new AspNetUsersFilter() { LockoutEnabled = false };
            param.AspNetUsersFilter = AspNetUsersFilter;
            bo.Search(param);
            if (param.AspNetUsersEntitys != null && param.AspNetUsersEntitys.Any())
            {
                param.AspNetUsersEntitys = param.AspNetUsersEntitys.Where(i => i.UserName != "dev").ToList();
            }
            return param.AspNetUsersEntitys;
        }
        public ActionResult Create(string Id)
        {
            var _Id = 0;
            var param = new UserInLiveChatParam();
            if (Int32.TryParse(Id, out _Id))
            {
                param.UserInLiveChatFilter = new UserInLiveChatFilter() { Id = _Id };
                _bo.GetById(param);
                ViewBag.Status = Utils.GetStatusList(param.UserInLiveChat.Status);
            }
            else
            {
                param.UserInLiveChat = new UserInLiveChat();
                ViewBag.Status = Utils.GetStatusList(0);
            }
            ViewBag.Users = GetAllUser();
            return PartialView(param);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult Create(UserInLiveChatParam modelInput)
        {
            try
            {
                if (modelInput != null && modelInput.UserInLiveChat != null)
                {
                    if (modelInput.UserInLiveChat.Id > 0)
                    {
                        _bo.Update(modelInput);
                        return Json(new { isSuccess = true, mess = Resources.Message.Msg_UpdateSuccesfull }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        var param = new UserInLiveChatParam() { UserInLiveChatFilter = new UserInLiveChatFilter(){ UserId = modelInput.UserInLiveChat.UserId }};
                        _bo.GetByUserId(param);
                        if (param.UserInLiveChat != null && !string.IsNullOrEmpty(param.UserInLiveChat.UserId))
                        {
                            return Json(new { isSuccess = false, mess = string.Format(Resources.Message.Msg_Duplicate,"") }, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            _bo.Insert(modelInput);
                            return Json(new { isSuccess = true, mess = Resources.Message.Msg_AddnewSuccesfull }, JsonRequestBehavior.AllowGet);
                        }
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
            var param = new UserInLiveChatParam();
            param.UserInLiveChatFilter = new UserInLiveChatFilter() { Id = Id ?? 0 };
            _bo.GetById(param);
            var item = param.UserInLiveChat;
            if (item == null)
            {
                ViewBag.Error = Resources.Message.Error_NotExit;
                return PartialView(new UserInLiveChat());
            }
            else
                return PartialView(item);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(UserInLiveChat obj)
        {
            try
            {
                var list = new List<UserInLiveChat> { obj };
                var param = new UserInLiveChatParam { UserInLiveChats = list };
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