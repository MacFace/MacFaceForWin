/*
 * $Id$
 */
using System;
using System.Diagnostics;

namespace MacFace
{
	/// <summary>
	/// MemoryUsageCounter ÇÃäTóvÇÃê‡ñæÇ≈Ç∑ÅB
	/// </summary>
	public class MemoryUsageCounter
	{
		private static ulong totalVisibleMemorySize;
		private static PerformanceCounter commitLimitCounter;
		
		private PerformanceCounter availableCounter;
		private PerformanceCounter committedCounter;
		private PerformanceCounter pageoutCounter;
		private PerformanceCounter pageinCounter;

		static MemoryUsageCounter()
		{
			System.Management.ManagementClass mc = new System.Management.ManagementClass("Win32_OperatingSystem");
			System.Management.ManagementObjectCollection moc = mc.GetInstances();
			foreach (System.Management.ManagementObject mo in moc)
			{
				totalVisibleMemorySize = (ulong)mo["TotalVisibleMemorySize"];
			}

			commitLimitCounter = new PerformanceCounter();
			commitLimitCounter.CategoryName = "Memory";
			commitLimitCounter.CounterName = "Commit Limit";
		}

		public MemoryUsageCounter()
		{
			availableCounter = new PerformanceCounter();
			availableCounter.CategoryName = "Memory";
			availableCounter.CounterName = "Available Bytes";

			committedCounter = new PerformanceCounter();
			committedCounter.CategoryName = "Memory";
			committedCounter.CounterName = "Committed Bytes";

			pageoutCounter = new PerformanceCounter();
			pageoutCounter.CategoryName = "Memory";
			pageoutCounter.CounterName = "Pages Output/sec";

			pageinCounter = new PerformanceCounter();
			pageinCounter.CategoryName = "Memory";
			pageinCounter.CounterName = "Pages Input/sec";
		}

		public MemoryUsage CurrentUsage()
		{
			int available = (int)availableCounter.NextValue();
			int committed = (int)committedCounter.NextValue();
			int pagein	= (int)pageinCounter.NextValue();
			int pageout   = (int)pageoutCounter.NextValue();

			return new MemoryUsage(available, committed, pagein, pageout);
		}

		public static ulong TotalVisibleMemorySize 
		{
			get { return totalVisibleMemorySize; }
		}

		public static ulong CommitLimit 
		{
			get { return (ulong)commitLimitCounter.NextValue(); }
		}
	}
}
