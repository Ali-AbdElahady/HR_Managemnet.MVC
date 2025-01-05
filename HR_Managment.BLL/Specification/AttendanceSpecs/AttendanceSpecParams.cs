using HR_Management.BLL.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR_Managment.BLL.Specification.AttendanceSpecs
{
    public class AttendanceSpecParams : EntitySpecParams
    {
        public int EmployeeId { get; set; }
        public string EmpName { get; set; }
        public DateTime? DateStart { get; set; }
        public DateTime? DateEnd { get; set; }
    }
}
