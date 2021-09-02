using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YuGet.Core.Builder;

namespace YuGet.Protocol.Builder
{
	public interface IModuleInstaller
	{
		void Setup(IYuGetOptionBuilder builder);
	}
}
