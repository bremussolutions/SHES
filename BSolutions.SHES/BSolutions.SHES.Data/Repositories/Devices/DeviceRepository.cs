using BSolutions.SHES.Models.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BSolutions.SHES.Models.Extensions;
using Microsoft.EntityFrameworkCore;

namespace BSolutions.SHES.Data.Repositories.Devices
{
    public class DeviceRepository : Repository<Device>, IDeviceRepository
    {
        #region --- Constructor ---

        /// <summary>Initializes a new instance of the <see cref="DeviceRepository" /> class.</summary>
        /// <param name="logger">The logger.</param>
        /// <param name="dbContext">The database context.</param>
        public DeviceRepository(ILogger<DeviceRepository> logger, ShesDbContext dbContext)
            : base(logger, dbContext)
        {
        }

        #endregion

        public List<Device> GetDevicesForLocation(ProjectItem projectItem)
        {
            try
            {
                // TODO: Dies muss definitiv noch optimiert werden!
                return this._dbContext.ProjectItems.Include("Children.Children.Children.Children.Children.Children.Children.Children.Children")
                    .First(pi => pi.Id == projectItem.Id)
                    .Children.Traverse(pi => pi.Children)
                    .Cast<Device>()
                    .ToList();


                //var query = this._dbContext.Set<TEntity>().Where(expression);

                //if (!string.IsNullOrWhiteSpace(includeProperties))
                //{
                //    string[] includes = includeProperties.Split(';');

                //    foreach (string include in includes)
                //    {
                //        query = query.Include(include);
                //    }
                //}

                //return await query.ToListAsync();
            }
            catch (Exception ex)
            {
                this._logger.LogCritical(ex, ex.Message);
                throw;
            }
        }

        
    }
}
