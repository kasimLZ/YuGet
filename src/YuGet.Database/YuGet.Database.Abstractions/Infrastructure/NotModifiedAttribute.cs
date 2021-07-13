using System;

namespace YuGet.Database.Abstractions.Infrastructure
{
	/// <summary>
	/// Model fields do not migrate tags, and marked fields are not changed when fields are updated
	/// </summary>
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	internal sealed class NotModifiedAttribute : Attribute
	{
	}
}
