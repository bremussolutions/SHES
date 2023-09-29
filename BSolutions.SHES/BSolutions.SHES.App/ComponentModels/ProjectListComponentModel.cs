using BSolutions.SHES.App.Messages;
using BSolutions.SHES.App.ViewModels;
using BSolutions.SHES.Models.Entities;
using BSolutions.SHES.Models.Observables;
using BSolutions.SHES.Services.Knx;
using BSolutions.SHES.Services.ProjectItems;
using BSolutions.SHES.Services.Projects;
using BSolutions.SHES.Shared.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;
using Windows.Storage;
using Windows.Storage.Pickers;

namespace BSolutions.SHES.App.ComponentModels
{
    public class ProjectListComponentModel : ObservableRecipient
    {
        private readonly ResourceLoader _resourceLoader;
        private readonly IProjectService _projectService;
        private readonly IProjectItemService _projectItemService;
        private readonly IKnxImportService _knxImportService;

        #region --- Properties ---

        private ObservableProject selectedProject;
        public ObservableProject SelectedProject
        {
            get => selectedProject;
            set
            {
                SetProperty(ref selectedProject, value);
                OpenProjectCommand.NotifyCanExecuteChanged();
                DeleteProjectDialogCommand.NotifyCanExecuteChanged();
            }
        }

        private ObservableProject newProject;
        public ObservableProject NewProject
        {
            get => newProject;
            set => SetProperty(ref newProject, value);
        }

        public ObservableCollection<ObservableProject> ProjectList { get; set; } = new ObservableCollection<ObservableProject>();

        private StorageFile _importProjectFile;
        public StorageFile ImportProjectFile
        {
            get => _importProjectFile;
            private set
            {
                SetProperty(ref _importProjectFile, value);
            }
        }

        private bool _isImportProjectProtected;
        public bool IsImportProjectProtected
        {
            get => _isImportProjectProtected;
            private set
            {
                SetProperty(ref _isImportProjectProtected, value);
                OnPropertyChanged(nameof(ImportProjectDialogHasErrors));
            }
        }

        private string _importProjectPassword;
        public string ImportProjectPassword
        {
            get => _importProjectPassword;
            set
            {
                SetProperty(ref _importProjectPassword, value);
                OnPropertyChanged(nameof(ImportProjectDialogHasErrors));
            }
        }

        private bool _importProjectStructure = true;
        public bool ImportProjectStructure
        {
            get => _importProjectStructure;
            set => SetProperty(ref _importProjectStructure, value);
        }

        private bool _importProjectDevices = true;
        public bool ImportProjectDevices
        {
            get => _importProjectDevices;
            set => SetProperty(ref _importProjectDevices, value);
        }

        public bool ImportProjectDialogHasErrors
        {
            get => this.IsImportProjectProtected && string.IsNullOrEmpty(ImportProjectPassword);
        }

        public IAsyncRelayCommand AddProjectDialogCommand { get; }
        public IAsyncRelayCommand AddProjectCommand { get; }
        public IAsyncRelayCommand ImportKnxProjectDialogCommand { get; }
        public IAsyncRelayCommand ImportKnxProjectCommand { get; }
        public IRelayCommand OpenProjectCommand { get; }
        public IAsyncRelayCommand DeleteProjectDialogCommand { get; }
        public IAsyncRelayCommand DeleteProjectCommand { get; }

        public bool AddProjectDialog { get; set; }

        #endregion

        #region --- Constructor ---

        public ProjectListComponentModel(IProjectService projectService, IProjectItemService projectItemService, IKnxImportService knxImportService)
        {
            // Resource Loader
            this._resourceLoader = ResourceLoader.GetForViewIndependentUse();

            // Services
            this._projectService = projectService;
            this._projectItemService = projectItemService;
            this._knxImportService = knxImportService;

            // Commands
            AddProjectDialogCommand = new AsyncRelayCommand<ContentDialog>(async (dialog) => await NewProjectDialog(dialog));
            AddProjectCommand = new AsyncRelayCommand<ContentDialog>(async (dialog) => await AddProject(dialog));
            ImportKnxProjectDialogCommand = new AsyncRelayCommand<ContentDialog>(async (dialog) => await this.ImportKnxProjectDialog(dialog));
            ImportKnxProjectCommand = new AsyncRelayCommand(ImportKnxProject);
            OpenProjectCommand = new RelayCommand(OpenProject, CanOpenProject);
            DeleteProjectDialogCommand = new AsyncRelayCommand<ContentDialog>(async (dialog) => await dialog.ShowAsync(), CanDeleteProjectDialog);
            DeleteProjectCommand = new AsyncRelayCommand<ContentDialog>(async (dialog) => await DeleteProject(dialog));

            // Properties
            this.NewProject = new ObservableProject(new Project());
        }

