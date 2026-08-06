using System;
using System.Collections.Generic;
using System.Text;

namespace FootballAnalysis.Data.Domain.Interfaces
{
    public interface IRepository<T> where T : class 
    {
        Task<IEnumerable<T>> ListAsync();
        Task<T> GetAsync(object id);
        Task<T> AddAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task<bool> DeleteAsync(T entity); 
        Task<bool> DeleteAsync(object id);

        // Predicate-based methods
        Task<T> AnyAsync(Func<T, bool> predicate);
        Task<IEnumerable<T>> AnyListAsync(Func<T, bool> predicate);
    }
}
