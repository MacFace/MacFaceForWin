/*
 * $Id$
 */
using System;

namespace MacFace
{
	/// <summary>
	/// MemoryUsage ÇÃäTóvÇÃê‡ñæÇ≈Ç∑ÅB
	/// </summary>
	public class MemoryUsage
	{
		private int available;
		private int committed;
		private int pagein;
		private int pageout;
		private int systemCache;
		private int kernelPaged;
		private int kernelNonPaged;
		private int driverTotal;
		private int systemCodeTotal;

		public MemoryUsage(int available, int committed, int pagein, int pageout,
			int systemCache, int kernelPaged, int kernelNonPaged, int driverTotal, int systemCodeTotal)
		{
			this.available = available;
			this.committed = committed;
			this.pagein = pagein;
			this.pageout = pageout;
			this.systemCache = systemCache;
			this.kernelPaged = kernelPaged;
			this.kernelNonPaged = kernelNonPaged;
			this.driverTotal = driverTotal;
			this.systemCodeTotal = systemCodeTotal;
		}

		public int Available
		{
			get { return available; }
		}

		public int Used
		{
			get { return committed + systemCache + kernelPaged + KernelNonPaged + DriverTotal + SystemCodeTotal; }
		}

		public int Committed
		{
			get { return committed; }
		}

		public int Pagein
		{
			get { return pagein; }
		}

		public int Pageout
		{
			get { return pageout; }
		}

		public int SystemCache
		{
			get { return systemCache; }
		}

		public int KernelPaged
		{
			get { return kernelPaged; }
		}

		public int KernelNonPaged
		{
			get { return kernelNonPaged; }
		}

		public int DriverTotal
		{
			get { return driverTotal; }
		}

		public int SystemCodeTotal
		{
			get { return systemCodeTotal; }
		}
	}
}
