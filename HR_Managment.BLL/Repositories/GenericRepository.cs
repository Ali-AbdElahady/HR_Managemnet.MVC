using HR_Management.DAL.Context;
using HR_Managment.BLL.Interfaces;
using HR_Managment.BLL.Specification;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR_Managment.BLL.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly HRSystemDbContext _dbContext;

        public GenericRepository(HRSystemDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task AddAsync(T entity)
        {
            await _dbContext.Set<T>().AddAsync(entity);
        }

        public void Delete(T entity)
        {
            _dbContext.Remove(entity);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
           return await _dbContext.Set<T>().ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }
        public void Update(T entity)
        {
            _dbContext.Set<T>().Update(entity);
        }

        public async Task<T> GetByIdWithSpecAsync(ISpecification<T> spec)
        {
            return await SpecificationEvalutor<T>.GetQuery(_dbContext.Set<T>(), spec).FirstOrDefaultAsync();
        }

        public async Task<int> GetCountWithSpecAsync(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).CountAsync();
        }

        public async Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecification<T> spec)
        {
            var data = await SpecificationEvalutor<T>.GetQuery(_dbContext.Set<T>(), spec).ToListAsync();
            return data;
        }
        private IQueryable<T> ApplySpecification(ISpecification<T> spec)
        {
            return SpecificationEvalutor<T>.GetQuery(_dbContext.Set<T>(), spec);
        }
    }
}
