using BSolutions.SHES.Models.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BSolutions.SHES.Data.Repositories.ProjectItems
{
    public interface IProjectItemRepository : IRepository<ProjectItem>
    {
        /// <summary>Gets the project items tree for a project asynchronous.</summary>
        /// <param name="projectId">The project identifier.</param>
        /// <param name="includeDevices">if set to <c>true</c> [include devices].</param>
        /// <returns>Returns the project items tree.</returns>
        Task<List<ProjectItem>> GetProjectItemTreeAsync(Guid projectId, bool includeDevices);
    }
}