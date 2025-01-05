using HR_Management.BLL.Specification;
using HR_Management.DAL.Entities;
namespace HR_Managment.BLL.Specification.EmployeeSpecs
{
    public class DepartmentSpecification : BassSpecification<Department>
    {
        public DepartmentSpecification(EntitySpecParams Params):base(D=>
            (String.IsNullOrWhiteSpace(Params.Search) || D.Name.ToLower().Contains(Params.Search.ToLower()))
        )
        {
            AddIncludes(E => E.Manager);
            ApplyPagination((Params.pageNumber - 1) * Params.PageSize, Params.PageSize);
        }

        public DepartmentSpecification(int id) : base(D => D.Id == id)
        {
            AddIncludes(E => E.Manager);
        }
    }
}
