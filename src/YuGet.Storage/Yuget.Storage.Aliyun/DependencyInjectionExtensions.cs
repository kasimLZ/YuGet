using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yuget.Storage.Aliyun
{
	public static class DependencyInjectionExtensions
	{
		public static IServiceCollection AddAliyunStorage(this IServiceCollection services)
		{
			return services;
		}
	}
}
