using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Filter
{
    public class AspNetUsersFilter
    {
        public string Id { get; set; }
        public int? Status { get; set; }
        public bool? LockoutEnabled { get; set; }
        public string keysearch { get; set; } = "";
        public string UserName { get; set; } = "";
    }
}
