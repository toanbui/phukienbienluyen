using DAO;
using Entities.Param;
using System.Transactions;

namespace BO
{
    public class AdvertisingBo
    {
        #region Action
        public void Insert(AdvertisingParam param)
        {
            var endep = param.Advertising;
            var dao = new AdvertisingDao();
            param.Advertising.Id = dao.Insert(endep);
        }
        public void Update(AdvertisingParam param)
        {
            var endep = param.Advertising;
            var dao = new AdvertisingDao();
            dao.Update(endep);
        }
        public void Delete(AdvertisingParam param)
        {
            var dao = new AdvertisingDao();
            using (var tran = new TransactionScope())
            {
                var paramDep = param.Advertisings;
                foreach (var endep in paramDep)
                {
                    dao.Delete(endep);
                }
                tran.Complete();
            }
        }
        #endregion

        #region Query
        public void Search(AdvertisingParam param)
        {
            var dao = new AdvertisingDao();
            dao.Search(param);
        }
        public void GetById(AdvertisingParam param)
        {
            var dao = new AdvertisingDao();
            dao.GetById(param);
        }
        #endregion
    }
}
