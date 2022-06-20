using BSolutions.SHES.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BSolutions.SHES.Data.Repositories.ProjectItems
{
    public class ProjectItemRepository : Repository<ProjectItem>, IProjectItemRepository
    {
        #region --- Constructor ---

        /// <summary>Initializes a new instance of the <see cref="ProjectItemRepository" /> class.</summary>
        /// <param name="logger">The logger.</param>
        /// <param name="dbContext">The database context.</param>
        public ProjectItemRepository(ILogger<ProjectItemRepository> logger, ShesDbContext dbContext)
            : base(logger, dbContext)
        {
        }

        #endregion

        #region --- IProjectItemRepository ---

        /// <summary>Gets the project items tree for a project asynchronous.</summary>
        /// <param name="projectId">The project identifier.</param>
        /// <param name="includeDevices">if set to <c>true</c> [include devices].</param>
        /// <returns>Returns the project items tree.</returns>
        public async Task<List<ProjectItem>> GetProjectItemTreeAsync(Guid projectId, bool includeDevices)
        {
            return await Task.Run(() =>
            {
                return this.LoadProjectItemChildren(projectId, includeDevices).ToList();
            });
        }

        #endregion

        #region --- Helper ---

        private IEnumerable<ProjectItem> LoadProjectItemChildren(Guid projectId, bool includeDevices)
        {
            IQueryable<ProjectItem> projectItems = this._dbContext.Buildings
                .Include(b => b.Children)
                .Where(b => b.Project.Id == projectId);

            foreach (ProjectItem entity in projectItems)
            {
                if (entity.Children != null && entity.Children.Any())
                    entity.Children = LoadProjectItemGrandchildren(entity, includeDevices).ToList();

                yield return entity;
            }
        }

        private IEnumerable<ProjectItem> LoadProjectItemGrandchildren(ProjectItem parent, bool includeDevices)
        {
            List<ProjectItem> children = this._dbContext.ProjectItems
                .Where(pi => pi.Parent != null && pi.Parent.Id == parent.Id)
                .Include(pi => pi.Parent)
                .ToList();

            if (!includeDevices)
            {
                children = children.Where(c => c.GetType() != typeof(Device)).ToList();
            }

            foreach (ProjectItem entity in children)
            {
                entity.Children = LoadProjectItemGrandchildren(entity, includeDevices).ToList();
                yield return entity;
            }
        }

        #endregion
    }
}
