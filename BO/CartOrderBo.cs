using DAO;
using Entities.Param;
using System.Transactions;

namespace BO
{
    public class CartOrderBo
    {
        #region Action
        public void Insert(CartOrderParam param)
        {
            var endep = param.CartOrder;
            var dao = new CartOrderDao();
            param.CartOrder.Id = dao.Insert(endep);
        }
        public void Update(CartOrderParam param)
        {
            var endep = param.CartOrder;
            var dao = new CartOrderDao();
            dao.Update(endep);
        }
        public void Approve(CartOrderParam param)
        {
            var endep = param.CartOrder;
            var dao = new CartOrderDao();
            dao.Approve(endep);
        }
        public void Delete(CartOrderParam param)
        {
            var dao = new CartOrderDao();
            using (var tran = new TransactionScope())
            {
                var paramDep = param.CartOrders;
                foreach (var endep in paramDep)
                {
                    dao.Delete(endep);
                }
                tran.Complete();
            }
        }
        #endregion

        #region Query
        public void Search(CartOrderParam param)
        {
            var dao = new CartOrderDao();
            dao.Search(param);
        }
        public void GetById(CartOrderParam param)
        {
            var dao = new CartOrderDao();
            dao.GetById(param);
        }
        #endregion
    }
}
