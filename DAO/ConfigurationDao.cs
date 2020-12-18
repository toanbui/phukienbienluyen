using DAO.Base;
using Entities.Entities;
using Entities.Param;
using System.Linq;

namespace DAO
{
    public class ConfigurationDao : DaoBase
    {
        #region Action
        public int Insert(Configuration item)
        {
            using (var dbContext = DaoContext())
            {
                dbContext.Configurations.InsertOnSubmit(item);
                dbContext.SubmitChanges();
                return item.Id;
            }
        }
        public void Update(Configuration item)
        {
            using (var dbContext = DaoContext())
            {
                var dbItem = dbContext.Configurations.FirstOrDefault(sitem => sitem.Id == item.Id);
                if (dbItem != null)
                {
                    dbItem.Id = item.Id;
                    dbItem.Name = item.Name;
                    dbItem.Value = item.Value;

                    dbContext.SubmitChanges();
                }
            }
        }

        public bool Delete(Configuration item)
        {
            using (var dbContext = DaoContext())
            {
                var dbItem = dbContext.Configurations.FirstOrDefault(en => en.Id == item.Id);

                if (dbItem != null)
                {
                    dbContext.Configurations.DeleteOnSubmit(dbItem);
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
        public void Search(ConfigurationParam param)
        {
            var filter = param.ConfigurationFilter;
            using (var dbContext = new CoreDataContext(ConnectionString))
            {
                var query = from n in dbContext.Configurations
                            where (filter.Id.HasValue == false || n.Id == filter.Id)
                            && (string.IsNullOrEmpty(filter.keysearch) || n.Name.ToLower().Contains(filter.keysearch.ToLower()))
                            select new ConfigurationEntity
                            {
                                Id = n.Id,
                                Name = n.Name,
                                Value = n.Value,

                            };

                if (param.PagingInfo != null)
                {
                    param.PagingInfo.RecordCount = query.Count();
                    param.ConfigurationEntitys = query.Skip(param.PagingInfo.RowStart).Take(param.PagingInfo.PageSize).ToList();
                }
                else
                {
                    param.ConfigurationEntitys = query.ToList();
                }
            }
        }
        public void GetById(ConfigurationParam param)
        {
            Search(param);
            if (param.ConfigurationEntitys != null && param.ConfigurationEntitys.Any())
            {
                param.Configuration = param.ConfigurationEntitys.FirstOrDefault();
            }
        }
        #endregion
    }
}
