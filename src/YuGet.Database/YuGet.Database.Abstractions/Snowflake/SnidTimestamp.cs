namespace System
{
	internal static class SnidTimestamp
	{
		/// <summary>
		/// Base time，1970-1-1 0:0:0 for UTC
		/// </summary>
		private static readonly DateTime UTCZERO = new(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

		/// <summary>
		/// Try to generate a timestamp.
		/// </summary>
		/// <param name="Time">Target time.</param>
		/// <returns></returns>
		public static ulong ToTimestamp(this DateTime Time) => Time.ToTimestamp(UTCZERO);

		/// <summary>
		/// Try to generate a timestamp.
		/// </summary>
		/// <param name="Time">Target time.</param>
		/// <param name="BaseTime"></param>
		/// <returns></returns>
		public static ulong ToTimestamp(this DateTime Time, DateTime BaseTime)
		{
			// If the current time is not UTC, try to convert to UTC
			if (Time.Kind != DateTimeKind.Utc) Time = Time.ToUniversalTime();

			// Specify that if the current time is less than the base, use the base time directly
			else if (Time < BaseTime) Time = BaseTime;

			return (ulong)(Time - BaseTime).TotalMilliseconds;
		}
	}
}
