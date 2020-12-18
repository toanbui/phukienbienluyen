using Entities.Base;
using Entities.Entities;
using Entities.Filter;
using System.Collections.Generic;

namespace Entities.Param
{
    public class AspNetUsersParam : BaseParam
    {
        public AspNetUser AspNetUser { get; set; }
        public List<AspNetUser> AspNetUsers { get; set; }
        public AspNetUsersEntity AspNetUsersEntity { get; set; }
        public List<AspNetUsersEntity> AspNetUsersEntitys { get; set; }
        public AspNetUsersFilter AspNetUsersFilter { get; set; }
    }
}
