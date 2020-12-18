using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;
using Utilities;

namespace CacheHelper
{
    public class CacheController
    {
        public static void SaveToCacheIIS(string cacheName, object data, int timeExpire)
        {
            if (data != null && (Utils.GetAppConfig("AllowCachePage") == "1" || Utils.GetAppConfig("AllowCachePage") == "true"))
                HttpContext.Current.Cache.Insert(cacheName, data, null, DateTime.Now.AddMinutes(timeExpire), Cache.NoSlidingExpiration);
        }
        public static T GetFromCacheIIS<T>(String key)
        {
            if (HttpContext.Current.Cache[key] != null && (Utils.GetAppConfig("AllowCachePage") == "1" || Utils.GetAppConfig("AllowCachePage") == "true"))
            {
                T result = (T)HttpContext.Current.Cache[key];
                return result;
            }
            else
            {
                return default(T);
            }
        }
        public static string GetFromCacheIIS(String key)
        {
            if (Utils.GetAppConfig("AllowCachePage") == "1" || Utils.GetAppConfig("AllowCachePage") == "true")
            {
                if (HttpContext.Current.Cache[key] != null)
                {
                    return HttpContext.Current.Cache[key].ToString();
                }
            }
            return "";
        }
        public static void FlushAll()
        {
            foreach (System.Collections.DictionaryEntry entry in HttpContext.Current.Cache)
            {
                HttpContext.Current.Cache.Remove((string)entry.Key);
            }
        }
    }
}
