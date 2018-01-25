using System.Collections.Generic;

namespace Procons.Durrah.Common
{
    public class Catalogue
    {
        public string Code { get; set; }
        public string Age { get; set; }
        public string WorkerType { get; set; }
        public string Gender { get; set; }
        public string Nationality { get; set; }
        public string MaritalStatus { get; set; }
        public string Language { get; set; }
        public string[] Languages { get; set; }
        public string Location { get; set; }
        public string Hobbies { get; set; }
        public string IsNew { get; set; }
        public int? Period { get; set; }
        public int? YearsOfExperience { get; set; }
        public string Country { get; set; }
    }
}