using BSolutions.SHES.App.Messages;
using BSolutions.SHES.Models.Observables;
using BSolutions.SHES.Shared.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using System.Collections.ObjectModel;
using System.Linq;

namespace BSolutions.SHES.App.ViewModels
{
    public class BuildingStructureViewModel : ObservableRecipient
    {
        public ObservableCollection<ObservableProjectItem> Devices { get; private set; } = new ObservableCollection<ObservableProjectItem>();

        private ObservableProjectItem _currentProjectItem;
        public ObservableProjectItem CurrentProjectItem
        {
            get => _currentProjectItem;
            private set
            {
                if (value != null)
                {
                    SetProperty(ref _currentProjectItem, value);
                }
            }
        }

        public BuildingStructureViewModel()
        {
            // Messages
            WeakReferenceMessenger.Default.Register<BuildingStructureViewModel, CurrentProjectItemChangedMessage>(this, (r, m) => r.CurrentProjectItem = m.Value);
        }

        private void LoadDevices()
        {

        }
    }
}
