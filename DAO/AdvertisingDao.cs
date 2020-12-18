using DAO.Base;
using Entities.Entities;
using Entities.Param;
using System.Linq;

namespace DAO
{
    public class AdvertisingDao : DaoBase
    {
        #region Action
        public int Insert(Advertising item)
        {
            using (var dbContext = DaoContext())
            {
                dbContext.Advertisings.InsertOnSubmit(item);
                dbContext.SubmitChanges();
                return item.Id;
            }
        }
        public void Update(Advertising item)
        {
            using (var dbContext = DaoContext())
            {
                var dbItem = dbContext.Advertisings.FirstOrDefault(sitem => sitem.Id == item.Id);
                if (dbItem != null)
                {
                    dbItem.Id = item.Id;
                    dbItem.PositionId = item.PositionId;
                    dbItem.Name = item.Name;
                    dbItem.Url = item.Url;
                    dbItem.Description = item.Description;
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

        public bool Delete(Advertising item)
        {
            using (var dbContext = DaoContext())
            {
                var dbItem = dbContext.Advertisings.FirstOrDefault(en => en.Id == item.Id);

                if (dbItem != null)
                {
                    dbContext.Advertisings.DeleteOnSubmit(dbItem);
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
        public void Search(AdvertisingParam param)
        {
            var filter = param.AdvertisingFilter;
            using (var dbContext = new CoreDataContext(ConnectionString))
            {
                var query = from n in dbContext.Advertisings
                            where (filter.Id.HasValue == false || n.Id == filter.Id)
                            && (string.IsNullOrEmpty(filter.keysearch) || n.Name.ToLower().Contains(filter.keysearch.ToLower()))
                            && (filter.Status.HasValue == false || filter.Status == n.Status)
                            && (filter.PositionId.HasValue == false || filter.PositionId == n.PositionId)
                            select new AdvertisingEntity
                            {
                                Id = n.Id,
                                PositionId = n.PositionId,
                                Name = n.Name,
                                Url = n.Url,
                                Description = n.Description,
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
                    param.AdvertisingEntitys = query.Skip(param.PagingInfo.RowStart).Take(param.PagingInfo.PageSize).ToList();
                }
                else
                {
                    param.AdvertisingEntitys = query.ToList();
                }
            }
        }
        public void GetById(AdvertisingParam param)
        {
            Search(param);
            if (param.AdvertisingEntitys != null && param.AdvertisingEntitys.Any())
            {
                param.Advertising = param.AdvertisingEntitys.FirstOrDefault();
            }
        }
        #endregion
    }
}
