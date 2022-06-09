using BSolutions.SHES.Models.Entities;
using BSolutions.SHES.Models.Observables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSolutions.SHES.Services.Devices
{
    public interface IDeviceService : IService
    {
        Task<List<ObservableDevice>> GetDevicesForLocationAsync(ObservableProjectItem observableProjectItem);
    }
}
