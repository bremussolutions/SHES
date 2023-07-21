using BSolutions.SHES.Data.Repositories.Devices;
using BSolutions.SHES.Models.Entities;
using BSolutions.SHES.Models.Observables;
using Microsoft.Extensions.Logging;
using Microsoft.Windows.ApplicationModel.Resources;
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

        #region --- Constructor ---

        /// <summary>Initializes a new instance of the <see cref="DeviceService" /> class.</summary>
        /// <param name="logger">The logger.</param>
        /// <param name="deviceRepository">The device repository.</param>
        public DeviceService(ResourceLoader resourceLoader, ILogger<DeviceService> logger, IDeviceRepository deviceRepository)
            : base(resourceLoader, logger)
        {
            this._deviceRepository = deviceRepository;
        }

        #endregion

        #region --- IDeviceService ---

        /// <summary>Gets the devices for location asynchronous.</summary>
        /// <param name="observableProjectItem">The observable project item.</param>
        /// <returns>Returns devices.</returns>
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

        #endregion
    }
}
