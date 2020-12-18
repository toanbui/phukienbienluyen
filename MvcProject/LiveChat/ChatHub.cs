using BO;
using Entities.Entities;
using Entities.Param;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Utilities;

namespace MvcProject.LiveChat
{
    public class ChatHub : Hub
    {
        private string ChatWindowFormat = @"<div class=""box box-primary box-solid direct-chat direct-chat-primary"" box-chatwindow data-group-name=""{0}"" data-attach="""" data-attach-fname="""" data-pageindex=""1"" data-pagesize=""10"" data-history-init=""false""> <div class=""box-header""><i class=""fa fa-paper-plane"" aria-hidden=""true""></i> <h3 class=""box-title"" chat-with></h3> <div class=""box-tools pull-right""> <button class=""btn btn-box-tool"" data-widget=""collapse""><i class=""fa fa-minus""></i></button> <button class=""btn btn-box-tool"" data-toggle=""tooltip"" title=""Contacts"" data-widget=""chat-pane-toggle""> <i class=""fa fa-comments""></i> </button> <button class=""btn btn-box-tool"" data-widget=""remove""><i class=""fa fa-times""></i></button> </div> </div><!-- /.box-header --> <div class=""box-body""> <!-- Conversations are loaded here --> <div class=""direct-chat-messages"" message-container> </div><!--/.direct-chat-messages--> </div><!-- /.box-body --> <div class=""box-footer""> <div class=""input-group""> <input type=""text"" input-message name=""message"" placeholder=""Nhập nội dung ..."" class=""form-control""> <span class=""input-group-btn""> <button type=""button"" class=""btn btn-primary btn-flat"">Gửi</button> </span> </div> </div><!-- /.box-footer--> </div>";
        private readonly static ConnectionMapping<LiveChatUser> _connections = new ConnectionMapping<LiveChatUser>();
        public override Task OnConnected()
        {
            var user = new LiveChatUser(Context.User.Identity.Name);
            if (!string.IsNullOrEmpty(user.Name))
                _connections.Add(user, Context.ConnectionId);

            Clients.All.SetOnline(_connections.DictConnection.JsonSerialize());
            Clients.Client(Context.ConnectionId).SetInfo(user.JsonSerialize());
            return base.OnConnected();
        }
        public override Task OnDisconnected(bool stopCalled)
        {
            var user = new LiveChatUser(Context.User.Identity.Name);
            if (!string.IsNullOrEmpty(user.Name))
                _connections.Remove(user, Context.ConnectionId);
            else
                _connections.RemoveByConnection(Context.ConnectionId); //when guest close tab

            Clients.All.SetOnline(_connections.DictConnection.JsonSerialize());
            return base.OnDisconnected(stopCalled);
        }

        public override Task OnReconnected()
        {
            var user = new LiveChatUser(Context.User.Identity.Name);

            if (!_connections.GetConnections(user).Contains(Context.ConnectionId))
            {
                _connections.Add(user, Context.ConnectionId);
            }

            return base.OnReconnected();
        }
        public void GuestConnected(LiveChatUser user)
        {
            if (user != null && !string.IsNullOrEmpty(user.Name) && !string.IsNullOrEmpty(user.Email))
            {
                if (!_connections.GetConnections(user).Contains(Context.ConnectionId))
                {
                    _connections.Add(user, Context.ConnectionId);
                }
            }
        }
        public void createwindow(LiveChatUser sender, string recieved)
        {
            //Add connection for guest
            GuestConnected(sender);

            var senderConnnect = _connections.GetConnectionsByUnsignName(sender.UnsignName);
            var recievedConnnect = _connections.GetConnectionsByUnsignName(recieved);
            var group = CreateGroupName(sender, recieved);

            AddToGroup(senderConnnect, group);
            AddToGroup(recievedConnnect, group);
            Clients.Group(group).CreateWindow(new
            {
                window = string.Format(ChatWindowFormat, group),
                sender = sender.JsonSerialize(),
                group,
                recieved = _connections.GetUserByUnsignName(recieved).JsonSerialize()
            });
        }
        public void chatprivate(string content, string file, string fileName, string group, LiveChatUser sender, string recieved)
        {
            var senderConnnect = _connections.GetConnectionsByUnsignName(sender.UnsignName);
            var recievedConnnect = _connections.GetConnectionsByUnsignName(recieved);

            //rejoin group
            AddToGroup(senderConnnect, group);
            AddToGroup(recievedConnnect, group);

            //get recievedInfo
            var recievedInfo = _connections.GetUserByUnsignName(recieved);

            var time = DateTime.Now;
            var original = content;

            if (!string.IsNullOrEmpty(file) && !string.IsNullOrEmpty(content))
            {
                content += string.Format("<br><img src='{0}' style='width : 222px;display : block'/>", file.ChangeThumbSize(300, 0));
            }
            else if (!string.IsNullOrEmpty(file))
            {
                content = string.Format("<img src='{0}' style='width : 222px;display : block'/>", file.ChangeThumbSize(300, 0));
            }

            Clients.Group(group).RecivedMessage(new
            {
                content,
                group,
                sender,
                recievedInfo,
                window = string.Format(ChatWindowFormat, group),
                time
            });

            //Insert to database
            var Bo = new LiveChatHistoryBo();
            var Param = new LiveChatHistoryParam()
            {
                LiveChatHistory = new LiveChatHistory()
                {
                    Sender = sender.UnsignName,
                    SenderName = sender.IsSupport ? sender.SupportName : sender.Name,
                    SenderIsSupport = sender.IsSupport,
                    SenderPhone = sender.PhoneNumber,
                    SenderEmail = sender.Email,
                    SenderAvatar = sender.Avatar,
                    Recieved = recievedInfo.UnsignName,
                    RecievedName = recievedInfo.IsSupport ? recievedInfo.SupportName : recievedInfo.Name,
                    RecievedIsSupport = recievedInfo.IsSupport,
                    RecievedPhone = recievedInfo.PhoneNumber,
                    RecievedEmail = recievedInfo.Email,
                    RecievedAvatar = recievedInfo.Avatar,
                    Created = time,
                    FileName = fileName,
                    FilePath = file,
                    Message = original,
                    GroupName = group
                }
            };
            Bo.Insert(Param);
        }
        public void typing(string group, LiveChatUser sender, string recieved)
        {
            Clients.OthersInGroup(group).Typing(new
            {
                group,
                sender,
                recieved
            });
        }
        private string CreateGroupName(LiveChatUser sender, string recieved)
        {
            List<string> lstId = new List<string>() { sender.UnsignName, sender.Email.ToKoDauAndGach(), recieved };
            lstId.Sort();
            return string.Join<string>("", lstId).Replace("@", "").Replace(".", "");
        }
        private void AddToGroup(IEnumerable<string> connects, string groupName)
        {
            foreach (var connect in connects)
            {
                Groups.Add(connect, groupName);
            }
        }
    }
}