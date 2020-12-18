using Entities.Base;
using Entities.Entities;
using Entities.Filter;
using System.Collections.Generic;

namespace Entities.Param
{
    public class PropsOfProductParam : BaseParam
    {
        public PropsOfProduct PropsOfProduct { get; set; }
        public List<PropsOfProduct> PropsOfProducts { get; set; }
        public PropsOfProductEntity PropsOfProductEntity { get; set; }
        public List<PropsOfProductEntity> PropsOfProductEntitys { get; set; }
        public PropsOfProductFilter PropsOfProductFilter { get; set; }
    }
}
