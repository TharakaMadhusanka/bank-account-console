using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Repositories
{
    public class GenericRepository<T>(GicBankDbContext bankDbContext) : IGenericRepository<T> where T : class
    {
        private readonly GicBankDbContext _context = bankDbContext;

        public async Task<bool> AnyAsync()
        {
            return await _context.Set<T>().AnyAsync();
        }
        public async Task<bool> ExistAsync(Expression<Func<T, bool>> expression)
        {
            return await _context.Set<T>().AnyAsync(expression);
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public virtual async Task<T?> GetByIdAsync(Guid id)
        {
            return await _context.Set<T>().FindAsync(id);

        }

        public virtual async Task<T> AddAsync(T entity)
        {
            var entry = _context.Entry(entity);
            if (entry.State == EntityState.Modified)
            {
                Console.WriteLine("Entity is modified");
            }
            else
            {
                Console.WriteLine("Entity state is not modified");
            }
            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public virtual async Task UpdateAsync(T entity)
        {

            _context.Set<T>().Update(entity);
            await _context.SaveChangesAsync();
        }

        public virtual async Task DeleteAsync(T entity)
        {
            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();

        }
    }
}
