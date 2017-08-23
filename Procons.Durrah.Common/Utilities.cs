namespace Procons.Durrah.Common
{
    using System.Configuration;
    public class Utilities
    {
        public static string GetConfigurationValue(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }
    }
}
