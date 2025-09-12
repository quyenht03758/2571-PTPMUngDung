using System;

namespace EmployeeManager
{
    public class Employee
    {
        public string Id { get; set; }          // Employee ID (unique)
        public string Name { get; set; }        // Full name
        public string Address { get; set; }     // Address
        public DateTime Birthday { get; set; }  // Date of birth
        public string Email { get; set; }       // Email
        public string Phone { get; set; }       // Phone number
    }
}
