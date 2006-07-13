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

			SYSTEM_PROCESSOR_PERFORMANCE_INFORMATION sysProcessorPerfInfo;
			UInt32 outLen;
			UInt32 len = (UInt32)(Marshal.SizeOf(typeof(SYSTEM_PROCESSOR_PERFORMANCE_INFORMATION)));
			NtQuerySystemInformation(SYSTEM_INFORMATION_CLASS.SystemProcessorPerformanceInformation,
				out sysProcessorPerfInfo
				, len
				, out outLen);
			
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

		enum SYSTEM_INFORMATION_CLASS
		{
			SystemBasicInformation = 0,
			SystemPerformanceInformation = 2,
			SystemTimeOfDayInformation = 3,
			SystemProcessInformation = 5,
			SystemProcessorPerformanceInformation = 8,
			SystemInterruptInformation = 23,
			SystemExceptionInformation = 33,
			SystemRegistryQuotaInformation = 37,
			SystemLookasideInformation = 45
		}

		struct SYSTEM_PROCESSOR_PERFORMANCE_INFORMATION 
		{
			public Int64 IdleTime;
			public Int64 KernelTime;
			public Int64 UserTime;
			public Int64 Reserved1_0;
			public Int64 Reserved1_1;
			public Int32 Reserved2;
		}

		[DllImport("ntdll.dll")]
		private extern static UInt32 NtQuerySystemInformation(
			SYSTEM_INFORMATION_CLASS SystemInformationClass,
			out SYSTEM_PROCESSOR_PERFORMANCE_INFORMATION SystemInformation,
			UInt32 SystemInformationLength,
			out UInt32 ReturnLength
		);

	}
}
