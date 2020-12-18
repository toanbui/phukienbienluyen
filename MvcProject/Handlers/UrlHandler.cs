using System.Linq;
using System.Web.Mvc;
using System;
using BO.ReadConfigs;
using Entities.Configs;

namespace MvcProject.Handlers
{
    public sealed class UrlHandler
    {
        public static Rules GetRoute(string url)
        {
            url = url ?? "/";
            url = url.ToLower();

            var rewrite = RewriteRules.Instance.GetByUrl(url);

            return rewrite;
        }
    }
}