using HR_Management.DAL.Entities;

namespace HR_Managemnet.PL.Helpers
{
    public class DepPageResult : PagedReuslt<Department>
    {
        public string? search { get; set; }
    }
}
