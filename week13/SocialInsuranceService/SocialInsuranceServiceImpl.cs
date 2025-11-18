using System;
using System.Collections.Generic;
using System.Linq;
using SocialInsuranceService.Models;

namespace SocialInsuranceService
{
    /// <summary>
    /// Implementation of the Social Insurance WCF Service.
    /// </summary>
    public class SocialInsuranceServiceImpl : ISocialInsuranceService
    {
        private static readonly List<Person> _persons;
        private static readonly List<SocialInsuranceProcess> _processes;

        /// <summary>
        /// Static constructor to initialize sample data.
        /// </summary>
        static SocialInsuranceServiceImpl()
        {
            // Initialize sample persons
            _persons = new List<Person>
            {
                new Person
                {
                    SocialNumber = "SI001",
                    FullName = "Nguyen Van A",
                    Address = "123 Le Loi, Ha Noi",
                    ExpDate = new DateTime(2025, 12, 31),
                    RegisterTreatmentLocation = "Bach Mai Hospital",
                    Description = "Active employee"
                },
                new Person
                {
                    SocialNumber = "SI002",
                    FullName = "Tran Thi B",
                    Address = "456 Nguyen Hue, Ho Chi Minh",
                    ExpDate = new DateTime(2026, 6, 30),
                    RegisterTreatmentLocation = "Cho Ray Hospital",
                    Description = "Active employee"
                },
                new Person
                {
                    SocialNumber = "SI003",
                    FullName = "Le Van C",
                    Address = "789 Tran Phu, Da Nang",
                    ExpDate = new DateTime(2025, 9, 15),
                    RegisterTreatmentLocation = "Da Nang Hospital",
                    Description = "Active employee"
                },
                new Person
                {
                    SocialNumber = "SI004",
                    FullName = "Pham Thi D",
                    Address = "321 Ba Trieu, Ha Noi",
                    ExpDate = new DateTime(2027, 3, 20),
                    RegisterTreatmentLocation = "Viet Duc Hospital",
                    Description = "Active employee"
                },
                new Person
                {
                    SocialNumber = "SI005",
                    FullName = "Hoang Van E",
                    Address = "555 Hai Ba Trung, Ha Noi",
                    ExpDate = new DateTime(2026, 8, 15),
                    RegisterTreatmentLocation = "108 Hospital",
                    Description = "Active employee"
                },
                new Person
                {
                    SocialNumber = "SI006",
                    FullName = "Nguyen Thi F",
                    Address = "777 Le Duan, Ho Chi Minh",
                    ExpDate = new DateTime(2025, 11, 30),
                    RegisterTreatmentLocation = "115 Hospital",
                    Description = "Active employee"
                },
                new Person
                {
                    SocialNumber = "SI007",
                    FullName = "Tran Van G",
                    Address = "999 Nguyen Trai, Ha Noi",
                    ExpDate = new DateTime(2026, 5, 20),
                    RegisterTreatmentLocation = "Saint Paul Hospital",
                    Description = "Active employee"
                },
                new Person
                {
                    SocialNumber = "SI008",
                    FullName = "Le Thi H",
                    Address = "246 Vo Van Tan, Ho Chi Minh",
                    ExpDate = new DateTime(2025, 10, 10),
                    RegisterTreatmentLocation = "Gia Dinh Hospital",
                    Description = "Active employee"
                }
            };

            // Initialize sample insurance processes - Realistic career histories with arbitrary start dates
            _processes = new List<SocialInsuranceProcess>
            {
                // SI001 - Nguyen Van A (5 periods across 4 companies: 2002-present)
                new SocialInsuranceProcess
                {
                    Id = 1,
                    BeginDate = new DateTime(2002, 3, 15),
                    EndDate = new DateTime(2005, 8, 31),
                    Company = "VNPT Technology",
                    Address = "VNPT Building, Ha Noi",
                    Position = "Junior Developer",
                    SalaryByMonth = 3500000,
                    Unit = "Software Development",
                    SalaryPay = 350000,
                    SocialNumber = "SI001"
                },
                new SocialInsuranceProcess
                {
                    Id = 2,
                    BeginDate = new DateTime(2005, 9, 1),
                    EndDate = new DateTime(2011, 12, 31),
                    Company = "FPT Software",
                    Address = "FPT Tower, Ha Noi",
                    Position = "Software Engineer",
                    SalaryByMonth = 8000000,
                    Unit = "Development Team",
                    SalaryPay = 800000,
                    SocialNumber = "SI001"
                },
                new SocialInsuranceProcess
                {
                    Id = 3,
                    BeginDate = new DateTime(2012, 1, 1),
                    EndDate = new DateTime(2018, 6, 30),
                    Company = "Viettel Group",
                    Address = "Viettel Building, Ha Noi",
                    Position = "Senior Developer",
                    SalaryByMonth = 18000000,
                    Unit = "R&D Department",
                    SalaryPay = 1800000,
                    SocialNumber = "SI001"
                },
                new SocialInsuranceProcess
                {
                    Id = 4,
                    BeginDate = new DateTime(2018, 7, 1),
                    EndDate = new DateTime(2022, 10, 15),
                    Company = "FPT Software",
                    Address = "FPT Tower, Da Nang",
                    Position = "Tech Lead",
                    SalaryByMonth = 28000000,
                    Unit = "Architecture Team",
                    SalaryPay = 2800000,
                    SocialNumber = "SI001"
                },
                new SocialInsuranceProcess
                {
                    Id = 5,
                    BeginDate = new DateTime(2022, 10, 16),
                    EndDate = new DateTime(2025, 12, 31),
                    Company = "VNG Corporation",
                    Address = "VNG Campus, Ha Noi",
                    Position = "Engineering Manager",
                    SalaryByMonth = 45000000,
                    Unit = "Engineering Department",
                    SalaryPay = 4500000,
                    SocialNumber = "SI001"
                },

                // SI002 - Tran Thi B (3 periods across 3 companies: 2005-present)
                new SocialInsuranceProcess
                {
                    Id = 6,
                    BeginDate = new DateTime(2005, 6, 1),
                    EndDate = new DateTime(2013, 5, 31),
                    Company = "VNPT Technology",
                    Address = "VNPT Building, Ho Chi Minh",
                    Position = "Business Analyst",
                    SalaryByMonth = 7000000,
                    Unit = "Business Analysis",
                    SalaryPay = 700000,
                    SocialNumber = "SI002"
                },
                new SocialInsuranceProcess
                {
                    Id = 7,
                    BeginDate = new DateTime(2013, 6, 1),
                    EndDate = new DateTime(2020, 12, 31),
                    Company = "FPT Software",
                    Address = "FPT Tower, Ho Chi Minh",
                    Position = "Senior BA",
                    SalaryByMonth = 22000000,
                    Unit = "Consulting Team",
                    SalaryPay = 2200000,
                    SocialNumber = "SI002"
                },
                new SocialInsuranceProcess
                {
                    Id = 8,
                    BeginDate = new DateTime(2021, 1, 1),
                    EndDate = new DateTime(2026, 6, 30),
                    Company = "TMA Solutions",
                    Address = "TMA Tower, Ho Chi Minh",
                    Position = "Principal BA",
                    SalaryByMonth = 38000000,
                    Unit = "Enterprise Solutions",
                    SalaryPay = 3800000,
                    SocialNumber = "SI002"
                },

                // SI003 - Le Van C (6 periods across 5 companies: 2003-present)
                new SocialInsuranceProcess
                {
                    Id = 9,
                    BeginDate = new DateTime(2003, 2, 10),
                    EndDate = new DateTime(2007, 7, 31),
                    Company = "TMA Solutions",
                    Address = "TMA Tower, Da Nang",
                    Position = "Tester",
                    SalaryByMonth = 4000000,
                    Unit = "Quality Assurance",
                    SalaryPay = 400000,
                    SocialNumber = "SI003"
                },
                new SocialInsuranceProcess
                {
                    Id = 10,
                    BeginDate = new DateTime(2007, 8, 1),
                    EndDate = new DateTime(2011, 11, 30),
                    Company = "FPT Software",
                    Address = "FPT Tower, Da Nang",
                    Position = "QA Engineer",
                    SalaryByMonth = 9000000,
                    Unit = "Quality Control",
                    SalaryPay = 900000,
                    SocialNumber = "SI003"
                },
                new SocialInsuranceProcess
                {
                    Id = 11,
                    BeginDate = new DateTime(2011, 12, 1),
                    EndDate = new DateTime(2015, 4, 15),
                    Company = "Viettel Group",
                    Address = "Viettel Building, Ha Noi",
                    Position = "Senior QA",
                    SalaryByMonth = 15000000,
                    Unit = "Testing Department",
                    SalaryPay = 1500000,
                    SocialNumber = "SI003"
                },
                new SocialInsuranceProcess
                {
                    Id = 12,
                    BeginDate = new DateTime(2015, 4, 16),
                    EndDate = new DateTime(2019, 8, 31),
                    Company = "VNG Corporation",
                    Address = "VNG Campus, Da Nang",
                    Position = "QA Lead",
                    SalaryByMonth = 24000000,
                    Unit = "QA Department",
                    SalaryPay = 2400000,
                    SocialNumber = "SI003"
                },
                new SocialInsuranceProcess
                {
                    Id = 13,
                    BeginDate = new DateTime(2019, 9, 1),
                    EndDate = new DateTime(2023, 3, 31),
                    Company = "TMA Solutions",
                    Address = "TMA Tower, Ho Chi Minh",
                    Position = "QA Manager",
                    SalaryByMonth = 35000000,
                    Unit = "Quality Management",
                    SalaryPay = 3500000,
                    SocialNumber = "SI003"
                },
                new SocialInsuranceProcess
                {
                    Id = 14,
                    BeginDate = new DateTime(2023, 4, 1),
                    EndDate = new DateTime(2026, 12, 31),
                    Company = "FPT Software",
                    Address = "FPT Tower, Ha Noi",
                    Position = "Director of QA",
                    SalaryByMonth = 55000000,
                    Unit = "Quality Assurance",
                    SalaryPay = 5500000,
                    SocialNumber = "SI003"
                },

                // SI004 - Pham Thi D (2 periods across 2 companies: 2011-present)
                new SocialInsuranceProcess
                {
                    Id = 15,
                    BeginDate = new DateTime(2011, 7, 1),
                    EndDate = new DateTime(2019, 12, 31),
                    Company = "Viettel Group",
                    Address = "Viettel Building, Ha Noi",
                    Position = "Project Manager",
                    SalaryByMonth = 25000000,
                    Unit = "Project Management Office",
                    SalaryPay = 2500000,
                    SocialNumber = "SI004"
                },
                new SocialInsuranceProcess
                {
                    Id = 16,
                    BeginDate = new DateTime(2020, 1, 1),
                    EndDate = new DateTime(2027, 12, 31),
                    Company = "FPT Software",
                    Address = "FPT Tower, Ha Noi",
                    Position = "Delivery Manager",
                    SalaryByMonth = 52000000,
                    Unit = "Delivery Department",
                    SalaryPay = 5200000,
                    SocialNumber = "SI004"
                },

                // SI005 - Hoang Van E (4 periods across 3 companies: 2008-present)
                new SocialInsuranceProcess
                {
                    Id = 17,
                    BeginDate = new DateTime(2008, 9, 15),
                    EndDate = new DateTime(2014, 6, 30),
                    Company = "FPT Software",
                    Address = "FPT Tower, Ha Noi",
                    Position = "Developer",
                    SalaryByMonth = 10000000,
                    Unit = "Web Development",
                    SalaryPay = 1000000,
                    SocialNumber = "SI005"
                },
                new SocialInsuranceProcess
                {
                    Id = 18,
                    BeginDate = new DateTime(2014, 7, 1),
                    EndDate = new DateTime(2019, 3, 15),
                    Company = "Viettel Group",
                    Address = "Viettel Building, Ha Noi",
                    Position = "Full Stack Developer",
                    SalaryByMonth = 19000000,
                    Unit = "Application Development",
                    SalaryPay = 1900000,
                    SocialNumber = "SI005"
                },
                new SocialInsuranceProcess
                {
                    Id = 19,
                    BeginDate = new DateTime(2019, 3, 16),
                    EndDate = new DateTime(2023, 10, 31),
                    Company = "TMA Solutions",
                    Address = "TMA Tower, Ha Noi",
                    Position = "Tech Lead",
                    SalaryByMonth = 32000000,
                    Unit = "Architecture Team",
                    SalaryPay = 3200000,
                    SocialNumber = "SI005"
                },
                new SocialInsuranceProcess
                {
                    Id = 20,
                    BeginDate = new DateTime(2023, 11, 1),
                    EndDate = new DateTime(2026, 8, 31),
                    Company = "FPT Software",
                    Address = "FPT Tower, Da Nang",
                    Position = "Principal Engineer",
                    SalaryByMonth = 48000000,
                    Unit = "Technology Office",
                    SalaryPay = 4800000,
                    SocialNumber = "SI005"
                },

                // SI006 - Nguyen Thi F (7 periods across 4 companies: 2002-present)
                new SocialInsuranceProcess
                {
                    Id = 21,
                    BeginDate = new DateTime(2002, 11, 1),
                    EndDate = new DateTime(2006, 5, 31),
                    Company = "VNPT Technology",
                    Address = "VNPT Building, Ho Chi Minh",
                    Position = "Junior Analyst",
                    SalaryByMonth = 3800000,
                    Unit = "Data Analysis",
                    SalaryPay = 380000,
                    SocialNumber = "SI006"
                },
                new SocialInsuranceProcess
                {
                    Id = 22,
                    BeginDate = new DateTime(2006, 6, 1),
                    EndDate = new DateTime(2010, 12, 31),
                    Company = "VNG Corporation",
                    Address = "VNG Campus, Ho Chi Minh",
                    Position = "Data Analyst",
                    SalaryByMonth = 8500000,
                    Unit = "Analytics",
                    SalaryPay = 850000,
                    SocialNumber = "SI006"
                },
                new SocialInsuranceProcess
                {
                    Id = 23,
                    BeginDate = new DateTime(2011, 1, 1),
                    EndDate = new DateTime(2014, 9, 30),
                    Company = "FPT Software",
                    Address = "FPT Tower, Ho Chi Minh",
                    Position = "Senior Analyst",
                    SalaryByMonth = 16000000,
                    Unit = "Business Intelligence",
                    SalaryPay = 1600000,
                    SocialNumber = "SI006"
                },
                new SocialInsuranceProcess
                {
                    Id = 24,
                    BeginDate = new DateTime(2014, 10, 1),
                    EndDate = new DateTime(2017, 7, 31),
                    Company = "Viettel Group",
                    Address = "Viettel Building, Ha Noi",
                    Position = "Lead Analyst",
                    SalaryByMonth = 23000000,
                    Unit = "Data Science",
                    SalaryPay = 2300000,
                    SocialNumber = "SI006"
                },
                new SocialInsuranceProcess
                {
                    Id = 25,
                    BeginDate = new DateTime(2017, 8, 1),
                    EndDate = new DateTime(2020, 11, 30),
                    Company = "VNG Corporation",
                    Address = "VNG Campus, Ho Chi Minh",
                    Position = "Senior Data Analyst",
                    SalaryByMonth = 30000000,
                    Unit = "Analytics Department",
                    SalaryPay = 3000000,
                    SocialNumber = "SI006"
                },
                new SocialInsuranceProcess
                {
                    Id = 26,
                    BeginDate = new DateTime(2020, 12, 1),
                    EndDate = new DateTime(2023, 6, 30),
                    Company = "VNPT Technology",
                    Address = "VNPT Building, Ho Chi Minh",
                    Position = "Data Science Manager",
                    SalaryByMonth = 38000000,
                    Unit = "AI & Analytics",
                    SalaryPay = 3800000,
                    SocialNumber = "SI006"
                },
                new SocialInsuranceProcess
                {
                    Id = 27,
                    BeginDate = new DateTime(2023, 7, 1),
                    EndDate = new DateTime(2027, 12, 31),
                    Company = "FPT Software",
                    Address = "FPT Tower, Ha Noi",
                    Position = "Head of Data",
                    SalaryByMonth = 58000000,
                    Unit = "Data & AI Division",
                    SalaryPay = 5800000,
                    SocialNumber = "SI006"
                },

                // SI007 - Tran Van G (4 periods across 4 companies: 2010-present)
                new SocialInsuranceProcess
                {
                    Id = 28,
                    BeginDate = new DateTime(2010, 4, 1),
                    EndDate = new DateTime(2015, 3, 31),
                    Company = "TMA Solutions",
                    Address = "TMA Tower, Ha Noi",
                    Position = "System Administrator",
                    SalaryByMonth = 11000000,
                    Unit = "IT Operations",
                    SalaryPay = 1100000,
                    SocialNumber = "SI007"
                },
                new SocialInsuranceProcess
                {
                    Id = 29,
                    BeginDate = new DateTime(2015, 4, 1),
                    EndDate = new DateTime(2019, 8, 31),
                    Company = "Viettel Group",
                    Address = "Viettel Building, Ha Noi",
                    Position = "DevOps Engineer",
                    SalaryByMonth = 21000000,
                    Unit = "Infrastructure",
                    SalaryPay = 2100000,
                    SocialNumber = "SI007"
                },
                new SocialInsuranceProcess
                {
                    Id = 30,
                    BeginDate = new DateTime(2019, 9, 1),
                    EndDate = new DateTime(2023, 5, 31),
                    Company = "FPT Software",
                    Address = "FPT Tower, Ha Noi",
                    Position = "Senior DevOps",
                    SalaryByMonth = 33000000,
                    Unit = "Cloud Operations",
                    SalaryPay = 3300000,
                    SocialNumber = "SI007"
                },
                new SocialInsuranceProcess
                {
                    Id = 31,
                    BeginDate = new DateTime(2023, 6, 1),
                    EndDate = new DateTime(2026, 5, 31),
                    Company = "VNG Corporation",
                    Address = "VNG Campus, Ha Noi",
                    Position = "DevOps Lead",
                    SalaryByMonth = 46000000,
                    Unit = "Platform Engineering",
                    SalaryPay = 4600000,
                    SocialNumber = "SI007"
                },

                // SI008 - Le Thi H (3 periods across 3 companies: 2012-present)
                new SocialInsuranceProcess
                {
                    Id = 32,
                    BeginDate = new DateTime(2012, 10, 1),
                    EndDate = new DateTime(2017, 9, 30),
                    Company = "VNPT Technology",
                    Address = "VNPT Building, Ho Chi Minh",
                    Position = "UI/UX Designer",
                    SalaryByMonth = 12000000,
                    Unit = "Design Team",
                    SalaryPay = 1200000,
                    SocialNumber = "SI008"
                },
                new SocialInsuranceProcess
                {
                    Id = 33,
                    BeginDate = new DateTime(2017, 10, 1),
                    EndDate = new DateTime(2022, 2, 28),
                    Company = "VNG Corporation",
                    Address = "VNG Campus, Ho Chi Minh",
                    Position = "Senior Designer",
                    SalaryByMonth = 24000000,
                    Unit = "Product Design",
                    SalaryPay = 2400000,
                    SocialNumber = "SI008"
                },
                new SocialInsuranceProcess
                {
                    Id = 34,
                    BeginDate = new DateTime(2022, 3, 1),
                    EndDate = new DateTime(2025, 10, 15),
                    Company = "FPT Software",
                    Address = "FPT Tower, Ho Chi Minh",
                    Position = "Design Lead",
                    SalaryByMonth = 39000000,
                    Unit = "UX Department",
                    SalaryPay = 3900000,
                    SocialNumber = "SI008"
                }
            };
        }

