using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace YuGet.Database.Models
{
	public class PackageTag
	{
		[ForeignKey("Package")]
		public Snid PackageId { get; set; }

		public virtual Package Package { get; set; }

		[ForeignKey("Tag")]
		public Snid TagId { get; set; }

		public virtual Tag Tag { get; set; }
	}
}
