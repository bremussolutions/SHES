using BSolutions.SHES.App.Messages;
using BSolutions.SHES.Models.Entities;
using BSolutions.SHES.Models.Observables;
using BSolutions.SHES.Services.Devices;
using BSolutions.SHES.Services.ProjectItems;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace BSolutions.SHES.App.ComponentModels
{
    public class ProjectItemMetadataComponentModel : ObservableRecipient
    {
        private readonly IProjectItemService _projectItemService;
        private readonly IDeviceService _deviceService;

        #region --- Properties ---

        private ObservableProjectItem _currentProjectItem;
        public ObservableProjectItem CurrentProjectItem
        {
            get => _currentProjectItem;
            private set
            {
                if (value != null)
                {
                    SetProperty(ref _currentProjectItem, value);

                    if (value.entity != null && value.entity.GetType() == typeof(Device))
                    {
                        this.CurrentDevice = new ObservableDevice((Device)value.entity);
                    }
                    else
                    {
                        this.CurrentDevice = null;
                        this.SelectedTabIndex = 0;
                    }
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
                OnPropertyChanged(nameof(this.DetailsTabVisibility));
            }
        }

        public Visibility DetailsTabVisibility
        {
            get => this.CurrentDevice != null ? Visibility.Visible : Visibility.Collapsed;
        }

        private int _selectedTabIndex = 0;
        public int SelectedTabIndex
        {
            get => _selectedTabIndex;
            set
            {
                SetProperty(ref _selectedTabIndex, value);
            }
        }

        #endregion

        #region --- Constructor ---

        /// <summary>Initializes a new instance of the <see cref="ProjectItemMetadataComponentModel" /> class.</summary>
        public ProjectItemMetadataComponentModel(IProjectItemService projectItemService, IDeviceService deviceService)
        {
            // Messages
            WeakReferenceMessenger.Default.Register<ProjectItemMetadataComponentModel, CurrentDevicesProjectItemChangedMessage>(this, (r, m) => r.CurrentProjectItem = m.Value);

            this._projectItemService = projectItemService;
            this._deviceService = deviceService;
        }

        #endregion

        #region --- Events ---

        /// <summary>Inputs the field value changed.</summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        public async void InputField_ValueChanged(object sender, object e)
        {
            await this._projectItemService.UpdateAsync(this.CurrentProjectItem);
        }

        #endregion
    }
}
