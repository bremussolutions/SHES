using BSolutions.SHES.App.ViewModels;
using CommunityToolkit.Mvvm.Messaging.Messages;

namespace BSolutions.SHES.App.Messages
{
    public class ApplicationInfoBarChangedMessage : ValueChangedMessage<AppInfoBarViewModel>
    {
        public ApplicationInfoBarChangedMessage(AppInfoBarViewModel options)
            : base(options)
        {
        }
    }
}
