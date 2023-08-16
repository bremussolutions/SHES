using BSolutions.SHES.App.Messages;
using BSolutions.SHES.Models.Entities;
using BSolutions.SHES.Models.Observables;
using BSolutions.SHES.Services.Devices;
using BSolutions.SHES.Services.ProjectItems;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace BSolutions.SHES.App.ComponentModels
{
    public class ProjectItemMetadataComponentModel : ObservableRecipient
    {
        private readonly IProjectItemService _projectItemService;
        private readonly IDeviceService _deviceService;
        private ObservableProjectItem _freezedProjectItem;

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

        /// <summary>
        /// This event freezes the current project item because when the item is changed in the tree view,
        /// the CurrentProjectItem property is changed first before the LostFocus event is called.
        /// </summary>
        /// <param name="sender">The event-sending input element.</param>
        /// <param name="e">The event parameters.</param>
        /// <remarks>The problem is that when changing an item in the tree view, the CurrentProjectItem is
        /// changed first and only then the LostFocus event is called.</remarks>
        public void InputField_GotFocus(object sender, object e)
        {
            this._freezedProjectItem = this.CurrentProjectItem;
        }

        /// <summary>
        /// This event is called when leaving the focus from a metadata input field and saves the changed project item.
        /// </summary>
        /// <param name="sender">The event-sending input element.</param>
        /// <param name="e">The event parameters.</param>
        public async void InputField_LostFocus(object sender, object e)
        {
            await this._projectItemService.UpdateAsync(this._freezedProjectItem);
        }

        #endregion
    }
}
