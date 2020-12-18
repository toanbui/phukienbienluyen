using DAO.Base;
using Entities.Entities;
using Entities.Param;
using System.Linq;

namespace DAO
{
    public class ProvinceDao : DaoBase
    {
        #region Action
        public int Insert(Province item)
        {
            using (var dbContext = DaoContext())
            {
                dbContext.Provinces.InsertOnSubmit(item);
                dbContext.SubmitChanges();
                return item.Id;
            }
        }
        public void Update(Province item)
        {
            using (var dbContext = DaoContext())
            {
                var dbItem = dbContext.Provinces.FirstOrDefault(sitem => sitem.Id == item.Id);
                if (dbItem != null)
                {
                    dbItem.Id = item.Id;
                    dbItem.Code = item.Code;
                    dbItem.Name = item.Name;
                    dbItem.Type = item.Type;

                    dbContext.SubmitChanges();
                }
            }
        }

        public bool Delete(Province item)
        {
            using (var dbContext = DaoContext())
            {
                var dbItem = dbContext.Provinces.FirstOrDefault(en => en.Id == item.Id);

                if (dbItem != null)
                {
                    dbContext.Provinces.DeleteOnSubmit(dbItem);
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
        public void Search(ProvinceParam param)
        {
            var filter = param.ProvinceFilter;
            using (var dbContext = new CoreDataContext(ConnectionString))
            {
                var query = from n in dbContext.Provinces
                            where (filter.Id.HasValue == false || n.Id == filter.Id)
                            select new ProvinceEntity
                            {
                                Id = n.Id,
                                Code = n.Code,
                                Name = n.Name,
                                Type = n.Type,

                            };

                if (param.PagingInfo != null)
                {
                    param.PagingInfo.RecordCount = query.Count();
                    param.ProvinceEntitys = query.Skip(param.PagingInfo.RowStart).Take(param.PagingInfo.PageSize).ToList();
                }
                else
                {
                    param.ProvinceEntitys = query.ToList();
                }
            }
        }
        public void GetById(ProvinceParam param)
        {
            Search(param);
            if (param.ProvinceEntitys != null && param.ProvinceEntitys.Any())
            {
                param.Province = param.ProvinceEntitys.FirstOrDefault();
                param.ProvinceEntity = param.ProvinceEntitys.FirstOrDefault();
            }
        }
        #endregion
    }
}
