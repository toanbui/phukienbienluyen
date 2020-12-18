using Entities.Base;
using Entities.Entities;
using Entities.Entities.FrontEnd;
using Entities.Filter;
using System.Collections.Generic;

namespace Entities.Param
{
    public class CartOrderParam : BaseParam
    {
        public CartOrder CartOrder { get; set; }
        public List<CartOrder> CartOrders { get; set; }
        public CartOrderEntity CartOrderEntity { get; set; }
        public List<CartOrderEntity> CartOrderEntitys { get; set; }
        public CartOrderFilter CartOrderFilter { get; set; }

        public List<ProductInCart> ProductInCarts { get; set; }
    }
}
