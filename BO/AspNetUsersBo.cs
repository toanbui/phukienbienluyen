using DAO;
using Entities.Param;
using System.Transactions;

namespace BO
{
    public class AspNetUsersBo
    {
        #region Action
        public void Insert(AspNetUsersParam param)
        {
            var endep = param.AspNetUser;
            var dao = new AspNetUsersDao();
            param.AspNetUser.Id = dao.Insert(endep);
        }
        public void Update(AspNetUsersParam param)
        {
            var endep = param.AspNetUser;
            var dao = new AspNetUsersDao();
            dao.Update(endep);
        }
        public void Delete(AspNetUsersParam param)
        {
            var dao = new AspNetUsersDao();
            using (var tran = new TransactionScope())
            {
                var paramDep = param.AspNetUsers;
                foreach (var endep in paramDep)
                {
                    dao.Delete(endep);
                }
                tran.Complete();
            }
        }
        public void ChangeStatus(AspNetUsersParam param)
        {
            var endep = param.AspNetUser;
            var dao = new AspNetUsersDao();
            dao.ChangeStatus(endep);
        }
        #endregion

        #region Query
        public void Search(AspNetUsersParam param)
        {
            var dao = new AspNetUsersDao();
            dao.Search(param);
        }
        public void GetById(AspNetUsersParam param)
        {
            var dao = new AspNetUsersDao();
            dao.GetById(param);
        }
        #endregion
    }
}
