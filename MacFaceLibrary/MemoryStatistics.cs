using System;
using System.Diagnostics;

namespace MacFace
{
	/// <summary>
	/// MemoryStatistics nngY
	/// </summary>
	public class MemoryStatistics
	{
		private MemoryUsage[] history;
		private int length;
		private int head;
		private int count;

		private MemoryUsage latest;

		private PerformanceCounter availableCounter;
		private PerformanceCounter committedCounter;
		private PerformanceCounter pageoutCounter;
		private PerformanceCounter pageinCounter;
		private PerformanceCounter systemCacheCounter;
		private PerformanceCounter kernelPagedCounter;
		private PerformanceCounter kernelNonPagedCounter;
		private PerformanceCounter driverTotalCounter;
		private PerformanceCounter systemCodeTotalCounter;
		private PerformanceCounter commitLimitCounter;

		private ulong totalVisibleMemorySize;

		public MemoryStatistics(int historySize)
		{
			history = new MemoryUsage[historySize];
			length = historySize;
			count = 0;
			head = 0;

			System.Management.ManagementClass mc = new System.Management.ManagementClass("Win32_OperatingSystem");
			System.Management.ManagementObjectCollection moc = mc.GetInstances();
			foreach (System.Management.ManagementObject mo in moc)
			{
				totalVisibleMemorySize = (ulong)mo["TotalVisibleMemorySize"];
			}

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

			systemCacheCounter = new PerformanceCounter();
			systemCacheCounter.CategoryName = "Memory";
			systemCacheCounter.CounterName = "Cache Bytes";

			kernelPagedCounter = new PerformanceCounter();
			kernelPagedCounter.CategoryName = "Memory";
			kernelPagedCounter.CounterName = "Pool Paged Bytes";

			kernelNonPagedCounter = new PerformanceCounter();
			kernelNonPagedCounter.CategoryName = "Memory";
			kernelNonPagedCounter.CounterName = "Pool Nonpaged Bytes";

			driverTotalCounter = new PerformanceCounter();
			driverTotalCounter.CategoryName = "Memory";
			driverTotalCounter.CounterName = "System Driver Total Bytes";

			systemCodeTotalCounter = new PerformanceCounter();
			systemCodeTotalCounter.CategoryName = "Memory";
			systemCodeTotalCounter.CounterName = "System Code Total Bytes";

			commitLimitCounter = new PerformanceCounter();
			commitLimitCounter.CategoryName = "Memory";
			commitLimitCounter.CounterName = "Commit Limit";
		}

	
		public MemoryUsage Latest 
		{
			get { return latest; }
		}

		public MemoryUsage this[int index]
		{
			get 
			{
				index = head - index - 1;
				if (index < 0) index += length;
				return history[index];
			}
		}

		public int Count 
		{
			get { return count; }
		}

		public ulong TotalVisibleMemorySize 
		{
			get { return totalVisibleMemorySize; }
		}

		public virtual ulong CommitLimit 
		{
			get { return (ulong)commitLimitCounter.NextValue(); }
		}

		public void Update() 
		{
			latest = NextValue();
			history[head++] = latest;

			if (head >= length) head = 0;
			if (count < length) count++;
		}

		protected virtual MemoryUsage NextValue()
		{
			int available      = (int)availableCounter.NextValue();
			int committed      = (int)committedCounter.NextValue();
			int pagein	       = (int)pageinCounter.NextValue();
			int pageout        = (int)pageoutCounter.NextValue();
			int systemCache    = (int)systemCacheCounter.NextValue();
			int kernelPaged    = (int)kernelPagedCounter.NextValue();
			int kernelNonPaged = (int)kernelNonPagedCounter.NextValue();
			int driverTotal    = (int)driverTotalCounter.NextValue();
			int systemCodeTotal = (int)systemCodeTotalCounter.NextValue();

			return new MemoryUsage(available, committed, pagein, pageout,
				systemCache, kernelPaged, kernelNonPaged, driverTotal, systemCodeTotal);
		}
	}
}
