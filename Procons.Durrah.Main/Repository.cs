namespace Procons.Durrah.Main
{
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    public class Repository<Y> where Y : DbContext, new()
    {
        private static Y dbConextInstance;
        public static Y DbConextInstance
        {
            get
            {
                if (dbConextInstance == null)
                {
                    dbConextInstance = new Y();
                    return dbConextInstance;
                }
                else
                    return dbConextInstance;
            }
        }

        public T Load<T>(object id) where T : class
        {
            T instance = null;
            try
            {
                instance = DbConextInstance.Set<T>().Find(id);
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(string.Concat("DBcontext Load failed: ", ex.Message));
            }
            return instance;
        }
        public List<T> GetData<T>() where T : class
        {
            List<T> list = new List<T>();
            try
            {
                list = DbConextInstance.Set<T>().ToList();
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(string.Concat("DBcontext GetData failed: ", ex.Message));
            }
            return list;
        }
        public bool Save<T>(T obj) where T : class
        {
            var success = false;
            try
            {
                DbConextInstance.Set<T>().AddOrUpdate(obj);
                DbConextInstance.SaveChanges();
                success = true;
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(string.Concat("DBcontext Save failed: ", ex.Message));
            }
            return success;
        }

        public bool Delete<T>(T obj) where T : class
        {
            var success = false;
            try
            {
                DbConextInstance.Set<T>().Attach(obj);
                DbConextInstance.Entry(obj).State = EntityState.Deleted;
                DbConextInstance.SaveChanges();
                success = true;
            }
            catch (System.Exception err)
            {

            }
            return success;
        }
    }
}
