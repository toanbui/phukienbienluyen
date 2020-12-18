using System;
using Entities.Configs;
using System.Text.RegularExpressions;
using System.IO;
using System.Web;
using Utilities;
using System.Linq;
using System.Collections.Generic;

namespace BO.ReadConfigs
{
    public class RewriteRules
    {
        private static readonly Lazy<RewriteRules> _lazy = new Lazy<RewriteRules>(() => new RewriteRules());
        public static readonly RewriteRules Instance = _lazy.Value;

        private static RulesConfig _rule;
        private Rules item;
        private Regex rex;
        private Match match;
        private readonly TimeSpan _regexTimeout = TimeSpan.FromSeconds(1);

        public RewriteRules()
        {

        }
        public static string ReadFileConfig()
        {
            var result = "";
            try
            {
                var fileContents = System.IO.File.ReadAllText(HttpContext.Current.Server.MapPath(@"~/configs/rewriterules.json"));
                _rule = fileContents.JsonDeserialize<RulesConfig>();
            }
            catch (Exception ex)
            {

            }
            return result;
        }
        public Rules GetByUrl(string url)
        {
            string Action = "Index";
            string Controller = "Home";
            string queryString = "";
            ReadFileConfig();
            item = null;
            if (_rule != null && _rule.Rules?.Count > 0)
            {
                foreach (var rule in _rule.Rules)
                {
                    rex = new Regex("^" + rule.Url, RegexOptions.Compiled | RegexOptions.CultureInvariant, _regexTimeout);
                    match = rex.Match(url);
                    if (match.Success)
                    {
                        item = new Rules
                        {
                            Url = url,
                            UrlReplace = match.Result(rule.UrlReplace)
                        };
                        
                        if (!string.IsNullOrEmpty(item.UrlReplace))
                        {
                            // /home/about?id=1&title=toan
                            rex = new Regex("/([a-zA-Z0-9-]+)/([a-zA-Z0-9-]+)\\?([a-zA-Z0-9-=&/.]+)", RegexOptions.Compiled | RegexOptions.CultureInvariant, _regexTimeout);
                            match = rex.Match(item.UrlReplace);
                            if (match.Success)
                            {
                                Controller = match.Groups[1].Value;
                                Action = match.Groups[2].Value;

                                queryString = match.Groups[3].Value;

                                if (!string.IsNullOrEmpty(queryString))
                                {
                                    var arrQuery = queryString.Split('&');
                                    if (arrQuery != null && arrQuery.Any())
                                    {
                                        var querys = new List<QueryParam>();
                                        foreach (var item in arrQuery)
                                        {
                                            if (!string.IsNullOrEmpty(item))
                                            {
                                                var arrParam = item.Split('=');
                                                if (arrParam != null && arrParam.Any())
                                                {
                                                    querys.Add(new QueryParam() { Name = arrParam[0], Value = arrParam[1] });
                                                }
                                            }
                                        }
                                        item.Querys = querys;
                                    }
                                }

                                item.Action = Action;
                                item.Controller = Controller;
                            }
                            else
                            {
                                // /home?id=1&title=toan
                                rex = new Regex("/([a-zA-Z0-9-]+)\\?([a-zA-Z0-9-=&/._]+)", RegexOptions.Compiled | RegexOptions.CultureInvariant, _regexTimeout);
                                match = rex.Match(item.UrlReplace);

                                Controller = match.Groups[1].Value;
                                queryString = match.Groups[2].Value;

                                if (!string.IsNullOrEmpty(queryString))
                                {
                                    var arrQuery = queryString.Split('&');
                                    if (arrQuery != null && arrQuery.Any())
                                    {
                                        var querys = new List<QueryParam>();
                                        foreach (var item in arrQuery)
                                        {
                                            if (!string.IsNullOrEmpty(item))
                                            {
                                                var arrParam = item.Split('=');
                                                if (arrParam != null && arrParam.Any())
                                                {
                                                    querys.Add(new QueryParam() { Name = arrParam[0], Value = arrParam[1] });
                                                }
                                            }
                                        }
                                        item.Querys = querys;
                                    }
                                }
                                item.Action = Action;
                                item.Controller = Controller;
                            }
                        }
                        
                        break;
                    }
                }
            }
            return item;
        }
    }
}
