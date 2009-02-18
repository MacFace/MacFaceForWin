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
            NtKernel.SYSTEM_PERFORMANCE_INFORMATION sysPerfInfo = NtKernel.QuerySystemPerformanceInformation();

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
	}
}
