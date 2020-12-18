using Entities.Base;
using Entities.Entities;
using Entities.Filter;
using System.Collections.Generic;

namespace Entities.Param
{
    public class ProvinceParam : BaseParam
    {
        public Province Province { get; set; }
        public List<Province> Provinces { get; set; }
        public ProvinceEntity ProvinceEntity { get; set; }
        public List<ProvinceEntity> ProvinceEntitys { get; set; }
        public ProvinceFilter ProvinceFilter { get; set; }
    }
}
