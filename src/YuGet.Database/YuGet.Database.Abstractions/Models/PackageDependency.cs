using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using YuGet.Database.Abstractions.Infrastructure;

namespace YuGet.Database.Models
{
	[Index("Id")]
	public class PackageDependency : EntitySet
	{
		public string Id { get; set; }	

		[MaxLength(256)]
		public string VersionRange { get; set; }

		[MaxLength(256)]
		public string TargetFramework { get; set; }

		[ForeignKey("Package")]
		public Snid PackageId { get; set; }

		public virtual Package Package { get; set; }
	}
}
