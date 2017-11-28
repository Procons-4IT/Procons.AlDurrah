using System.Collections.Generic;

namespace Procons.Durrah.Common
{
    public class Worker
    {
        /// <summary>
        /// Worker Code
        /// </summary>
        public string WorkerCode { get; set; }
        public string WorkerName { get; set; }
        public string SerialNumber { get; set; }
        public string Agent { get; set; }
        public string Mobile { get; set; }
        public int Age { get; set; }
        /// <summary>
        /// Related Item Name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Related Item Code
        /// </summary>
        public string Code { get; set; }
        public string BirthDate { get; set; }
        public string Gender { get; set; }
        public string Nationality { get; set; }
        public string Religion { get; set; }
        public string MaritalStatus { get; set; }
        public string Language { get; set; }
        public string Photo { get; set; }
        public string License { get; set; }
        public double Price { get; set; }
        public double Salary { get; set; }
        public string Weight { get; set; }
        public string Height { get; set; }
        public string Education { get; set; }
        public string Passport { get; set; }
        public string Video { get; set; }
        public string PassportNumber { get; set; }
        public string PassportIssDate { get; set; }
        public string PassportExpDate { get; set; }
        public string PassportPoIssue { get; set; }
        public string CivilId { get; set; }
        public string Status { get; set; }
        public string WorkerType { get; set; }
        public List<LookupItem> Languages { get; set; }

    }
}