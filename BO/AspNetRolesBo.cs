using DAO;
using Entities.Param;
using System.Transactions;

namespace BO
{
    public class AspNetRolesBo
    {
        #region Action
        public void Insert(AspNetRolesParam param)
        {
            var endep = param.AspNetRole;
            var dao = new AspNetRolesDao();
            param.AspNetRole.Id = dao.Insert(endep);
        }
        public void Update(AspNetRolesParam param)
        {
            var endep = param.AspNetRole;
            var dao = new AspNetRolesDao();
            dao.Update(endep);
        }
        public void Delete(AspNetRolesParam param)
        {
            var dao = new AspNetRolesDao();
            using (var tran = new TransactionScope())
            {
                var paramDep = param.AspNetRoles;
                foreach (var endep in paramDep)
                {
                    dao.Delete(endep);
                }
                tran.Complete();
            }
        }
        #endregion

        #region Query
        public void Search(AspNetRolesParam param)
        {
            var dao = new AspNetRolesDao();
            dao.Search(param);
        }
        public void GetById(AspNetRolesParam param)
        {
            var dao = new AspNetRolesDao();
            dao.GetById(param);
        }
        #endregion
    }
}
