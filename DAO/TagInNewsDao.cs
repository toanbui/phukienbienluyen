using DAO.Base;
using Entities.Entities;
using Entities.Param;
using System.Linq;

namespace DAO
{
    public class TagInNewsDao : DaoBase
    {
        #region Action
        public int Insert(TagInNew item)
        {
            using (var dbContext = DaoContext())
            {
                dbContext.TagInNews.InsertOnSubmit(item);
                dbContext.SubmitChanges();
                return item.Id;
            }
        }
        public void Update(TagInNew item)
        {
            using (var dbContext = DaoContext())
            {
                var dbItem = dbContext.TagInNews.FirstOrDefault(sitem => sitem.Id == item.Id);
                if (dbItem != null)
                {
                    dbItem.Id = item.Id;
                    dbItem.NewsId = item.NewsId;
                    dbItem.TagId = item.TagId;

                    dbContext.SubmitChanges();
                }
            }
        }

        public bool Delete(TagInNew item)
        {
            using (var dbContext = DaoContext())
            {
                var dbItem = dbContext.TagInNews.FirstOrDefault(en => en.Id == item.Id);

                if (dbItem != null)
                {
                    dbContext.TagInNews.DeleteOnSubmit(dbItem);
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
        public void Search(TagInNewsParam param)
        {
            var filter = param.TagInNewsFilter;
            using (var dbContext = new CoreDataContext(ConnectionString))
            {
                var query = from n in dbContext.TagInNews
                            where (filter.Id.HasValue == false || n.Id == filter.Id)
                            && (filter.NewsId.HasValue == false || filter.NewsId == n.NewsId)
                            && (filter.TagId.HasValue == false || filter.TagId == n.TagId)
                            select new TagInNewsEntity
                            {
                                Id = n.Id,
                                NewsId = n.NewsId,
                                TagId = n.TagId,

                            };

                if (param.PagingInfo != null)
                {
                    param.PagingInfo.RecordCount = query.Count();
                    param.TagInNewsEntitys = query.Skip(param.PagingInfo.RowStart).Take(param.PagingInfo.PageSize).ToList();
                }
                else
                {
                    param.TagInNewsEntitys = query.ToList();
                }
            }
        }
        public void GetById(TagInNewsParam param)
        {
            Search(param);
            if (param.TagInNewsEntitys != null && param.TagInNewsEntitys.Any())
            {
                param.TagInNews = param.TagInNewsEntitys.FirstOrDefault();
                param.TagInNewsEntity = param.TagInNewsEntitys.FirstOrDefault();
            }
        }
        #endregion
    }
}
