using Entities.Base;
using Entities.Entities;
using Entities.Filter;
using System.Collections.Generic;

namespace Entities.Param
{
    public class AdvertisingParam : BaseParam
    {
        public Advertising Advertising { get; set; }
        public List<Advertising> Advertisings { get; set; }
        public AdvertisingEntity AdvertisingEntity { get; set; }
        public List<AdvertisingEntity> AdvertisingEntitys { get; set; }
        public AdvertisingFilter AdvertisingFilter { get; set; }
    }
}
