using Entities.Base;
using Entities.Entities;
using Entities.Entities.FrontEnd;
using Entities.Filter;
using System.Collections.Generic;

namespace Entities.Param
{
    public class ProductParam : BaseParam
    {
        public Product Product { get; set; }
        public List<Product> Products { get; set; }
        public ProductEntity ProductEntity { get; set; }
        public List<ProductEntity> ProductEntitys { get; set; }
        public ProductFilter ProductFilter { get; set; }
        public List<PropsOfProductEntity> PropsOfProductEntitys { get; set; }
        public List<int> PropsOfProductIds { get; set; }
        public List<ProductInCart> ProductInCarts { get; set; }
        public int TotalProductInCart { get; set; }
        public long TotalPriceInCart { get; set; }
    }
}
