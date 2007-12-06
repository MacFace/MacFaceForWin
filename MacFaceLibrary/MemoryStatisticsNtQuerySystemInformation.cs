using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace MacFace
{
	/// <summary>
	/// MemoryStatisticsNtQuerySystemInformation ÇÃäTóvÇÃê‡ñæÇ≈Ç∑ÅB
	/// </summary>
	public class MemoryStatisticsNtQuerySystemInformation : MemoryStatistics
	{
		
		public MemoryStatisticsNtQuerySystemInformation(int historySize) : base(historySize)
		{
			NextValue();
		}

		public override ulong CommitLimit 
		{
			get
			{
				return _commitLimit;
			}
		}

		private ulong _commitLimit = 0;
		private DateTime _prevCollectTime;
		private UInt32 _prevPagesRead;
		private UInt32 _prevPagesWritten;

		protected override MemoryUsage NextValue()
		{
			Unmanaged.SYSTEM_PERFORMANCE_INFORMATION sysPerfInfo = new MacFace.MemoryStatisticsNtQuerySystemInformation.Unmanaged.SYSTEM_PERFORMANCE_INFORMATION();
			Int32 sysInfoLen = Marshal.SizeOf(typeof(Unmanaged.SYSTEM_PERFORMANCE_INFORMATION));
			UInt32 retLen;

			Unmanaged.NtQuerySystemInformation(Unmanaged.SYSTEM_INFORMATION_CLASS.SystemPerformanceInformation,
				out sysPerfInfo,
				(UInt32)sysInfoLen,
				out retLen);

			UInt32 pageSize = 4096;
			
			_commitLimit = (UInt64)(sysPerfInfo.TotalCommitLimit * pageSize);
			
			Int32 pageIn = (Int32)((sysPerfInfo.PagesRead - _prevPagesRead)/ ((TimeSpan)(DateTime.Now - _prevCollectTime)).TotalSeconds);
			Int32 pageOut = (Int32)((sysPerfInfo.PagefilePagesWritten - _prevPagesWritten)/ ((TimeSpan)(DateTime.Now - _prevCollectTime)).TotalSeconds);
			_prevCollectTime = DateTime.Now;
			_prevPagesRead = sysPerfInfo.PagesRead;
			_prevPagesWritten = sysPerfInfo.PagefilePagesWritten;

			return new MemoryUsage(
				(UInt64)sysPerfInfo.AvailablePages * pageSize,
				(UInt64)sysPerfInfo.TotalCommittedPages * pageSize,
				pageIn,
				pageOut,
				(UInt64)sysPerfInfo.MmSystemCachePage * pageSize,
				(UInt64)sysPerfInfo.PagedPoolUsage * pageSize,
				(UInt64)sysPerfInfo.NonPagedPoolUsage * pageSize,
				(UInt64)sysPerfInfo.SystemDriverPage * pageSize,
				(UInt64)sysPerfInfo.SystemCodePage * pageSize);
		}

		private class Unmanaged
		{
			public enum SYSTEM_INFORMATION_CLASS
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

			[StructLayout(LayoutKind.Sequential)]
			public struct SYSTEM_PERFORMANCE_INFORMATION 
			{
				public Int64  IdleTime;
				public Int64  ReadTransferCount;
				public Int64  WriteTransferCount;
				public Int64  OtherTransferCount;
				public UInt32 ReadOperationCount;
				public UInt32 WriteOperationCount;
				public UInt32 OtherOperationCount;
				public UInt32 AvailablePages;
				public UInt32 TotalCommittedPages;
				public UInt32 TotalCommitLimit;
				public UInt32 PeakCommitment;
				public UInt32 PageFaults;
				public UInt32 WriteCopyFaults;
				public UInt32 TransitionFaults;
				public UInt32 CacheTransitionFaults;
				public UInt32 DemandZeroFaults;
				public UInt32 PagesRead;
				public UInt32 PageReadIos;
				public UInt32 CacheReads;
				public UInt32 CacheIos;
				public UInt32 PagefilePagesWritten;
				public UInt32 PagefilePageWriteIos;
				public UInt32 MappedFilePagesWritten;
				public UInt32 MappedFilePageWriteIos;
				public UInt32 PagedPoolUsage;
				public UInt32 NonPagedPoolUsage;
				public UInt32 PagedPoolAllocs;
				public UInt32 PagedPoolFrees;
				public UInt32 NonPagedPoolAllocs;
				public UInt32 NonPagedPoolFrees;
				public UInt32 TotalFreeSystemPtes;
				public UInt32 SystemCodePage;
				public UInt32 TotalSystemDriverPages;
				public UInt32 TotalSystemCodePages;
				public UInt32 SmallNonPagedLookasideListAllocateHits;
				public UInt32 SmallPagedLookasideListAllocateHits;
				public UInt32 Reserved3;
				public UInt32 MmSystemCachePage;
				public UInt32 PagedPoolPage;
				public UInt32 SystemDriverPage;
				public UInt32 FastReadNoWait;
				public UInt32 FastReadWait;
				public UInt32 FastReadResourceMiss;
				public UInt32 FastReadNotPossible;
				public UInt32 FastMdlReadNoWait;
				public UInt32 FastMdlReadWait;
				public UInt32 FastMdlReadResourceMiss;
				public UInt32 FastMdlReadNotPossible;
				public UInt32 MapDataNoWait;
				public UInt32 MapDataWait;
				public UInt32 MapDataNoWaitMiss;
				public UInt32 MapDataWaitMiss;
				public UInt32 PinMappedDataCount;
				public UInt32 PinReadNoWait;
				public UInt32 PinReadWait;
				public UInt32 PinReadNoWaitMiss;
				public UInt32 PinReadWaitMiss;
				public UInt32 CopyReadNoWait;
				public UInt32 CopyReadWait;
				public UInt32 CopyReadNoWaitMiss;
				public UInt32 CopyReadWaitMiss;
				public UInt32 MdlReadNoWait;
				public UInt32 MdlReadWait;
				public UInt32 MdlReadNoWaitMiss;
				public UInt32 MdlReadWaitMiss;
				public UInt32 ReadAheadIos;
				public UInt32 LazyWriteIos;
				public UInt32 LazyWritePages;
				public UInt32 DataFlushes;
				public UInt32 DataPages;
				public UInt32 ContextSwitches;
				public UInt32 FirstLevelTbFills;
				public UInt32 SecondLevelTbFills;
				public UInt32 SystemCalls;
			}

			[DllImport("ntdll.dll")]
			public extern static UInt32 NtQuerySystemInformation(
				SYSTEM_INFORMATION_CLASS SystemInformationClass,
				out SYSTEM_PERFORMANCE_INFORMATION SystemInformation,
				UInt32 SystemInformationLength,
				out UInt32 ReturnLength
				);
		}

	}
}
