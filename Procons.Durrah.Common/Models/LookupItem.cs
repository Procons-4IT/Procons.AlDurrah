namespace Procons.Durrah.Common
{
    public class LookupItem
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public LookupItem(string name, string value)
        {
            Name = name;
            Value = value;
        }
    }
}
