using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Filter
{
    public class UserInLiveChatFilter
    {
        public int? Id { get; set; }
        public int? Status { get; set; }
		public bool OrderDateDesc { get; set; }
        public string keysearch { get; set; } = "";
        public string UserId { get; set; } = "";
    }
}
