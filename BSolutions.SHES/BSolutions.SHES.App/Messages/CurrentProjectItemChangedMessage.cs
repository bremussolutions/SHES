using BSolutions.SHES.Models.Entities;
using BSolutions.SHES.Models.Observables;
using CommunityToolkit.Mvvm.Messaging.Messages;

namespace BSolutions.SHES.App.Messages
{
    public class CurrentProjectItemChangedMessage : ValueChangedMessage<ObservableProjectItem>
    {
        public CurrentProjectItemChangedMessage(ObservableProjectItem projectItem)
            : base(projectItem)
        {
        }
    }
}
