using HR_Management.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace HR_Managment.BLL.Specification.EmployeeSpecs
{
    public class EmployeeSpecification : BassSpecification<Employee>
    {
        public EmployeeSpecification(EmployeeSpecParams Params):base(E=>
        (Params.DepartmentId == 0 || E.DepartmentId == Params.DepartmentId) &&
        (String.IsNullOrWhiteSpace(Params.Search) || E.FirstName.ToLower().Contains(Params.Search.ToLower()) || E.FirstName.ToLower().Contains(Params.Search.ToLower())) &&
        (String.IsNullOrWhiteSpace(Params.JobTitle) || E.JobTitle.ToLower().Contains(Params.JobTitle.ToLower()) )
        )
        {
            AddIncludes(E => E.Department);
            ApplyPagination((Params.pageNumber - 1) * Params.PageSize, Params.PageSize);
        }

        public EmployeeSpecification(int id) : base(D => D.Id == id)
        {
            Includes.Add(D => D.Department);
        }
    }
}
