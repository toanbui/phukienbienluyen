using DAO.Base;
using Entities.Entities;
using Entities.Param;
using System.Linq;

namespace DAO
{
    public class SysMenuDao : DaoBase
    {
        #region Action
        public int Insert(SysMenu item)
        {
            using (var dbContext = DaoContext())
            {
                dbContext.SysMenus.InsertOnSubmit(item);
                dbContext.SubmitChanges();
                return item.Id;
            }
        }
        public void Update(SysMenu item)
        {
            using (var dbContext = DaoContext())
            {
                var dbItem = dbContext.SysMenus.FirstOrDefault(sitem => sitem.Id == item.Id);
                if (dbItem != null)
                {
                    dbItem.Id = item.Id;
                    dbItem.Name = item.Name;
                    dbItem.Controller = item.Controller;
                    dbItem.Icon = item.Icon;
                    dbItem.Created = item.Created;
                    dbItem.CreatedBy = item.CreatedBy;
                    dbItem.Modified = item.Modified;
                    dbItem.ModifiedBy = item.ModifiedBy;
                    dbItem.Status = item.Status;
                    dbContext.SubmitChanges();
                }
            }
        }

        public bool Delete(SysMenu item)
        {
            using (var dbContext = DaoContext())
            {
                var dbItem = dbContext.SysMenus.FirstOrDefault(en => en.Id == item.Id);

                if (dbItem != null)
                {
                    dbContext.SysMenus.DeleteOnSubmit(dbItem);
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
        public void Search(SysMenuParam param)
        {
            var filter = param.SysMenuFilter;
            using (var dbContext = new CoreDataContext(ConnectionString))
            {
                var query = from n in dbContext.SysMenus
                            where (filter.Id.HasValue == false || n.Id == filter.Id)
                            && (string.IsNullOrEmpty(filter.keysearch) || n.Name.ToLower().Contains(filter.keysearch.ToLower()))
                            && (filter.Status.HasValue  == false || filter.Status == n.Status)
                            select new SysMenuEntity
                            {
                                Id = n.Id,
                                Name = n.Name,
                                Controller = n.Controller,
                                Icon = n.Icon,
                                Created = n.Created,
                                CreatedBy = n.CreatedBy,
                                Modified = n.Modified,
                                ModifiedBy = n.ModifiedBy,
                                Status = n.Status
                            };

                if (param.PagingInfo != null)
                {
                    param.PagingInfo.RecordCount = query.Count();
                    param.SysMenuEntitys = query.Skip(param.PagingInfo.RowStart).Take(param.PagingInfo.PageSize).ToList();
                }
                else
                {
                    param.SysMenuEntitys = query.ToList();
                }
            }
        }
        public void GetById(SysMenuParam param)
        {
            Search(param);
            if (param.SysMenuEntitys != null && param.SysMenuEntitys.Any())
            {
                param.SysMenu = param.SysMenuEntitys.FirstOrDefault();
            }
        }
        #endregion
    }
}
