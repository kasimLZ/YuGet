using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YuGet.Database
{
	public abstract class DbBaseSet
	{
		[Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
		public SFID Id { get; set; }
	}
}
