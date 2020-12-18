using DAO.Base;
using DAO.Format;
using Entities.Entities;
using Entities.Entities.FrontEnd;
using Entities.Param;
using System.Collections.Generic;
using System.Linq;

namespace DAO
{
    public class ProductDao : DaoBase
    {
        #region Action
        public int Insert(Product item)
        {
            using (var dbContext = DaoContext())
            {
                dbContext.Products.InsertOnSubmit(item);
                dbContext.SubmitChanges();
                return item.Id;
            }
        }
        public void Update(Product item)
        {
            using (var dbContext = DaoContext())
            {
                var dbItem = dbContext.Products.FirstOrDefault(sitem => sitem.Id == item.Id);
                if (dbItem != null)
                {
                    dbItem.Name = item.Name;
                    dbItem.Description = item.Description;
                    dbItem.Avatar = item.Avatar;
                    dbItem.ZoneId = item.ZoneId;
                    dbItem.Body = item.Body;
                    dbItem.Price = item.Price;
                    dbItem.Status = item.Status;

                    dbContext.SubmitChanges();
                }
            }
        }

        public bool Delete(Product item)
        {
            using (var dbContext = DaoContext())
            {
                var dbItem = dbContext.Products.FirstOrDefault(en => en.Id == item.Id);

                if (dbItem != null)
                {
                    dbContext.Products.DeleteOnSubmit(dbItem);
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
        public void Search(ProductParam param)
        {
            var filter = param.ProductFilter;
            using (var dbContext = new CoreDataContext(ConnectionString))
            {
                var query = from n in dbContext.Products
                            where (filter.Id.HasValue == false || n.Id == filter.Id)
                            && (string.IsNullOrEmpty(filter.keysearch) || n.Name.ToLower().Contains(filter.keysearch.ToLower()))
                            && (filter.Status.HasValue == false || filter.Status == n.Status)
                            orderby !filter.OrderDateDesc ? null : n.Created descending , filter.OrderDateDesc ? null : n.Created ascending
                            select new ProductEntity
                            {
                                Id = n.Id,
                                Name = n.Name,
                                Description = n.Description,
                                Avatar = n.Avatar,
                                ZoneId = n.ZoneId,
                                Body = n.Body,
                                Price = n.Price,
                                Created = n.Created,
                                CreatedBy = n.CreatedBy,
                                Modified = n.Modified,
                                ModifiedBy = n.ModifiedBy,
                                Status = n.Status,

                            };

                if (param.PagingInfo != null)
                {
                    param.PagingInfo.RecordCount = query.Count();
                    param.ProductEntitys = query.Skip(param.PagingInfo.RowStart).Take(param.PagingInfo.PageSize).ToList();
                    FormatData.FormatProduct(param.ProductEntitys);
                }
                else
                {
                    param.ProductEntitys = query.ToList();
                    FormatData.FormatProduct(param.ProductEntitys);
                }
            }
        }
        public void GetById(ProductParam param)
        {
            Search(param);
            if (param.ProductEntitys != null && param.ProductEntitys.Any())
            {
                param.ProductEntity = param.ProductEntitys.FirstOrDefault();
                param.Product = param.ProductEntitys.FirstOrDefault();
            }
        }
        #endregion

        #region FrontEnd
        public void GetByListId(ProductParam param)
        {
            Search(param);
            if (param.ProductEntitys != null && param.ProductEntitys.Any())
            {
                var query = from n in param.ProductEntitys
                            where
                            param.ProductFilter.ListId.Contains(n.Id)
                            select new ProductInCart
                            {
                                Id = n.Id,
                                Name = n.Name,
                                Description = n.Description,
                                Avatar = n.Avatar,
                                ZoneId = n.ZoneId,
                                Body = n.Body,
                                Price = n.Price,
                                Created = n.Created,
                                CreatedBy = n.CreatedBy,
                                Modified = n.Modified,
                                ModifiedBy = n.ModifiedBy,
                                Status = n.Status,
                                FirstAvatar = n.FirstAvatar
                            };
                param.ProductInCarts = query.ToList();
            }
        }
        public void FeSearchOrderByPrice(ProductParam param)
        {
            var filter = param.ProductFilter;
            using (var dbContext = new CoreDataContext(ConnectionString))
            {
                var query = from n in dbContext.Products
                            where (filter.Id.HasValue == false || n.Id == filter.Id)
                            && (string.IsNullOrEmpty(filter.keysearch) || n.Name.ToLower().Contains(filter.keysearch.ToLower()))
                            && (filter.Status.HasValue == false || filter.Status == n.Status)
                            && (filter.ZoneId.HasValue == false || filter.ZoneId == n.ZoneId)
                            orderby !filter.OrderPriceDesc ? 0 : n.Price descending, filter.OrderPriceDesc ? 0 : n.Price ascending
                            select new ProductEntity
                            {
                                Id = n.Id,
                                Name = n.Name,
                                Description = n.Description,
                                Avatar = n.Avatar,
                                ZoneId = n.ZoneId,
                                Body = n.Body,
                                Price = n.Price,
                                Created = n.Created,
                                CreatedBy = n.CreatedBy,
                                Modified = n.Modified,
                                ModifiedBy = n.ModifiedBy,
                                Status = n.Status,

                            };

                if (param.PagingInfo != null)
                {
                    param.PagingInfo.RecordCount = query.Count();
                    param.ProductEntitys = query.Skip(param.PagingInfo.RowStart).Take(param.PagingInfo.PageSize).ToList();
                    FormatData.FormatProduct(param.ProductEntitys);
                }
                else
                {
                    param.ProductEntitys = query.ToList();
                    FormatData.FormatProduct(param.ProductEntitys);
                }
            }
        }
        #endregion
    }
}
