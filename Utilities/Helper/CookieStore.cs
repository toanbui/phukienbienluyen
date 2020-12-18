using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;

namespace Utilities.Helper
{
    public class CookieStore
    {
        private const string KeyEncode = "MvcCookie";
        public static void SetCookie(string key, string value, TimeSpan expires)
        {
            HttpCookie cookie = new HttpCookie(key, Encrypt(value));

            if (HttpContext.Current.Request.Cookies[key] != null)
            {
                var cookieOld = HttpContext.Current.Request.Cookies[key];
                cookieOld.Expires = DateTime.Now.Add(expires);
                cookieOld.Value = cookie.Value;
                HttpContext.Current.Response.Cookies.Add(cookieOld);
            }
            else
            {
                cookie.Expires = DateTime.Now.Add(expires);
                HttpContext.Current.Response.Cookies.Add(cookie);
            }
        }
        public static string GetCookie(string key)
        {
            try
            {
                string value = string.Empty;
                HttpCookie cookie = HttpContext.Current.Request.Cookies[key];

                if (cookie != null)
                {
                    // For security purpose, we need to encrypt the value.
                    value = Decrypt(cookie.Value);
                }
                return value;
            }
            catch{}
            return "";
        }
        public static T GetCookie<T>(string key)
        {
            try
            {
                string value = string.Empty;
                HttpCookie cookie = HttpContext.Current.Request.Cookies[key];

                if (cookie != null)
                {
                    // For security purpose, we need to encrypt the value.
                    value = cookie.Value;
                    if (!string.IsNullOrEmpty(value))
                    {
                        value = Decrypt(value);
                        return value.JsonToObject<T>();
                    }
                }
            }
            catch {}
            return default(T);
        }
        public static void RemoveByKey(string key)
        {
            if (HttpContext.Current.Request.Cookies[key] != null)
            {
                var c = new HttpCookie(key);
                c.Expires = DateTime.Now.AddDays(-1);
                HttpContext.Current.Response.Cookies.Add(c);
            }
        }
        public static string Encrypt(string value)
        {
            try
            {
                var cookieText = Encoding.UTF8.GetBytes(value);
                return Convert.ToBase64String(MachineKey.Protect(cookieText, KeyEncode));
            }
            catch{}
            return "";
        }
        public static string Decrypt(string value)
        {
            try
            {
                var bytes = Convert.FromBase64String(value);
                var output = MachineKey.Unprotect(bytes, KeyEncode);
                return Encoding.UTF8.GetString(output);
            }
            catch{}
            return "";
        }
    }
}
