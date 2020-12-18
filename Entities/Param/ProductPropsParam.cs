using Entities.Base;
using Entities.Entities;
using Entities.Filter;
using System.Collections.Generic;

namespace Entities.Param
{
    public class ProductPropsParam : BaseParam
    {
        public ProductProp ProductProps { get; set; }
        public List<ProductProp> ProductPropss { get; set; }
        public ProductPropsEntity ProductPropsEntity { get; set; }
        public List<ProductPropsEntity> ProductPropsEntitys { get; set; }
        public ProductPropsFilter ProductPropsFilter { get; set; }
    }
}
