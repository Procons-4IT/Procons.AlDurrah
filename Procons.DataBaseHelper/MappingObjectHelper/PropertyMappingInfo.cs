namespace Procons.DataBaseHelper
{
    using System;
    using System.Reflection;

    internal sealed class PropertyMappingInfo
    {
        private string _dataFieldName;
        private object _defaultValue;
        private PropertyInfo _propInfo;

        internal PropertyMappingInfo() : this(string.Empty, null, null)
        {
        }

        internal PropertyMappingInfo(string dataFieldName, object defaultValue, PropertyInfo info)
        {
            this._dataFieldName = dataFieldName;
            this._defaultValue = defaultValue;
            this._propInfo = info;
        }

        public string DataFieldName
        {
            get
            {
                if (string.IsNullOrEmpty(this._dataFieldName))
                {
                    this._dataFieldName = this._propInfo.Name;
                }
                return this._dataFieldName;
            }
        }

        public object DefaultValue
        {
            get
            {
                return this._defaultValue;
            }
        }

        public PropertyInfo PropertyInfo
        {
            get
            {
                return this._propInfo;
            }
        }
    }
}

