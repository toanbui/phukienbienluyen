using DAO.Base;
using Entities.Entities;
using Entities.Param;
using System.Linq;

namespace DAO
{
    public class AspNetRolesDao : DaoBase
    {
        #region Action
        public string Insert(AspNetRole item)
        {
            using (var dbContext = DaoContext())
            {
                dbContext.AspNetRoles.InsertOnSubmit(item);
                dbContext.SubmitChanges();
                return item.Id;
            }
        }
        public void Update(AspNetRole item)
        {
            using (var dbContext = DaoContext())
            {
                var dbItem = dbContext.AspNetRoles.FirstOrDefault(sitem => sitem.Id == item.Id);
                if (dbItem != null)
                {
                    dbItem.Id = item.Id;
                    dbItem.Name = item.Name;
                    dbContext.SubmitChanges();
                }
            }
        }

        public bool Delete(AspNetRole item)
        {
            using (var dbContext = DaoContext())
            {
                var dbItem = dbContext.AspNetRoles.FirstOrDefault(en => en.Id == item.Id);

                if (dbItem != null)
                {
                    dbContext.AspNetRoles.DeleteOnSubmit(dbItem);
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
        public void Search(AspNetRolesParam param)
        {
            var filter = param.AspNetRolesFilter;
            using (var dbContext = new CoreDataContext(ConnectionString))
            {
                var query = from n in dbContext.AspNetRoles
                            where(string.IsNullOrEmpty(filter.keysearch) || n.Name.ToLower().Contains(filter.keysearch.ToLower()))
                            select new AspNetRolesEntity
                            {
                                Id = n.Id,
                                Name = n.Name,
                            };

                if (param.PagingInfo != null)
                {
                    param.PagingInfo.RecordCount = query.Count();
                    param.AspNetRolesEntitys = query.Skip(param.PagingInfo.RowStart).Take(param.PagingInfo.PageSize).ToList();
                }
                else
                {
                    param.AspNetRolesEntitys = query.ToList();
                }
            }
        }
        public void GetById(AspNetRolesParam param)
        {
            Search(param);
            if (param.AspNetRolesEntitys != null && param.AspNetRolesEntitys.Any())
            {
                param.AspNetRole = param.AspNetRolesEntitys.FirstOrDefault();
            }
        }
        #endregion
    }
}
