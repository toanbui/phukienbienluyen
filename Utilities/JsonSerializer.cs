using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Utilities
{
    public interface IJsonConverter
    {
        /// <summary>
        /// Serialize object with converter
        /// </summary>
        /// <param name="obj">Object to be converted</param>
        /// <returns></returns>
        string Serialize(object obj);

        /// <summary>
        /// Serialize object with converter
        /// </summary>
        /// <param name="obj">Object to be converted</param>
        /// <param name="isJavascriptDate">
        /// true: use Javascript datetime convert
        /// </param>
        /// <returns></returns>
        string Serialize(object obj, bool isJavascriptDate);

        /// <summary>
        /// Serialize object with converter
        /// </summary>
        /// <param name="obj">Object to be converted</param>
        /// <param name="customDate">
        /// Use datetime convert
        /// </param>
        /// <returns></returns>
        string Serialize(object obj, string customDate);

        /// <summary>
        /// Deserialize object with converter
        /// </summary>
        /// <typeparam name="T">Return object type</typeparam>
        /// <param name="json">String json to convert</param>
        /// <returns></returns>
        T Deserialize<T>(string json);

        /// <summary>
        /// Deserialize object with converter
        /// </summary>
        /// <typeparam name="T">Return object type</typeparam>
        /// <param name="json">String json to convert</param>
        /// <param name="isJavascriptDate">
        /// true: use Javascript datetime convert
        /// </param>
        /// <returns></returns>
        T Deserialize<T>(string json, bool isJavascriptDate);

        /// <summary>
        /// Deserializes the specified json.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json">The json.</param>
        /// <param name="customDate">The custom date.</param>
        /// <returns></returns>
        T Deserialize<T>(string json, string customDate);
    }

    public class NewtonsoftJsonConverter : IJsonConverter
    {
        /// <summary>
        /// Serialize object with converter
        /// </summary>
        /// <param name="obj">Object to be converted</param>
        /// <returns></returns>
        public string Serialize(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        /// <summary>
        /// Serialize object with converter
        /// </summary>
        /// <param name="obj">Object to be converted</param>
        /// <param name="isJavascriptDate">
        /// true: use Javascript datetime convert
        /// </param>
        /// <returns></returns>
        public string Serialize(object obj, bool isJavascriptDate)
        {
            return isJavascriptDate
                       ? JsonConvert.SerializeObject(obj, new JavaScriptDateTimeConverter())
                       : JsonConvert.SerializeObject(obj);
        }

        public string Serialize(object obj, string customDate)
        {
            var datetimeformat = new IsoDateTimeConverter { DateTimeFormat = customDate };
            return JsonConvert.SerializeObject(obj, datetimeformat);
        }

        /// <summary>
        /// Deserialize object with converter
        /// </summary>
        /// <typeparam name="T">Return object type</typeparam>
        /// <param name="json">String json to convert</param>
        /// <returns></returns>
        public T Deserialize<T>(string json)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(json);
            }
            catch(System.Exception ex)
            {
                return default(T);
            }
        }

        /// <summary>
        /// Deserialize object with converter
        /// </summary>
        /// <typeparam name="T">Return object type</typeparam>
        /// <param name="json">String json to convert</param>
        /// <param name="isJavascriptDate">
        /// true: use Javascript datetime convert
        /// </param>
        /// <returns></returns>
        public T Deserialize<T>(string json, bool isJavascriptDate)
        {
            return isJavascriptDate
                       ? JsonConvert.DeserializeObject<T>(json, new JavaScriptDateTimeConverter())
                       : JsonConvert.DeserializeObject<T>(json);
        }

        /// <summary>
        /// Deserializes the specified json.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json">The json.</param>
        /// <param name="customDate">The custom date.</param>
        /// <returns></returns>
        public T Deserialize<T>(string json, string customDate)
        {
            var datetimeformat = new IsoDateTimeConverter { DateTimeFormat = customDate };
            return JsonConvert.DeserializeObject<T>(json, datetimeformat);
        }
    }

    public static class JsonManager
    {
        private const string JsonType = "Newtonsoft";
        public static IJsonConverter Converter;
        public const string IsoDateTimeFullFormat = "MM/dd/yyyy HH:mm:ss";
        public const string IsoDateFormat = "MM/dd/yyyy";

        static JsonManager()
        {
            switch (JsonType)
            {
                case "Newtonsoft":
                    Converter = new NewtonsoftJsonConverter();
                    break;
            }
        }
    }
}
