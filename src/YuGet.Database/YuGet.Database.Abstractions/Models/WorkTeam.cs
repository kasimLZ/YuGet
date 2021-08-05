using System.Collections.Generic;
using YuGet.Database.Abstractions.Infrastructure;

namespace YuGet.Database.Models
{
	public class WorkTeam : EntitySet
	{
		public string Name { get; set; }

		public string Description { get; set; }

		public virtual ICollection<WorkTeamMember> TeamMembers { get; set; }
	}
}
