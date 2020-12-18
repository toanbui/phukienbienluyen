using DAO;
using Entities.Param;
using System.Transactions;

namespace BO
{
    public class ConfigurationBo
    {
        #region Action
        public void Insert(ConfigurationParam param)
        {
            var endep = param.Configuration;
            var dao = new ConfigurationDao();
            param.Configuration.Id = dao.Insert(endep);
        }
        public void Update(ConfigurationParam param)
        {
            var endep = param.Configuration;
            var dao = new ConfigurationDao();
            dao.Update(endep);
        }
        public void Delete(ConfigurationParam param)
        {
            var dao = new ConfigurationDao();
            using (var tran = new TransactionScope())
            {
                var paramDep = param.Configurations;
                foreach (var endep in paramDep)
                {
                    dao.Delete(endep);
                }
                tran.Complete();
            }
        }
        #endregion

        #region Query
        public void Search(ConfigurationParam param)
        {
            var dao = new ConfigurationDao();
            dao.Search(param);
        }
        public void GetById(ConfigurationParam param)
        {
            var dao = new ConfigurationDao();
            dao.GetById(param);
        }
        #endregion
    }
}
