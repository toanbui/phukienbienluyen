using Entities.Base;
using Entities.Entities;
using Entities.Filter;
using System.Collections.Generic;

namespace Entities.Param
{
    public class TagInNewsParam : BaseParam
    {
        public TagInNew TagInNews { get; set; }
        public List<TagInNew> TagInNewss { get; set; }
        public TagInNewsEntity TagInNewsEntity { get; set; }
        public List<TagInNewsEntity> TagInNewsEntitys { get; set; }
        public TagInNewsFilter TagInNewsFilter { get; set; }
    }
}
