using System;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Reflection;

namespace MyTwse.Infrastructure
{
    public static class EnumExtensions
    {
        public static HttpStatusCode GetHttpStatusCode(this System.Enum value)
        {
            var field = value.GetType().GetField(value.ToString());
            var attr = field?.GetCustomAttribute<HttpStatusCodeAttribute>();
            return attr?.HttpStatusCode ?? HttpStatusCode.InternalServerError;
        }

        public static string GetDescription(this System.Enum value)
        {
            var field = value.GetType().GetField(value.ToString());
            var attr = field?.GetCustomAttribute<DescriptionAttribute>();
            return attr?.Description ?? value.ToString();
        }

        public static string ToJson<T>(this T obj)
        {
            return System.Text.Json.JsonSerializer.Serialize(obj);
        }
    }
}
