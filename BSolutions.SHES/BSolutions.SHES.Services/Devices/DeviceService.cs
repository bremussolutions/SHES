using BSolutions.SHES.Data.Repositories.Devices;
using BSolutions.SHES.Models.Entities;
using BSolutions.SHES.Models.Observables;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace BSolutions.SHES.Services.Devices
{
    public class DeviceService : ServiceBase, IDeviceService
    {
        private readonly IDeviceRepository _deviceRepository;

        public DeviceService(ILogger<DeviceService> logger, IDeviceRepository deviceRepository)
            : base(logger)
        {
            this._deviceRepository = deviceRepository;
        }

        public async Task<List<ObservableDevice>> GetDevicesForLocationAsync(ObservableProjectItem observableProjectItem)
        {
            try
            {
                var devices = await this._deviceRepository.GetDevicesForLocationAsync(observableProjectItem.entity);

                return devices.Select(d => new ObservableDevice(d)).ToList();
            }
            catch
            {
                return null;
            }
        }
    }
}
