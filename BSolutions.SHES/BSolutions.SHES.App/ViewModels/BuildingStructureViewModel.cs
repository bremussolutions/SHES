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
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using Windows.UI.Core;
using Microsoft.UI.Xaml;

namespace BSolutions.SHES.App.ViewModels
{
    public class BuildingStructureViewModel : ObservableRecipient
    {
        private Visibility _locationTabVisibility = Visibility.Visible;
        private Visibility _cabinetTabVisibility = Visibility.Collapsed;

        #region --- Properties ---

        private ObservableProjectItem _currentLocation;
        public ObservableProjectItem CurrentProjectItem
        {
            get => _currentLocation;
            private set
            {
                if (value != null)
                {
                    ObservableProjectItem previousItem = _currentLocation;
                    SetProperty(ref _currentLocation, value);

                    if (value.Id != previousItem?.Id)
                    {
                        this.DetailsVisibility(value.entity.GetType());
                    }
                }
            }
        }

        public Visibility LocationTabVisibility
        {
            get => _locationTabVisibility;
            private set
            {
                SetProperty(ref _locationTabVisibility, value);
            }
        }

        public Visibility CabinetTabVisibility
        {
            get => _cabinetTabVisibility;
            private set
            {
                SetProperty(ref _cabinetTabVisibility, value);
            }
        }

        public List<DeviceType> DeviceTypes { get; set; }

        #endregion

        #region --- Constructor ---

        /// <summary>Initializes a new instance of the <see cref="BuildingStructureViewModel" /> class.</summary>
        /// <param name="projectItemService">The project item service.</param>
        /// <param name="deviceService">The device service.</param>
        public BuildingStructureViewModel()
        {
            // Messages
            WeakReferenceMessenger.Default.Register<BuildingStructureViewModel, CurrentTreeProjectItemChangedMessage>(this, (r, m) => r.CurrentProjectItem = m.Value);

            // Device Types
            // https://code.4noobz.net/wpf-enum-binding-with-description-in-a-combobox/
            this.DeviceTypes = Enum.GetValues(typeof(DeviceType))
                .Cast<DeviceType>()
                .ToList();
        }

        #endregion

        private void DetailsVisibility(Type locationType)
        {
            this.LocationTabVisibility = Visibility.Collapsed;
            this.CabinetTabVisibility = Visibility.Collapsed;

            switch (locationType.Name)
            {
                case "Cabinet":
                    this.LocationTabVisibility = Visibility.Visible;
                    this.CabinetTabVisibility = Visibility.Visible;
                    break;
                default:
                    this.LocationTabVisibility = Visibility.Visible;
                    break;
            }
        }
    }
}
