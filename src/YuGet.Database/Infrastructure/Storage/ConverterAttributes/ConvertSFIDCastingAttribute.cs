using System;

namespace YuGet.Database.Infrastructure.Storage
{
	internal class ConvertSFIDCastingAttribute: ConvertCastingAttribute
	{
		internal ConvertSFIDCastingAttribute(): base(typeof(SFID), typeof(long)) { }
	}
}
