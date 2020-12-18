using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Filter
{
    public class ProductZoneFilter
    {
        public int? Id { get; set; }
        public int? Status { get; set; }
        public int? ParentId { get; set; }
        public string keysearch { get; set; } = "";
    }
}
