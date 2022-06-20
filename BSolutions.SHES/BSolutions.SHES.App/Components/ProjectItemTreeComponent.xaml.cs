using BSolutions.SHES.App.ComponentModels;
using BSolutions.SHES.Models.Entities;
using BSolutions.SHES.Models.Observables;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace BSolutions.SHES.App.Components
{
    public sealed partial class ProjectItemTreeComponent : UserControl
    {
        public ProjectItemTreeComponentModel ViewModel { get; }

        public ProjectItemTreeComponent()
        {
            ViewModel = App.GetService<ProjectItemTreeComponentModel>();
            this.InitializeComponent();
        }

        private void ProjectElementTree_Tapped(object sender, Microsoft.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            this.ViewModel.SelectedProjectItem = this.ProjectElementTree.SelectedItem as ObservableProjectItem;
        }
    }
}
