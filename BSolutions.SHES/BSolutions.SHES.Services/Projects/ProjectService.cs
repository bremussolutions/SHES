using BSolutions.SHES.Data.Repositories.Projects;
using BSolutions.SHES.Models.Entities;
using BSolutions.SHES.Models.Observables;
using BSolutions.SHES.Services.Exceptions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSolutions.SHES.Services.Projects
{
    public class ProjectService : ServiceBase, IProjectService
    {
        private readonly IProjectRepository _projectRepository;

        public ProjectService(ILogger<ProjectService> logger, IProjectRepository projectRepository)
            : base(logger)
        {
            this._projectRepository = projectRepository;
        }

        /// <summary>
        /// Gets all projects.
        /// </summary>
        /// <returns>Returns a list of all projects.</returns>
        public async Task<ObservableCollection<ObservableProject>> GetAllAsync()
        {
            try
            {
                var projects = await this._projectRepository.GetAllAsync();
                return new ObservableCollection<ObservableProject>(projects.Select(p => new ObservableProject(p)));
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Inserts a new project.
        /// </summary>
        /// <param name="name">The project name.</param>
        /// <param name="number">The unique project number.</param>
        /// <param name="customer">The customer name.</param>
        /// <param name="address">The project address.</param>
        /// <returns>Returns the new project with auto-generated Id.</returns>
        public async Task<ObservableProject> InsertAsync(ObservableProject observableProject)
        {
            if (string.IsNullOrWhiteSpace(observableProject.Name))
            {
                throw new ArgumentNullException();
            }

            var project = new Project
            {
                Name = observableProject.Name,
                Number = observableProject.Number,
            };

            try
            {
                // Check if project with same name exists
                if (await this._projectRepository.ExistsAsync(project))
                {
                    throw new EntityAlreadyExistsException($"A project with name '{observableProject.Name}' already exists.");
                }

                // Add default building
                project.Buildings = new List<Building>(new[]
                {
                    new Building { Name = observableProject.Name }
                });

                // Insert new project
                await this._projectRepository.AddAsync(project);
                return new ObservableProject(project);
            }
            catch
            {
                return null;
            }
        }

        public async Task<ObservableProject> UpdateAsync(ObservableProject observableProject)
        {
            try
            {
                observableProject.LastModificationTime = DateTime.Now;

                // Update new project
                await this._projectRepository.UpdateAsync(observableProject.entity);
                return observableProject;
            }
            catch
            {
                return null;
            }
        }

        public async Task<bool> DeleteAsync(ObservableProject observableProject)
        {
            try
            {
                await this._projectRepository.DeleteAsync(observableProject.entity);
            }
            catch
            {
                return false;
            }

            return true;
        }

        public async Task<bool> ExistsAsync(string name)
        {
            return await this._projectRepository.ExistsAsync(name);
        }
    }
}
