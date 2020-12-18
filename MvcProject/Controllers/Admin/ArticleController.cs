using BO;
using Entities.Base;
using Entities.Entities;
using Entities.Filter;
using Entities.Param;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using MvcProject.Base;
using MvcProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using Utilities;
using Utilities.Helper;

namespace MvcProject.Controllers.Admin
{
    [AdminAuthorize(Roles = "Article , Manager , Admin")]
    public class ArticleController : AdminController
    {
        // GET: Article
        private ArticleBo _bo = new ArticleBo();
        ApplicationDbContext context = new ApplicationDbContext();
        public ActionResult Index()
        {
            ViewBag.Status = Utils.GetStatusList();
            return View();
        }
        public ActionResult AjaxLoadList(GridFilterSetting<ArticleEntity> gridFilterSetting, string keysearch , int? status)
        {
            var pagininfo = new EtsPaging { RowStart = gridFilterSetting.iDisplayStart, PageSize = gridFilterSetting.iDisplayLength };
            var param = new ArticleParam() { PagingInfo = pagininfo };
            var ArticleFilter = new ArticleFilter() { keysearch = keysearch, Status = status , OrderDateDesc = true };
            param.ArticleFilter = ArticleFilter;
            _bo.Search(param);
            long count = pagininfo.RecordCount;
            return Json(new { aaData = param.ArticleEntitys, recordsTotal = count, recordsFiltered = count, amount = 0x2710 }, JsonRequestBehavior.AllowGet);
        }
        public List<TagInNewsEntity> GetTagInNews(long newsId)
        {
            var param = new TagInNewsParam();
            param.TagInNewsFilter = new TagInNewsFilter() { NewsId = newsId };
            new TagInNewsBo().Search(param);
            return param.TagInNewsEntitys;
        }
        public List<TagEntity> GetAllTag()
        {
            var param = new TagParam();
            param.TagFilter = new TagFilter() { Status = (int)Utilities.Constants.RecordStatus.Published };
            new TagBo().Search(param);
            return param.TagEntitys;
        }
        public ActionResult Create(string Id , string NewsId)
        {
            var _Id = 0;
            long _NewsId = 0;
            var param = new ArticleParam();

            Int64.TryParse(NewsId, out _NewsId);

            ViewBag.Tags = GetAllTag();
            param.TagInNewsIds = GetTagInNews(_NewsId).Select(i => i.TagId ?? 0).ToList();

            if (Int32.TryParse(Id, out _Id))
            {
                param.ArticleFilter = new ArticleFilter() { Id = _Id };
                _bo.GetById(param);
                ViewBag.Status = Utils.GetStatusList(param.Article.Status);

                
                param.TagInNewsEntitys = GetTagInNews(_NewsId);
            }
            else
            {
                param.Article = new Article();
                ViewBag.Status = Utils.GetStatusList(0);
                ViewBag.Create = true;
            }
            return PartialView(param);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult Create(ArticleParam modelInput, List<int> hdTag)
        {
            try
            {
                if (modelInput != null && modelInput.Article != null)
                {
                    var Imagelink = UploadHelper.UpLoadFile(Request.Files["AvatarInput"], "Upload/Article/");
                    if (!string.IsNullOrEmpty(Imagelink))
                    {
                        modelInput.Article.Avatar = Imagelink;
                    }

                    if (modelInput.Article.Id > 0)
                    {
                        modelInput.Article.Modified = DateTime.Now;
                        modelInput.Article.ModifiedBy = User.Identity.Name;
                        _bo.Update(modelInput);
                        return Json(new { isSuccess = true, mess = Resources.Message.Msg_UpdateSuccesfull }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        modelInput.Article.Created = DateTime.Now;
                        modelInput.Article.Modified = DateTime.Now;
                        modelInput.Article.CreatedBy = User.Identity.Name;
                        modelInput.Article.ModifiedBy = User.Identity.Name;
                        modelInput.Article.NewsId = Utils.GeneratorId();
                        modelInput.Article.Url = modelInput.Article.Title.BuildNewsUrl(modelInput.Article.NewsId);
                        _bo.Insert(modelInput);

                        if (hdTag != null && hdTag.Any() && modelInput.Article.Id > 0)
                        {

                            var _tagInNewsBo = new TagInNewsBo();
                            foreach (var item in hdTag)
                            {
                                var TagInNews = new TagInNew() { TagId = item, NewsId = modelInput.Article.Id };
                                var _paramPropsOfProduct = new TagInNewsParam() { TagInNews = TagInNews };
                                _tagInNewsBo.Insert(_paramPropsOfProduct);
                            }

                        }
                        return Json(new { isSuccess = true, mess = Resources.Message.Msg_AddnewSuccesfull }, JsonRequestBehavior.AllowGet);
                    }
                }
                return Json(new { isSuccess = false, mess = Resources.Message.Msg_Invalid }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { isSuccess = false, mess = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult Delete(int? Id)
        {
            var param = new ArticleParam();
            param.ArticleFilter = new ArticleFilter() { Id = Id ?? 0 };
            _bo.GetById(param);
            var item = param.Article;
            if (item == null)
            {
                ViewBag.Error = Resources.Message.Error_NotExit;
                return PartialView(new Article());
            }
            else
                return PartialView(item);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Article obj)
        {
            try
            {
                var list = new List<Article> { obj };
                var param = new ArticleParam { Articles = list };
                _bo.Delete(param);
                return Json(new { isSuccess = true, mess = Resources.Message.Msg_DeleteSuccesfull }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { isSuccess = false, mess = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult DeleteTag(string id, string deleteId)
        {
            long _id = 0;
            var _deleteId = 0;

            if (Int64.TryParse(id, out _id))
            {
                if (Int32.TryParse(deleteId, out _deleteId))
                {
                    var BO = new TagInNewsBo();
                    var param = new TagInNewsParam() { TagInNewsFilter = new TagInNewsFilter() { NewsId = _id, TagId = _deleteId } };
                    BO.Search(param);
                    if (param.TagInNewsEntitys != null)
                    {
                        var item = param.TagInNewsEntitys.FirstOrDefault();
                        var list = new List<TagInNew> { item };
                        var paramDelete = new TagInNewsParam { TagInNewss = list };
                        BO.Delete(paramDelete);
                        return Json(new { isSuccess = false, mess = Resources.Message.Msg_Successfull }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            return Json(new { isSuccess = false, mess = Resources.Message.Msg_Invalid }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult AddTag(string id, string addId)
        {
            long _id = 0;
            var _addId = 0;

            if (Int64.TryParse(id, out _id))
            {
                if (Int32.TryParse(addId, out _addId))
                {
                    var BO = new TagInNewsBo();
                    var item = new TagInNew() { NewsId = _id, TagId = _addId };
                    var param = new TagInNewsParam() { TagInNews = item };
                    BO.Insert(param);
                    return Json(new { isSuccess = false, mess = Resources.Message.Msg_Successfull }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { isSuccess = false, mess = Resources.Message.Msg_Invalid }, JsonRequestBehavior.AllowGet);
        }
    }
}