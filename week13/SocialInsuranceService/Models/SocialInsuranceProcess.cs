using System;
using System.Runtime.Serialization;

namespace SocialInsuranceService.Models
{
    /// <summary>
    /// Represents a social insurance payment process for an employee.
    /// </summary>
    [DataContract]
    public class SocialInsuranceProcess
    {
        /// <summary>
        /// Unique identifier for the process record.
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Start date of the insurance period.
        /// </summary>
        [DataMember]
        public DateTime BeginDate { get; set; }

        /// <summary>
        /// End date of the insurance period.
        /// </summary>
        [DataMember]
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Company name where the employee worked.
        /// </summary>
        [DataMember]
        public string Company { get; set; }

        /// <summary>
        /// Company address.
        /// </summary>
        [DataMember]
        public string Address { get; set; }

        /// <summary>
        /// Job position of the employee.
        /// </summary>
        [DataMember]
        public string Position { get; set; }

        /// <summary>
        /// Monthly salary.
        /// </summary>
        [DataMember]
        public decimal SalaryByMonth { get; set; }

        /// <summary>
        /// Organizational unit within the company.
        /// </summary>
        [DataMember]
        public string Unit { get; set; }

        /// <summary>
        /// Amount of insurance payment.
        /// </summary>
        [DataMember]
        public decimal SalaryPay { get; set; }

        /// <summary>
        /// Social security number (foreign key to Person).
        /// </summary>
        [DataMember]
        public string SocialNumber { get; set; }
    }
}
