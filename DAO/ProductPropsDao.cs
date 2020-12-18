using DAO.Base;
using Entities.Entities;
using Entities.Param;
using System.Linq;

namespace DAO
{
    public class ProductPropsDao : DaoBase
    {
        #region Action
        public int Insert(ProductProp item)
        {
            using (var dbContext = DaoContext())
            {
                dbContext.ProductProps.InsertOnSubmit(item);
                dbContext.SubmitChanges();
                return item.Id;
            }
        }
        public void Update(ProductProp item)
        {
            using (var dbContext = DaoContext())
            {
                var dbItem = dbContext.ProductProps.FirstOrDefault(sitem => sitem.Id == item.Id);
                if (dbItem != null)
                {
                    dbItem.Name = item.Name;
                    dbItem.Modified = item.Modified;
                    dbItem.ModifiedBy = item.ModifiedBy;
                    dbItem.Status = item.Status;

                    dbContext.SubmitChanges();
                }
            }
        }

        public bool Delete(ProductProp item)
        {
            using (var dbContext = DaoContext())
            {
                var dbItem = dbContext.ProductProps.FirstOrDefault(en => en.Id == item.Id);

                if (dbItem != null)
                {
                    dbContext.ProductProps.DeleteOnSubmit(dbItem);
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
        public void Search(ProductPropsParam param)
        {
            var filter = param.ProductPropsFilter;
            using (var dbContext = new CoreDataContext(ConnectionString))
            {
                var query = from n in dbContext.ProductProps
                            where (filter.Id.HasValue == false || n.Id == filter.Id)
                            && (string.IsNullOrEmpty(filter.keysearch) || n.Name.ToLower().Contains(filter.keysearch.ToLower()))
                            && (filter.Status.HasValue == false || filter.Status == n.Status)
                            select new ProductPropsEntity
                            {
                                Id = n.Id,
                                Name = n.Name,
                                Created = n.Created,
                                CreatedBy = n.CreatedBy,
                                Modified = n.Modified,
                                ModifiedBy = n.ModifiedBy,
                                Status = n.Status,

                            };

                if (param.PagingInfo != null)
                {
                    param.PagingInfo.RecordCount = query.Count();
                    param.ProductPropsEntitys = query.Skip(param.PagingInfo.RowStart).Take(param.PagingInfo.PageSize).ToList();
                }
                else
                {
                    param.ProductPropsEntitys = query.ToList();
                }
            }
        }
        public void GetById(ProductPropsParam param)
        {
            Search(param);
            if (param.ProductPropsEntitys != null && param.ProductPropsEntitys.Any())
            {
                param.ProductProps = param.ProductPropsEntitys.FirstOrDefault();
            }
        }
        #endregion
    }
}
