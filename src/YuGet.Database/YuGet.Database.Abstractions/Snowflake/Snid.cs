using System.Collections.Generic;

namespace System
{
	/// <summary>
	/// Snowflake distributed self-incrementing ID
	/// </summary>
	public struct Snid
	{
		private Snid(ulong id) => Value = id;

		private Snid(ulong a, ulong b, ulong c) : this(a | b | c) { }

		private ulong Value { get; set; }

		private static readonly SnidScope Default = new();

		private static readonly Dictionary<string, SnidScope> ScopeCollection = new();

		/// <inheritdoc cref="SnidScope(ushort, byte, byte, DateTime?)"/>
		/// <param name="Name">The domain name, once occupied, cannot be replaced or modified unless the application is restarted. Please note</param>
		public static void AddScope(string Name, ushort workerId = 0, byte workerIdBits = 2, byte sequenceBits = 8, DateTime? twepoch = null)
		{
			if (ScopeCollection.ContainsKey(Name))
				throw new("The current configuration domain already exists and cannot be replaced or modified.");
			ScopeCollection.Add(Name, new SnidScope(workerId, workerIdBits, sequenceBits, twepoch));
		}

		/// <summary>
		/// Generate a new ID
		/// </summary>
		/// <returns></returns>
		public static Snid NewID() => NewID(Default);

		/// <inheritdoc cref="NewID()"/>
		/// <param name="ScopeName">sdfsdf</param>
		/// <returns></returns>
		public static Snid NewID(string ScopeName)
		{
			if (ScopeCollection.TryGetValue(ScopeName, out var scope))
			{
				return NewID(scope);
			}
			throw new KeyNotFoundException("");
		}

		/// <inheritdoc cref="NewID()"/>
		/// <param name="scope">sdfsdf</param>
		/// <returns></returns>
		private static Snid NewID(SnidScope scope)
		{
			ulong timestamp;

			lock (scope)
			{
				timestamp = DateTime.UtcNow.ToTimestamp();
				if (scope.LastTimestamp == timestamp)
				{
					// Generate ID in the same subtle
					scope.Sequence = (scope.Sequence + 1) & scope.SequenceMask;

					// Use the & operation to calculate whether the count generated in the microsecond has reached the upper limit.
					if (scope.Sequence == 0)
					{
						// If the ID count generated within a subtle has reached the upper limit, wait until the next subtle
						timestamp = TillNextMillis(scope.LastTimestamp);
					}
				}
				else
				{
					// Generate IDs in different microseconds
					scope.Sequence = 0; //Counter clear
				}
				if (timestamp < scope.LastTimestamp)
				{
					//If the current timestamp is smaller than the last time the ID was generated, 
					//an exception is thrown because there is no guarantee that the currently generated ID was not generated before.
					throw new($"Clock moved backwards.  Refusing to generate id for {scope.LastTimestamp - timestamp} milliseconds");
				}
				scope.LastTimestamp = timestamp; //Save the current timestamp as the timestamp of the last generated ID

			}

			return new((timestamp - scope.Twepoch) << scope.TimestampLeftShift, (ulong)scope.WorkerId << scope.WorkerIdShift, scope.Sequence);
		}

		/// <summary>
		/// Get the next microsecond timestamp
		/// </summary>
		/// <param name="lastTimestamp"></param>
		/// <returns></returns>
		private static ulong TillNextMillis(ulong lastTimestamp)
		{
			ulong timestamp = DateTime.UtcNow.ToTimestamp();
			while (timestamp <= lastTimestamp)
				timestamp = DateTime.UtcNow.ToTimestamp();
			return timestamp;
		}

		public override bool Equals(object obj)
		{
			if (obj is Snid id)
			{
				id.Value = Value;
			}
			return false;
		}

		public override string ToString() => Value.ToString();

		public override int GetHashCode() => base.GetHashCode();

		public static bool operator ==(Snid a, Snid b) => a.Value == b.Value;
		public static bool operator !=(Snid a, Snid b) => a.Value != b.Value;

		public static implicit operator ulong(Snid id) => id.Value;
		public static implicit operator Snid(ulong id) => new(id);

	}
}
