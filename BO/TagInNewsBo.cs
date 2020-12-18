using DAO;
using Entities.Param;
using System.Transactions;

namespace BO
{
    public class TagInNewsBo
    {
        #region Action
        public void Insert(TagInNewsParam param)
        {
            var endep = param.TagInNews;
            var dao = new TagInNewsDao();
            param.TagInNews.Id = dao.Insert(endep);
        }
        public void Update(TagInNewsParam param)
        {
            var endep = param.TagInNews;
            var dao = new TagInNewsDao();
            dao.Update(endep);
        }
        public void Delete(TagInNewsParam param)
        {
            var dao = new TagInNewsDao();
            using (var tran = new TransactionScope())
            {
                var paramDep = param.TagInNewss;
                foreach (var endep in paramDep)
                {
                    dao.Delete(endep);
                }
                tran.Complete();
            }
        }
        #endregion

        #region Query
        public void Search(TagInNewsParam param)
        {
            var dao = new TagInNewsDao();
            dao.Search(param);
        }
        public void GetById(TagInNewsParam param)
        {
            var dao = new TagInNewsDao();
            dao.GetById(param);
        }
        #endregion
    }
}
