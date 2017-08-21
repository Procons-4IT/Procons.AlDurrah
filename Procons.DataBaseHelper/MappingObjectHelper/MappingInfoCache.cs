namespace Procons.DataBaseHelper
{
    using System;
    using System.Collections.Generic;

    internal static class MappingInfoCache
    {
        private static Dictionary<string, List<PropertyMappingInfo>> cache = new Dictionary<string, List<PropertyMappingInfo>>();

        public static void ClearCache()
        {
            cache.Clear();
        }

        internal static List<PropertyMappingInfo> GetCache(string typeName)
        {
            List<PropertyMappingInfo> list = null;
            try
            {
                list = cache[typeName];
            }
            catch (KeyNotFoundException)
            {
            }
            return list;
        }

        internal static void SetCache(string typeName, List<PropertyMappingInfo> mappingInfoList)
        {
            cache[typeName] = mappingInfoList;
        }
    }
}

