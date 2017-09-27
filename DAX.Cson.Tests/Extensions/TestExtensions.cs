using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DAX.Cson.Tests.Extensions
{
    public static class TestExtensions
    {
        public static bool HasProperty(this Type type, string name)
        {
            return type.GetProperty(name) != null;
        }

        public static string AssumeLocalPath(this string path)
        {
            if (Path.IsPathRooted(path)) return path;

            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path);
        }

        public static string ToJson(this object obj)
        {
            return JsonConvert.SerializeObject(obj, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All });
        }

        public static string PrettifyJson(this string jsonString)
        {
            try
            {
                return JObject.Parse(jsonString)
                    .ToString(Formatting.Indented);
            }
            catch
            {
                return jsonString;
            }
        }
    }
}