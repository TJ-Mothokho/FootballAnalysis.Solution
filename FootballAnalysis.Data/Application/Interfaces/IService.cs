using System;
using System.Collections.Generic;
using System.Text;

namespace FootballAnalysis.Data.Application.Interfaces
{
    public interface IService<TGet, TCreate, TUpdate> 
        where TGet : class 
        where TCreate : class 
        where TUpdate : class
    {
        Task<IEnumerable<TGet>> GetAllAsync();
        Task<TGet> GetByIdAsync(Guid id);
        Task<TGet> CreateAsync(TCreate entity);
        Task<TGet> UpdateAsync(Guid id, TUpdate entity);
        Task<bool> DeleteAsync(Guid id);
    }
}
