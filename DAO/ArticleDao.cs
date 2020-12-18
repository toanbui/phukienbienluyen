using DAO.Base;
using Entities.Entities;
using Entities.Param;
using System.Linq;

namespace DAO
{
    public class ArticleDao : DaoBase
    {
        #region Action
        public int Insert(Article item)
        {
            using (var dbContext = DaoContext())
            {
                dbContext.Articles.InsertOnSubmit(item);
                dbContext.SubmitChanges();
                return item.Id;
            }
        }
        public void Update(Article item)
        {
            using (var dbContext = DaoContext())
            {
                var dbItem = dbContext.Articles.FirstOrDefault(sitem => sitem.Id == item.Id);
                if (dbItem != null)
                {
                    dbItem.Id = item.Id;
                    dbItem.NewsId = item.NewsId;
                    dbItem.Title = item.Title;
                    dbItem.Avatar = item.Avatar;
                    dbItem.Sapo = item.Sapo;
                    dbItem.Body = item.Body;
                    dbItem.Url = item.Url;
                    dbItem.Author = item.Author;
                    dbItem.Invisible = item.Invisible;
                    dbItem.Created = item.Created;
                    dbItem.CreatedBy = item.CreatedBy;
                    dbItem.Modified = item.Modified;
                    dbItem.ModifiedBy = item.ModifiedBy;
                    dbItem.Status = item.Status;

                    dbContext.SubmitChanges();
                }
            }
        }

        public bool Delete(Article item)
        {
            using (var dbContext = DaoContext())
            {
                var dbItem = dbContext.Articles.FirstOrDefault(en => en.Id == item.Id);

                if (dbItem != null)
                {
                    dbContext.Articles.DeleteOnSubmit(dbItem);
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
        public void Search(ArticleParam param)
        {
            var filter = param.ArticleFilter;
            using (var dbContext = new CoreDataContext(ConnectionString))
            {
                var query = from n in dbContext.Articles
                            where (filter.Id.HasValue == false || n.Id == filter.Id)
                            && (string.IsNullOrEmpty(filter.keysearch) || n.Title.ToLower().Contains(filter.keysearch.ToLower()))
                            && (filter.Status.HasValue == false || filter.Status == n.Status)
                            && (filter.Invisible.HasValue == false || filter.Invisible == n.Invisible)
                            && (filter.NewsId.HasValue == false || filter.NewsId == n.NewsId)
                            orderby !filter.OrderDateDesc ? null : n.Created descending
                            select new ArticleEntity
                            {
                                Id = n.Id,
                                NewsId = n.NewsId,
                                Title = n.Title,
                                Avatar = n.Avatar,
                                Sapo = n.Sapo,
                                Body = n.Body,
                                Url = n.Url,
                                Author = n.Author,
                                Invisible = n.Invisible,
                                Created = n.Created,
                                CreatedBy = n.CreatedBy,
                                Modified = n.Modified,
                                ModifiedBy = n.ModifiedBy,
                                Status = n.Status,

                            };

                if (param.PagingInfo != null)
                {
                    param.PagingInfo.RecordCount = query.Count();
                    param.ArticleEntitys = query.Skip(param.PagingInfo.RowStart).Take(param.PagingInfo.PageSize).ToList();
                }
                else
                {
                    param.ArticleEntitys = query.ToList();
                }
            }
        }
        public void GetById(ArticleParam param)
        {
            Search(param);
            if (param.ArticleEntitys != null && param.ArticleEntitys.Any())
            {
                param.Article = param.ArticleEntitys.FirstOrDefault();
                param.ArticleEntity = param.ArticleEntitys.FirstOrDefault();
            }
        }
        #endregion
    }
}
