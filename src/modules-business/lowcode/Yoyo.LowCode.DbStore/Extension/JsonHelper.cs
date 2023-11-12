using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace Yoyo.LowCode.Extension
{
    /// <summary>
    /// JsonHelper
    /// </summary>
    public static class JsonHelper
    {
        public static string ToJson(this object obj)
        {
            var timeConverter = new IsoDateTimeConverter { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" };
            return JsonConvert.SerializeObject(obj, timeConverter);
        }

        public static string ToJson(this object obj, string datetimeformats)
        {
            var timeConverter = new IsoDateTimeConverter { DateTimeFormat = datetimeformats };
            return JsonConvert.SerializeObject(obj, timeConverter);
        }

        public static T ToObject<T>(this string json)
        {
            return json == null ? default(T) : JsonConvert.DeserializeObject<T>(json);
        }

        public static T ToObject<T>(this object json)
        {
            return json == null ? default(T) : JsonConvert.DeserializeObject<T>(json.ToJson());
        }

        public static List<T> ToList<T>(this string json)
        {
            return json == null ? null : JsonConvert.DeserializeObject<List<T>>(json);
        }

        public static DataTable ToTable(this string json)
        {
            return json == null ? null : JsonConvert.DeserializeObject<DataTable>(json);
        }

        public static JObject ToObject(this string json)
        {
            return json == null ? JObject.Parse("{}") : JObject.Parse(json.Replace("&nbsp;", ""));
        }

        public static string PraseToJson(string json)
        {
            JsonSerializer s = new JsonSerializer();
            JsonReader reader = new JsonTextReader(new StringReader(json));
            Object jsonObject = s.Deserialize(reader);
            StringWriter sWriter = new StringWriter();
            JsonWriter writer = new JsonTextWriter(sWriter);
            writer.Formatting = Formatting.Indented;
            s.Serialize(writer, jsonObject);
            return sWriter.ToString();
        }

        public static string GetValue(this Dictionary<string, string> dic, string key)
        {
            return dic.ContainsKey(key) ? dic[key] : null;
        }
    }
}
