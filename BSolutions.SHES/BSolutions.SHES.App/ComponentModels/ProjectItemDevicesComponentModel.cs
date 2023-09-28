using BSolutions.SHES.Models.Observables;
using BSolutions.SHES.Services.ProjectItems;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.WinUI.UI.Controls;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using System.Timers;
using Microsoft.UI.Dispatching;
using BSolutions.SHES.Services.Devices;
using CommunityToolkit.Mvvm.Messaging;
using BSolutions.SHES.App.Messages;

namespace BSolutions.SHES.App.ComponentModels
{
    public class ProjectItemDevicesComponentModel : ObservableRecipient
    {
        private readonly IDeviceService _deviceService;
        private readonly IProjectItemService _projectItemService;

        private DispatcherQueue _dispatcherQueue = DispatcherQueue.GetForCurrentThread();
        private List<ObservableDevice> _devicesForCurrentLocation;
        private readonly Timer _dataGridFilterTimer = new();
        private string _currentSortColumn;
        private DataGridSortDirection _currentSortDirection;

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
                    ObservableProjectItem previousItem = _currentProjectItem;
                    SetProperty(ref _currentProjectItem, value);

                    //if (value.Id != previousItem?.Id)
                    //{
                        this.LoadDevicesForLocationAsync();
                    //}
                }
            }
        }

        private ObservableDevice _currentDevice;
        public ObservableDevice CurrentDevice
        {
            get => _currentDevice;
            private set
            {
                SetProperty(ref _currentDevice, value);
            }
        }

        private string _dataGridFilter;
        public string DataGridFilter
        {
            get => _dataGridFilter;
            set
            {
                SetProperty(ref _dataGridFilter, value);
                this._dataGridFilterTimer.Stop();
                this._dataGridFilterTimer.Start();
            }
        }

        #endregion

        #region --- Constructor ---

        /// <summary>Initializes a new instance of the <see cref="ProjectItemDevicesComponentModel" /> class.</summary>
        public ProjectItemDevicesComponentModel(IDeviceService deviceService, IProjectItemService projectItemService)
        {
            // Messages
            WeakReferenceMessenger.Default.Register<ProjectItemDevicesComponentModel, CurrentTreeProjectItemChangedMessage>(this, (r, m) => r.CurrentProjectItem = m.Value);

            this._deviceService = deviceService;
            this._projectItemService = projectItemService;

            // Data Grid Filter Timer
            this._dataGridFilterTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            this._dataGridFilterTimer.Interval = 500;
            this._dataGridFilterTimer.AutoReset = false;
        }

        #endregion

        #region --- Events ---

        /// <summary>Handles the SelectionChanged event of the DataGrid control.</summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="SelectionChangedEventArgs" /> instance containing the event data.</param>
        public void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.CurrentDevice = ((DataGrid)sender).SelectedItem as ObservableDevice;
        }

        /// <summary>Handles the Sorting event of the DataGrid control.</summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DataGridColumnEventArgs" /> instance containing the event data.</param>
        public void DataGrid_Sorting(object sender, DataGridColumnEventArgs e)
        {
            DataGrid dataGrid = (DataGrid)sender;

            // Sorting columns
            if (e.Column.Tag != null)
            {
                this._currentSortColumn = e.Column.Tag.ToString();

                // Ascending
                if (e.Column.SortDirection == null || e.Column.SortDirection == DataGridSortDirection.Descending)
                {
                    this._currentSortDirection = DataGridSortDirection.Ascending;
                    e.Column.SortDirection = DataGridSortDirection.Ascending;
                }
                // Descending
                else
                {
                    this._currentSortDirection = DataGridSortDirection.Descending;
                    e.Column.SortDirection = DataGridSortDirection.Descending;
                }

                this.SortDevices();
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

        private void SortDevices()
        {
            if (!string.IsNullOrEmpty(this._currentSortColumn))
            {
                IQueryable<ObservableDevice> query = this.Devices.AsQueryable();

                // Ascending
                if (this._currentSortDirection == DataGridSortDirection.Ascending)
                {
                    query = query.OrderBy(this._currentSortColumn);
                }
                // Descending
                else
                {
                    query = query.OrderBy($"{this._currentSortColumn} desc");
                }

                this.Devices = new ObservableCollection<ObservableDevice>(query.ToList());
                OnPropertyChanged(nameof(this.Devices));
            }
        }

        private async void LoadDevicesForLocationAsync()
        {
            this._devicesForCurrentLocation = await this._deviceService.GetDevicesForLocationAsync(this.CurrentProjectItem);
            this.Devices = new ObservableCollection<ObservableDevice>(this._devicesForCurrentLocation);
            this.OnPropertyChanged(nameof(this.Devices));

            // Apply existing filter
            this.OnTimedEvent(this._dataGridFilterTimer, null);

            // Apply existing sorting
            this.SortDevices();
        }

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            if (this._devicesForCurrentLocation != null && !string.IsNullOrWhiteSpace(this.DataGridFilter))
            {
                this.Devices = new ObservableCollection<ObservableDevice>(this._devicesForCurrentLocation.Where(d => d.Name.Contains(this.DataGridFilter)));
                this._dispatcherQueue.TryEnqueue(() => this.OnPropertyChanged(nameof(this.Devices)));
            }
        }
    }
}
