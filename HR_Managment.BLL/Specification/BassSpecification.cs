using System.Linq.Expressions;

namespace HR_Managment.BLL.Specification
{
    public class BassSpecification<T> : ISpecification<T> where T : class
    {
        public Expression<Func<T, bool>> Criteria { get; set; }
        public List<Expression<Func<T, object>>> Includes { get; set; } = new List<Expression<Func<T, object>>>();
        public Expression<Func<T, object>> OrderBy { get; set; }
        public Expression<Func<T, object>> OrderByDesc { get; set; }
        public int Take { get; set; }
        public int Skip { get; set; }
        public bool IsPaginationEnabled { get; set; }
        public BassSpecification(){}
        public BassSpecification(Expression<Func<T,bool>> Criteria)
        {
            this.Criteria = Criteria;
        }
        protected void AddIncludes(Expression<Func<T, object>> Include)
        {
            Includes.Add(Include);
        }
        public void AddOrderBy(Expression<Func<T, object>> orderByExpression)
        {
            OrderBy = orderByExpression;
        }
        public void AddOrderByDesc(Expression<Func<T, object>> orderByDescExpression)
        {
            OrderByDesc = orderByDescExpression;
        }
        public void ApplyPagination(int skip, int take)
        {
            IsPaginationEnabled = true;
            Take = take;
            Skip = skip;
        }
    }
}
