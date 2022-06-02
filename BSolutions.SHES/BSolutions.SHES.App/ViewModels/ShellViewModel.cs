using System;

using BSolutions.SHES.App.Contracts.Services;
using BSolutions.SHES.App.Messages;
using BSolutions.SHES.App.Views;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.ComponentModel;

using Microsoft.UI.Xaml.Navigation;
using BSolutions.SHES.Models.Observables;
using Microsoft.UI.Xaml;

namespace BSolutions.SHES.App.ViewModels
{
    public class ShellViewModel : ObservableRecipient
    {
        private bool _isBackEnabled;
        private object _selected;
        private ObservableProject _currentProject;

        public INavigationService NavigationService { get; }

        public INavigationViewService NavigationViewService { get; }

        public bool IsBackEnabled
        {
            get { return _isBackEnabled; }
            set { SetProperty(ref _isBackEnabled, value); }
        }

        public object Selected
        {
            get { return _selected; }
            set { SetProperty(ref _selected, value); }
        }

        
        public ObservableProject CurrentProject
        {
            get => _currentProject;
            private set
            {
                SetProperty(ref _currentProject, value);
                OnPropertyChanged(nameof(this.ProjectNavigationVisibility));
            }
        }

        public Visibility ProjectNavigationVisibility
        {
            get => _currentProject != null ? Visibility.Visible : Visibility.Collapsed;
        }

        public ShellViewModel(INavigationService navigationService, INavigationViewService navigationViewService)
        {
            NavigationService = navigationService;
            NavigationService.Navigated += OnNavigated;
            NavigationViewService = navigationViewService;

            WeakReferenceMessenger.Default.Register<ShellViewModel, CurrentProjectChangedMessage>(this, (r, m) => r.CurrentProject = m.Value);
        }

        private void OnNavigated(object sender, NavigationEventArgs e)
        {
            IsBackEnabled = NavigationService.CanGoBack;
            if (e.SourcePageType == typeof(SettingsPage))
            {
                Selected = NavigationViewService.SettingsItem;
                return;
            }

            var selectedItem = NavigationViewService.GetSelectedItem(e.SourcePageType);
            if (selectedItem != null)
            {
                WeakReferenceMessenger.Default.Send(new CurrentProjectChangedMessage(this.CurrentProject));
                Selected = selectedItem;
            }
        }
    }
}
