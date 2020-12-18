using Entities.Base;
using Entities.Entities;
using Entities.Filter;
using System.Collections.Generic;

namespace Entities.Param
{
    public class TagParam : BaseParam
    {
        public Tag Tag { get; set; }
        public List<Tag> Tags { get; set; }
        public TagEntity TagEntity { get; set; }
        public List<TagEntity> TagEntitys { get; set; }
        public TagFilter TagFilter { get; set; }
    }
}
