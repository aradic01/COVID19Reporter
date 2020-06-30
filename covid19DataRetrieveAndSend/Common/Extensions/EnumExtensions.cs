using System;

namespace covid19DataRetrieveAndSend.Common.Extensions
{
    public static class EnumExtensions
    {
        public static T ToEnumOrDefault<T>(this string value) where T : struct
        {
            if (string.IsNullOrEmpty(value)) return default;

            return Enum.TryParse<T>(value, true, out var result) ? result : default;
        }
    }
}
