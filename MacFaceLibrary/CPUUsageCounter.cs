/*
 * $Id$
 */
using System;
using System.Diagnostics;

namespace MacFace
{
	/// <summary>
	/// HostStatistics ÇÃäTóvÇÃê‡ñæÇ≈Ç∑ÅB
	/// </summary>
	public class CPUUsageCounter
	{
		private PerformanceCounter userCounter;
		private PerformanceCounter systemCounter;
		private PerformanceCounter idleCounter;

		public CPUUsageCounter()
		{
			userCounter = new PerformanceCounter();
			userCounter.CategoryName = "Processor";
			userCounter.CounterName = "% User Time";
			userCounter.InstanceName = "_Total";

			systemCounter = new PerformanceCounter();
			systemCounter.CategoryName = "Processor";
			systemCounter.CounterName = "% Privileged Time";
			systemCounter.InstanceName = "_Total";

			idleCounter = new PerformanceCounter();
			idleCounter.CategoryName = "Processor";
			idleCounter.CounterName = "% Idle Time";
			idleCounter.InstanceName = "_Total";
		}

		public CPUUsage CurrentUsage()
		{
			int user   = (int)userCounter.NextValue();
			int system = (int)systemCounter.NextValue();
			int idle   = (int)idleCounter.NextValue();

			return new CPUUsage(user, system, idle);
		}

	}
}
