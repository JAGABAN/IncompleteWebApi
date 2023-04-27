using MyPersonalProject.Models;
using System.Linq.Expressions;

namespace MyPersonalProject.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        Task<List<T>> GetAllContactsAsync(Expression<Func<T, bool>> filter = null);

        Task<T> GetAsync(Expression<Func<T, bool>> filter = null);
        public  Task CreateAsync(T entity);

        Task RemoveAsync(T entity);

        Task SaveAsync();
    }
}
