using System;

namespace BSolutions.SHES.App.Contracts.Services
{
    public interface IPageService
    {
        Type GetPageType(string key);
    }
}
