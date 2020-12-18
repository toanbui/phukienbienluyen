using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities
{
    public class Config
    {
        public static string DOMAIN
        {
            get
            {
                return Utils.GetAppConfig("Domain").TrimEnd('/');
            }
        }
        public static string PageExtension
        {
            get
            {
                return Utils.GetAppConfig("PageExtension").TrimEnd('/');
            }
        }
        public static string WaterMark
        {
            get
            {
                return Utils.GetAppConfig("WaterMark").TrimEnd('/');
            }
        }
        public static bool GeneratorCode
        {
            get
            {
                return (Utils.GetAppConfig("GeneratorCode") == "1" || Utils.GetAppConfig("GeneratorCode") == "true");
            }
        }
        public static bool AllowWaterMark
        {
            get
            {
                return (Utils.GetAppConfig("AllowWaterMark") == "1" || Utils.GetAppConfig("AllowWaterMark") == "true");
            }
        }
    }
}
