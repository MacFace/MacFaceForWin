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
		private PerformanceCounter availableCounter;
		private PerformanceCounter committedCounter;
		private PerformanceCounter pageoutCounter;
		private PerformanceCounter pageinCounter;

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
	}
}
