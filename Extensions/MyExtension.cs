using System.Text.Json;

namespace MyTwse.Extensions
{
    public static class MyExtension
    {
        public static int? TryToInt(this string input)
        {
            int result = 0;
            if (int.TryParse(input, out result))
            {
                return result;
            }
            return null;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonText"></param>
        /// <param name="propertyNameCaseInsensitive">設定在做序列化/反序列化時，尊重名稱大小寫</param>
        /// <returns></returns>
        public static T JsonConvertToModel<T>(this string jsonText, bool propertyNameCaseInsensitive = true)
        {
            if (string.IsNullOrWhiteSpace(jsonText))
                return default(T);

            var options = new JsonSerializerOptions
            {

                PropertyNameCaseInsensitive = propertyNameCaseInsensitive
            };

            return JsonSerializer.Deserialize<T>(jsonText, options);
        }
    }
}
