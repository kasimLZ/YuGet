using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;
using System.Linq;
using YuGet.Database.Abstractions.Infrastructure;

namespace YuGet.Database.SQLServer
{
	internal sealed class SQLServerDbContext : YuGetDbContext<SQLServerDbContext>, IYuGetDbContext
	{
		public SQLServerDbContext(DbContextOptions<SQLServerDbContext> options): base(options) { }

		/// <summary>
		/// The SQL Server error code for when a unique contraint is violated.
		/// </summary>
		private const int UniqueConstraintViolationErrorCode = 2627;

		public override bool IsUniqueConstraintViolationException(DbUpdateException exception)
		{
			if (exception.GetBaseException() is SqlException sqlException)
			{
				return sqlException.Errors
					.OfType<SqlError>()
					.Any(error => error.Number == UniqueConstraintViolationErrorCode);
			}

			return false;
		}
	}
}
