using System;
using System.ComponentModel.DataAnnotations.Schema;
using YuGet.Database.Abstractions.Infrastructure;

namespace YuGet.Database.Models
{
	public class ApiKey : EntitySet
	{
		public string Name { get; set; }

		public string Key { get; set; }

		public string Pattern { get; set; }

		public DateTime CreateDate { get; set; }

		public DateTime? ExprieDate { get; set; }

		public bool CanCreatePackage { get; set; }

		public bool CanUpdatePackage { get; set; }

		public bool CanDeletePackage { get; set; }

		[ForeignKey("Owner")]
		public Guid OwnerId { get; set; }

		public virtual UserAccount Owner { get; set; }

		[ForeignKey("OwnerTeamMember")]
		public Guid? OwnerTeamMemberId { get; set; }

		public virtual WorkTeamMember OwnerTeamMember { get; set; }
	}
}
