using DAO.Base;
using Entities.Entities;
using Entities.Param;
using System.Linq;

namespace DAO
{
    public class UserInLiveChatDao : DaoBase
    {
        #region Action
        public int Insert(UserInLiveChat item)
        {
            using (var dbContext = DaoContext())
            {
                dbContext.UserInLiveChats.InsertOnSubmit(item);
                dbContext.SubmitChanges();
                return item.Id;
            }
        }
        public void Update(UserInLiveChat item)
        {
            using (var dbContext = DaoContext())
            {
                var dbItem = dbContext.UserInLiveChats.FirstOrDefault(sitem => sitem.Id == item.Id);
                if (dbItem != null)
                {
                    dbItem.Id = item.Id;
                    dbItem.UserId = item.UserId;
                    dbItem.About = item.About;
                    dbItem.SupportAvatar = item.SupportAvatar;
                    dbItem.Order = item.Order;
                    dbItem.Status = item.Status;

                    dbContext.SubmitChanges();
                }
            }
        }

        public bool Delete(UserInLiveChat item)
        {
            using (var dbContext = DaoContext())
            {
                var dbItem = dbContext.UserInLiveChats.FirstOrDefault(en => en.Id == item.Id);

                if (dbItem != null)
                {
                    dbContext.UserInLiveChats.DeleteOnSubmit(dbItem);
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
        public void Search(UserInLiveChatParam param)
        {
            var filter = param.UserInLiveChatFilter;
            using (var dbContext = new CoreDataContext(ConnectionString))
            {
                var query = from n in dbContext.UserInLiveChats
                            join u in dbContext.AspNetUsers on n.UserId equals u.Id
                            where (filter.Id.HasValue == false || n.Id == filter.Id)
                            && (string.IsNullOrEmpty(filter.keysearch) || u.Name.ToLower().Contains(filter.keysearch.ToLower()))
                            && (string.IsNullOrEmpty(filter.UserId) || n.UserId == filter.UserId)
                            && (filter.Status.HasValue == false || filter.Status == n.Status)
                            select new UserInLiveChatEntity
                            {
                                Id = n.Id,
                                UserId = n.UserId,
                                UserName = u.UserName,
                                Name = u.Name,
                                About = n.About,
                                SupportAvatar = n.SupportAvatar,
                                Avatar = u.Avatar,
                                Order = n.Order,
                                Status = n.Status,
                            };

                if (param.PagingInfo != null)
                {
                    param.PagingInfo.RecordCount = query.Count();
                    param.UserInLiveChatEntitys = query.Skip(param.PagingInfo.RowStart).Take(param.PagingInfo.PageSize).ToList();
                }
                else
                {
                    param.UserInLiveChatEntitys = query.ToList();
                }
            }
        }
        public void GetById(UserInLiveChatParam param)
        {
            Search(param);
            if (param.UserInLiveChatEntitys != null && param.UserInLiveChatEntitys.Any())
            {
                param.UserInLiveChat = param.UserInLiveChatEntitys.FirstOrDefault();
                param.UserInLiveChatEntity = param.UserInLiveChatEntitys.FirstOrDefault();
            }
        }
        public void GetByUserId(UserInLiveChatParam param)
        {
            Search(param);
            if (param.UserInLiveChatEntitys != null && param.UserInLiveChatEntitys.Any())
            {
                param.UserInLiveChat = param.UserInLiveChatEntitys.FirstOrDefault();
                param.UserInLiveChatEntity = param.UserInLiveChatEntitys.FirstOrDefault();
            }
        }
        #endregion
    }
}
