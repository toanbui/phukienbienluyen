using DAO;
using Entities.Param;
using System.Transactions;

namespace BO
{
    public class CustomerToolsBo
    {
        #region Action
        public void Insert(CustomerToolsParam param)
        {
            var endep = param.CustomerTool;
            var dao = new CustomerToolsDao();
            dao.Insert(endep);
        }
        public void Update(CustomerToolsParam param)
        {
            var endep = param.CustomerTool;
            var dao = new CustomerToolsDao();
            dao.Update(endep);
        }
        public void Delete(CustomerToolsParam param)
        {
            var dao = new CustomerToolsDao();
            using (var tran = new TransactionScope())
            {
                var paramDep = param.CustomerTools;
                foreach (var endep in paramDep)
                {
                    dao.Delete(endep);
                }
                tran.Complete();
            }
        }
        #endregion

        #region Query
        public void Search(CustomerToolsParam param)
        {
            var dao = new CustomerToolsDao();
            dao.Search(param);
        }
        public void GetById(CustomerToolsParam param)
        {
            var dao = new CustomerToolsDao();
            dao.GetById(param);
        }
        #endregion
    }
}
