namespace YuGet.Database.Abstractions.Options
{
	public sealed class YuGetDatabaseOption
	{
		public const string SectionName = "YuGet";

		public string DatabaseType { get; set; }

		public string ConnectString { get; set; }
	}
}
