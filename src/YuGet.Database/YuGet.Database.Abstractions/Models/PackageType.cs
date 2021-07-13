using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using YuGet.Database.Abstractions.Infrastructure;

namespace YuGet.Database.Models
{
	[Index("Name")]
	public class PackageType : EntitySet
	{
		[MaxLength(512)]
		public string Name { get; set; }

		[MaxLength(64)]
		public string Version { get; set; }

		[ForeignKey("Package")]
		public Snid PackageId { get; set; }

		public virtual Package Package { get; set; }
	}
}
