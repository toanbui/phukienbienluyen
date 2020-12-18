using DAO.Base;
using Entities.Entities;
using Entities.Param;
using System.Linq;

namespace DAO
{
    public class AspNetUserRolesDao : DaoBase
    {
        #region Action
        public string Insert(AspNetUserRole item)
        {
            using (var dbContext = DaoContext())
            {
                dbContext.AspNetUserRoles.InsertOnSubmit(item);
                dbContext.SubmitChanges();
                return item.RoleId;
            }
        }
        public void Update(AspNetUserRole item)
        {
            using (var dbContext = DaoContext())
            {
                var dbItem = dbContext.AspNetUserRoles.FirstOrDefault(sitem => sitem.RoleId == item.RoleId);
                if (dbItem != null)
                {
                    dbItem.UserId = item.UserId;
                    dbItem.RoleId = item.RoleId;

                    dbContext.SubmitChanges();
                }
            }
        }

        public bool Delete(AspNetUserRole item)
        {
            using (var dbContext = DaoContext())
            {
                var dbItem = dbContext.AspNetUserRoles.FirstOrDefault(en => en.RoleId == item.RoleId);

                if (dbItem != null)
                {
                    dbContext.AspNetUserRoles.DeleteOnSubmit(dbItem);
                    dbContext.SubmitChanges();
                }
            }
            return true;
        }
        //public bool DeleteUpdate(NewsEntity item)
        //{
        //    using (var dbContext = DaoContext())
        //    {
        //        var dbItem = dbContext.News.FirstOrDefault(en => en.NewsId == item.NewsId);
        //        if (dbItem != null)
        //        {
        //            dbContext.SubmitChanges();
        //        }
        //    }
        //    return true;
        //}
        #endregion
        #region Query
        public void Search(AspNetUserRolesParam param)
        {
            var filter = param.AspNetUserRolesFilter;
            using (var dbContext = new CoreDataContext(ConnectionString))
            {
                var query = from n in dbContext.AspNetUserRoles
                            where (string.IsNullOrEmpty(filter.RoleId) || filter.RoleId == n.RoleId)
                            && (string.IsNullOrEmpty(filter.UserId) || filter.UserId == n.UserId)
                            select new AspNetUserRolesEntity
                            {
                                UserId = n.UserId,
                                RoleId = n.RoleId,
                            };

                if (param.PagingInfo != null)
                {
                    param.PagingInfo.RecordCount = query.Count();
                    param.AspNetUserRolesEntitys = query.Skip(param.PagingInfo.RowStart).Take(param.PagingInfo.PageSize).ToList();
                }
                else
                {
                    param.AspNetUserRolesEntitys = query.ToList();
                }
            }
        }
        public void GetById(AspNetUserRolesParam param)
        {
            Search(param);
            if (param.AspNetUserRolesEntitys != null && param.AspNetUserRolesEntitys.Any())
            {
                param.AspNetUserRoles = param.AspNetUserRolesEntitys.FirstOrDefault();
            }
        }
        #endregion
    }
}
