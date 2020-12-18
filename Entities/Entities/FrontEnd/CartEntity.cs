using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Entities.FrontEnd
{
    public class CartEntity
    {
        public List<ProductInCart> ListCart { get; set; }
    }
    public class ProductInCart : Product
    {
        public int Number { get; set; }
        public string FirstAvatar { get; set; }
        public long TotalPrice { get; set; }
    }
}
