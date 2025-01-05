using HR_Management.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR_Managment.BLL.Specification.AttendanceSpecs
{
    public class AttendanceSpecification : BassSpecification<Attendance>
    {
        public AttendanceSpecification(AttendanceSpecParams Params):base(A=>
            (A.EmployeeId == Params.EmployeeId) &&
            (!Params.DateStart.HasValue || A.Date >= Params.DateStart) &&
            (!Params.DateEnd.HasValue || A.Date <= Params.DateEnd)
        ) 
        {
            AddIncludes(A => A.Employee);
            ApplyPagination((Params.pageNumber - 1) * Params.PageSize, Params.PageSize);
        }
    }
}
