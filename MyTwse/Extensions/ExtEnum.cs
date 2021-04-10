using System;
using System.ComponentModel;
using System.Linq;
using System.Net;

namespace MyTwse
{
    public static class ExtEnum
    {
        public static string GetDescription(this System.Enum value)
        {
            var field = value.GetType().GetField(value.ToString());
            if (field != null)
            {
                var attr = field.GetCustomAttributes(typeof(DescriptionAttribute), true).SingleOrDefault() as DescriptionAttribute;
                if (attr != null)
                {
                    return attr.Description;
                }
            }
            return string.Empty;
        }
        public static HttpStatusCode GetHttpStatusCode(this System.Enum value)
        {
            var field = value.GetType().GetField(value.ToString());
            if (field != null)
            {
                var attr = field.GetCustomAttributes(typeof(HttpStatusCodeAttribute), true).SingleOrDefault() as HttpStatusCodeAttribute;
                if (attr != null)
                {
                    return attr.HttpStatusCode;
                }
            }
            throw new NotImplementedException();
        }
    }
}
