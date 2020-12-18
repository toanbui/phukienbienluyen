using DAO;
using Entities.Param;
using System.Transactions;

namespace BO
{
    public class ProductPropsBo
    {
        #region Action
        public void Insert(ProductPropsParam param)
        {
            var endep = param.ProductProps;
            var dao = new ProductPropsDao();
            param.ProductProps.Id = dao.Insert(endep);
        }
        public void Update(ProductPropsParam param)
        {
            var endep = param.ProductProps;
            var dao = new ProductPropsDao();
            dao.Update(endep);
        }
        public void Delete(ProductPropsParam param)
        {
            var dao = new ProductPropsDao();
            using (var tran = new TransactionScope())
            {
                var paramDep = param.ProductPropss;
                foreach (var endep in paramDep)
                {
                    dao.Delete(endep);
                }
                tran.Complete();
            }
        }
        #endregion

        #region Query
        public void Search(ProductPropsParam param)
        {
            var dao = new ProductPropsDao();
            dao.Search(param);
        }
        public void GetById(ProductPropsParam param)
        {
            var dao = new ProductPropsDao();
            dao.GetById(param);
        }
        #endregion
    }
}
