using DAO;
using Entities.Param;
using System.Transactions;

namespace BO
{
    public class SysMenuBo
    {
        #region Action
        public void Insert(SysMenuParam param)
        {
            var endep = param.SysMenu;
            var dao = new SysMenuDao();
            param.SysMenu.Id = dao.Insert(endep);
        }
        public void Update(SysMenuParam param)
        {
            var endep = param.SysMenu;
            var dao = new SysMenuDao();
            dao.Update(endep);
        }
        public void Delete(SysMenuParam param)
        {
            var dao = new SysMenuDao();
            using (var tran = new TransactionScope())
            {
                var paramDep = param.SysMenus;
                foreach (var endep in paramDep)
                {
                    dao.Delete(endep);
                }
                tran.Complete();
            }
        }
        #endregion

        #region Query
        public void Search(SysMenuParam param)
        {
            var dao = new SysMenuDao();
            dao.Search(param);
        }
        public void GetById(SysMenuParam param)
        {
            var dao = new SysMenuDao();
            dao.GetById(param);
        }
        #endregion
    }
}
