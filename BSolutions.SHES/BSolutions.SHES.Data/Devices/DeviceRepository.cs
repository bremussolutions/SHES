using BSolutions.SHES.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace BSolutions.SHES.Data.Repositories.Projects
{
    public class DeviceRepository : Repository<Device>, IDeviceRepository
    {
        public DeviceRepository(ILogger<DeviceRepository> logger, ShesDbContext dbContext)
            : base(logger, dbContext)
        {
        }
    }
}
