using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR_Management.DAL.Entities
{
    public class Employee
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime HireDate { get; set; }
        public decimal Salary { get; set; }
        public string JobTitle { get; set; }
        public int DepartmentId { get; set; }
        public Department Department { get; set; }
        public Department Manage { get; set; }
        public ICollection<Attendance> Attendances { get; set; } = new HashSet<Attendance>();
    }
}
