using HR_Management.BLL.Specification;
using HR_Management.DAL.Entities;
using HR_Managment.BLL.Specification.EmployeeSpecs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR_Managment.BLL.Specification.DepartmentSpecs
{
    public class DepartmentsFilterCount : BassSpecification<Department>
    {
        public DepartmentsFilterCount(EntitySpecParams Params) : base(D =>
            string.IsNullOrEmpty(Params.Search) || D.Name.ToLower().Contains(Params.Search))
        {

        }
    }
}
