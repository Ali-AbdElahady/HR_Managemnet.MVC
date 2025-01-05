using HR_Management.DAL.Entities;

namespace HR_Managemnet.PL.Helpers
{
    public class empPageResult : PagedReuslt<Employee>
    {
        public int? departmentId { get; set; }
        public string? search { get; set; }
    }
}
