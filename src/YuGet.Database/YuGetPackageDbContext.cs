using Microsoft.EntityFrameworkCore;
using System;
using YuGet.Database.Infrastructure.Storage;

namespace YuGet.Database
{
	internal class YuGetPackageDbContext: DbContext
	{
		public YuGetPackageDbContext(DbContextOptions<YuGetPackageDbContext> options) : base(options) { }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseLazyLoadingProxies();
		

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.UseDefualtTypeConvter<SFID, long>().UseConvterProvider();
		}
	}
}
