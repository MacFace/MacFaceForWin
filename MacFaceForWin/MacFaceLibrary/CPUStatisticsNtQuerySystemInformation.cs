// $Id$
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
			
		Int64 idleTimePrev = 0;
		Int64 kernelTimePrev = 0;
		Int64 userTimePrev = 0;

		public CPUStatisticsNtQuerySystemInformation(int historySize) : base(historySize)
		{
			NextValue();
		}

		protected override CPUUsage NextValue()
		{
			Int64 idleTime;
			Int64 kernelTime;
			Int64 userTime;

			NtKernel.SYSTEM_PROCESSOR_PERFORMANCE_INFORMATION sysProcessorPerfInfo = NtKernel.QuerySystemProcessorPerformanceInfomation(1)[0];
			
			idleTime = sysProcessorPerfInfo.IdleTime;
			kernelTime = sysProcessorPerfInfo.KernelTime - idleTime;
			userTime = sysProcessorPerfInfo.UserTime;
				
			Int64 idleTimeDiff = idleTime - idleTimePrev;
            Int64 userTimeDiff = userTime - userTimePrev;
            Int64 kernelTimeDiff = kernelTime - kernelTimePrev;
            Int64 totalTimeDiff = idleTimeDiff + userTimeDiff + kernelTimeDiff;

			idleTimePrev = idleTime;
			kernelTimePrev = kernelTime;
			userTimePrev = userTime;

			return new CPUUsage(
				(Int32)(((Double)userTimeDiff / totalTimeDiff) * 100),
				(Int32)(((Double)kernelTimeDiff / totalTimeDiff) * 100),
				(Int32)(((Double)idleTimeDiff / totalTimeDiff) * 100)
			);
		}
	}
}
