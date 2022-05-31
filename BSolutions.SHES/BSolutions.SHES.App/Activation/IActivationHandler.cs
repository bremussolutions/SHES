using System.Threading.Tasks;

namespace BSolutions.SHES.App.Activation
{
    public interface IActivationHandler
    {
        bool CanHandle(object args);

        Task HandleAsync(object args);
    }
}
