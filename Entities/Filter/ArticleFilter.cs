using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Filter
{
    public class ArticleFilter
    {
        public int? Id { get; set; }
        public long? NewsId { get; set; }
        public int? Status { get; set; }
        public bool OrderDateDesc { get; set; }
        public bool? Invisible { get; set; }
        public string keysearch { get; set; } = "";
    }
}
