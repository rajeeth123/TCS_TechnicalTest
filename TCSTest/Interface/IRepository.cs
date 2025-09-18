using System.Collections.Generic;
using System.Threading.Tasks;

namespace TCSTest.Repositories
{
    public interface IRepository<T> where T : class
    {
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<T?> GetAsync(System.Guid id);
        Task<T> AddAsync(T item);
        Task<T?> UpdateAsync(System.Guid id, T item);
        Task<bool> DeleteAsync(System.Guid id);
    }
}
