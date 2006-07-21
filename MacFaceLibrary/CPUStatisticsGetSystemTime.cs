using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace MacFace
{
	/// <summary>
	/// CPUStatisticsGetSystemTime
	/// </summary>
	public class CPUStatisticsGetSystemTime : CPUStatistics
	{
			
		UInt64 idleTimePrev = 0;
		UInt64 kernelTimePrev = 0;
		UInt64 userTimePrev = 0;

		public CPUStatisticsGetSystemTime(int historySize) : base(historySize)
		{
			NextValue();
		}

		protected override CPUUsage NextValue()
		{
			UInt64 idleTime;
			UInt64 kernelTime;
			UInt64 userTime;

			GetSystemTimes(out idleTime, out kernelTime, out userTime);
				
			Int32 idleTimeDiff = (Int32)(idleTime - idleTimePrev);
			Int32 userTimeDiff = (Int32)(userTime - userTimePrev);
			Int32 kernelTimeDiff = (Int32)(kernelTime - kernelTimePrev);

			Int32 total = userTimeDiff + kernelTimeDiff;
			Int32 sys   = kernelTimeDiff - idleTimeDiff;

			idleTimePrev = idleTime;
			kernelTimePrev = kernelTime;
			userTimePrev = userTime;

			return new CPUUsage(
				(userTimeDiff * 100) / total,
				(sys * 100) / total,
				(idleTimeDiff * 100) / total
			);
		}

		[DllImport("kernel32.dll")]
		private extern static Boolean GetSystemTimes(
			out UInt64 lpIdleTime,
			out UInt64 lpKernelTime,
			out UInt64 lpUserTime
		);

	}
}
