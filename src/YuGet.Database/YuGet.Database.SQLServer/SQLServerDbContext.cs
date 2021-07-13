using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using YuGet.Database.Abstractions.Infrastructure;

namespace YuGet.Database.SQLServer
{
	internal class SQLServerDbContext : YuGetDbContext<SQLServerDbContext>, IYuGetDbContext
	{
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
