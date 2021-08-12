using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using YuGet.Database.Abstractions.Infrastructure;

namespace YuGet.Database.Models
{
	public class WorkTeamMember : EntitySet
	{
		[ForeignKey("User")]
		public Guid UserId { get; set; }

		public virtual UserAccount User { get; set; }

		[ForeignKey("WorkTeam")]
		public Guid TeamId { get; set; }

		public virtual WorkTeam WorkTeam { get; set; }

		public virtual ICollection<ApiKey> ApiKeys { get; set; }
	}
}
