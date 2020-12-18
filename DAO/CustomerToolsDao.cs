using DAO.Base;
using Entities.Entities;
using Entities.Param;
using System.Linq;

namespace DAO
{
    public class CustomerToolsDao : DaoBase
    {
        #region Action
        public int Insert(CustomerTool item)
        {
            using (var dbContext = DaoContext())
            {
                dbContext.CustomerTools.InsertOnSubmit(item);
                dbContext.SubmitChanges();
                return item.Id;
            }
        }
        public void Update(CustomerTool item)
        {
            using (var dbContext = DaoContext())
            {
                var dbItem = dbContext.CustomerTools.FirstOrDefault(sitem => sitem.Id == item.Id);
                if (dbItem != null)
                {
                    dbItem.Id = item.Id;
                    dbItem.Keygen = item.Keygen;
                    dbItem.CustName = item.CustName;
                    dbItem.MachineId = item.MachineId;
                    dbItem.Email = item.Email;
                    dbItem.RegisterDate = item.RegisterDate;
                    dbItem.Status = item.Status;

                    dbContext.SubmitChanges();
                }
            }
        }

        public bool Delete(CustomerTool item)
        {
            using (var dbContext = DaoContext())
            {
                var dbItem = dbContext.CustomerTools.FirstOrDefault(en => en.Id == item.Id);

                if (dbItem != null)
                {
                    dbContext.CustomerTools.DeleteOnSubmit(dbItem);
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
        public void Search(CustomerToolsParam param)
        {
            var filter = param.CustomerToolsFilter;
            using (var dbContext = new CoreDataContext(ConnectionString))
            {
                var query = from n in dbContext.CustomerTools
                            where (filter.Id.HasValue == false || n.Id == filter.Id)
                            && (string.IsNullOrEmpty(filter.keysearch) || n.CustName.ToLower().Contains(filter.keysearch.ToLower()))
                            && (string.IsNullOrEmpty(filter.Email) || n.Email.ToLower().Contains(filter.Email.ToLower()))
                            && (string.IsNullOrEmpty(filter.MachineId) || n.Keygen.ToLower().Contains(filter.MachineId.ToLower()))
                            && (filter.Status.HasValue == false || filter.Status == n.Status)
                            orderby !filter.OrderDateDesc ? null : n.RegisterDate descending, filter.OrderDateDesc ? null : n.RegisterDate ascending
                            select new CustomerToolsEntity
                            {
                                Id = n.Id,
                                Keygen = n.Keygen,
                                CustName = n.CustName,
                                MachineId = n.MachineId,
                                Email = n.Email,
                                RegisterDate = n.RegisterDate,
                                Status = n.Status,

                            };

                if (param.PagingInfo != null)
                {
                    param.PagingInfo.RecordCount = query.Count();
                    param.CustomerToolsEntitys = query.Skip(param.PagingInfo.RowStart).Take(param.PagingInfo.PageSize).ToList();
                }
                else
                {
                    param.CustomerToolsEntitys = query.ToList();
                }
            }
        }
        public void GetById(CustomerToolsParam param)
        {
            Search(param);
            if (param.CustomerToolsEntitys != null && param.CustomerToolsEntitys.Any())
            {
                param.CustomerTool = param.CustomerToolsEntitys.FirstOrDefault();
                param.CustomerToolsEntity = param.CustomerToolsEntitys.FirstOrDefault();
            }
        }
        #endregion
    }
}
