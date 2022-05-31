using BSolutions.SHES.App.ViewModels;

using Microsoft.UI.Xaml.Controls;

namespace BSolutions.SHES.App.Views
{
    public sealed partial class ContentGridPage : Page
    {
        public ContentGridViewModel ViewModel { get; }

        public ContentGridPage()
        {
            ViewModel = App.GetService<ContentGridViewModel>();
            InitializeComponent();
        }
    }
}
