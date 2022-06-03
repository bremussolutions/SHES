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
        public ProjectItemRepository(ILogger<ProjectItemRepository> logger, ShesDbContext dbContext)
            : base(logger, dbContext)
        {
        }

        public async Task<List<ProjectItem>> GetProjectItemTreeAsync(Guid projectId, bool includeDevices)
        {
            return await Task.Run(() =>
            {
                return this.LoadProjectItemChildren(projectId, includeDevices).ToList();
            });
        }

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
            IQueryable<ProjectItem> children = this._dbContext.ProjectItems
            .Include(pi => pi.Parent);

            if(includeDevices)
            {
                children = children.Where(pi => pi.Parent != null && pi.Parent.Id == parent.Id);
            }
            else
            {
                children = children.Where(pi => pi.Parent != null && pi.Parent.Id == parent.Id && pi.Discriminator != nameof(Device));
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
