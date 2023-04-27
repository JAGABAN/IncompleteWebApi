using Microsoft.EntityFrameworkCore;
using MyPersonalProject.Data;
using MyPersonalProject.Models;
using MyPersonalProject.Repository.IRepository;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MyPersonalProject.Repository
{
    public class ContactRepository : IContactRepository
    {
        private readonly ApplicationDbContext _database;

        public ContactRepository(ApplicationDbContext database)
        {
            _database = database;
        }
        public async Task CreateAsync(Contact entity)
        {
            await _database.Contacts.AddAsync(entity);
            await SaveAsync();
        }

        public async Task<Contact> GetAsync(Expression<Func<Contact, bool>> filter = null)
        {
            IQueryable<Contact> query = _database.Contacts;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            return await query.FirstOrDefaultAsync();
        }

        public async Task<List<Contact>> GetAllContactsAsync(Expression<Func<Contact, bool>> filter = null)
        {
            IQueryable<Contact> query = _database.Contacts;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            return await query.ToListAsync();
        }

        public async Task RemoveAsync(Contact entity)
        {
            _database.Contacts.Remove(entity);
            await SaveAsync();
        }

        public async Task SaveAsync()
        {
            await _database.SaveChangesAsync();
        }

      public async Task<Contact> UpdateAsync(Contact entity)
        {
          entity.UpdatedDate = DateTime.Now;
            _database.Contacts.Update(entity);
            await _database.SaveChangesAsync();
            return entity;

        }

       
    }
}
