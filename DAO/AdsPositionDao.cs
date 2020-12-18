using DAO.Base;
using Entities.Entities;
using Entities.Param;
using System.Linq;

namespace DAO
{
    public class AdsPositionDao : DaoBase
    {
        #region Action
        public int Insert(AdsPosition item)
        {
            using (var dbContext = DaoContext())
            {
                dbContext.AdsPositions.InsertOnSubmit(item);
                dbContext.SubmitChanges();
                return item.Id;
            }
        }
        public void Update(AdsPosition item)
        {
            using (var dbContext = DaoContext())
            {
                var dbItem = dbContext.AdsPositions.FirstOrDefault(sitem => sitem.Id == item.Id);
                if (dbItem != null)
                {
                    dbItem.Id = item.Id;
                    dbItem.Name = item.Name;
                    dbItem.PositionName = item.PositionName;
                    dbItem.Status = item.Status;

                    dbContext.SubmitChanges();
                }
            }
        }

        public bool Delete(AdsPosition item)
        {
            using (var dbContext = DaoContext())
            {
                var dbItem = dbContext.AdsPositions.FirstOrDefault(en => en.Id == item.Id);

                if (dbItem != null)
                {
                    dbContext.AdsPositions.DeleteOnSubmit(dbItem);
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
        public void Search(AdsPositionParam param)
        {
            var filter = param.AdsPositionFilter;
            using (var dbContext = new CoreDataContext(ConnectionString))
            {
                var query = from n in dbContext.AdsPositions
                            where (filter.Id.HasValue == false || n.Id == filter.Id)
                            && (string.IsNullOrEmpty(filter.keysearch) || n.Name.ToLower().Contains(filter.keysearch.ToLower()))
                            && (filter.Status.HasValue == false || filter.Status == n.Status)
                            select new AdsPositionEntity
                            {
                                Id = n.Id,
                                Name = n.Name,
                                Status = n.Status,
                                PositionName = n.PositionName,

                            };

                if (param.PagingInfo != null)
                {
                    param.PagingInfo.RecordCount = query.Count();
                    param.AdsPositionEntitys = query.Skip(param.PagingInfo.RowStart).Take(param.PagingInfo.PageSize).ToList();
                }
                else
                {
                    param.AdsPositionEntitys = query.ToList();
                }
            }
        }
        public void GetById(AdsPositionParam param)
        {
            Search(param);
            if (param.AdsPositionEntitys != null && param.AdsPositionEntitys.Any())
            {
                param.AdsPosition = param.AdsPositionEntitys.FirstOrDefault();
            }
        }
        #endregion
    }
}
