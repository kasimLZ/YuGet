namespace System
{
	public static class DateTimeExtenstions
	{
		public static readonly DateTime UtcZero = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

		/// <summary>
		/// Convert <see cref="DateTime"/> to Unix timestamp
		/// </summary>
		/// <param name="date"></param>
		/// <returns></returns>
		public static long ToUinxTimestamp(this DateTime date) =>
			(long)(date.ToUniversalTime() - UtcZero).TotalMilliseconds;

		/// <summary>
		/// Convert Unix timestamp to <see cref="DateTime"/>
		/// </summary>
		/// <param name="timestamp"></param>
		/// <returns></returns>
		public static DateTime ToUtcDateTime(this long timestamp) =>
			UtcZero.AddMilliseconds(timestamp);
	}
}
