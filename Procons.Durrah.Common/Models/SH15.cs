namespace Procons.Durrah.Common
{
    using System.ComponentModel.DataAnnotations;

    public class SH15
    {
        [Key]
        private int id;
        private string name;
        private string code;
        private string accType;
        private double amount;
        public string AccType { get { return accType; } set { accType = value; } }
        public string Code { get { return code; } set { code = value; } }
        public string Name { get { return name; } set { name = value; } }
        public int Id { get { return id; } set { id = value; } }
        public double Amount { get { return amount; } set { amount = value; } }
    }
}