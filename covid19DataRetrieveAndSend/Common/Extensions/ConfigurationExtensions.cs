using System;
using System.Globalization;
using Microsoft.Extensions.Configuration;

namespace covid19DataRetrieveAndSend.Common.Extensions
{
    public static class ConfigurationExtensions
    {
        public static T GetAs<T>(this IConfiguration config, string key)
        {
            try
            {
                return (T)GetAsObject(config[key], typeof(T));
            }
            catch (Exception e)
            {
                throw new FormatException($"Error getting configuration parameter. Key: '{key}', type: '{typeof(T).Name}'.", e);
            }
        }

        private static object GetAsObject(string stringValue, Type type) => Convert.ChangeType(stringValue, Nullable.GetUnderlyingType(type) ?? type,
            CultureInfo.InvariantCulture);
    }
}
