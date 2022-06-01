using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BSolutions.SHES.Data.Repositories
{
    public interface IRepository<TEntity> where TEntity : class, new()
    {
        Task<List<TEntity>> GetAllAsync();

        Task<TEntity> GetByIdAsync(Guid id);

        Task<List<TEntity>> GetByExpressionAsync(Expression<Func<TEntity, bool>> expression, string includeProperties = "");

        Task AddAsync(TEntity entity);

        Task UpdateAsync(TEntity entity);

        Task DeleteAsync(TEntity entity);
    }
}
