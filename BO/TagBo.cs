using DAO;
using Entities.Param;
using System.Transactions;

namespace BO
{
    public class TagBo
    {
        #region Action
        public void Insert(TagParam param)
        {
            var endep = param.Tag;
            var dao = new TagDao();
            param.Tag.Id = dao.Insert(endep);
        }
        public void Update(TagParam param)
        {
            var endep = param.Tag;
            var dao = new TagDao();
            dao.Update(endep);
        }
        public void Delete(TagParam param)
        {
            var dao = new TagDao();
            using (var tran = new TransactionScope())
            {
                var paramDep = param.Tags;
                foreach (var endep in paramDep)
                {
                    dao.Delete(endep);
                }
                tran.Complete();
            }
        }
        #endregion

        #region Query
        public void Search(TagParam param)
        {
            var dao = new TagDao();
            dao.Search(param);
        }
        public void GetById(TagParam param)
        {
            var dao = new TagDao();
            dao.GetById(param);
        }
        #endregion
    }
}
