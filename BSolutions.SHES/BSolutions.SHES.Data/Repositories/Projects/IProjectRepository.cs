using BSolutions.SHES.Models.Entities;
using System.Threading.Tasks;

namespace BSolutions.SHES.Data.Repositories.Projects
{
    public interface IProjectRepository : IRepository<Project>
    {
        /// <summary>Check if a project exists asynchronous.</summary>
        /// <param name="project">The project.</param>
        /// <returns>Returns the result of the check.</returns>
        Task<bool> ExistsAsync(Project project);

        /// <summary>Check by name if a project exists asynchronous.</summary>
        /// <param name="name">The project name.</param>
        /// <returns>Returns the result of the check.</returns>
        Task<bool> ExistsAsync(string name);
    }
}