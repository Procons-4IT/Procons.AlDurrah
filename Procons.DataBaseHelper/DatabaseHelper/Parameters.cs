namespace Procons.DataBaseHelper
{
    using System.Collections.Generic;

    public class Parameters
    {
        public Dictionary<string, object> paramsList = new Dictionary<string, object>();
        public Parameters Add(string key, object value)
        {
            paramsList.Add(key, value);
            return this;
        }
    }
}
