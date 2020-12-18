using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Filter
{
    public class PropsOfProductFilter
    {
        public int? Id { get; set; }
        public int? Status { get; set; }
        public int? PropsId { get; set; }
        public int? ProductId { get; set; }
        public string keysearch { get; set; } = "";
    }
}
