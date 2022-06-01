using BSolutions.SHES.Models.Observables;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace BSolutions.SHES.Services.ProjectItems
{
    public interface IProjectItemService : IService
    {
        Task<ObservableProjectItem> AddAsync(ObservableProjectItem observableProjectItem);
        Task AddRangeAsync(ObservableCollection<ObservableProjectItem> observableProjectItems);
        Task<ObservableCollection<ObservableProjectItem>> GetProjectItemsAsync(ObservableProject observableProject);
        Task<ObservableProjectItem> UpdateAsync(ObservableProjectItem observableProjectItem);
    }
}