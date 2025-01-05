using Microsoft.EntityFrameworkCore;

namespace HR_Managment.BLL.Specification
{
    public class SpecificationEvalutor<T> where T : class
    {
        public static IQueryable<T> GetQuery(IQueryable<T> inputQuery, ISpecification<T> spec)
        {
            var query = inputQuery.AsQueryable();
            if (spec.Criteria != null)
            {
                query = query.Where(spec.Criteria);
            }
            if (spec.OrderBy != null)
            {
                query = query.OrderBy(spec.OrderBy);
            }
            if (spec.OrderByDesc != null)
            {
                query = query.OrderByDescending(spec.OrderByDesc);
            }
            if (spec.IsPaginationEnabled)
            {
                query = query.Skip(spec.Skip).Take(spec.Take);
            }
            if (spec.Includes != null)
            {
                query = spec.Includes.Aggregate(query, (current, inclue) => current.Include(inclue));
            }
            return query;
        }
    }
}