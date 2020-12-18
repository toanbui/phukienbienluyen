using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Filter
{
    public class AdvertisingFilter
    {
        public int? Id { get; set; }
        public int? Status { get; set; }
        public int? PositionId { get; set; }
        public string keysearch { get; set; } = "";
    }
}
