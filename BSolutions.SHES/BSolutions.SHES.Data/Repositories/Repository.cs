using BSolutions.SHES.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BSolutions.SHES.Data.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : EntityBase, new()
    {
        #region --- Fields ---

        protected readonly ILogger _logger;
        protected readonly ShesDbContext _dbContext;

        #endregion

        #region --- Constructor ---

        public Repository(ILogger logger, ShesDbContext dbContext)
        {
            this._logger = logger;
            this._dbContext = dbContext;
        }

        #endregion

        public async Task<List<TEntity>> GetAllAsync()
        {
            try
            {
                return await this._dbContext.Set<TEntity>().ToListAsync();
            }
            catch (Exception ex)
            {
                this._logger.LogCritical(ex, ex.Message);
                throw;
            }
        }

        public async Task<TEntity> GetByIdAsync(Guid id)
        {
            try
            {
                return await this._dbContext.Set<TEntity>().FindAsync(id);
            }
            catch (Exception ex)
            {
                this._logger.LogCritical(ex, ex.Message);
                throw;
            }
        }

        public async Task<List<TEntity>> GetByExpressionAsync(Expression<Func<TEntity, bool>> expression, string includeProperties = "")
        {
            try
            {
                var query = this._dbContext.Set<TEntity>().Where(expression);

                if (!string.IsNullOrWhiteSpace(includeProperties))
                {
                    string[] includes = includeProperties.Split(';');

                    foreach (string include in includes)
                    {
                        query = query.Include(include);
                    }
                }

                return await query.ToListAsync();
            }
            catch (Exception ex)
            {
                this._logger.LogCritical(ex, ex.Message);
                throw;
            }
        }

        public async Task AddAsync(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity), "Entity must not be null");
            }

            try
            {
                await this._dbContext.AddAsync(entity);
                await this._dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                this._logger.LogCritical(ex, ex.Message);
                throw;
            }
        }

        public async Task UpdateAsync(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity), "Entity must not be null");
            }

            try
            {
                this.DetachEntity(entity);

                // set Modified flag in your entry
                this._dbContext.Entry(entity).State = EntityState.Modified;

                await this._dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                this._logger.LogCritical(ex, ex.Message);
                throw;
            }
        }

        public async Task DeleteAsync(TEntity entity)
        {
            try
            {
                this.DetachEntity(entity);

                this._dbContext.Remove(entity);
                await this._dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                this._logger.LogCritical(ex, ex.Message);
                throw;
            }
        }

        private void DetachEntity(TEntity entity)
        {
            var localEntity = this._dbContext.Set<TEntity>().Local.FirstOrDefault(entry => entry.Id.Equals(entity.Id));

            // check if local is not null 
            if (localEntity != null)
            {
                // detach
                this._dbContext.Entry(localEntity).State = EntityState.Detached;
            }
        }
    }
}
