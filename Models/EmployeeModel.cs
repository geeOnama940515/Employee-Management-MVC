using System.ComponentModel.DataAnnotations;

namespace EmployeeCRUD.Models
{
    public class EmployeeModel : ActionsModel
    {
        public int Id { get; set; }
        public string EmpNumber { get; set; }
        // Properties for individual name components
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime DateHired { get; set; }
        public string Department { get; set; }
        public string Designation { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        // Property for Address
        public string Address { get; set; }

        // Computed property for FullName
        public string FullName
        {
            get
            {
                // Concatenate FirstName, MiddleName, and LastName, handling null or empty middle name
                return string.IsNullOrWhiteSpace(MiddleName)
                    ? $"{FirstName} {LastName}"
                    : $"{FirstName} {MiddleName} {LastName}";
            }
        }


    }
}
