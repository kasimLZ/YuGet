namespace System
{
	/// <summary>
	/// Snowflake Incremental ID Configuration Model
	/// </summary>
	public struct SFIDSetting
	{
		/// <summary> <see cref="WorkerId"/> </summary>
		private long workerId;

		/// <summary> <see cref="BeginTime"/> </summary>
		private DateTime beginTime;

		/// <summary> <see cref="BeginTimeStamp"/> </summary>
		private long? beginTimeStamp;

		/// <summary> <see cref="WorkerIdBits"/> </summary>
		private int workerIdBits;

		/// <summary> <see cref="SequenceBits"/> </summary>
		private int sequenceBits;

		/// <summary> <see cref="MaxWorkerId"/> </summary>
		private long? maxWorkerId;

		/// <summary> <see cref="TimeStampLeftShift"/> </summary>
		private int? timeStampLeftShift;

		/// <summary> <see cref="SequenceMask"/> </summary>
		private long? sequenceMask;

		public bool Lock { get; private set; }

		/// <summary>
		/// Machine code.
		/// This will be used in a distributed system to distinguish ID created by different hosts.
		/// No length validity check is performed when entering. 
		/// For example, the length of the machine code is set to 2 digits, but the actual machine code is set to 4.
		/// </summary>
		public long WorkerId
		{
			get => workerId;
			set
			{
				if (Lock) throw new InvalidOperationException();
				workerId = 128 > value && value > 0 ? value : throw new ArgumentOutOfRangeException();
			}
		}

		/// <summary>
		/// The starting time of the timestamp of the ID pool. 
		/// In new projects, set the time as much as possible when the project is first deployed. 
		/// The shorter time will effectively extend the effective time of the ID pool.
		/// </summary>
		public DateTime BeginTime
		{
			get => beginTime;
			set
			{
				if (Lock) throw new InvalidOperationException();
				var time = value.ToUniversalTime();
				beginTime = DateTime.UtcNow >= time && time >= DateTimeExtenstions.UtcZero ? time : throw new ArgumentOutOfRangeException();
			}
		}

		/// <summary>
		/// The number of machine code bytes, the default 4 bytes is used to save the machine code.
		/// Defined as Long type will appear, the maximum offset is 64 bits, so moving left 64 bits has no meaning.
		/// To ensure performance, the maximum offset is currently 7 bits and the minimum is 1 bit, and the default is 12 bits.
		/// </summary>
		public int WorkerIdBits
		{
			get => workerIdBits;
			set
			{
				if (Lock) throw new InvalidOperationException();
				workerIdBits = 8 > value && value > 0 ? value : throw new ArgumentOutOfRangeException();
			}
		}

		/// <summary>
		/// Counter byte number, 12 bytes are used to save the count code.
		/// The recommended length is no more than 20 digits, and the shortest is no less than 8 digits. 
		/// Too long sequence number bits may lead to a shorter ID pool life. 
		/// Too short sequence numbers may cause concurrency restrictions in milliseconds.
		/// </summary>
		public int SequenceBits
		{
			get => sequenceBits;
			set
			{
				if (Lock) throw new InvalidOperationException();
				sequenceBits = 20 > value && value > 8 ? value : throw new ArgumentOutOfRangeException();
			}
		}

		/// <summary>
		/// Initial reference timestamp, which needs to be less than the current time point.
		/// Keep this timestamp consistent for distributed projects.
		/// </summary>
		public long BeginTimeStamp
		{
			get
			{
				if (!Lock || !beginTimeStamp.HasValue)
					beginTimeStamp = BeginTime.ToUinxTimestamp();
				return beginTimeStamp.Value;
			}
		}

		/// <summary>
		/// The maximum number of machine IDs
		/// </summary>
		public long MaxWorkerId
		{
			get
			{
				if (!Lock || !maxWorkerId.HasValue)
					maxWorkerId = BitMaxValue(WorkerIdBits);
				return maxWorkerId.Value;
			}
		}

		/// <summary>
		/// Machine code data left shift number
		/// </summary>
		public int WorkerIdShift => SequenceBits;

		/// <summary>
		/// Timestamp left shift number
		/// </summary>
		public int TimeStampLeftShift
		{
			get
			{
				if (!Lock || !timeStampLeftShift.HasValue)
					timeStampLeftShift = SequenceBits + WorkerIdBits;
				return timeStampLeftShift.Value;
			}
		}

		/// <summary>
		/// Count can be generated in one microsecond, if it reaches this value, wait until the next subtle is generated
		/// </summary>
		public long SequenceMask
		{
			get
			{
				if (!Lock || !sequenceMask.HasValue)
					sequenceMask = BitMaxValue(SequenceBits);
				return sequenceMask.Value;
			}
		}

		/// <summary>
		/// Specify the maximum available number of digits (calculated by binary)
		/// </summary>
		/// <param name="bits">Binary digits</param>
		/// <returns></returns>
		private long BitMaxValue(int bits) => -1L ^ -1L << bits;

		/// <summary>
		/// Detect plausibility and lock the configuration model
		/// </summary>
		internal void CheckAndLockSetting()
		{
			Lock = true;
			if (workerId > MaxWorkerId)
			{
				Lock = false;
				throw new ArgumentOutOfRangeException();
			}
		}

		/// <summary>
		/// Get the default configuration object
		/// </summary>
		/// <returns></returns>
		public static SFIDSetting GetDefault()
		{
			return new SFIDSetting
			{
				workerId = 0, 
				beginTime = DateTimeExtenstions.UtcZero,
				workerIdBits = 2, //The default mechanical bit length is 2 digits and supports up to 4 machines
				sequenceBits = 12 //The maximum number of generated IDs per millisecond is 8192
			};
		}

		
	}
}
