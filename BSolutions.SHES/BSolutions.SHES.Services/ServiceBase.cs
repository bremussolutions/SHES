using Microsoft.Extensions.Logging;

namespace BSolutions.SHES.Services
{
    public abstract class ServiceBase
    {
        protected readonly ILogger _logger;

        public ServiceBase(ILogger logger)
        {
            this._logger = logger;
        }
    }
}
