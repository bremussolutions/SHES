using BSolutions.SHES.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace BSolutions.SHES.Data.Repositories.Projects
{
    public class ProjectRepository : Repository<Project>, IProjectRepository
    {
        #region --- Constructor ---

        /// <summary>Initializes a new instance of the <see cref="ProjectRepository" /> class.</summary>
        /// <param name="logger">The logger.</param>
        /// <param name="dbContext">The database context.</param>
        public ProjectRepository(ILogger<ProjectRepository> logger, ShesDbContext dbContext)
            : base(logger, dbContext)
        {
        }

        #endregion

        #region --- IProjectRepository ---

        /// <summary>Check if a project exists asynchronous.</summary>
        /// <param name="project">The project.</param>
        /// <returns>Returns the result of the check.</returns>
        public async Task<bool> ExistsAsync(Project project)
        {
            return await this.ExistsAsync(project.Name);
        }

        /// <summary>Check by name if a project exists asynchronous.</summary>
        /// <param name="name">The project name.</param>
        /// <returns>Returns the result of the check.</returns>
        public async Task<bool> ExistsAsync(string name)
        {
            try
            {
                return await this._dbContext.Projects.AnyAsync(p => p.Name == name);
            }
            catch (Exception ex)
            {
                this._logger.LogCritical(ex, ex.Message);
                throw;
            }
        }

        #endregion
    }
}
