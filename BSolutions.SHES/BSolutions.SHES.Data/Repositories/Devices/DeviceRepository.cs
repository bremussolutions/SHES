using BSolutions.SHES.Models.Entities;
using Microsoft.Extensions.Logging;

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
    }
}
