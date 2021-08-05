using System;
using System.ComponentModel.DataAnnotations.Schema;
using YuGet.Database.Abstractions.Infrastructure;

namespace YuGet.Database.Models
{
	public class PackageApiKey : EntitySet
	{
		[ForeignKey("Package")]
		public Guid PackageId { get; set; }

		public Package Package { get; set; }

		[ForeignKey("ApiKey")]
		public Guid ApiKeyId { get; set; }

		public ApiKey ApiKey { get; set; }
	}
}
