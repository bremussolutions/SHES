using BSolutions.SHES.App.ComponentModels;
using Microsoft.UI.Xaml.Controls;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace BSolutions.SHES.App.Components
{
    public sealed partial class ProjectItemDevicesComponent : UserControl
    {
        public ProjectItemDevicesComponentModel ViewModel { get; }

        public ProjectItemDevicesComponent()
        {
            ViewModel = App.GetService<ProjectItemDevicesComponentModel>();
            this.InitializeComponent();
        }

        private void UserControl_SizeChanged(object sender, Microsoft.UI.Xaml.SizeChangedEventArgs e)
        {
            this.DataGridScrollViewer.Height = e.NewSize.Height - 50;
        }
    }
}
