using HR_Management.DAL.Context;
using HR_Managment.BLL.Interfaces;
using System.Collections;

namespace HR_Managment.BLL.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly HRSystemDbContext _dbContext;
        private Hashtable _repositories;

        public UnitOfWork(HRSystemDbContext dbContext)
        {
            _dbContext = dbContext;
            _repositories = new Hashtable();
        }
        public async Task<int> CompleteAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }

        public async ValueTask DisposeAsync()
        {
             await _dbContext.DisposeAsync();
        }

        public IGenericRepository<T> GenerateGenericRepo<T>() where T : class
        {
            var type = typeof(T).Name;
            if (!_repositories.ContainsKey(type)) 
            {
                var Repository = new GenericRepository<T>(_dbContext);
                _repositories.Add(type, Repository);
            }
            return _repositories[type] as IGenericRepository<T>;
        }
    }
}
