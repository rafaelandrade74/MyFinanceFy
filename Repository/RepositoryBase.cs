using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using MyFinanceFy.Data;
using MyFinanceFy.Repository.Contracts;

namespace MyFinanceFy.Repository
{
    public class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
         protected ApplicationDbContext dbContext { get; set; }
        public RepositoryBase(ApplicationDbContext _dbContext)
        {
            dbContext = _dbContext;
        }

        public IQueryable<T> FindAll()
        {
            return dbContext.Set<T>().AsNoTracking();
        }
        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression)
        {
            return dbContext.Set<T>()
                .Where(expression).AsNoTracking();
        }

        public void Create(T entity)
        {
            dbContext.Set<T>().Add(entity);
            dbContext.SaveChanges();
        }
        public void Update(T entity)
        {
            dbContext.Set<T>().Update(entity);
            dbContext.SaveChanges();
        }
        public void Delete(T entity)
        {
            dbContext.Set<T>().Remove(entity);
            dbContext.SaveChanges();
        }
    }
}