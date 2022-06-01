using BSolutions.SHES.Models.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BSolutions.SHES.Data.Repositories.ProjectItems
{
    public interface IProjectItemRepository : IRepository<ProjectItem>
    {
        Task<List<ProjectItem>> GetProjectItemTreeAsync(Guid projectId);
    }
}