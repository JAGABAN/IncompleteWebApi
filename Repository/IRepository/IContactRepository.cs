using MyPersonalProject.Models;
using System.Linq.Expressions;

namespace MyPersonalProject.Repository.IRepository
{
    public interface IContactRepository:IRepository<Contact>
    {

        Task<Contact> UpdateAsync(Contact entity);

       // public Task UpdateAsync(Contact entity);

    }
}
