using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace MacFace
{
	/// <summary>
	/// CPUStatisticsNtQuerySystemInformation
	/// </summary>
	public class CPUStatisticsNtQuerySystemInformation : CPUStatistics
	{
			
		UInt64 idleTimePrev = 0;
		UInt64 kernelTimePrev = 0;
		UInt64 userTimePrev = 0;

		public CPUStatisticsNtQuerySystemInformation(int historySize) : base(historySize)
		{
			NextValue();
		}

		protected override CPUUsage NextValue()
		{
			UInt64 idleTime;
			UInt64 kernelTime;
			UInt64 userTime;

			NtKernel.SYSTEM_PROCESSOR_PERFORMANCE_INFORMATION sysProcessorPerfInfo = NtKernel.QuerySystemProcessorPerformanceInfomation();
			
			idleTime = (UInt64)sysProcessorPerfInfo.IdleTime;
			kernelTime = (UInt64)sysProcessorPerfInfo.KernelTime;
			userTime = (UInt64)sysProcessorPerfInfo.UserTime;
				
			Int32 idleTimeDiff = (Int32)(idleTime - idleTimePrev);
			Int32 userTimeDiff = (Int32)(userTime - userTimePrev);
			Int32 kernelTimeDiff = (Int32)(kernelTime - kernelTimePrev);
			Int32 systemTimeDiff = (Int32)(userTimeDiff + kernelTimeDiff);

			idleTimePrev = idleTime;
			kernelTimePrev = kernelTime;
			userTimePrev = userTime;

			return new CPUUsage(
				(Int32)(100 - ((Double)idleTimeDiff / userTimePrev) * 100),
				(Int32)(100 - ((Double)idleTimeDiff / kernelTimePrev) * 100),
				(Int32)(((Double)idleTimeDiff / (systemTimeDiff)) * 100)
			);
		}
	}
}
