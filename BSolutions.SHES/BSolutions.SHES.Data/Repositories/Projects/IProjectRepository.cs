using BSolutions.SHES.Models.Entities;
using System.Threading.Tasks;

namespace BSolutions.SHES.Data.Repositories.Projects
{
    public interface IProjectRepository : IRepository<Project>
    {
        Task<bool> ExistsAsync(Project project);
        Task<bool> ExistsAsync(string name);
    }
}