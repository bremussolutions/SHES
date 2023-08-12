using BSolutions.SHES.Models.Entities;
using BSolutions.SHES.Models.Observables;
using CommunityToolkit.Mvvm.Messaging.Messages;

namespace BSolutions.SHES.App.Messages
{
    public class CurrentTreeProjectItemChangedMessage : ValueChangedMessage<ObservableProjectItem>
    {
        public CurrentTreeProjectItemChangedMessage(ObservableProjectItem projectItem)
            : base(projectItem)
        {
        }
    }
}
