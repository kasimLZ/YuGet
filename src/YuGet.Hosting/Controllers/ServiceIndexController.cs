using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;
using YuGet.Core;
using YuGet.Core.Models.Abstraction;

namespace YuGet.Hosting.Controllers
{
	public class ServiceIndexController : Controller
    {
        private readonly IServiceIndexService _serviceIndex;

        public ServiceIndexController(IServiceIndexService serviceIndex)
        {
            _serviceIndex = serviceIndex ?? throw new ArgumentNullException(nameof(serviceIndex));
        }

        // GET v3/index
        [HttpGet]
        public async Task<ServiceIndexResponse> GetAsync(CancellationToken cancellationToken)
        {
            return await _serviceIndex.GetAsync(cancellationToken);
        }
    }
}
