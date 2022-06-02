using System.Threading.Tasks;

namespace BSolutions.SHES.Services.Knx
{
    public interface IKnxImportService
    {
        Task<KnxImportResult> ImportProjectAsync(string path, KnxImportOptions options, string password = "");
        Task<bool> ProtectionCheckAsync(string path);
    }
}