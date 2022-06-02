using BSolutions.SHES.App.Messages;
using BSolutions.SHES.Models;
using BSolutions.SHES.Models.Entities;
using BSolutions.SHES.Models.Observables;
using BSolutions.SHES.Services.ProjectItems;
using BSolutions.SHES.Shared.Extensions;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.ComponentModel;
using BSolutions.SHES.Models.Helpers;
using BSolutions.SHES.Models.Extensions;

namespace BSolutions.SHES.App.ComponentModels
{
    public class ProjectItemTreeComponentModel : ObservableRecipient
    {
        private readonly IProjectItemService _projectItemService;

        #region --- Properties ---

        public IAsyncRelayCommand AddProjectItemDialogCommand { get; }
        public IAsyncRelayCommand AddProjectItemCommand { get; }

        public ObservableCollection<ObservableProjectItem> ProjectItems { get; } = new ObservableCollection<ObservableProjectItem>();
        public ObservableCollection<ProjectItemTypeInfo> RestrictedProjectItemInfos { get; } = new ObservableCollection<ProjectItemTypeInfo>();

        private ObservableProject currentProject;
        public ObservableProject CurrentProject
        {
            get => currentProject;
            private set
            {
                SetProperty(ref currentProject, value);
            }
        }

        private ObservableProjectItem _selectedProjectItem;
        public ObservableProjectItem SelectedProjectItem
        {
            get => _selectedProjectItem;
            set
            {
                SetProperty(ref _selectedProjectItem, value);
                this.UpdateProjectItemTypes();
            }
        }

        private ProjectItemTypeInfo _newProjectItemType;
        public ProjectItemTypeInfo NewProjectItemType
        {
            get => _newProjectItemType;
            set
            {
                SetProperty(ref _newProjectItemType, value);
            }
        }

        private string _newProjectItemName;
        public string NewProjectItemName
        {
            get => _newProjectItemName;
            set
            {
                SetProperty(ref _newProjectItemName, value);
            }
        }

        #endregion

        #region --- Constructor ---

        /// <summary>Initializes a new instance of the <see cref="ProjectItemTreeComponentModel" /> class.</summary>
        /// <param name="projectItemService">The project item service.</param>
        public ProjectItemTreeComponentModel(IProjectItemService projectItemService)
        {
            this._projectItemService = projectItemService;

            // Commands
            AddProjectItemDialogCommand = new AsyncRelayCommand<ContentDialog>(async (dialog) => await AddProjectItemDialog(dialog));
            AddProjectItemCommand = new AsyncRelayCommand(AddProjectItem);

            // Messages
            WeakReferenceMessenger.Default.Register<ProjectItemTreeComponentModel, CurrentProjectChangedMessage>(this, (r, m) => r.CurrentProject = m.Value);
        }

        #endregion

        #region --- Events ---

        public async void OnLoaded(object sender, RoutedEventArgs e)
        {
            var projectItems = await this._projectItemService.GetProjectItemsAsync(this.CurrentProject);
            this.ProjectItems.AddRange(projectItems);
        }

        #endregion

        #region --- Commands ---

        /// <summary>Opens the dialog to add a project item.</summary>
        /// <param name="dialog">The dialog.</param>
        private async Task AddProjectItemDialog(ContentDialog dialog)
        {
            await dialog.ShowAsync();
        }

        /// <summary>Adds a project item.</summary>
        private async Task AddProjectItem()
        {
            ObservableProjectItem item = new ObservableProjectItem(ReflectionHelper.GetInstance<ProjectItem>(this.NewProjectItemType.FullName));
            item.Parent = this.SelectedProjectItem;
            item.Name = this.NewProjectItemName;

            // Insert new project item
            await this._projectItemService.AddAsync(item);

            // Add new project item to project tree
            this.SelectedProjectItem.Children.Add(item);
        }

        #endregion

        /// <summary>Updates the project item types for project item creation.</summary>
        private void UpdateProjectItemTypes()
        {
            this.RestrictedProjectItemInfos.Clear();

            if (this.SelectedProjectItem != null)
            {
                this.RestrictedProjectItemInfos.AddRange(this.SelectedProjectItem?.entity.GetRestrictChildrenInfos());
            }
        }
    }
}
