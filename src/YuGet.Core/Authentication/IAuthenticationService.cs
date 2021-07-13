using System.Threading;
using System.Threading.Tasks;

namespace YuGet.Core
{
    public interface IAuthenticationService
    {
        Task<bool> AuthenticateAsync(string apiKey, CancellationToken cancellationToken);
    }
}
