using HR_Management.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR_Managment.BLL.Specification.AttendanceSpecs
{
    public class AttendanceFilterCount : BassSpecification<Attendance>
    {
        public AttendanceFilterCount(AttendanceSpecParams Params):base(A=>
             (A.EmployeeId == Params.EmployeeId) 
        //&& (String.IsNullOrEmpty(Params.Date.ToString()) || A.Date == Params.Date)
            )
        {
            
        }
    }
}
