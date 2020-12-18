using DAO.Base;
using Entities.Entities;
using Entities.Param;
using System.Linq;

namespace DAO
{
    public class DistrictDao : DaoBase
    {
        #region Action
        public int Insert(District item)
        {
            using (var dbContext = DaoContext())
            {
                dbContext.Districts.InsertOnSubmit(item);
                dbContext.SubmitChanges();
                return item.Id;
            }
        }
        public void Update(District item)
        {
            using (var dbContext = DaoContext())
            {
                var dbItem = dbContext.Districts.FirstOrDefault(sitem => sitem.Id == item.Id);
                if (dbItem != null)
                {
                    dbItem.Id = item.Id;
                    dbItem.Code = item.Code;
                    dbItem.Name = item.Name;
                    dbItem.Type = item.Type;
                    dbItem.CityId = item.CityId;
                    dbItem.Location = item.Location;

                    dbContext.SubmitChanges();
                }
            }
        }

        public bool Delete(District item)
        {
            using (var dbContext = DaoContext())
            {
                var dbItem = dbContext.Districts.FirstOrDefault(en => en.Id == item.Id);

                if (dbItem != null)
                {
                    dbContext.Districts.DeleteOnSubmit(dbItem);
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
        public void Search(DistrictParam param)
        {
            var filter = param.DistrictFilter;
            using (var dbContext = new CoreDataContext(ConnectionString))
            {
                var query = from n in dbContext.Districts
                            where (filter.Id.HasValue == false || n.Id == filter.Id)
                            && (string.IsNullOrEmpty(filter.keysearch) || n.Name.ToLower().Contains(filter.keysearch.ToLower()))
                            && (string.IsNullOrEmpty(filter.ProvinceCode) || n.CityId == filter.ProvinceCode)
                            select new DistrictEntity
                            {
                                Id = n.Id,
                                Code = n.Code,
                                Name = n.Name,
                                Type = n.Type,
                                CityId = n.CityId,
                                Location = n.Location,
                            };

                if (param.PagingInfo != null)
                {
                    param.PagingInfo.RecordCount = query.Count();
                    param.DistrictEntitys = query.Skip(param.PagingInfo.RowStart).Take(param.PagingInfo.PageSize).ToList();
                }
                else
                {
                    param.DistrictEntitys = query.ToList();
                }
            }
        }
        public void GetById(DistrictParam param)
        {
            Search(param);
            if (param.DistrictEntitys != null && param.DistrictEntitys.Any())
            {
                param.District = param.DistrictEntitys.FirstOrDefault();
                param.DistrictEntity = param.DistrictEntitys.FirstOrDefault();
            }
        }
        #endregion
    }
}
