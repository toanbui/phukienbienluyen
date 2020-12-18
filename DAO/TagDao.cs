using DAO.Base;
using Entities.Entities;
using Entities.Param;
using System.Linq;

namespace DAO
{
    public class TagDao : DaoBase
    {
        #region Action
        public int Insert(Tag item)
        {
            using (var dbContext = DaoContext())
            {
                dbContext.Tags.InsertOnSubmit(item);
                dbContext.SubmitChanges();
                return item.Id;
            }
        }
        public void Update(Tag item)
        {
            using (var dbContext = DaoContext())
            {
                var dbItem = dbContext.Tags.FirstOrDefault(sitem => sitem.Id == item.Id);
                if (dbItem != null)
                {
                    dbItem.Status = item.Status;
                    dbContext.SubmitChanges();
                }
            }
        }

        public bool Delete(Tag item)
        {
            using (var dbContext = DaoContext())
            {
                var dbItem = dbContext.Tags.FirstOrDefault(en => en.Id == item.Id);

                if (dbItem != null)
                {
                    dbContext.Tags.DeleteOnSubmit(dbItem);
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
        public void Search(TagParam param)
        {
            var filter = param.TagFilter;
            using (var dbContext = new CoreDataContext(ConnectionString))
            {
                var query = from n in dbContext.Tags
                            where (filter.Id.HasValue == false || n.Id == filter.Id)
                            && (string.IsNullOrEmpty(filter.keysearch) || n.Name.ToLower().Contains(filter.keysearch.ToLower()))
                            && (filter.Status.HasValue == false || filter.Status == n.Status)
                            select new TagEntity
                            {
                                Id = n.Id,
                                Name = n.Name,
                                TagUrl = n.TagUrl,
                                Status = n.Status,

                            };

                if (param.PagingInfo != null)
                {
                    param.PagingInfo.RecordCount = query.Count();
                    param.TagEntitys = query.Skip(param.PagingInfo.RowStart).Take(param.PagingInfo.PageSize).ToList();
                }
                else
                {
                    param.TagEntitys = query.ToList();
                }
            }
        }
        public void GetById(TagParam param)
        {
            Search(param);
            if (param.TagEntitys != null && param.TagEntitys.Any())
            {
                param.Tag = param.TagEntitys.FirstOrDefault();
                param.TagEntity = param.TagEntitys.FirstOrDefault();
            }
        }
        #endregion
    }
}
