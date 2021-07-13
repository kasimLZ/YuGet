namespace YuGet.Database.Abstractions.Options
{
	internal sealed class YuGetDatabaseOption
	{
		public const string SectionName = "YuGet";

		public string DatabaseType { get; set; }

		public string ConnectString { get; set; }
	}
}
