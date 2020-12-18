using BO;
using Entities.Base;
using Entities.Param;
using Entities.Filter;
using MvcProject.Base;
using MvcProject.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Entities.Param.FrontEnd;
using Utilities;

namespace MvcProject.Controllers
{
    //Route : front/Home
    public class NewsController : FrontController
    {
        public ArticleBo ArticleBo = new ArticleBo();
        public ActionResult Detail(long newsId , string url)
        {
            var model = new NewsModel();

            #region Article
            ArticleParam articleParam = new ArticleParam() { ArticleFilter = new ArticleFilter() { NewsId = newsId , Status = (int)Utilities.Constants.RecordStatus.Published } };
            ArticleBo.GetById(articleParam);
            #endregion

            model.articleEntity = articleParam.ArticleEntity;
            if (model.articleEntity != null && model.articleEntity.NewsId > 0)
            {
                if (url.TrimEnd() != model.articleEntity.Url.TrimEnd())
                {
                    return Redirect(model.articleEntity.Url.TrimEnd());
                }
                #region Meta
                ViewBag.Title = model.articleEntity.Title;
                ViewBag.MetaSeo = Utilities.Utils.FillMeta(model.articleEntity.Title, model.articleEntity.Sapo, string.Join(", ", new List<string>()));
                ViewBag.OgImage = model.articleEntity.Avatar.ChangeThumbSize(500,0).BuildAbsoluteUrl(Config.DOMAIN);
                ViewBag.OgUrl = model.articleEntity.Title.BuildShortNewsUrl(model.articleEntity.NewsId).BuildAbsoluteUrl(Config.DOMAIN);
                ViewBag.Canonical = url.BuildAbsoluteUrl(Config.DOMAIN);
                #endregion
            }


            return View(model);
        }
    }
}