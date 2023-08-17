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

        public async Task<List<Device>> GetDevicesForLocationAsync(ProjectItem projectItem)
        {
            try
            {
                // TODO: Dies muss definitiv noch optimiert werden!
                var parent = await this._dbContext.ProjectItems.Include("Children.Children.Children.Children.Children.Children.Children.Children.Children")
                    .FirstOrDefaultAsync(pi => pi.Id == projectItem.Id);

                if (parent != null)
                { 
                    return parent.Children.Traverse(pi => pi.Children, typeof(Device))
                        .Cast<Device>()
                        .ToList();
                }

                return new List<Device>();
            }
            catch (Exception ex)
            {
                this._logger.LogCritical(ex, ex.Message);
                throw;
            }
        }

        
    }
}
