using System;
using System.Diagnostics;

namespace MacFace
{
	/// <summary>
	/// CPUStats nngY
	/// </summary>
	public class CPUStatistics
	{
		private CPUUsage[] history;
		private int length;
		private int head;
		private int count;

		private CPUUsage latest;

		private PerformanceCounter userCounter;
		private PerformanceCounter systemCounter;
		private PerformanceCounter idleCounter;

		public CPUStatistics(int historySize)
		{
			history = new CPUUsage[historySize];
			length = historySize;
			count = 0;
			head = 0;

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

		public CPUUsage Latest 
		{
			get { return latest; }
		}

		public CPUUsage this[int index]
		{
			get {
				index = head - index - 1;
				if (index < 0) index += length;
				return history[index];
			}
		}

		public int Count 
		{
			get { return count; }
		}

		public void Update() 
		{
			latest = NextValue();
			history[head++] = latest;

			if (head >= length) head = 0;
			if (count < length) count++;
		}

		protected virtual CPUUsage NextValue()
		{
			int user   = (int)userCounter.NextValue();
			int system = (int)systemCounter.NextValue();
			int idle   = (int)idleCounter.NextValue();

			return new CPUUsage(user, system, idle);
		}
	
	}
}
