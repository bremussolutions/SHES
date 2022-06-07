using BSolutions.SHES.Models.Entities;
using BSolutions.SHES.Models.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSolutions.SHES.Models.Observables
{
    public class ObservableDevice : ObservableProjectItem
    {
        public DeviceType Type
        {
            get => ((Device)entity).Type;
            set => SetProperty(((Device)entity).Type, value, (Device)entity, (u, n) => u.Type = n);
        }

        #region --- Constructors ---

        public ObservableDevice()
            : this(new Device())
        { }

        public ObservableDevice(Device device)
            : base(device)
        { }

        #endregion
    }
}
