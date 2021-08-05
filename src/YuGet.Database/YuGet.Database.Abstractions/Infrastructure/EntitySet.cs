using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YuGet.Database.Abstractions.Infrastructure
{
	/// <summary>
	/// Database custom table infrastructure with main key
	/// </summary>
	public abstract class EntitySet
	{
		/// <summary>
		/// Primary key
		/// </summary>
		[Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
		public Guid Id { get; set; }

	}
}
