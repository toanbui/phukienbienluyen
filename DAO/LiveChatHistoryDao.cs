using DAO.Base;
using Entities.Entities;
using Entities.Param;
using System.Linq;

namespace DAO
{
    public class LiveChatHistoryDao : DaoBase
    {
        #region Action
        public int Insert(LiveChatHistory item)
        {
            using (var dbContext = DaoContext())
            {
                dbContext.LiveChatHistories.InsertOnSubmit(item);
                dbContext.SubmitChanges();
                return item.Id;
            }
        }
        public void Update(LiveChatHistory item)
        {
            using (var dbContext = DaoContext())
            {
                var dbItem = dbContext.LiveChatHistories.FirstOrDefault(sitem => sitem.Id == item.Id);
                if (dbItem != null)
                {
                    dbItem.Id = item.Id;
                    dbItem.Sender = item.Sender;
                    dbItem.SenderName = item.SenderName;
                    dbItem.SenderIsSupport = item.SenderIsSupport;
                    dbItem.SenderPhone = item.SenderPhone;
                    dbItem.SenderEmail = item.SenderEmail;
                    dbItem.SenderAvatar = item.SenderAvatar;
                    dbItem.Recieved = item.Recieved;
                    dbItem.RecievedName = item.RecievedName;
                    dbItem.RecievedIsSupport = item.RecievedIsSupport;
                    dbItem.RecievedPhone = item.RecievedPhone;
                    dbItem.RecievedEmail = item.RecievedEmail;
                    dbItem.RecievedAvatar = item.RecievedAvatar;
                    dbItem.Created = item.Created;
                    dbItem.FileName = item.FileName;
                    dbItem.FilePath = item.FilePath;
                    dbItem.Message = item.Message;
                    dbItem.GroupName = item.GroupName;

                    dbContext.SubmitChanges();
                }
            }
        }

        public bool Delete(LiveChatHistory item)
        {
            using (var dbContext = DaoContext())
            {
                var dbItem = dbContext.LiveChatHistories.FirstOrDefault(en => en.Id == item.Id);

                if (dbItem != null)
                {
                    dbContext.LiveChatHistories.DeleteOnSubmit(dbItem);
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
        public void Search(LiveChatHistoryParam param)
        {
            var filter = param.LiveChatHistoryFilter;
            using (var dbContext = new CoreDataContext(ConnectionString))
            {
                var query = from n in dbContext.LiveChatHistories
                            where (filter.Id.HasValue == false || n.Id == filter.Id)
                            && (string.IsNullOrEmpty(filter.keysearch) || n.SenderName.ToLower().Contains(filter.keysearch.ToLower()) || n.RecievedName.ToLower().Contains(filter.keysearch.ToLower()))
                            && (string.IsNullOrEmpty(filter.groupName) || n.GroupName.ToLower() == filter.groupName.ToLower())
                            orderby n.Created descending
                            select new LiveChatHistoryEntity
                            {
                                Id = n.Id,
                                Sender = n.Sender,
                                SenderName = n.SenderName,
                                SenderIsSupport = n.SenderIsSupport,
                                SenderPhone = n.SenderPhone,
                                SenderEmail = n.SenderEmail,
                                SenderAvatar = n.SenderAvatar,
                                Recieved = n.Recieved,
                                RecievedName = n.RecievedName,
                                RecievedIsSupport = n.RecievedIsSupport,
                                RecievedPhone = n.RecievedPhone,
                                RecievedEmail = n.RecievedEmail,
                                RecievedAvatar = n.RecievedAvatar,
                                Created = n.Created,
                                FileName = n.FileName,
                                FilePath = n.FilePath,
                                Message = n.Message,
                                GroupName = n.GroupName,

                            };

                if (param.PagingInfo != null)
                {
                    param.PagingInfo.RecordCount = query.Count();
                    param.LiveChatHistoryEntitys = query.Skip(param.PagingInfo.RowStart).Take(param.PagingInfo.PageSize).ToList();
                }
                else
                {
                    param.LiveChatHistoryEntitys = query.ToList();
                }
            }
        }
        public void GetById(LiveChatHistoryParam param)
        {
            Search(param);
            if (param.LiveChatHistoryEntitys != null && param.LiveChatHistoryEntitys.Any())
            {
                param.LiveChatHistory = param.LiveChatHistoryEntitys.FirstOrDefault();
                param.LiveChatHistoryEntity = param.LiveChatHistoryEntitys.FirstOrDefault();
            }
        }
        #endregion
    }
}
