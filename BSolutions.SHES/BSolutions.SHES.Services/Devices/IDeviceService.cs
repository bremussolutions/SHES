using BSolutions.SHES.Models.Entities;
using BSolutions.SHES.Models.Observables;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace BSolutions.SHES.Services.Devices
{
    public interface IDeviceService : IService
    {
        Task<List<ObservableDevice>> GetDevicesForLocationAsync(ObservableProjectItem observableProjectItem);
        //Task<ObservableDevice> UpdateAsync(ObservableDevice observableDevice);
        //Task UpdateRangeAsync(ObservableCollection<ObservableDevice> observableDevices);
    }
}
