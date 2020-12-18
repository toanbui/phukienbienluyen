using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Filter
{
    public class AspNetUserRolesFilter
    {
        public string UserId { get; set; }
        public string RoleId { get; set; }
        public string keysearch { get; set; } = "";
    }
}
