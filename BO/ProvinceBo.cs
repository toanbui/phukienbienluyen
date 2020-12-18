using DAO;
using Entities.Param;
using System.Transactions;

namespace BO
{
    public class ProvinceBo
    {
        #region Action
        public void Insert(ProvinceParam param)
        {
            var endep = param.Province;
            var dao = new ProvinceDao();
            param.Province.Id = dao.Insert(endep);
        }
        public void Update(ProvinceParam param)
        {
            var endep = param.Province;
            var dao = new ProvinceDao();
            dao.Update(endep);
        }
        public void Delete(ProvinceParam param)
        {
            var dao = new ProvinceDao();
            using (var tran = new TransactionScope())
            {
                var paramDep = param.Provinces;
                foreach (var endep in paramDep)
                {
                    dao.Delete(endep);
                }
                tran.Complete();
            }
        }
        #endregion

        #region Query
        public void Search(ProvinceParam param)
        {
            var dao = new ProvinceDao();
            dao.Search(param);
        }
        public void GetById(ProvinceParam param)
        {
            var dao = new ProvinceDao();
            dao.GetById(param);
        }
        #endregion
    }
}
