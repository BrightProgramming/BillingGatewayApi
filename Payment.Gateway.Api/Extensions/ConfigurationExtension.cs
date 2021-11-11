using System;
using Microsoft.Extensions.Configuration;

namespace Payment.Gateway.Api.Extensions
{
    /// <summary>
    /// Extension methods for configuration
    /// </summary>
    public static class ConfigurationExtensions
    {
        /// <summary>
        /// Add some more configuration
        /// </summary>
        /// <returns></returns>
        public static T BindConfigurationSection<T>(this IConfiguration configuration, string section, params object[] constructorArgs) where T : class
        {
            var settings = Activator.CreateInstance(typeof(T), constructorArgs);
            configuration.GetSection(section).Bind(settings);
            return settings as T;
        }
    }
}
