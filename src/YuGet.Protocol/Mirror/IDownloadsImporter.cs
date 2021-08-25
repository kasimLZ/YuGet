using System.Threading;
using System.Threading.Tasks;

namespace YuGet.Core
{
	public interface IDownloadsImporter
	{
		Task ImportAsync(CancellationToken cancellationToken);
	}
}
