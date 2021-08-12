using System.Collections.Generic;
using YuGet.Database.Abstractions.Infrastructure;

namespace YuGet.Database.Models
{
	public class UserAccount : EntitySet
	{
		public string ShowName { get; set; }

		public string UserName { get; set; }

		public string UserPassword { get; set; }

		public virtual ICollection<WorkTeamMember> TeamMembers{ get; set; }

		public virtual ICollection<ApiKey> ApiKeys { get; set; }
	}
}
