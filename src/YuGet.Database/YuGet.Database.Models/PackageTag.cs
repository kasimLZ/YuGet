using System;
using System.ComponentModel.DataAnnotations.Schema;
using YuGet.Database.Abstractions.Infrastructure;

namespace YuGet.Database.Models
{
	public class PackageTag : EntitySet
	{
		[ForeignKey("Package")]
		public Guid PackageId { get; set; }

		public virtual Package Package { get; set; }

		[ForeignKey("Tag")]
		public Guid TagId { get; set; }

		public virtual Tag Tag { get; set; }
	}
}
