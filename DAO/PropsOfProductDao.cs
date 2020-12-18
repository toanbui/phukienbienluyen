using DAO.Base;
using Entities.Entities;
using Entities.Param;
using System.Linq;

namespace DAO
{
    public class PropsOfProductDao : DaoBase
    {
        #region Action
        public int Insert(PropsOfProduct item)
        {
            using (var dbContext = DaoContext())
            {
                dbContext.PropsOfProducts.InsertOnSubmit(item);
                dbContext.SubmitChanges();
                return item.Id;
            }
        }
        public void Update(PropsOfProduct item)
        {
            using (var dbContext = DaoContext())
            {
                var dbItem = dbContext.PropsOfProducts.FirstOrDefault(sitem => sitem.Id == item.Id);
                if (dbItem != null)
                {
                    dbItem.Id = item.Id;
                    dbItem.ProductId = item.ProductId;
                    dbItem.PropsId = item.PropsId;

                    dbContext.SubmitChanges();
                }
            }
        }

        public bool Delete(PropsOfProduct item)
        {
            using (var dbContext = DaoContext())
            {
                var dbItem = dbContext.PropsOfProducts.FirstOrDefault(en => en.Id == item.Id);

                if (dbItem != null)
                {
                    dbContext.PropsOfProducts.DeleteOnSubmit(dbItem);
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
        public void Search(PropsOfProductParam param)
        {
            var filter = param.PropsOfProductFilter;
            using (var dbContext = new CoreDataContext(ConnectionString))
            {
                var query = from n in dbContext.PropsOfProducts
                            where (filter.Id.HasValue == false || n.Id == filter.Id)
                            && (filter.PropsId.HasValue == false || n.PropsId == filter.PropsId)
                            && (filter.ProductId.HasValue == false || n.ProductId == filter.ProductId)
                            select new PropsOfProductEntity
                            {
                                Id = n.Id,
                                ProductId = n.ProductId,
                                PropsId = n.PropsId,

                            };

                if (param.PagingInfo != null)
                {
                    param.PagingInfo.RecordCount = query.Count();
                    param.PropsOfProductEntitys = query.Skip(param.PagingInfo.RowStart).Take(param.PagingInfo.PageSize).ToList();
                }
                else
                {
                    param.PropsOfProductEntitys = query.ToList();
                }
            }
        }
        public void GetById(PropsOfProductParam param)
        {
            Search(param);
            if (param.PropsOfProductEntitys != null && param.PropsOfProductEntitys.Any())
            {
                param.PropsOfProduct = param.PropsOfProductEntitys.FirstOrDefault();
            }
        }
        #endregion
    }
}
