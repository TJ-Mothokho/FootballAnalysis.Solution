using System;
using System.Collections.Generic;
using System.Text;

namespace FootballAnalysis.Data.Domain.Interfaces
{
    public interface IRepository<T> where T : class 
    {
        Task<IEnumerable<T>> ListAsync();
        Task<T> GetAsync(object id);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity); 
        Task DeleteAsync(object id);
    }
}
