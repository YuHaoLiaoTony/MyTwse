using System.Globalization;

namespace MyTwse.Infrastructure
{
    public static class StringExtensions
    {
        public static decimal? TryToDecimal(this string value)
        {
            if (decimal.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal result))
                return result;
            return null;
        }

        public static int? TryToInt(this string value)
        {
            if (int.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out int result))
                return result;
            return null;
        }

        public static bool IsNumber(this string value)
        {
            return int.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out _);
        }
    }
}
