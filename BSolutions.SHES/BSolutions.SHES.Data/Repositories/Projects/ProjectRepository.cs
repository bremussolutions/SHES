using BSolutions.SHES.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace BSolutions.SHES.Data.Repositories.Projects
{
    public class ProjectRepository : Repository<Project>, IProjectRepository
    {
        public ProjectRepository(ILogger<ProjectRepository> logger, ShesDbContext dbContext)
            : base(logger, dbContext)
        {
        }

        public async Task<bool> ExistsAsync(Project project)
        {
            return await this.ExistsAsync(project.Name);
        }

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
    }
}
