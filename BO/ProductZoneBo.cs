using DAO;
using Entities.Param;
using System.Transactions;

namespace BO
{
    public class ProductZoneBo
    {
        #region Action
        public void Insert(ProductZoneParam param)
        {
            var endep = param.ProductZone;
            var dao = new ProductZoneDao();
            param.ProductZone.Id = dao.Insert(endep);
        }
        public void Update(ProductZoneParam param)
        {
            var endep = param.ProductZone;
            var dao = new ProductZoneDao();
            dao.Update(endep);
        }
        public void Delete(ProductZoneParam param)
        {
            var dao = new ProductZoneDao();
            using (var tran = new TransactionScope())
            {
                var paramDep = param.ProductZones;
                foreach (var endep in paramDep)
                {
                    dao.Delete(endep);
                }
                tran.Complete();
            }
        }
        #endregion

        #region Query
        public void Search(ProductZoneParam param)
        {
            var dao = new ProductZoneDao();
            dao.Search(param);
        }
        public void GetById(ProductZoneParam param)
        {
            var dao = new ProductZoneDao();
            dao.GetById(param);
        }
        #endregion
    }
}
