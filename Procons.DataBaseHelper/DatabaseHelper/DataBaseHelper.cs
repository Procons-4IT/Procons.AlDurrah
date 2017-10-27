namespace Procons.DataBaseHelper
{
    using Sap.Data.Hana;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data;
    using System.Data.Common;
    using System.Data.SqlClient;

    public class DatabaseHelper<T> where T : DbConnection
    {
        private T _connection;
        public T Connection
        {
            get
            {
                _connection = (T)Activator.CreateInstance(typeof(T), ConnectionString);
                _connection.Open();
                return _connection;
            }
        }
        public string ConnectionString { get; set; }
        public DatabaseHelper()
        {
            if (ConfigurationManager.ConnectionStrings.Count > 0)
                ConnectionString = ConfigurationManager.ConnectionStrings[0].ConnectionString;
        }
        public DatabaseHelper(string connString)
        {
            ConnectionString = connString;
        }
        public DbDataReader ExecuteQuery(string query, Parameters parameters)
        {
            try
            {
                var command = Connection.CreateCommand();
                command.CommandText = query;
                if (parameters != null)
                {
                    object param;
                    command.CommandType = CommandType.StoredProcedure;
                    foreach (KeyValuePair<string, object> kvp in parameters.paramsList)
                    {
                        if (typeof(T).Equals(typeof(HanaConnection)))
                            param = new HanaParameter(kvp.Key, kvp.Value);
                        else
                            param = new SqlParameter(kvp.Key, kvp.Value);
                        command.Parameters.Add(param);
                    }
                }
                var result = command.ExecuteReader();
                Connection.Close();
                return result;
            }
            catch(Exception ex)
            {
                Connection.Close();
                return null;
            }

        }
        public DbDataReader ExecuteQuery(string query)
        {
            var result = ExecuteQuery(query, null);
            return result;
        }
    }
}
