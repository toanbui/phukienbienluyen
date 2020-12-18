using Entities.Base;
using Entities.Entities;
using Entities.Filter;
using System.Collections.Generic;

namespace Entities.Param
{
    public class ArticleParam : BaseParam
    {
        public Article Article { get; set; }
        public List<Article> Articles { get; set; }
        public ArticleEntity ArticleEntity { get; set; }
        public List<ArticleEntity> ArticleEntitys { get; set; }
        public ArticleFilter ArticleFilter { get; set; }
        public List<TagInNewsEntity> TagInNewsEntitys { get; set; }
        public List<int> TagInNewsIds { get; set; }
    }
}
