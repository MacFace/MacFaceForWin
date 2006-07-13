using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace MacFace
{
	/// <summary>
	/// MemoryStatisticsGlobalMemoryStatusEx ÇÃäTóvÇÃê‡ñæÇ≈Ç∑ÅB
	/// </summary>
	public class MemoryStatisticsGlobalMemoryStatusEx : MemoryStatistics
	{
		public MemoryStatisticsGlobalMemoryStatusEx(int historySize) : base(historySize)
		{
		}

		public override ulong CommitLimit 
		{
			get
			{
				Unmanaged.MEMORYSTATUSEX memStat = new Unmanaged.MEMORYSTATUSEX();
				memStat.dwLength = (UInt32)Marshal.SizeOf(typeof(Unmanaged.MEMORYSTATUSEX));
				Unmanaged.GlobalMemoryStatusEx(ref memStat);
				
				return (ulong)memStat.ullTotalPageFile;
			}
		}

		protected override MemoryUsage NextValue()
		{
			Unmanaged.MEMORYSTATUSEX memStat = new Unmanaged.MEMORYSTATUSEX();
			memStat.dwLength = (UInt32)Marshal.SizeOf(typeof(Unmanaged.MEMORYSTATUSEX));
			Unmanaged.GlobalMemoryStatusEx(ref memStat);

			int available      = (int)(memStat.ullAvailPageFile);
			int committed      = (int)(memStat.ullTotalPageFile - memStat.ullAvailPageFile);
			
			return new MemoryUsage(available, committed, 0, 0,
				0, 0, 0, 0, 0);
		}

		private class Unmanaged
		{
			[DllImport("Kernel32.dll")]
			public extern static Boolean GlobalMemoryStatusEx(
				[In][Out] ref MEMORYSTATUSEX lpBuffer
			);
			
			[StructLayout(LayoutKind.Sequential)]
			public struct MEMORYSTATUSEX
			{
				public UInt32 dwLength;
				public UInt32 dwMemoryLoad;
				public UInt64 ullTotalPhys;
				public UInt64 ullAvailPhys;
				public UInt64 ullTotalPageFile;
				public UInt64 ullAvailPageFile;
				public UInt64 ullTotalVirtual;
				public UInt64 ullAvailVirtual;
				public UInt64 ullAvailExtendedVirtual;
			}
		}

	}
}
