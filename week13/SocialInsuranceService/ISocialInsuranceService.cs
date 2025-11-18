using System;
using System.Collections.Generic;
using System.ServiceModel;
using SocialInsuranceService.Models;

namespace SocialInsuranceService
{
    /// <summary>
    /// WCF Service contract for Social Insurance management operations.
    /// </summary>
    [ServiceContract]
    public interface ISocialInsuranceService
    {
        /// <summary>
        /// Retrieves all social insurance process records for a specific employee.
        /// </summary>
        /// <param name="socialNumber">Social security number of the employee.</param>
        /// <returns>List of all insurance process records for the employee.</returns>
        [OperationContract]
        List<SocialInsuranceProcess> GetAllSocialInsuranceProcessBySocialNumber(string socialNumber);

        /// <summary>
        /// Retrieves social insurance process records for an employee within a date range.
        /// </summary>
        /// <param name="socialNumber">Social security number of the employee.</param>
        /// <param name="begin">Start date of the period.</param>
        /// <param name="end">End date of the period.</param>
        /// <returns>List of insurance process records within the specified date range.</returns>
        [OperationContract]
        List<SocialInsuranceProcess> GetSocialInsuranceProcessBySocialNumberByDate(
            string socialNumber,
            DateTime begin,
            DateTime end);

        /// <summary>
        /// Retrieves all social insurance process records for a specific company.
        /// </summary>
        /// <param name="company">Company name.</param>
        /// <returns>List of all insurance process records for the specified company.</returns>
        [OperationContract]
        List<SocialInsuranceProcess> GetAllSocialInsuranceProcessByCompanyName(string company);

        /// <summary>
        /// Gets person information by social number.
        /// </summary>
        /// <param name="socialNumber">Social security number.</param>
        /// <returns>Person information.</returns>
        [OperationContract]
        Person GetPersonBySocialNumber(string socialNumber);

        /// <summary>
        /// Gets all persons in the system.
        /// </summary>
        /// <returns>List of all persons.</returns>
        [OperationContract]
        List<Person> GetAllPersons();
    }
}
