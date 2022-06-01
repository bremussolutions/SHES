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

        public async Task<List<ProjectItem>> GetProjectItemTreeAsync(Guid projectId)
        {
            return await Task.Run(() =>
            {
                return this.LoadProjectItemChildren(projectId).ToList();
            });
        }

        #region --- Helper ---

        private IEnumerable<ProjectItem> LoadProjectItemChildren(Guid projectId)
        {
            IQueryable<ProjectItem> projectItems = this._dbContext.Buildings
                .Include(b => b.Children)
                .Where(b => b.Project.Id == projectId);

            foreach (ProjectItem entity in projectItems)
            {
                if (entity.Children != null && entity.Children.Any())
                    entity.Children = LoadProjectItemGrandchildren(entity).ToList();

                yield return entity;
            }
        }

        private IEnumerable<ProjectItem> LoadProjectItemGrandchildren(ProjectItem parent)
        {
            IQueryable<ProjectItem> children = this._dbContext.ProjectItems
            .Include(x => x.Parent)
            .Where(w => w.Parent != null && w.Parent.Id == parent.Id);

            foreach (ProjectItem entity in children)
            {
                entity.Children = LoadProjectItemGrandchildren(entity).ToList();
                yield return entity;
            }
        }

        #endregion
    }
}
