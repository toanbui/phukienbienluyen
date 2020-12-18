using DAO;
using Entities.Param;
using System.Transactions;

namespace BO
{
    public class DistrictBo
    {
        #region Action
        public void Insert(DistrictParam param)
        {
            var endep = param.District;
            var dao = new DistrictDao();
            param.District.Id = dao.Insert(endep);
        }
        public void Update(DistrictParam param)
        {
            var endep = param.District;
            var dao = new DistrictDao();
            dao.Update(endep);
        }
        public void Delete(DistrictParam param)
        {
            var dao = new DistrictDao();
            using (var tran = new TransactionScope())
            {
                var paramDep = param.Districts;
                foreach (var endep in paramDep)
                {
                    dao.Delete(endep);
                }
                tran.Complete();
            }
        }
        #endregion

        #region Query
        public void Search(DistrictParam param)
        {
            var dao = new DistrictDao();
            dao.Search(param);
        }
        public void GetById(DistrictParam param)
        {
            var dao = new DistrictDao();
            dao.GetById(param);
        }
        #endregion
    }
}
