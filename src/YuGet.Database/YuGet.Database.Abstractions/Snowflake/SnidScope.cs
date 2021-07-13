namespace System
{
	internal sealed class SnidScope
	{
		/// <summary>
		/// Construct Snid data scope.
		/// </summary>
		/// <param name="workerId">Work id, please keep it unique in the cluster project, the default value is 0</param>
		/// <param name="workerIdBits">Length of job id, default 2</param>
		/// <param name="sequenceBits">The same placeholder length of id can be generated in uniform milliseconds, the default is 8</param>
		/// <param name="twepoch">Start counting time</param>
		internal SnidScope(ushort workerId = 0, byte workerIdBits = 2, byte sequenceBits = 6, DateTime? twepoch = null)
		{
			if (workerIdBits > 8) 
				throw new ArgumentException("Please do not exceed 8 WorkID.");

			WorkerIdBits = workerIdBits;

			MaxWorkerId = (ushort)(ushort.MaxValue ^ ushort.MaxValue << workerIdBits);

			if (workerId > MaxWorkerId) 
				throw new ArgumentException("The set job ID exceeds the maximum available ID.");

			WorkerId = workerId;

			if (sequenceBits > 20) 
				throw new ArgumentException("If the millisecond counter is too large, it may cause a waste of id. Please use a counter with less than 20 bits");

			WorkerIdShift = SequenceBits = sequenceBits;

			TimestampLeftShift = (byte)(workerIdBits + sequenceBits);

			if (TimestampLeftShift > 22) 
				throw new ArgumentException("The job id and counter id are too large, resulting in too short time");

			
			SequenceMask = uint.MaxValue ^ uint.MaxValue << sequenceBits;
			Twepoch = twepoch.HasValue ? twepoch.Value.ToTimestamp() : 0L;
		}

		/// <summary>
		/// Initial reference timestamp, which needs to be less than the current time point.
		/// Keep this timestamp consistent for distributed projects.
		/// </summary>
		public ulong Twepoch { get; } = 0L;

		/// <summary>
		/// Last timestamp
		/// </summary>
		public ulong LastTimestamp { get; internal set; } = 0L;

		/// <summary>
		/// Machine code
		/// </summary>
		public ushort WorkerId { get; }

		/// <summary>
		/// The number of machine code bytes, the default 4 bytes is used to save the machine code.
		/// Defined as Long type will appear, the maximum offset is 64 bits, so moving left 64 bits has no meaning.
		/// </summary>
		public byte WorkerIdBits { get; }

		/// <summary>
		/// The maximum number of machine IDs
		/// </summary>
		public ushort MaxWorkerId { get; }

		/// <summary>
		/// Machine code data left shift number
		/// </summary>
		public byte WorkerIdShift { get; }

		/// <summary>
		/// Timestamp left shift number
		/// </summary>
		public byte TimestampLeftShift { get; }

		/// <summary>
		/// Millisecond counter
		/// </summary>
		public uint Sequence { get; internal set; } = 0;

		/// <summary>
		/// Counter byte number, 10 bytes are used to save the count code
		/// </summary>
		public byte SequenceBits { get; }

		/// <summary>
		/// Count can be generated in one microsecond, if it reaches this value, wait until the next subtle is generated
		/// </summary>
		public uint SequenceMask { get; }
	}
}
