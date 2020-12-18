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
using System.Security.Claims;
using System.Web.Mvc;
using Utilities;
using Utilities.Helper;

namespace MvcProject.Controllers.Admin
{
    [AdminAuthorize(Roles = "Admin")]
    public class AspNetUsersController : AdminController
    {
        // GET: AspNetUsers
        private AspNetUsersBo _bo = new AspNetUsersBo();
        private AspNetRolesBo _boRoles = new AspNetRolesBo();
        private AspNetUserRolesBo _boUserRoles = new AspNetUserRolesBo();
        ApplicationDbContext context = new ApplicationDbContext();
        public ActionResult Index()
        {
            ViewBag.Status = Utils.GetStatusList();
            return View();
        }
        public ActionResult AjaxLoadList(GridFilterSetting<AspNetUsersEntity> gridFilterSetting, string keysearch, bool? status)
        {
            var pagininfo = new EtsPaging { RowStart = gridFilterSetting.iDisplayStart, PageSize = gridFilterSetting.iDisplayLength };
            var param = new AspNetUsersParam() { PagingInfo = pagininfo };
            var AspNetUsersFilter = new AspNetUsersFilter() { keysearch = keysearch, LockoutEnabled = status };
            param.AspNetUsersFilter = AspNetUsersFilter;
            _bo.Search(param);
            long count = pagininfo.RecordCount;

            //filter only developer can view developer user
            var myRoles = User.GetRoles();
            if (!myRoles.Contains("Developer"))
            {
                if (param.AspNetUsersEntitys != null && param.AspNetUsersEntitys.Any())
                {
                    param.AspNetUsersEntitys = param.AspNetUsersEntitys.Where(i => i.UserName != "dev").ToList();
                }
            }

            return Json(new { aaData = param.AspNetUsersEntitys, recordsTotal = count, recordsFiltered = count, amount = 0x2710 }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Create(string Id)
        {
            var param = new AspNetUsersParam();
            if (!string.IsNullOrEmpty(Id) && Id != "0")
            {
                param.AspNetUsersFilter = new AspNetUsersFilter() { Id = Id };
                _bo.GetById(param);
            }
            else
            {
                param.AspNetUser = new AspNetUser();
                ViewBag.Status = Utils.GetStatusList(0);
            }
            return PartialView(param);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult Create(AspNetUsersParam modelInput)
        {
            try
            {
                if (modelInput != null && modelInput.AspNetUser != null)
                {

                    if (!string.IsNullOrEmpty(modelInput.AspNetUser.Id))
                    {
                        var Imagelink = UploadHelper.UpLoadFile(Request.Files["AvatarInput"], "Upload/AspNetUsers/");
                        if (!string.IsNullOrEmpty(Imagelink))
                        {
                            modelInput.AspNetUser.Avatar = Imagelink;
                        }

                        modelInput.AspNetUser.Modified = DateTime.Now;
                        modelInput.AspNetUser.ModifiedBy = User.Identity.Name;
                        if (modelInput.AspNetUser.LockoutEnabled)
                        {
                            modelInput.AspNetUser.LockoutEndDateUtc = DateTime.Now.AddYears(1000);
                        }
                        else
                        {
                            modelInput.AspNetUser.LockoutEndDateUtc = DateTime.Now.AddYears(-1);
                        }
                        _bo.Update(modelInput);
                        return Json(new { isSuccess = true, mess = Resources.Message.Msg_UpdateSuccesfull }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                        var newUser = new ApplicationUser();
                        newUser.UserName = modelInput.AspNetUser.UserName;
                        newUser.Email = modelInput.AspNetUser.Email;
                        var chkUser = UserManager.Create(newUser, modelInput.AspNetUser.PasswordHash);
                        if (chkUser.Succeeded)
                        {
                            UserManager.AddToRole(newUser.Id, "DashBoard");
                            var param = new AspNetUsersParam();
                            if (!string.IsNullOrEmpty(newUser.Id) && newUser.Id != "0")
                            {
                                param.AspNetUsersFilter = new AspNetUsersFilter() { Id = newUser.Id };
                                _bo.GetById(param);
                                if (param.AspNetUser != null && !string.IsNullOrEmpty(param.AspNetUser.Id))
                                {
                                    param.AspNetUser.Created = DateTime.Now;
                                    param.AspNetUser.Modified = DateTime.Now;
                                    param.AspNetUser.CreatedBy = User.Identity.Name;
                                    param.AspNetUser.ModifiedBy = User.Identity.Name;
                                    param.AspNetUser.PhoneNumber = modelInput.AspNetUser.PhoneNumber;
                                    param.AspNetUser.Name = modelInput.AspNetUser.Name;
                                }
                                _bo.Update(param);
                            }
                            return Json(new { isSuccess = true, mess = Resources.Message.Msg_AddnewSuccesfull }, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            return Json(new { isSuccess = false, mess = chkUser.Errors }, JsonRequestBehavior.AllowGet);
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
        public ActionResult Delete(string Id)
        {
            var param = new AspNetUsersParam();
            param.AspNetUsersFilter = new AspNetUsersFilter() { Id = Id ?? "" };
            _bo.GetById(param);
            var item = param.AspNetUser;
            if (item == null)
            {
                ViewBag.Error = Resources.Message.Error_NotExit;
                return PartialView(new AspNetUser());
            }
            else
                return PartialView(item);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(AspNetUser obj)
        {
            try
            {
                var myRoles = User.GetRoles();
                if (!myRoles.Contains("Admin") || !myRoles.Contains("Developer"))
                {
                    return Json(new { isSuccess = false, mess = Resources.Message.Msg_DontHavePermission }, JsonRequestBehavior.AllowGet);
                }
                var list = new List<AspNetUser> { obj };
                var param = new AspNetUsersParam { AspNetUsers = list };
                _bo.Delete(param);
                return Json(new { isSuccess = true, mess = Resources.Message.Msg_DeleteSuccesfull }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { isSuccess = false, mess = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult LockUnlock(string id, string status)
        {
            try
            {
                var param = new AspNetUsersParam();
                if (!string.IsNullOrEmpty(id) && id != "0")
                {
                    param.AspNetUsersFilter = new AspNetUsersFilter() { Id = id };
                    _bo.GetById(param);

                    var item = param.AspNetUser;
                    if (item != null && !string.IsNullOrEmpty(item.Id))
                    {
                        switch (status)
                        {
                            case "lock":
                                {
                                    param.AspNetUser.LockoutEndDateUtc = DateTime.Now.AddYears(1000);
                                    param.AspNetUser.LockoutEnabled = true;
                                    _bo.ChangeStatus(param);
                                    return Json(new { isSuccess = true, mess = Resources.Message.Msg_ChangeStatusSuccesfull }, JsonRequestBehavior.AllowGet);
                                }
                            case "unlock":
                                {
                                    param.AspNetUser.LockoutEndDateUtc = DateTime.Now.AddYears(-1);
                                    param.AspNetUser.LockoutEnabled = false;
                                    _bo.ChangeStatus(param);
                                    return Json(new { isSuccess = true, mess = Resources.Message.Msg_ChangeStatusSuccesfull }, JsonRequestBehavior.AllowGet);
                                }
                        }
                    }

                }
                return Json(new { isSuccess = false, mess = Resources.Message.Msg_DeleteSuccesfull }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { isSuccess = false, mess = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult UserInRoles(string Id)
        {
            ViewBag.Id = Id;
            return View();
        }
        public ActionResult AjaxUserInRoles(GridFilterSetting<AspNetRolesEntity> gridFilterSetting, string keysearch, int? status, string uid)
        {
            var pagininfo = new EtsPaging { RowStart = gridFilterSetting.iDisplayStart, PageSize = gridFilterSetting.iDisplayLength };
            var param = new AspNetRolesParam() { PagingInfo = pagininfo };
            var AspNetRolesFilter = new AspNetRolesFilter() { keysearch = keysearch, Status = status };
            param.AspNetRolesFilter = AspNetRolesFilter;
            _boRoles.Search(param);
            long count = pagininfo.RecordCount;

            //filter only developer can view developer role
            var myRoles = User.GetRoles();
            if (!myRoles.Contains("Developer"))
            {
                if (param.AspNetRolesEntitys != null && param.AspNetRolesEntitys.Any())
                {
                    param.AspNetRolesEntitys = param.AspNetRolesEntitys.Where(i => i.Name != "Developer").ToList();
                }
            }
            //Get list roles by userId
            var paramUserRoles = new AspNetUserRolesParam() { AspNetUserRolesFilter = new AspNetUserRolesFilter() { UserId = uid } };
            _boUserRoles.Search(paramUserRoles);

            if (param.AspNetRolesEntitys != null && param.AspNetRolesEntitys.Any())
            {
                foreach (var item in param.AspNetRolesEntitys)
                {
                    item.HasRole = false;
                    item.UserId = uid;
                    var role = paramUserRoles.AspNetUserRolesEntitys.Where(i => i.RoleId == item.Id).FirstOrDefault();
                    if (role != null && !string.IsNullOrEmpty(role.RoleId))
                    {
                        item.HasRole = true;
                    }
                }
            }
            return Json(new { aaData = param.AspNetRolesEntitys, recordsTotal = count, recordsFiltered = count, amount = 0x2710 }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult CreateUIR(string roleName, string uid)
        {
            try
            {
                var myRoles = User.GetRoles();
                if (!myRoles.Contains("Admin") && !myRoles.Contains("Developer"))
                {
                    return Json(new { isSuccess = false, mess = Resources.Message.Msg_DontHavePermission }, JsonRequestBehavior.AllowGet);
                }
                var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                var result = UserManager.AddToRole(uid, roleName);
                if (result != null && result.Succeeded)
                {
                    return Json(new { isSuccess = true, mess = Resources.Message.Msg_AddRoleSuccesfull }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { isSuccess = false, mess = Resources.Message.Msg_UserAlreadyExistsRole }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { isSuccess = false, mess = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult DeleteUIR(string roleName, string uid)
        {
            try
            {
                var myRoles = User.GetRoles();
                if (!myRoles.Contains("Admin") && !myRoles.Contains("Developer"))
                {
                    return Json(new { isSuccess = false, mess = Resources.Message.Msg_DontHavePermission }, JsonRequestBehavior.AllowGet);
                }
                var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                var result = UserManager.RemoveFromRole(uid, roleName);
                if (result != null && result.Succeeded)
                {
                    return Json(new { isSuccess = true, mess = Resources.Message.Msg_DeleteRoleSuccesfull }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { isSuccess = false, mess = Resources.Message.Msg_UserIsNotInRoles }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { isSuccess = false, mess = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult ChangePass(string Id)
        {
            var param = new AspNetUsersParam();
            if (!string.IsNullOrEmpty(Id) && Id != "0")
            {
                param.AspNetUsersFilter = new AspNetUsersFilter() { Id = Id };
                _bo.GetById(param);
            }
            else
            {
                param.AspNetUser = new AspNetUser();
                ViewBag.Status = Utils.GetStatusList(0);
            }
            return PartialView(param);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult ChangePass(AspNetUsersParam modelInput)
        {
            try
            {
                if (modelInput != null && modelInput.AspNetUser != null)
                {

                    if (!string.IsNullOrEmpty(modelInput.AspNetUser.Id))
                    {
                        var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

                        UserManager.RemovePassword(modelInput.AspNetUser.Id.TrimEnd());
                        UserManager.AddPassword(modelInput.AspNetUser.Id.TrimEnd(), modelInput.AspNetUser.PasswordHash);

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