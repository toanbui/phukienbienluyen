using System.Collections.Generic;

namespace Entities.Configs
{
    public class RulesConfig
    {
        public List<Rules> Rules { get; set; }
    }

    public class Rules
    {
        public string Url { get; set; }
        public string UrlReplace { get; set; }
        public string Action { get; set; }
        public string Controller { get; set; }
        public List<QueryParam> Querys { get; set; }
    }
    public class QueryParam
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
