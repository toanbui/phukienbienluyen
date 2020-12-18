using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Entities
{
    public class ProductEntity : Product 
    {
        public List<string> ListAvatar { get; set; }
        public string FirstAvatar { get; set; }
    }
}
