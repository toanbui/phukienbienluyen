using DAO.Base;
using Entities.Entities;
using Entities.Param;
using System.Linq;
using Utilities;

namespace DAO
{
    public class ProductZoneDao : DaoBase
    {
        #region Action
        public int Insert(ProductZone item)
        {
            using (var dbContext = DaoContext())
            {
                dbContext.ProductZones.InsertOnSubmit(item);
                dbContext.SubmitChanges();
                return item.Id;
            }
        }
        public void Update(ProductZone item)
        {
            using (var dbContext = DaoContext())
            {
                var dbItem = dbContext.ProductZones.FirstOrDefault(sitem => sitem.Id == item.Id);
                if (dbItem != null)
                {
                    dbItem.Name = item.Name;
                    dbItem.ParentId = item.ParentId;
                    dbItem.Invisible = item.Invisible;
                    dbItem.Order = item.Order;
                    dbItem.Status = item.Status;

                    dbContext.SubmitChanges();
                }
            }
        }

        public bool Delete(ProductZone item)
        {
            using (var dbContext = DaoContext())
            {
                var dbItem = dbContext.ProductZones.FirstOrDefault(en => en.Id == item.Id);

                if (dbItem != null)
                {
                    dbContext.ProductZones.DeleteOnSubmit(dbItem);
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
        public void Search(ProductZoneParam param)
        {
            var filter = param.ProductZoneFilter;
            using (var dbContext = new CoreDataContext(ConnectionString))
            {
                var query = from n in dbContext.ProductZones
                            where (filter.Id.HasValue == false || n.Id == filter.Id)
                            && (string.IsNullOrEmpty(filter.keysearch) || n.Name.ToLower().Contains(filter.keysearch.ToLower()))
                            && (filter.Status.HasValue == false || filter.Status == n.Status)
                            && (filter.ParentId.HasValue == false || filter.ParentId == n.ParentId)
                            select new ProductZoneEntity
                            {
                                Id = n.Id,
                                Name = n.Name,
                                ParentId = n.ParentId,
                                Invisible = n.Invisible,
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
                    param.ProductZoneEntitys = query.Skip(param.PagingInfo.RowStart).Take(param.PagingInfo.PageSize).ToList();
                }
                else
                {
                    param.ProductZoneEntitys = query.ToList();
                }
            }
        }
        public void GetById(ProductZoneParam param)
        {
            Search(param);
            if (param.ProductZoneEntitys != null && param.ProductZoneEntitys.Any())
            {
                param.ProductZone = param.ProductZoneEntitys.FirstOrDefault();
            }
        }
        #endregion
    }
}
