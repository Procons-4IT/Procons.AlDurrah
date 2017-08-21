namespace Procons.DataBaseHelper
{
    using System;

    [AttributeUsage(AttributeTargets.Property)]
    public sealed class DataMappingAttribute : Attribute
    {
        private string _dataFieldName;
        private object _nullValue;

        public DataMappingAttribute()
        {
        }
        public DataMappingAttribute(object nullValue) : this(string.Empty, nullValue)
        {
        }

        public DataMappingAttribute(string dataFieldName, object nullValue)
        {
            this._dataFieldName = dataFieldName;
            this._nullValue = nullValue;
        }

        public string DataFieldName
        {
            get
            {
                return this._dataFieldName;
            }
        }

        public object NullValue
        {
            get
            {
                return this._nullValue;
            }
        }
    }
}

