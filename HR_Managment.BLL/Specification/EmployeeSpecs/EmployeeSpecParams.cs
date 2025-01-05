

using HR_Management.BLL.Specification;

namespace HR_Managment.BLL.Specification.EmployeeSpecs
{
    public class EmployeeSpecParams : EntitySpecParams
    {
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public decimal Salary { get; set; }
        //public DateTime HireDate { get; set; }
        public string JobTitle { get; set; }
        public int DepartmentId { get; set; }
    }
}
