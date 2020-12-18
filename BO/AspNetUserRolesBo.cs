using DAO;
using Entities.Param;
using System.Transactions;

namespace BO
{
    public class AspNetUserRolesBo
    {
        #region Action
        public void Insert(AspNetUserRolesParam param)
        {
            var endep = param.AspNetUserRoles;
            var dao = new AspNetUserRolesDao();
            param.AspNetUserRoles.UserId = dao.Insert(endep);
        }
        public void Update(AspNetUserRolesParam param)
        {
            var endep = param.AspNetUserRoles;
            var dao = new AspNetUserRolesDao();
            dao.Update(endep);
        }
        public void Delete(AspNetUserRolesParam param)
        {
            var dao = new AspNetUserRolesDao();
            using (var tran = new TransactionScope())
            {
                var paramDep = param.AspNetUserRoless;
                foreach (var endep in paramDep)
                {
                    dao.Delete(endep);
                }
                tran.Complete();
            }
        }
        #endregion

        #region Query
        public void Search(AspNetUserRolesParam param)
        {
            var dao = new AspNetUserRolesDao();
            dao.Search(param);
        }
        public void GetById(AspNetUserRolesParam param)
        {
            var dao = new AspNetUserRolesDao();
            dao.GetById(param);
        }
        #endregion
    }
}
