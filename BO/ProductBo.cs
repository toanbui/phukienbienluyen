using DAO;
using Entities.Param;
using System.Transactions;

namespace BO
{
    public class ProductBo
    {
        #region Action
        public void Insert(ProductParam param)
        {
            var endep = param.Product;
            var dao = new ProductDao();
            param.Product.Id = dao.Insert(endep);
        }
        public void Update(ProductParam param)
        {
            var endep = param.Product;
            var dao = new ProductDao();
            dao.Update(endep);
        }
        public void Delete(ProductParam param)
        {
            var dao = new ProductDao();
            using (var tran = new TransactionScope())
            {
                var paramDep = param.Products;
                foreach (var endep in paramDep)
                {
                    dao.Delete(endep);
                }
                tran.Complete();
            }
        }
        #endregion

        #region Query
        public void Search(ProductParam param)
        {
            var dao = new ProductDao();
            dao.Search(param);
        }
        public void GetById(ProductParam param)
        {
            var dao = new ProductDao();
            dao.GetById(param);
        }
        #endregion

        #region Front End
        public void GetByListId(ProductParam param)
        {
            var dao = new ProductDao();
            dao.GetByListId(param);
        }
        public void FeSearchOrderByPrice(ProductParam param)
        {
            var dao = new ProductDao();
            dao.FeSearchOrderByPrice(param);
        }
        #endregion
    }
}
