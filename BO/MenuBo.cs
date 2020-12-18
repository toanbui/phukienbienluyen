using DAO;
using Entities.Param;
using System.Transactions;

namespace BO
{
    public class MenuBo
    {
        #region Action
        public void Insert(MenuParam param)
        {
            var endep = param.Menu;
            var dao = new MenuDao();
            param.Menu.Id = dao.Insert(endep);
        }
        public void Update(MenuParam param)
        {
            var endep = param.Menu;
            var dao = new MenuDao();
            dao.Update(endep);
        }
        public void Delete(MenuParam param)
        {
            var dao = new MenuDao();
            using (var tran = new TransactionScope())
            {
                var paramDep = param.Menus;
                foreach (var endep in paramDep)
                {
                    dao.Delete(endep);
                }
                tran.Complete();
            }
        }
        #endregion

        #region Query
        public void Search(MenuParam param)
        {
            var dao = new MenuDao();
            dao.Search(param);
        }
        public void GetById(MenuParam param)
        {
            var dao = new MenuDao();
            dao.GetById(param);
        }
        #endregion
    }
}
