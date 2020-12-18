using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Filter
{
    public class TagInNewsFilter
    {
        public int? Id { get; set; }
        public long? NewsId { get; set; }
        public int? TagId { get; set; }
    }
}
