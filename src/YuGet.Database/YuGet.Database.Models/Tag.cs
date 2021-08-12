using System.Collections.Generic;
using YuGet.Database.Abstractions.Infrastructure;

namespace YuGet.Database.Models
{
	public class Tag : EntitySet
	{
		public string Name { get; set; }

		public virtual ICollection<PackageTag> Packages { get; set; }
	}
}
