namespace System
{
	/// <summary>
	/// Static method part of snowflake distributed auto-increment ID
	/// </summary>
	public partial struct SFID
	{
		/// <summary>
		/// Default configuration model
		/// </summary>
		private static SFIDSetting DefaultSetting = SFIDSetting.GetDefault();

		/// <summary>
		/// Millisecond counter
		/// </summary>
		private static long SequenceCursor = 0L;

		/// <summary>
		/// Last timestamp
		/// </summary>
		public static long LastTimestamp = -1L;

		/// <summary>
		/// Thread lock object
		/// </summary>
		private static readonly object locker = new object();

		/// <summary>
		/// Generate a new ID
		/// </summary>
		/// <returns></returns>
		public static SFID NewID()
		{
			DateTime date = DateTime.UtcNow;
			long timestamp = date.ToUinxTimestamp();
			long sequence = 0;

			lock (locker)
			{
				if (LastTimestamp == timestamp)
				{
					//Generate ID in the same subtle
					//Use the & operation to calculate whether the count generated in the microsecond has reached the upper limit.
					SequenceCursor = (SequenceCursor + 1) & DefaultSetting.SequenceMask;
					if (SequenceCursor == 0)
					{
						//If the ID count generated within a subtle has reached the upper limit, wait until the next subtle
						timestamp = TillNextMillis(LastTimestamp);
					}
				}
				else
				{
					//Generate IDs in different microseconds
					SequenceCursor = 0; //Counter clear
				}
				sequence = SequenceCursor;
			}

			if (timestamp < LastTimestamp)
			{
				//If the current timestamp is smaller than the last time the ID was generated, 
				//an exception is thrown because there is no guarantee that the currently generated ID was not generated before.
				throw new Exception(string.Format("Clock moved backwards.  Refusing to generate id for {0} milliseconds", LastTimestamp - timestamp));
			}

			//Save the current timestamp as the timestamp of the last generated ID
			LastTimestamp = timestamp;
			return new SFID {
				MachineId = DefaultSetting.WorkerId,
				Sequence = sequence,
				UtcTime = date,
				UtcTimestamp = timestamp,
				SnowflakeId = Combination(timestamp, DefaultSetting.WorkerId, sequence, DefaultSetting)
			};

		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="UtcTimestamp"></param>
		/// <param name="WorkerId"></param>
		/// <param name="Sequence"></param>
		/// <param name="setting"></param>
		/// <returns></returns>
		private static long Combination(long UtcTimestamp, long WorkerId, long Sequence, SFIDSetting setting)
		{
			return (UtcTimestamp - setting.BeginTimeStamp << setting.TimeStampLeftShift) | WorkerId << setting.WorkerIdShift | Sequence;
		}


		/// <summary>
		/// 设置
		/// </summary>
		/// <param name="SettingSetup"></param>
		public static void SetDefualtSetting(Action<SFIDSetting> SettingSetup)
		{
			var setting = SFIDSetting.GetDefault();
			SettingSetup(setting);
			setting.CheckAndLockSetting();
			DefaultSetting = setting;

			SequenceCursor = 0L;
			LastTimestamp = -1L;
		}

		/// <summary>
		/// Get the next microsecond timestamp
		/// </summary>
		/// <param name="lastTimestamp"></param>
		/// <returns></returns>
		private static long TillNextMillis(long lastTimestamp)
		{
			long timestamp = DateTime.Now.ToUinxTimestamp();
			while (timestamp <= lastTimestamp)
				timestamp = DateTime.Now.ToUinxTimestamp();
			return timestamp;
		}
	}
}
