using System;
using System.Linq;

using BSolutions.SHES.App.Contracts.ViewModels;
using BSolutions.SHES.App.Core.Contracts.Services;
using BSolutions.SHES.App.Core.Models;

using CommunityToolkit.Mvvm.ComponentModel;

namespace BSolutions.SHES.App.ViewModels
{
    public class ContentGridDetailViewModel : ObservableRecipient, INavigationAware
    {
        private readonly ISampleDataService _sampleDataService;
        private SampleOrder _item;

        public SampleOrder Item
        {
            get { return _item; }
            set { SetProperty(ref _item, value); }
        }

        public ContentGridDetailViewModel(ISampleDataService sampleDataService)
        {
            _sampleDataService = sampleDataService;
        }

        public async void OnNavigatedTo(object parameter)
        {
            if (parameter is long orderID)
            {
                var data = await _sampleDataService.GetContentGridDataAsync();
                Item = data.First(i => i.OrderID == orderID);
            }
        }

        public void OnNavigatedFrom()
        {
        }
    }
}
