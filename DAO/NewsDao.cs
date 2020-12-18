using DAO.Base;
using Entities.Entities;
using System.Linq;

namespace DAO
{
    public class NewsDao : DaoBase
    {
        //#region Action
        //public long Insert(New item)
        //{
        //    using (var dbContext = new CoreDataContext(ConnectionString))
        //    {
        //        dbContext.News.InsertOnSubmit(item);
        //        dbContext.SubmitChanges();
        //        return item.NewsId;
        //    }
        //}
        //public void Update(NewsEntity item)
        //{
        //    using (var dbContext = DaoContext())
        //    {
        //        var dbItem = dbContext.News.FirstOrDefault(sitem => sitem.NewsId == item.NewsId);
        //        if (dbItem != null)
        //        {

                    

        //            dbContext.SubmitChanges();
        //        }
        //    }
        //}

        //public bool Delete(NewsEntity item)
        //{
        //    using (var dbContext = DaoContext())
        //    {
        //        var dbItem = dbContext.News.FirstOrDefault(en => en.NewsId == item.NewsId);

        //        if (dbItem != null)
        //        {
        //            dbContext.News.DeleteOnSubmit(dbItem);
        //            dbContext.SubmitChanges();
        //        }
        //    }
        //    return true;
        //}
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
        //#endregion
    }
}
