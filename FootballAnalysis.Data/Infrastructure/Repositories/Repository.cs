using FootballAnalysis.Data.Domain.Interfaces;
using FootballAnalysis.Data.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace FootballAnalysis.Data.Infrastructure.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;

        public Repository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<T> AddAsync(T entity)
        {
            var _entity = await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
            return _entity.Entity;
        }

        public async Task<bool> DeleteAsync(T entity)
        {
            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(object id)
        {
            var entity = await _context.Set<T>().FindAsync(id);
            if (entity == null)
                throw new KeyNotFoundException($"Entity of type {typeof(T).Name} with id {id} not found.");

            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<T> GetAsync(object id)
        {
            var entity = await _context.Set<T>().FindAsync(id);
            return entity ?? throw new KeyNotFoundException($"Entity of type {typeof(T).Name} with id {id} not found.");
        }

        public async Task<IEnumerable<T>> ListAsync()
        {
            var entities = await _context.Set<T>().ToListAsync();
            return entities ?? throw new KeyNotFoundException($"No entities of type {typeof(T).Name} found.");
        }

        public async Task<T> UpdateAsync(T entity)
        {
            var _entity = _context.Set<T>().Update(entity);
            await _context.SaveChangesAsync();
            return _entity.Entity;
        }
    }
}
