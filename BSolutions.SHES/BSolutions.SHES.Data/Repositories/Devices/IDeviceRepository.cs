using BSolutions.SHES.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BSolutions.SHES.Data.Repositories.Devices
{
    public interface IDeviceRepository : IRepository<Device>
    {
        Task<List<Device>> GetDevicesForLocationAsync(ProjectItem projectItem);
    }
}