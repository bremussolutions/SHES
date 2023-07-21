using Microsoft.Extensions.Logging;
using Microsoft.Windows.ApplicationModel.Resources;

namespace BSolutions.SHES.Services
{
    public abstract class ServiceBase
    {
        protected readonly ILogger _logger;
        protected readonly ResourceLoader _resourceLoader;

        public ServiceBase(ResourceLoader resourceLoader, ILogger logger)
        {
            this._resourceLoader = resourceLoader;
            this._logger = logger;
        }
    }
}
