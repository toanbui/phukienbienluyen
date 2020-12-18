using DAO;
using Entities.Param;
using System.Transactions;

namespace BO
{
    public class PropsOfProductBo
    {
        #region Action
        public void Insert(PropsOfProductParam param)
        {
            var endep = param.PropsOfProduct;
            var dao = new PropsOfProductDao();
            param.PropsOfProduct.Id = dao.Insert(endep);
        }
        public void Update(PropsOfProductParam param)
        {
            var endep = param.PropsOfProduct;
            var dao = new PropsOfProductDao();
            dao.Update(endep);
        }
        public void Delete(PropsOfProductParam param)
        {
            var dao = new PropsOfProductDao();
            using (var tran = new TransactionScope())
            {
                var paramDep = param.PropsOfProducts;
                foreach (var endep in paramDep)
                {
                    dao.Delete(endep);
                }
                tran.Complete();
            }
        }
        #endregion

        #region Query
        public void Search(PropsOfProductParam param)
        {
            var dao = new PropsOfProductDao();
            dao.Search(param);
        }
        public void GetById(PropsOfProductParam param)
        {
            var dao = new PropsOfProductDao();
            dao.GetById(param);
        }
        #endregion
    }
}
