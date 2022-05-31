using BSolutions.SHES.App.ViewModels;

using Microsoft.UI.Xaml.Controls;

namespace BSolutions.SHES.App.Views
{
    public sealed partial class MainPage : Page
    {
        public MainViewModel ViewModel { get; }

        public MainPage()
        {
            ViewModel = App.GetService<MainViewModel>();
            InitializeComponent();
        }
    }
}
