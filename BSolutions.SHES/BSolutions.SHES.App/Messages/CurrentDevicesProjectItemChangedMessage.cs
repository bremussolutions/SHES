using BSolutions.SHES.Models.Entities;
using BSolutions.SHES.Models.Observables;
using CommunityToolkit.Mvvm.Messaging.Messages;

namespace BSolutions.SHES.App.Messages
{
    public class CurrentDevicesProjectItemChangedMessage : ValueChangedMessage<ObservableProjectItem>
    {
        public CurrentDevicesProjectItemChangedMessage(ObservableProjectItem projectItem)
            : base(projectItem)
        {
        }
    }
}
