using HR_Management.DAL.Entities;

namespace HR_Managemnet.PL.Helpers
{
    public class AttendancePageResult : PagedReuslt<Attendance>
    {
        public int EmployeeId { get; set; }
        public string EmpName { get; set; }
        public DateTime? DateStart { get; set; }
        public DateTime? DateEnd { get; set; }
    }
}
