namespace Procons.DataBaseHelper
{
    using DataBaseHelper;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Reflection;
    public static class MappingHelper
    {
        private static T CreateObject<T>(DbDataReader dr, List<PropertyMappingInfo> propInfoList, int[] ordinals)
            where T : class, new()
        {
            T local = Activator.CreateInstance<T>();
            for (int i = 0; i <= (propInfoList.Count - 1); i++)
            {
                if (propInfoList[i].PropertyInfo.CanWrite)
                {
                    Type propertyType = propInfoList[i].PropertyInfo.PropertyType;
                    object defaultValue = propInfoList[i].DefaultValue;
                    if (!((ordinals[i] == -1) || dr.IsDBNull(ordinals[i])))
                    {
                        defaultValue = dr.GetValue(ordinals[i]);
                    }
                    try
                    {
                        propInfoList[i].PropertyInfo.SetValue(local, defaultValue, null);
                    }
                    catch
                    {
                        try
                        {
                            if (propertyType.BaseType.Equals(typeof(Enum)))
                            {
                                propInfoList[i].PropertyInfo.SetValue(local, Enum.ToObject(propertyType, defaultValue), null);
                            }
                            else
                            {
                                propInfoList[i].PropertyInfo.SetValue(local, Convert.ChangeType(defaultValue, propertyType), null);
                            }
                        }
                        catch
                        {
                        }
                    }
                }
            }
            return local;
        }

        public static List<T> FillCollection<T>(DbDataReader dr) where T : class, new()
        {
            List<T> list = new List<T>();
            try
            {
                List<PropertyMappingInfo> properties = GetProperties(typeof(T));
                int[] ordinals = GetOrdinals(properties, dr);
                while (dr.Read())
                {
                    T item = CreateObject<T>(dr, properties, ordinals);
                    list.Add(item);
                }
            }
            finally
            {
                if (!dr.IsClosed)
                {
                    dr.Close();
                }
            }
            return list;
        }

        public static T FillObject<T>(DbDataReader dr) where T : class, new()
        {
            T local = default(T);
            try
            {
                var properties = GetProperties(typeof(T));
                var ordinals = GetOrdinals(properties, dr);
                if (dr.Read())
                {
                    local = CreateObject<T>(dr, properties, ordinals);
                }
            }
            finally
            {
                if (!dr.IsClosed)
                {
                    dr.Close();
                }
            }
            return local;
        }

        private static int[] GetOrdinals(List<PropertyMappingInfo> propMapList, IDataReader dr)
        {
            var numArray = new int[propMapList.Count];
            if (dr != null)
            {
                for (int i = 0; i <= (propMapList.Count - 1); i++)
                {
                    numArray[i] = -1;
                    try
                    {
                        numArray[i] = dr.GetOrdinal(propMapList[i].DataFieldName);
                    }
                    catch (IndexOutOfRangeException)
                    {

                    }
                }
            }
            return numArray;
        }

        private static List<PropertyMappingInfo> GetProperties(Type objType)
        {
            var cache = MappingInfoCache.GetCache(objType.Name);
            if (cache == null)
            {
                cache = LoadPropertyMappingInfo(objType);
                MappingInfoCache.SetCache(objType.Name, cache);
            }
            return cache;
        }

        private static List<PropertyMappingInfo> PopulatePropertyMappingList(Type objType)
        {
            var list = new List<PropertyMappingInfo>();
            foreach (PropertyInfo info in objType.GetProperties())
            {
                var customAttribute = (DataMappingAttribute)Attribute.GetCustomAttribute(info, typeof(DataMappingAttribute));
                if (customAttribute != null)
                {
                    var item = new PropertyMappingInfo(customAttribute.DataFieldName, customAttribute.NullValue, info);
                    list.Add(item);
                }
            }
            return list;
        }

        private static List<PropertyMappingInfo> LoadPropertyMappingInfo(Type objType)
        {
            var list = new List<PropertyMappingInfo>();
            var customAttribute = (MappingEntityAttribute)Attribute.GetCustomAttribute(objType, typeof(MappingEntityAttribute));
            var isMappingObject = false;

            if (customAttribute != null)
            {
                if (customAttribute.GetType().Equals(typeof(MappingEntityAttribute)))
                    isMappingObject = true;
            }
            if (isMappingObject)
                list = PopulatePropertyMappingList(objType);
            else
            {
                foreach (PropertyInfo info in objType.GetProperties())
                {
                    var item = new PropertyMappingInfo(info.Name, null, info);
                    list.Add(item);
                }
            }
            return list;
        }
    }
}

