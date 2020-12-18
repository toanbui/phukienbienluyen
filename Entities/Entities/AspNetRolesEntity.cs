using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Entities
{
    public class AspNetRolesEntity : AspNetRole 
    {
        public bool HasRole { get; set; }
        public string UserId { get; set; }
    }
}
