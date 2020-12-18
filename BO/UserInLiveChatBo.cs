using DAO;
using Entities.Param;
using System.Transactions;

namespace BO
{
    public class UserInLiveChatBo
    {
        #region Action
        public void Insert(UserInLiveChatParam param)
        {
            var endep = param.UserInLiveChat;
            var dao = new UserInLiveChatDao();
            param.UserInLiveChat.Id = dao.Insert(endep);
        }
        public void Update(UserInLiveChatParam param)
        {
            var endep = param.UserInLiveChat;
            var dao = new UserInLiveChatDao();
            dao.Update(endep);
        }
        public void Delete(UserInLiveChatParam param)
        {
            var dao = new UserInLiveChatDao();
            using (var tran = new TransactionScope())
            {
                var paramDep = param.UserInLiveChats;
                foreach (var endep in paramDep)
                {
                    dao.Delete(endep);
                }
                tran.Complete();
            }
        }
        #endregion

        #region Query
        public void Search(UserInLiveChatParam param)
        {
            var dao = new UserInLiveChatDao();
            dao.Search(param);
        }
        public void GetById(UserInLiveChatParam param)
        {
            var dao = new UserInLiveChatDao();
            dao.GetById(param);
        }
        public void GetByUserId(UserInLiveChatParam param)
        {
            var dao = new UserInLiveChatDao();
            dao.GetByUserId(param);
        }
        #endregion
    }
}
