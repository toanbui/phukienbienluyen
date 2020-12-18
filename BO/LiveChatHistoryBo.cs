using DAO;
using Entities.Param;
using System.Transactions;

namespace BO
{
    public class LiveChatHistoryBo
    {
        #region Action
        public void Insert(LiveChatHistoryParam param)
        {
            var endep = param.LiveChatHistory;
            var dao = new LiveChatHistoryDao();
            param.LiveChatHistory.Id = dao.Insert(endep);
        }
        public void Update(LiveChatHistoryParam param)
        {
            var endep = param.LiveChatHistory;
            var dao = new LiveChatHistoryDao();
            dao.Update(endep);
        }
        public void Delete(LiveChatHistoryParam param)
        {
            var dao = new LiveChatHistoryDao();
            using (var tran = new TransactionScope())
            {
                var paramDep = param.LiveChatHistorys;
                foreach (var endep in paramDep)
                {
                    dao.Delete(endep);
                }
                tran.Complete();
            }
        }
        #endregion

        #region Query
        public void Search(LiveChatHistoryParam param)
        {
            var dao = new LiveChatHistoryDao();
            dao.Search(param);
        }
        public void GetById(LiveChatHistoryParam param)
        {
            var dao = new LiveChatHistoryDao();
            dao.GetById(param);
        }
        #endregion
    }
}
