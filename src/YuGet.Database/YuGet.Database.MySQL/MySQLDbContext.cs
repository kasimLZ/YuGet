using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using System.Threading;
using System.Threading.Tasks;
using YuGet.Database.Abstractions.Infrastructure;
using YuGet.Database.Models;

namespace YuGet.Database.MySQL
{
	internal sealed class MySQLDbContext : YuGetDbContext<MySQLDbContext>, IYuGetDbContext
	{
        /// <summary>
        /// The MySQL Server error code for when a unique constraint is violated.
        /// </summary>
        private const int UniqueConstraintViolationErrorCode = 1062;


        public MySQLDbContext(DbContextOptions<MySQLDbContext> options) : base(options) { }

		public override bool IsUniqueConstraintViolationException(DbUpdateException exception)
		{
            return exception.InnerException is MySqlException mysqlException &&
                  mysqlException.Number == UniqueConstraintViolationErrorCode;
        }

        /// <summary>
        /// MySQL does not support LIMIT clauses in subqueries for certain subquery operators.
        /// See: https://dev.mysql.com/doc/refman/8.0/en/subquery-restrictions.html
        /// </summary>
        public override bool SupportsLimitInSubqueries => false;
    }
}
