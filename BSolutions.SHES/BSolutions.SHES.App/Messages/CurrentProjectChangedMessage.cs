using BSolutions.SHES.Models.Observables;
using CommunityToolkit.Mvvm.Messaging.Messages;

namespace BSolutions.SHES.App.Messages
{
    public sealed class CurrentProjectChangedMessage : ValueChangedMessage<ObservableProject>
    {
        public CurrentProjectChangedMessage(ObservableProject project)
            : base(project)
        {
        }
    }
}
