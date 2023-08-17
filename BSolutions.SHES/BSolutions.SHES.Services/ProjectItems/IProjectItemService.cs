using BSolutions.SHES.Models.Entities;
using BSolutions.SHES.Models.Observables;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace BSolutions.SHES.Services.ProjectItems
{
    public interface IProjectItemService : IService
    {
        Task<ObservableProjectItem> AddAsync(ObservableProjectItem observableProjectItem);
        Task AddRangeAsync(ObservableCollection<ObservableProjectItem> observableProjectItems);
        Task<ObservableCollection<ObservableProjectItem>> GetProjectItemsAsync(ObservableProject observableProject, bool includeDevices = false);
        Task<ObservableProjectItem> UpdateAsync(ObservableProjectItem observableProjectItem);
        Task UpdateRangeAsync(ObservableCollection<ObservableProjectItem> observableProjectItems);
        Task<bool> DeleteAsync(ObservableProjectItem observableProjectItem);
    }
}