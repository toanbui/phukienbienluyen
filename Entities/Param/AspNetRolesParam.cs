using Entities.Base;
using Entities.Entities;
using Entities.Filter;
using System.Collections.Generic;

namespace Entities.Param
{
    public class AspNetRolesParam : BaseParam
    {
        public AspNetRole AspNetRole { get; set; }
        public List<AspNetRole> AspNetRoles { get; set; }
        public AspNetRolesEntity AspNetRolesEntity { get; set; }
        public List<AspNetRolesEntity> AspNetRolesEntitys { get; set; }
        public AspNetRolesFilter AspNetRolesFilter { get; set; }
    }
}
