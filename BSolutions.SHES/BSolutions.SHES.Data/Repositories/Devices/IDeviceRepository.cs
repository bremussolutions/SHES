using BSolutions.SHES.Models.Entities;
using System.Collections.Generic;

namespace BSolutions.SHES.Data.Repositories.Devices
{
    public interface IDeviceRepository : IRepository<Device>
    {
        List<Device> GetDevicesForLocation(ProjectItem projectItem);
    }
}