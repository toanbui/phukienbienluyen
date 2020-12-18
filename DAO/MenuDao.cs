using DAO.Base;
using Entities.Entities;
using Entities.Param;
using System.Linq;

namespace DAO
{
    public class MenuDao : DaoBase
    {
        #region Action
        public int Insert(Menu item)
        {
            using (var dbContext = DaoContext())
            {
                dbContext.Menus.InsertOnSubmit(item);
                dbContext.SubmitChanges();
                return item.Id;
            }
        }
        public void Update(Menu item)
        {
            using (var dbContext = DaoContext())
            {
                var dbItem = dbContext.Menus.FirstOrDefault(sitem => sitem.Id == item.Id);
                if (dbItem != null)
                {
                    dbItem.Id = item.Id;
dbItem.Name = item.Name;
dbItem.Url = item.Url;
dbItem.ParentId = item.ParentId;
dbItem.Avatar = item.Avatar;
dbItem.Created = item.Created;
dbItem.CreatedBy = item.CreatedBy;
dbItem.Modified = item.Modified;
dbItem.ModifiedBy = item.ModifiedBy;
dbItem.Order = item.Order;
dbItem.Status = item.Status;

					dbContext.SubmitChanges();
                }
            }
        }

        public bool Delete(Menu item)
        {
            using (var dbContext = DaoContext())
            {
                var dbItem = dbContext.Menus.FirstOrDefault(en => en.Id == item.Id);

                if (dbItem != null)
                {
                    dbContext.Menus.DeleteOnSubmit(dbItem);
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
        public void Search(MenuParam param)
        {
            var filter = param.MenuFilter;
            using (var dbContext = new CoreDataContext(ConnectionString))
            {
                var query = from n in dbContext.Menus
                            where (filter.Id.HasValue == false || n.Id == filter.Id)
                            && (string.IsNullOrEmpty(filter.keysearch) || n.Name.ToLower().Contains(filter.keysearch.ToLower()))
                            && (filter.Status.HasValue  == false || filter.Status == n.Status)
                            select new MenuEntity
                            {
                                Id = n.Id,
Name = n.Name,
Url = n.Url,
ParentId = n.ParentId,
Avatar = n.Avatar,
Created = n.Created,
CreatedBy = n.CreatedBy,
Modified = n.Modified,
ModifiedBy = n.ModifiedBy,
Order = n.Order,
Status = n.Status,

                            };

                if (param.PagingInfo != null)
                {
                    param.PagingInfo.RecordCount = query.Count();
                    param.MenuEntitys = query.Skip(param.PagingInfo.RowStart).Take(param.PagingInfo.PageSize).ToList();
                }
                else
                {
                    param.MenuEntitys = query.ToList();
                }
            }
        }
        public void GetById(MenuParam param)
        {
            Search(param);
            if (param.MenuEntitys != null && param.MenuEntitys.Any())
            {
                param.Menu = param.MenuEntitys.FirstOrDefault();
            }
        }
        #endregion
    }
}
