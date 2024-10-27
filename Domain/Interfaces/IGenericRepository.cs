using System.Linq.Expressions;

namespace Domain.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<bool> AnyAsync();
        Task<bool> ExistAsync(Expression<Func<T, bool>> expression);
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetByIdAsync(Guid id);
        Task<T> AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task UpsertEntity(T entity);
    }
}
