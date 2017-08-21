namespace Procons.Durrah.Common
{
    public class Factory
    {
        public static T DeclareClass<T>() where T : new()
        {
            return new T();
        }
    }
}
