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
using Microsoft.UI.Xaml.Controls;
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

        private ObservableProjectItem _currentLocation;
        public ObservableProjectItem CurrentLocation
        {
            get => _currentLocation;
            private set
            {
                if (value != null)
                {
                    SetProperty(ref _currentLocation, value);
                    this.LoadDevicesForLocationAsync();
                }
            }
        }

        private ObservableDevice _currentDevice;
        public ObservableDevice CurrentDevice
        {
            get => _currentDevice;
            private set
            {
                if (value != null)
                {
                    SetProperty(ref _currentDevice, value);
                }
            }
        }

        public List<DeviceType> DeviceTypes { get; set; }

        #endregion

        #region --- Constructor ---

        /// <summary>Initializes a new instance of the <see cref="BuildingStructureViewModel" /> class.</summary>
        /// <param name="projectItemService">The project item service.</param>
        /// <param name="deviceService">The device service.</param>
        public BuildingStructureViewModel(IProjectItemService projectItemService, IDeviceService deviceService)
        {
            this._projectItemService = projectItemService;
            this._deviceService = deviceService;

            // Messages
            WeakReferenceMessenger.Default.Register<BuildingStructureViewModel, CurrentLocationChangedMessage>(this, (r, m) => r.CurrentLocation = m.Value);

            // Device Types
            // https://code.4noobz.net/wpf-enum-binding-with-description-in-a-combobox/
            this.DeviceTypes = Enum.GetValues(typeof(DeviceType))
                .Cast<DeviceType>()
                .ToList();
        }

        #endregion

        #region --- Events ---

        /// <summary>Handles the CellEditEnded event of the DataGrid control.</summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DataGridCellEditEndedEventArgs" /> instance containing the event data.</param>
        public async void DataGrid_CellEditEnded(object sender, DataGridCellEditEndedEventArgs e)
        {
            ObservableDevice changedDevice = e.Row.DataContext as ObservableDevice;

            if (changedDevice != null)
            {
                await this._projectItemService.UpdateAsync(changedDevice);
            }
        }

        /// <summary>Handles the SelectionChanged event of the DataGrid control.</summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="SelectionChangedEventArgs" /> instance containing the event data.</param>
        public void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.CurrentDevice = ((DataGrid)sender).SelectedItem as ObservableDevice;
        }

        /// <summary>Inputs the field value changed.</summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        public async void InputField_ValueChanged(object sender, object e)
        {
            await this._projectItemService.UpdateAsync(this._currentDevice);
        }

        /// <summary>Handles the Sorting event of the DataGrid control.</summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DataGridColumnEventArgs" /> instance containing the event data.</param>
        public void DataGrid_Sorting(object sender, DataGridColumnEventArgs e)
        {
            DataGrid dataGrid = (DataGrid)sender;

            // Sorting columns
            if(e.Column.Tag != null)
            {
                if (e.Column.Tag.ToString() == "Location")
                {
                    if (e.Column.SortDirection == null || e.Column.SortDirection == DataGridSortDirection.Descending)
                    {
                        this.Devices = new ObservableCollection<ObservableDevice>(this.Devices.OrderBy(d => d.Parent.Name));
                        e.Column.SortDirection = DataGridSortDirection.Ascending;
                    }
                    else
                    {
                        this.Devices = new ObservableCollection<ObservableDevice>(this.Devices.OrderByDescending(d => d.Parent.Name));
                        e.Column.SortDirection = DataGridSortDirection.Descending;
                    }
                }
                else
                {
                    if (e.Column.SortDirection == null || e.Column.SortDirection == DataGridSortDirection.Descending)
                    {
                        this.Devices = new ObservableCollection<ObservableDevice>(this.Devices.OrderBy(e.Column.Tag.ToString()));
                        e.Column.SortDirection = DataGridSortDirection.Ascending;
                    }
                    else
                    {
                        this.Devices = new ObservableCollection<ObservableDevice>(this.Devices.OrderByDescending(e.Column.Tag.ToString()));
                        e.Column.SortDirection = DataGridSortDirection.Descending;
                    }
                }

                OnPropertyChanged(nameof(this.Devices));
            }

            // Remove sorting indicators from other columns
            foreach (var column in dataGrid.Columns)
            {
                if (column.Tag != null && column.Tag.ToString() != e.Column.Tag.ToString())
                {
                    column.SortDirection = null;
                }
            }
        }

        #endregion

        private async void LoadDevicesForLocationAsync()
        {
            var devices = await this._deviceService.GetDevicesForLocationAsync(this.CurrentLocation);


            this.Devices.Clear();
            this.Devices.AddRange(devices);
        }
    }
}
