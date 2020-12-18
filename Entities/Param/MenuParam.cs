using Entities.Base;
using Entities.Entities;
using Entities.Filter;
using System.Collections.Generic;

namespace Entities.Param
{
    public class MenuParam : BaseParam
    {
        public Menu Menu { get; set; }
        public List<Menu> Menus { get; set; }
        public MenuEntity MenuEntity { get; set; }
        public List<MenuEntity> MenuEntitys { get; set; }
        public MenuFilter MenuFilter { get; set; }
    }
}
