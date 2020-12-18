using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using static Utilities.Constants;

namespace Utilities
{
    public class Utils
    {
        private static Dictionary<string, List<SelectListItem>> _Source = new Dictionary<string, List<SelectListItem>>();
        public static string GetAppConfig(string name)
        {
            return ConfigurationManager.AppSettings[name] ?? string.Empty;
        }
        public static List<SelectListItem> GetEnumSource<T>(int? value = null)
        {
            List<SelectListItem> result = null;
            string key = (typeof(T)).Name;
            if (!_Source.ContainsKey(key))
            {
                result = new List<SelectListItem>();
                _Source.Add(key, result);

                foreach (T item in Enum.GetValues(typeof(T)))
                {
                    result.Add(new SelectListItem { Text = item.ToString(), Value = ((int)(object)item).ToString(), Selected = value.HasValue && value.Value == ((int)(object)item) });
                }
            }

            if (value.HasValue)
            {
                string stringValue = value.Value.ToString();
                foreach (SelectListItem item in _Source[key])
                {
                    item.Selected = stringValue.Equals(item.Value);
                }
            }
            else
            {
                foreach (SelectListItem item in _Source[key])
                {
                    item.Selected = false;
                }
            }

            return _Source[key];
        }
        public static List<SelectListItem> GetStatusList(int? value = null)
        {
            var list = GetEnumSource<RecordStatus>(value);
            if (list != null && list.Any())
            {
                foreach (var item in list)
                {
                    switch (item.Text)
                    {
                        case "Draft":
                            {
                                item.Text = "Nháp";
                                break;
                            }
                        case "Pending":
                            {
                                item.Text = "Chờ duyệt";
                                break;
                            }
                        case "Published":
                            {
                                item.Text = "Hoạt động";
                                break;
                            }
                        case "UnPublished":
                            {
                                item.Text = "Ngừng hoạt động";
                                break;
                            }
                        case "Locked":
                            {
                                item.Text = "Khóa";
                                break;
                            }
                        default:
                            break;
                    }
                }
            }
            return list;
        }
        public static long GeneratorId()
        {
            return DateTime.Now.ToString("yyyyMMddHHmmss").ChangeType<long>();
        }
        public static string FillMeta(string title, string description, string keyword)
        {
            string metaDesc = string.Format("<meta name=\"description\" content=\"{0}\" />\r\n", description.EncodeTitle());
            string metaKeyword = string.Format("\t<meta name=\"keywords\" content=\"{0}\" />\r\n", keyword);
            string metaNewsKeyword = string.Format("\t<meta name=\"news_keywords\" content=\"{0}\" />\r\n", keyword);
            string metaOgTitle = string.Format("\t<meta property=\"og:title\" content=\"{0}\" />\r\n", title.EncodeTitle());
            string metaOgDesc = string.Format("\t<meta property=\"og:description\" content=\"{0}\" />", description.EncodeTitle());

            return string.Concat(metaDesc, metaKeyword, metaNewsKeyword, metaOgTitle, metaOgDesc);
        }
    }
}
