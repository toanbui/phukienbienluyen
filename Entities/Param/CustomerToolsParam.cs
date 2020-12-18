using Entities.Base;
using Entities.Entities;
using Entities.Filter;
using System.Collections.Generic;

namespace Entities.Param
{
    public class CustomerToolsParam : BaseParam
    {
        public CustomerTool CustomerTool { get; set; }
        public List<CustomerTool> CustomerTools { get; set; }
        public CustomerToolsEntity CustomerToolsEntity { get; set; }
        public List<CustomerToolsEntity> CustomerToolsEntitys { get; set; }
        public CustomerToolsFilter CustomerToolsFilter { get; set; }
    }
}
