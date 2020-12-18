using BO;
using Entities.Param;
using Entities.Filter;
using MvcProject.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Utilities;
using System.Web.Mvc;
using MvcProject.LiveChat;
using Utilities.Helper;
using Entities.Base;

namespace MvcProject.Controllers
{
    public class LiveChatController : FrontController
    {
        // GET: LiveChat
        public ActionResult Index()
        {
            var model = new LiveChat.LiveChatModel();

            var bo = new UserInLiveChatBo();
            var param = new UserInLiveChatParam();
            var UserInLiveChatFilter = new UserInLiveChatFilter() { Status = (int)Utilities.Constants.RecordStatus.Published };
            param.UserInLiveChatFilter = UserInLiveChatFilter;
            bo.Search(param);

            model.UserInLiveChatParam = param;

            return PartialView(model);
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Register(LiveChatModel modelInput)
        {
            try

            {
                if (modelInput != null)
                {
                    modelInput.LiveChatUser.Avatar = "/api/thumb?w=500&h=500&path=/Image/Admin/default-avatar.jpg";
                    Utilities.Helper.CookieStore.SetCookie(Utilities.Constants.LiveChatGuest, modelInput.LiveChatUser.JsonSerialize(), TimeSpan.FromDays(1000));
                    return Json(new { isSuccess = true, mess = "OK", data = modelInput.LiveChatUser.JsonSerialize() }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { isSuccess = true, mess = "Có lỗi xảy ra", data = modelInput }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { isSuccess = false, mess = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult AttachFile(string hdGroup)
        {
            try
            {
                var fileName = Request.Files["fileInput"].FileName;
                var filePath = UploadHelper.UpLoadFile(Request.Files["fileInput"], "Upload/LiveChat/");
                return Json(new { isSuccess = true, mess = "Thành công", data = new { fileName , filePath , group = hdGroup } }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { isSuccess = false, mess = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult History(string group , int pageIndex, int pageSize)
        {
            try
            {
                var PageInfo = new EtsPaging { RowStart = (pageIndex - 1) * pageSize, PageSize = pageSize };
                var Bo = new LiveChatHistoryBo();
                var Param = new LiveChatHistoryParam() { PagingInfo = PageInfo, LiveChatHistoryFilter =  new LiveChatHistoryFilter() { groupName = group } };
                Bo.Search(Param);
                if (Param.LiveChatHistoryEntitys != null && Param.LiveChatHistoryEntitys.Any())
                {
                    var data = from i in Param.LiveChatHistoryEntitys
                               select new
                               {
                                   recievedInfo = new LiveChatUser
                                   {
                                       Name = i.Recieved,
                                       Avatar = i.RecievedAvatar,
                                   },
                                   content = i.Message,
                                   @group,
                                   time = i.Created,
                                   sender = new LiveChatUser
                                   {
                                       Name = i.SenderIsSupport == true ? i.Sender : i.SenderName,
                                       SupportName = i.SenderIsSupport == true ? i.SenderName : i.Sender,
                                       IsSupport = i.SenderIsSupport ?? false,
                                       Email = i.SenderEmail,
                                       PhoneNumber = i.SenderPhone,
                                       Avatar = i.SenderAvatar,
                                   }
                               };
                    return Json(new { isSuccess = true, mess = "Thành công", data = data }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { isSuccess = false, mess = "Có lỗi xảy ra", data = ""}, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { isSuccess = false, mess = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}