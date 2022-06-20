using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BSolutions.SHES.Data.Repositories
{
    public interface IRepository<TEntity> where TEntity : class, new()
    {
        /// <summary>Gets all entities asynchronous.</summary>
        /// <returns>Returns a list of all entities.</returns>
        Task<List<TEntity>> GetAllAsync();

        /// <summary>Gets an entity by identifier asynchronous.</summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Returns an entity.</returns>
        Task<TEntity> GetByIdAsync(Guid id);

        /// <summary>Gets an entity by expression asynchronous.</summary>
        /// <param name="expression">The expression.</param>
        /// <param name="includeProperties">The include properties.</param>
        /// <returns>Returns an entity.</returns>
        Task<List<TEntity>> GetByExpressionAsync(Expression<Func<TEntity, bool>> expression, string includeProperties = "");

        /// <summary>Adds an entity asynchronous.</summary>
        /// <param name="entity">The entity.</param>
        /// <exception cref="System.ArgumentNullException">entity - Entity must not be null.</exception>
        Task AddAsync(TEntity entity);

        /// <summary>Updates an entity asynchronous.</summary>
        /// <param name="entity">The entity.</param>
        /// <exception cref="System.ArgumentNullException">entity - Entity must not be null.</exception>
        Task UpdateAsync(TEntity entity);

        /// <summary>Deletes an entity asynchronous.</summary>
        /// <param name="entity">The entity.</param>
        Task DeleteAsync(TEntity entity);
    }
}
