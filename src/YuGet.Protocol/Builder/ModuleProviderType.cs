using System;
using System.Reflection;

namespace YuGet.Protocol.Builder
{
	public enum ModuleProviderType
	{
		[RequiredType]
		Database = 1,

		[RequiredType]
		Stroage = 2,

		Authentication = 3,

		[MultipleType]
		Host = 4,
	}

	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
	internal class RequiredType : Attribute { }

	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
	internal class MultipleType : Attribute { }

	public static class ModuleProviderTypeExtensions 
	{
		public static bool IsRequired(this ModuleProviderType module) => module.HasAttribute<RequiredType>();

		public static bool AllowMultiple(this ModuleProviderType module) => module.HasAttribute<MultipleType>();

		private static bool HasAttribute<TAttribute>(this ModuleProviderType module) where TAttribute : Attribute 
		{
			return module.GetType().GetCustomAttribute<TAttribute>() == null;
		}
	}
}
