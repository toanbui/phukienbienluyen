using Entities.Base;
using Entities.Entities;
using Entities.Filter;
using System.Collections.Generic;

namespace Entities.Param
{
    public class SysMenuParam : BaseParam
    {
        public SysMenu SysMenu { get; set; }
        public List<SysMenu> SysMenus { get; set; }
        public SysMenuEntity SysMenuEntity { get; set; }
        public List<SysMenuEntity> SysMenuEntitys { get; set; }
        public SysMenuFilter SysMenuFilter { get; set; }
    }
}
