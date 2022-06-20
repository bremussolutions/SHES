using BSolutions.SHES.App.Messages;
using BSolutions.SHES.Models.Entities;
using BSolutions.SHES.Models.Enumerations;
using BSolutions.SHES.Models.Observables;
using BSolutions.SHES.Services.Devices;
using BSolutions.SHES.Services.ProjectItems;
using BSolutions.SHES.Shared.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.WinUI.UI.Controls;
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
        private readonly IProjectItemService _projectItemService;

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
                    this.LoadDevicesForProjectItemAsync();
                }
            }
        }

        public List<DeviceType> DeviceTypes { get; set; }

        #endregion

        #region --- Constructor ---

        public BuildingStructureViewModel(IProjectItemService projectItemService, IDeviceService deviceService)
        {
            this._projectItemService = projectItemService;
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

        #region --- Events ---

        public async void DataGrid_CellEditEnded(object sender, DataGridCellEditEndedEventArgs e)
        {

            ObservableDevice changedDevice = e.Row.DataContext as ObservableDevice;

            if (changedDevice != null)
            {
                await this._projectItemService.UpdateAsync(changedDevice);
                //await this._deviceService.UpdateAsync(changedDevice);
            }
        }

        #endregion

        private async void LoadDevicesForProjectItemAsync()
        {
            //this._deviceService.UpdateRangeAsync(this.Devices);
            var devices = await this._deviceService.GetDevicesForLocationAsync(this.CurrentProjectItem);


            this.Devices.Clear();
            this.Devices.AddRange(devices);
        }
    }
}