        /// <summary>
        /// Gets all insurance processes for a specific social number.
        /// </summary>
        public List<SocialInsuranceProcess> GetAllSocialInsuranceProcessBySocialNumber(string socialNumber)
        {
            Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] GetAllSocialInsuranceProcessBySocialNumber('{socialNumber}') called");

            var result = _processes
                .Where(p => p.SocialNumber == socialNumber)
                .OrderBy(p => p.BeginDate)
                .ToList();

            Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] Returning {result.Count} records");
            return result;
        }

        /// <summary>
        /// Gets insurance processes for a social number within a date range.
        /// </summary>
        public List<SocialInsuranceProcess> GetSocialInsuranceProcessBySocialNumberByDate(
            string socialNumber,
            DateTime begin,
            DateTime end)
        {
            Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] GetSocialInsuranceProcessBySocialNumberByDate('{socialNumber}', {begin:yyyy-MM-dd}, {end:yyyy-MM-dd}) called");

            var result = _processes
                .Where(p => p.SocialNumber == socialNumber &&
                           p.BeginDate >= begin &&
                           p.EndDate <= end)
                .OrderBy(p => p.BeginDate)
                .ToList();

            Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] Returning {result.Count} records");
            return result;
        }

        /// <summary>
        /// Gets all insurance processes for a specific company.
        /// Supports both exact match and partial match (contains).
        /// </summary>
        public List<SocialInsuranceProcess> GetAllSocialInsuranceProcessByCompanyName(string company)
        {
            Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] GetAllSocialInsuranceProcessByCompanyName('{company}') called");

            // Trim whitespace from input
            company = company?.Trim() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(company))
            {
                Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] Empty company name - returning empty list");
                return new List<SocialInsuranceProcess>();
            }

            // First try exact match (case-insensitive)
            var exactMatches = _processes
                .Where(p => p.Company.Equals(company, StringComparison.OrdinalIgnoreCase))
                .OrderBy(p => p.SocialNumber)
                .ThenBy(p => p.BeginDate)
                .ToList();

            if (exactMatches.Count > 0)
            {
                Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] Exact match found - Returning {exactMatches.Count} records");
                return exactMatches;
            }

            // If no exact match, try partial match (contains)
            var partialMatches = _processes
                .Where(p => p.Company.IndexOf(company, StringComparison.OrdinalIgnoreCase) >= 0)
                .OrderBy(p => p.SocialNumber)
                .ThenBy(p => p.BeginDate)
                .ToList();

            Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] Partial match - Returning {partialMatches.Count} records");
            return partialMatches;
        }

        /// <summary>
        /// Gets person information by social number.
        /// </summary>
        public Person GetPersonBySocialNumber(string socialNumber)
        {
            Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] GetPersonBySocialNumber('{socialNumber}') called");

            var person = _persons.FirstOrDefault(p => p.SocialNumber == socialNumber);

            Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] Person {(person != null ? "found" : "not found")}");
            return person;
        }

        /// <summary>
        /// Gets all persons in the system.
        /// </summary>
        public List<Person> GetAllPersons()
        {
            Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] GetAllPersons() called");
            Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] Returning {_persons.Count} persons");
            return _persons;
        }
    }
}
