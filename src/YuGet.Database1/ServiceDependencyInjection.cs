using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YuGet.Database;

namespace YuGet
{
	public static class ServiceDependencyInjection
	{
		public static void AddSqlServer(this IServiceCollection services)
		{
			services.AddDbContext<YuGetPackageDbContext>(opt => {
				
			});
		}
	}
}
