using DAO;
using Entities.Param;
using System.Transactions;

namespace BO
{
    public class ArticleBo
    {
        #region Action
        public void Insert(ArticleParam param)
        {
            var endep = param.Article;
            var dao = new ArticleDao();
            param.Article.Id = dao.Insert(endep);
        }
        public void Update(ArticleParam param)
        {
            var endep = param.Article;
            var dao = new ArticleDao();
            dao.Update(endep);
        }
        public void Delete(ArticleParam param)
        {
            var dao = new ArticleDao();
            using (var tran = new TransactionScope())
            {
                var paramDep = param.Articles;
                foreach (var endep in paramDep)
                {
                    dao.Delete(endep);
                }
                tran.Complete();
            }
        }
        #endregion

        #region Query
        public void Search(ArticleParam param)
        {
            var dao = new ArticleDao();
            dao.Search(param);
        }
        public void GetById(ArticleParam param)
        {
            var dao = new ArticleDao();
            dao.GetById(param);
        }
        #endregion
    }
}
