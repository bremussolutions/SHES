using BSolutions.SHES.App.Messages;
using BSolutions.SHES.Models.Enumerations;
using BSolutions.SHES.Models.Observables;
using BSolutions.SHES.Services.Devices;
using BSolutions.SHES.Shared.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace BSolutions.SHES.App.ViewModels
{
    public class BuildingStructureViewModel : ObservableRecipient
    {
        private readonly IDeviceService _deviceService;

        #region --- Properties ---

        public ObservableCollection<ObservableDevice> Devices { get; private set; } = new ObservableCollection<ObservableDevice>();

        private ObservableProjectItem _currentProjectItem;
        public ObservableProjectItem CurrentProjectItem
        {
            get => _currentProjectItem;
            private set
            {
                if (value != null)
                {
                    SetProperty(ref _currentProjectItem, value);
                    this.LoadDevicesForProjectItemAsync().Wait();
                }
            }
        }

        public List<DeviceType> DeviceTypes { get; set; }

        #endregion

        #region --- Constructor ---

        public BuildingStructureViewModel(IDeviceService deviceService)
        {
            this._deviceService = deviceService;

            // Messages
            WeakReferenceMessenger.Default.Register<BuildingStructureViewModel, CurrentProjectItemChangedMessage>(this, (r, m) => r.CurrentProjectItem = m.Value);

            // Device Types
            // https://code.4noobz.net/wpf-enum-binding-with-description-in-a-combobox/
            this.DeviceTypes = Enum.GetValues(typeof(DeviceType))
                .Cast<DeviceType>()
                .ToList();
        }

        #endregion

        private async Task LoadDevicesForProjectItemAsync()
        {
            var devices = await this._deviceService.GetDevicesForLocationAsync(this.CurrentProjectItem);

            this.Devices.Clear();
            this.Devices.AddRange(devices);
        }
    }
}
