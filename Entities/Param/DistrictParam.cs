using Entities.Base;
using Entities.Entities;
using Entities.Filter;
using System.Collections.Generic;

namespace Entities.Param
{
    public class DistrictParam : BaseParam
    {
        public District District { get; set; }
        public List<District> Districts { get; set; }
        public DistrictEntity DistrictEntity { get; set; }
        public List<DistrictEntity> DistrictEntitys { get; set; }
        public DistrictFilter DistrictFilter { get; set; }
    }
}
