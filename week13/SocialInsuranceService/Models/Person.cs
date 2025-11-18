using System;
using System.Runtime.Serialization;

namespace SocialInsuranceService.Models
{
    /// <summary>
    /// Represents an employee/person in the social insurance system.
    /// </summary>
    [DataContract]
    public class Person
    {
        /// <summary>
        /// Social security number (primary key).
        /// </summary>
        [DataMember]
        public string SocialNumber { get; set; }

        /// <summary>
        /// Full name of the person.
        /// </summary>
        [DataMember]
        public string FullName { get; set; }

        /// <summary>
        /// Address of the person.
        /// </summary>
        [DataMember]
        public string Address { get; set; }

        /// <summary>
        /// Expiration date of the insurance.
        /// </summary>
        [DataMember]
        public DateTime ExpDate { get; set; }

        /// <summary>
        /// Registered treatment location.
        /// </summary>
        [DataMember]
        public string RegisterTreatmentLocation { get; set; }

        /// <summary>
        /// Additional description or notes.
        /// </summary>
        [DataMember]
        public string Description { get; set; }
    }
}
