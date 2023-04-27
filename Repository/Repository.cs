using Microsoft.EntityFrameworkCore;
using MyPersonalProject.Data;
using MyPersonalProject.Models;
using MyPersonalProject.Repository.IRepository;
using System.Linq.Expressions;

namespace MyPersonalProject.Repository
{
    public class Repository<T>:IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _database;
        internal DbSet<T> dbSet;

        public Repository(ApplicationDbContext database)
        {
            _database = database;
            this.dbSet = _database.Set<T>();
        }
        public async Task CreateAsync(T entity)
        {
            await dbSet.AddAsync(entity);
            await SaveAsync();
        }

        public async Task<T> GetAsync(Expression<Func<T, bool>> filter = null)
        {
            IQueryable<T> query = dbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            return await query.FirstOrDefaultAsync();
        }

        public async Task<List<T>> GetAllContactsAsync(Expression<Func<T, bool>> filter = null)
        {
            IQueryable<T> query = dbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            return await query.ToListAsync();
        }

        public async Task RemoveAsync(T entity)
        {
            dbSet.Remove(entity);
            await SaveAsync();
        }

        public async Task SaveAsync()
        {
            await _database.SaveChangesAsync();
        }

       
    }
}
