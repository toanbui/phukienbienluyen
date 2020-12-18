using Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Param.FrontEnd
{
    public class CheckOutModel
    {
        public ProductParam ProductParam { get; set; }
        public List<ProvinceEntity> ProvinceEntitys { get; set; }
        public List<DistrictEntity> DistrictEntitys { get; set; } = new List<DistrictEntity>();
        public CartOrder CartOrder { get; set; } = new CartOrder();
    }
}
