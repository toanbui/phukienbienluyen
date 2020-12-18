using DAO;
using Entities.Param;
using System.Transactions;

namespace BO
{
    public class AdsPositionBo
    {
        #region Action
        public void Insert(AdsPositionParam param)
        {
            var endep = param.AdsPosition;
            var dao = new AdsPositionDao();
            param.AdsPosition.Id = dao.Insert(endep);
        }
        public void Update(AdsPositionParam param)
        {
            var endep = param.AdsPosition;
            var dao = new AdsPositionDao();
            dao.Update(endep);
        }
        public void Delete(AdsPositionParam param)
        {
            var dao = new AdsPositionDao();
            using (var tran = new TransactionScope())
            {
                var paramDep = param.AdsPositions;
                foreach (var endep in paramDep)
                {
                    dao.Delete(endep);
                }
                tran.Complete();
            }
        }
        #endregion

        #region Query
        public void Search(AdsPositionParam param)
        {
            var dao = new AdsPositionDao();
            dao.Search(param);
        }
        public void GetById(AdsPositionParam param)
        {
            var dao = new AdsPositionDao();
            dao.GetById(param);
        }
        #endregion
    }
}
