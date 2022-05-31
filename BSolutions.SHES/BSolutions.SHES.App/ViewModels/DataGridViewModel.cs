using System;
using System.Collections.ObjectModel;

using BSolutions.SHES.App.Contracts.ViewModels;
using BSolutions.SHES.App.Core.Contracts.Services;
using BSolutions.SHES.App.Core.Models;

using CommunityToolkit.Mvvm.ComponentModel;

namespace BSolutions.SHES.App.ViewModels
{
    public class DataGridViewModel : ObservableRecipient, INavigationAware
    {
        private readonly ISampleDataService _sampleDataService;

        public ObservableCollection<SampleOrder> Source { get; } = new ObservableCollection<SampleOrder>();

        public DataGridViewModel(ISampleDataService sampleDataService)
        {
            _sampleDataService = sampleDataService;
        }

        public async void OnNavigatedTo(object parameter)
        {
            Source.Clear();

            // TODO: Replace with real data.
            var data = await _sampleDataService.GetGridDataAsync();

            foreach (var item in data)
            {
                Source.Add(item);
            }
        }

        public void OnNavigatedFrom()
        {
        }
    }
}
