using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Entities
{
    public class UserInLiveChatEntity : UserInLiveChat 
    {
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Avatar { get; set; }
    }
}
