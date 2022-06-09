using BSolutions.SHES.Models.Attributes;
using BSolutions.SHES.Models.Entities;
using BSolutions.SHES.Models.Enumerations;
using BSolutions.SHES.Shared.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BSolutions.SHES.Models.Observables
{
    public class ObservableDevice : ObservableProjectItem
    {
        public DeviceType Type
        {
            get => ((Device)entity).Type;
            set => SetProperty(((Device)entity).Type, value, (Device)entity, (u, n) => u.Type = n);
        }

        public BusType BusType
        {
            get => ((Device)entity).BusType;
            set => SetProperty(((Device)entity).BusType, value, (Device)entity, (u, n) => u.BusType = n);
        }

        public string TypeIcon
        {
            get => this.BusType.GetEnumAttribute<BusTypeInfoAttribute>()?.Icon;
        }

        public List<DeviceType> DeviceTypes
        {
            get => Enum.GetValues(typeof(DeviceType)).Cast<DeviceType>().ToList();
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
