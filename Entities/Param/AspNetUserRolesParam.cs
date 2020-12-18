using Entities.Base;
using Entities.Entities;
using Entities.Filter;
using System.Collections.Generic;

namespace Entities.Param
{
    public class AspNetUserRolesParam : BaseParam
    {
        public AspNetUserRole AspNetUserRoles { get; set; }
        public List<AspNetUserRole> AspNetUserRoless { get; set; }
        public AspNetUserRolesEntity AspNetUserRolesEntity { get; set; }
        public List<AspNetUserRolesEntity> AspNetUserRolesEntitys { get; set; }
        public AspNetUserRolesFilter AspNetUserRolesFilter { get; set; }
    }
}
