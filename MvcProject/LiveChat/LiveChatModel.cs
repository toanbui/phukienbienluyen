using Entities.Param;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcProject.LiveChat
{
    public class LiveChatModel
    {
        public UserInLiveChatParam UserInLiveChatParam { get; set; }
        public LiveChatUser LiveChatUser { get; set; }
    }
}