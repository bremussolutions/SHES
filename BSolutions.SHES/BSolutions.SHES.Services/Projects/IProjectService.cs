using BSolutions.SHES.Models.Observables;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace BSolutions.SHES.Services.Projects
{
    public interface IProjectService : IService
    {
        Task<bool> DeleteAsync(ObservableProject observableProject);
        Task<bool> ExistsAsync(string name);
        Task<ObservableCollection<ObservableProject>> GetAllAsync();
        Task<ObservableProject> InsertAsync(ObservableProject observableProject);
        Task<ObservableProject> UpdateAsync(ObservableProject observableProject);
    }
}