using BSolutions.SHES.Data.Repositories.ProjectItems;
using BSolutions.SHES.Models.Entities;
using BSolutions.SHES.Models.Observables;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSolutions.SHES.Services.ProjectItems
{
    public class ProjectItemService : ServiceBase, IProjectItemService
    {
        private readonly IProjectItemRepository _projectItemRepository;

        #region --- Constructor ---

        public ProjectItemService(ILogger<ProjectItemService> logger, IProjectItemRepository projectItemRepository)
            : base(logger)
        {
            this._projectItemRepository = projectItemRepository;
        }

        #endregion

        public async Task<ObservableProjectItem> AddAsync(ObservableProjectItem observableProjectItem)
        {
            try
            {
                // Insert new project item
                await this._projectItemRepository.AddAsync(observableProjectItem.entity);
                return observableProjectItem;
            }
            catch
            {
                return null;
            }
        }

        public async Task AddRangeAsync(ObservableCollection<ObservableProjectItem> observableProjectItems)
        {
            // Insert new project items
            foreach(var item in observableProjectItems)
            {
                await this._projectItemRepository.AddAsync(item.entity);
            }
        }

        public async Task<ObservableProjectItem> UpdateAsync(ObservableProjectItem observableProjectItem)
        {
            try
            {
                observableProjectItem.LastModificationTime = DateTime.Now;

                // Update new project
                await this._projectItemRepository.UpdateAsync(observableProjectItem.entity);
                return observableProjectItem;
            }
            catch
            {
                return null;
            }
        }

        public async Task UpdateRangeAsync(ObservableCollection<ObservableProjectItem> observableProjectItems)
        {
            // Update project items
            foreach(var item in observableProjectItems)
            {
                await this._projectItemRepository.UpdateAsync(item.entity);
            }
        }

        public async Task<ObservableCollection<ObservableProjectItem>> GetProjectItemsAsync(ObservableProject observableProject, bool includeDevices = false)
        {
            try
            {
                var projectItemTree = await this._projectItemRepository.GetProjectItemTreeAsync(observableProject.Id, includeDevices);
                return new ObservableCollection<ObservableProjectItem>(projectItemTree.Select(pi => new ObservableProjectItem(pi)));
            }
            catch
            {
                return null;
            }
        }
    }
}
