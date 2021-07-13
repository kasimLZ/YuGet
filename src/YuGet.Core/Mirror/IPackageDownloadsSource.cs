using System.Collections.Generic;
using System.Threading.Tasks;

namespace YuGet.Core
{
    public interface IPackageDownloadsSource
    {
        Task<Dictionary<string, Dictionary<string, long>>> GetPackageDownloadsAsync();
    }
}
