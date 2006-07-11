using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace MacFace
{
	/// <summary>
	/// MemoryStatisticsPSAPI ÇÃäTóvÇÃê‡ñæÇ≈Ç∑ÅB
	/// </summary>
	public class MemoryStatisticsPSAPI : MemoryStatistics
	{
		public MemoryStatisticsPSAPI(int historySize) : base(historySize)
		{
		}

		public override ulong CommitLimit 
		{
			get
			{
				Unmanaged.PERFORMANCE_INFORMATION pInfo;
				pInfo.cb = (UInt32)Marshal.SizeOf(typeof(Unmanaged.PERFORMANCE_INFORMATION));
				Unmanaged.GetPerformanceInfo(out pInfo, (UInt32)Marshal.SizeOf(typeof(Unmanaged.PERFORMANCE_INFORMATION)));
				
				return (ulong)pInfo.CommitLimit * pInfo.PageSize;
			}
		}

		protected override MemoryUsage NextValue()
		{
			Unmanaged.PERFORMANCE_INFORMATION pInfo;
			pInfo.cb = (UInt32)Marshal.SizeOf(typeof(Unmanaged.PERFORMANCE_INFORMATION));
			Unmanaged.GetPerformanceInfo(out pInfo, (UInt32)Marshal.SizeOf(typeof(Unmanaged.PERFORMANCE_INFORMATION)));

			int available      = (int)(pInfo.PhysicalAvailable * pInfo.PageSize);
			int committed      = (int)(pInfo.CommitTotal * pInfo.PageSize);
			
			// TODO:
			int pagein	       = (int)0;
			int pageout        = (int)0;

			int systemCache    = (int)(pInfo.SystemCache * pInfo.PageSize);
			int kernelPaged    = (int)(pInfo.KernelPaged * pInfo.PageSize);
			int kernelNonPaged = (int)(pInfo.KernelNonpaged * pInfo.PageSize);

			// XXX: Ç∆ÇËÇ†Ç¶Ç∏ï–ï˚Ç…êUÇËï™ÇØÇƒÇµÇ‹Ç§
			// int kernelTotal = usage.KernelNonPaged + usage.KernelPaged + usage.DriverTotal + usage.SystemCodeTotal;
			int driverTotal    = (int)0;
			int systemCodeTotal = (int)((pInfo.KernelTotal - (pInfo.KernelNonpaged + pInfo.KernelPaged)) * pInfo.PageSize);

			return new MemoryUsage(available, committed, pagein, pageout,
				systemCache, kernelPaged, kernelNonPaged, driverTotal, systemCodeTotal);
		}

		private class Unmanaged
		{
			[DllImport("psapi.dll")]
			public extern static Boolean GetPerformanceInfo(
				[Out] out PERFORMANCE_INFORMATION pPerformanceInformation,
				[In]  UInt32 cb
			);
			
			[StructLayout(LayoutKind.Sequential)]
			public struct PERFORMANCE_INFORMATION
			{
				public UInt32 cb;
				// SIZE_T -> UInt32 (x86)
				public UInt32 CommitTotal;
				public UInt32 CommitLimit;
				public UInt32 CommitPeak;
				public UInt32 PhysicalTotal;
				public UInt32 PhysicalAvailable;
				public UInt32 SystemCache;
				public UInt32 KernelTotal;
				public UInt32 KernelPaged;
				public UInt32 KernelNonpaged;
				public UInt32 PageSize;
				public UInt32 HandleCount;
				public UInt32 ProcessCount;
				public UInt32 ThreadCount;
			}
		}

	}
}
