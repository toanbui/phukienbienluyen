using Entities.Entities.FrontEnd;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Utilities
{
    public static class Method
    {
        public static string UrlEncode(this string s)
        {
            return HttpUtility.UrlEncode(s);
        }
        public static string CreateMD5(this string input)
        {
            input += "minhthudautay";
            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }

        public static string EncodeTitle(this object s)
        {
            return HttpUtility.HtmlEncode(s);
        }

        public static string ToKoDau(this string s)
        {
            return ConvertHelper.UnicodeToKoDau(s);
        }

        public static string ToKoDauAndGach(this string s)
        {
            if (string.IsNullOrEmpty(s)) return "";
            return ConvertHelper.UnicodeToKoDauAndGach(s);
        }

        //public static string RemoveDangerousTag(this string s)
        //{
        //    return Utils.RemoveDangerousTag(s);
        //}

        public static T ChangeType<T>(this object o)
        {
            return ConvertHelper.ConvertTo<T>(o);
        }

        //public static List<T> ConvertDataTable<T>(this DataTable dt)
        //{
        //    return Utils.ConvertDataTable<T>(dt);
        //}

        public static string JsonSerialize(this object o)
        {
            return JsonManager.Converter.Serialize(o);
        }

        public static T JsonDeserialize<T>(this string s)
        {
            return JsonManager.Converter.Deserialize<T>(s);
        }

        /// <summary>
        /// Json To Object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="keys"></param>
        /// <returns></returns>
        public static IEnumerable<T> JsonToListObject<T>(this IEnumerable<string> listJson)
        {
            foreach (var key in listJson)
            {
                if(!string.IsNullOrEmpty(key))
                    yield return JsonManager.Converter.Deserialize<T>(key);
            }
        }
        public static T JsonToObject<T>(this string json)
        {
            return JsonManager.Converter.Deserialize<T>(json);
        }

        public static string ToStringList(this List<string> source)
        {
            if (source == null) return "";
            return string.Join(";", source);
        }
        public static List<string> ToList(this string source)
        {
            if (source == null || string.IsNullOrEmpty(source)) return null;
            var result = new List<string>();
            var array = source.Split(';');
            if (array != null && array.Any())
            {
                for (int i = 0; i < array.Length; i++)
                {
                    result.Add(array[i]);
                }
            }
            return result;
        }
        public static string ToCurrency(this int value)
        {
            return String.Format("{0:C}", value);
        }
        public static string ToCurrency(this long value)
        {
            var info = System.Globalization.CultureInfo.GetCultureInfo("vi-VN");
            return String.Format(info, "{0:c}", value);
        }
        public static CartEntity GetValue(this CartEntity source)
        {
            if (source == null) return null;
            var query = from n in source.ListCart
                        select new ProductInCart
                        {
                            Number = n.Number,
                            Price = n.Price,
                            Id = n.Id
                        };
            var myCart = new CartEntity();
            myCart.ListCart = query.ToList();
            return myCart;
        }
        public static string ChangeThumbSize(this string imgPath, int w, int h)
        {
            if (string.IsNullOrEmpty(imgPath)) return "";
            if (h == 0)
                return ChangeThumbWidth(imgPath, w);
            //api/thumbimage?w=200&h=200&path=/Upload/Product/2019_2_16/system2e30ede4-dce4-45a6-8b40-6a30773e9215.jpg
            return string.Format("/api/thumb?w={0}&h={1}&path=/{2}", w, h, imgPath.TrimStart('/'));
        }
        public static string ChangeThumbWidth(this string imgPath, int w)
        {
            //api/thumbimage?w=200&path=/Upload/Product/2019_2_16/system2e30ede4-dce4-45a6-8b40-6a30773e9215.jpg
            return string.Format("/api/thumb?w={0}&path=/{1}", w,imgPath.TrimStart('/'));
        }
        public static string ChangeThumbSize(this string imgPath, int ow, int oh, int nw, int nh)
        {
            return imgPath.Replace($"{ow}_{oh}", $"{nw}_{nh}");
        }
        public static string BuildNewsUrl(this string title, long newsId)
        {
            if (string.IsNullOrEmpty(title)) return "";
            return string.Format("/tin-tuc/{0}-{1}{2}", title.ToKoDauAndGach(), newsId, Config.PageExtension);
        }
        public static string BuildShortNewsUrl(this string title, long newsId)
        {
            if (string.IsNullOrEmpty(title)) return "";
            return string.Format("/tin-tuc-{0}{1}", newsId, Config.PageExtension);
        }
        public static string BuildAbsoluteUrl(this string relativeUrl, string domain = "")
        {
            if (string.IsNullOrEmpty(relativeUrl) || relativeUrl.StartsWith("http"))
                return relativeUrl;
            return string.Format("{0}/{1}", string.IsNullOrEmpty(domain) ? Config.DOMAIN.TrimEnd('/') : domain.TrimEnd('/'), relativeUrl.TrimStart('/'));
        }
        public static string BuildKeyCache(this string url)
        {
            return url.BuildAbsoluteUrl();
        }
        public static List<string> GetRoles(this IPrincipal user)
        {
            return ((ClaimsIdentity)user.Identity).Claims
                           .Where(c => c.Type == ClaimTypes.Role)
                           .Select(c => c.Value)
                           .ToList();
        }
        public static string BuildViewPath(this ControllerContext context , bool IsAdmin = false)
        {
            var action = context.RouteData.GetRequiredString("action");
            var controller = context.RouteData.GetRequiredString("controller");
            string viewPath = "";
            viewPath = IsAdmin ? string.Format("~/Views/Admin/{0}/{1}.cshtml", controller,action) : string.Format("~/Views/{0}/{1}.cshtml", controller, action);
            return viewPath;
        }
        public static string BuildViewPath(this ControllerContext context , string action = "", bool IsAdminView = false)
        {
            action = string.IsNullOrEmpty(action) ? context.RouteData.GetRequiredString("action") : action;
            var controller = context.RouteData.GetRequiredString("controller");
            string viewPath = "";
            viewPath = IsAdminView ? string.Format("~/Views/Admin/{0}/{1}.cshtml", controller,action) : string.Format("~/Views/{0}/{1}.cshtml", controller, action);
            return viewPath;
        }
        public static string BuildViewPath(this string viewPath, string controller, string action , bool IsAdmin = false)
        {
            viewPath = IsAdmin ? string.Format("~/Views/Admin/{0}/{1}.cshtml", controller,action) : string.Format("~/Views/{0}/{1}.cshtml", controller, action);
            return viewPath;
        }

    }
}