        #endregion

        #region --- Events ---

        public async void OnLoaded(object sender, RoutedEventArgs e)
        {
            this.ProjectList.AddRange(await this._projectService.GetAllAsync());
        }

        #endregion

        #region ---  Commands ---

        private async Task NewProjectDialog(ContentDialog dialog)
        {
            this.NewProject = new ObservableProject(new Project());
            await dialog.ShowAsync();
        }

        private async Task AddProject(ContentDialog dialog)
        {
            // Add project
            await this._projectService.InsertAsync(this.NewProject);
            this.ProjectList.Add(this.NewProject);

            WeakReferenceMessenger.Default.Send(new ApplicationInfoBarChangedMessage(new AppInfoBarViewModel
            {
                IsOpen = true,
                Severity = InfoBarSeverity.Success,
                Title = this._resourceLoader.GetString("Shell_AppInfoBar_Success"),
                Message = this._resourceLoader.GetString("Main_ProjectList_NewProjectDialog_Success")
            }));
        }

        private async Task ImportKnxProjectDialog(ContentDialog dialog)
        {
            // Create the file picker
            var filePicker = new FileOpenPicker();

            // Get the current window's HWND by passing in the Window object
            var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(App.MainWindow);

            // Associate the HWND with the file picker
            WinRT.Interop.InitializeWithWindow.Initialize(filePicker, hwnd);

            // Use file picker like normal!
            filePicker.FileTypeFilter.Add(".knxproj");
            filePicker.CommitButtonText = "Importieren";
            this.ImportProjectFile = await filePicker.PickSingleFileAsync();

            if (this.ImportProjectFile != null)
            {
                this.IsImportProjectProtected = await this._knxImportService.ProtectionCheckAsync(this.ImportProjectFile.Path);
                await dialog.ShowAsync();
            }
        }

        private async Task ImportKnxProject()
        {
            var options = new KnxImportOptions
            {
                ImportStructure = this.ImportProjectStructure,
                ImportDevices = this.ImportProjectDevices
            };

            var result = await this._knxImportService.ImportProjectAsync(this.ImportProjectFile.Path, options, this.ImportProjectPassword);

            if (result.Successful)
            {
                this.ProjectList.Add(result.Data.Project);
            }
            else
            {
                WeakReferenceMessenger.Default.Send(new ApplicationInfoBarChangedMessage(new AppInfoBarViewModel
                {
                    IsOpen = true,
                    Severity = InfoBarSeverity.Error,
                    Title = this._resourceLoader.GetString("Shell_AppInfoBar_Error"),
                    Message = result.ErrorMessage
                }));
            }
        }

        private void OpenProject()
        {
            // Set current project
            WeakReferenceMessenger.Default.Send(new CurrentProjectChangedMessage(this.SelectedProject));
        }

        private bool CanOpenProject()
        {
            return this.SelectedProject != null;
        }

        private bool CanDeleteProjectDialog(ContentDialog dialog)
        {
            return selectedProject != null;
        }

        private async Task DeleteProject(ContentDialog dialog)
        {
            await this._projectService.DeleteAsync(this.selectedProject);
            this.ProjectList.Remove(this.selectedProject);
            WeakReferenceMessenger.Default.Send(new CurrentProjectChangedMessage(null));

            WeakReferenceMessenger.Default.Send(new ApplicationInfoBarChangedMessage(new AppInfoBarViewModel
            {
                IsOpen = true,
                Severity = InfoBarSeverity.Success,
                Title = this._resourceLoader.GetString("Shell_AppInfoBar_Success"),
                Message = this._resourceLoader.GetString("Main_ProjectList_NewProjectDialog_Success")
            }));
        }

        #endregion
    }
}
