using System.Threading.Tasks;

namespace BSolutions.SHES.App.Contracts.Services
{
    public interface IActivationService
    {
        Task ActivateAsync(object activationArgs);
    }
}
