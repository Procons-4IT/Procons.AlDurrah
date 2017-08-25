namespace Procons.Durrah.Common
{
    using Newtonsoft.Json;
    public class SboCred
    {

        public SboCred()
        { }

        public SboCred(string user, string pass, string company)
        {
            UserName = user;
            Password = pass;
            CompanyDB = company;
        }

        public bool IsValid()
        {
            return (!string.IsNullOrEmpty(UserName) && !string.IsNullOrEmpty(Password) && !string.IsNullOrEmpty(CompanyDB));
        }

        public string GetJsonString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        public string UserName = string.Empty;
        public string Password = string.Empty;
        public string CompanyDB = string.Empty;
    }
}