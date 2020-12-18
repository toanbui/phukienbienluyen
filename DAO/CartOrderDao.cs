using DAO.Base;
using Entities.Entities;
using Entities.Param;
using System.Linq;

namespace DAO
{
    public class CartOrderDao : DaoBase
    {
        #region Action
        public int Insert(CartOrder item)
        {
            using (var dbContext = DaoContext())
            {
                dbContext.CartOrders.InsertOnSubmit(item);
                dbContext.SubmitChanges();
                return item.Id;
            }
        }
        public void Update(CartOrder item)
        {
            using (var dbContext = DaoContext())
            {
                var dbItem = dbContext.CartOrders.FirstOrDefault(sitem => sitem.Id == item.Id);
                if (dbItem != null)
                {
                    dbItem.Id = item.Id;
                    dbItem.Name = item.Name;
                    dbItem.Email = item.Email;
                    dbItem.Phone = item.Phone;
                    dbItem.Province = item.Province;
                    dbItem.District = item.District;
                    dbItem.Address = item.Address;
                    dbItem.ZipCode = item.ZipCode;
                    dbItem.ToltalPrice = item.ToltalPrice;
                    dbItem.ProductIds = item.ProductIds;
                    dbItem.Created = item.Created;
                    dbItem.Modified = item.Modified;
                    dbItem.ModifiedBy = item.ModifiedBy;
                    dbItem.Approved = item.Approved;
                    dbItem.ApprovedBy = item.ApprovedBy;
                    dbItem.Status = item.Status;

                    dbContext.SubmitChanges();
                }
            }
        }
        public void Approve(CartOrder item)
        {
            using (var dbContext = DaoContext())
            {
                var dbItem = dbContext.CartOrders.FirstOrDefault(sitem => sitem.Id == item.Id);
                if (dbItem != null)
                {
                    dbItem.Approved = item.Approved;
                    dbItem.ApprovedBy = item.ApprovedBy;
                    dbContext.SubmitChanges();
                }
            }
        }

        public bool Delete(CartOrder item)
        {
            using (var dbContext = DaoContext())
            {
                var dbItem = dbContext.CartOrders.FirstOrDefault(en => en.Id == item.Id);

                if (dbItem != null)
                {
                    dbContext.CartOrders.DeleteOnSubmit(dbItem);
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
        public void Search(CartOrderParam param)
        {
            var filter = param.CartOrderFilter;
            using (var dbContext = new CoreDataContext(ConnectionString))
            {
                var query = from n in dbContext.CartOrders
                            where (filter.Id.HasValue == false || n.Id == filter.Id)
                            && (string.IsNullOrEmpty(filter.keysearch) || n.Name.ToLower().Contains(filter.keysearch.ToLower()))
                            && (filter.Status.HasValue == false || filter.Status == n.Status)
                            select new CartOrderEntity
                            {
                                Id = n.Id,
                                Name = n.Name,
                                Email = n.Email,
                                Phone = n.Phone,
                                Province = n.Province,
                                District = n.District,
                                Address = n.Address,
                                ZipCode = n.ZipCode,
                                ProductIds = n.ProductIds,
                                Created = n.Created,
                                Modified = n.Modified,
                                ModifiedBy = n.ModifiedBy,
                                Approved = n.Approved,
                                ApprovedBy = n.ApprovedBy,
                                Status = n.Status,

                            };

                if (param.PagingInfo != null)
                {
                    param.PagingInfo.RecordCount = query.Count();
                    param.CartOrderEntitys = query.Skip(param.PagingInfo.RowStart).Take(param.PagingInfo.PageSize).ToList();
                }
                else
                {
                    param.CartOrderEntitys = query.ToList();
                }
            }
        }
        public void GetById(CartOrderParam param)
        {
            Search(param);
            if (param.CartOrderEntitys != null && param.CartOrderEntitys.Any())
            {
                param.CartOrder = param.CartOrderEntitys.FirstOrDefault();
                param.CartOrderEntity = param.CartOrderEntitys.FirstOrDefault();
            }
        }
        #endregion
    }
}
