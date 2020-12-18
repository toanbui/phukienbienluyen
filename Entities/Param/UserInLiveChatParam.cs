using Entities.Base;
using Entities.Entities;
using Entities.Filter;
using System.Collections.Generic;

namespace Entities.Param
{
    public class UserInLiveChatParam : BaseParam
    {
        public UserInLiveChat UserInLiveChat { get; set; }
        public List<UserInLiveChat> UserInLiveChats { get; set; }
        public UserInLiveChatEntity UserInLiveChatEntity { get; set; }
        public List<UserInLiveChatEntity> UserInLiveChatEntitys { get; set; }
        public UserInLiveChatFilter UserInLiveChatFilter { get; set; }
    }
}
