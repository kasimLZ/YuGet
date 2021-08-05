using Microsoft.EntityFrameworkCore;
using YuGet.Database.Abstractions.Options;

namespace YuGet.Database
{
	public interface IDatabaseRegister<TContext>
		where TContext : DbContext, IYuGetDbContext
	{
		string Name { get; }

		void OptionsBuilder(DbContextOptionsBuilder<TContext> builder, YuGetDatabaseOption option);
	}
}
