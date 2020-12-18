using Entities.Base;
using Entities.Entities;
using Entities.Filter;
using System.Collections.Generic;

namespace Entities.Param
{
    public class ProductZoneParam : BaseParam
    {
        public ProductZone ProductZone { get; set; }
        public List<ProductZone> ProductZones { get; set; }
        public ProductZoneEntity ProductZoneEntity { get; set; }
        public List<ProductZoneEntity> ProductZoneEntitys { get; set; }
        public ProductZoneFilter ProductZoneFilter { get; set; }
    }
}
