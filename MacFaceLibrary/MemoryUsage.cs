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
		private UInt64 available;
        private UInt64 committed;
		private int pagein;
		private int pageout;
        private UInt64 systemCache;
        private UInt64 kernelPaged;
        private UInt64 kernelNonPaged;
        private UInt64 driverTotal;
        private UInt64 systemCodeTotal;

        public MemoryUsage(UInt64 available, UInt64 committed, int pagein, int pageout,
            UInt64 systemCache, UInt64 kernelPaged, UInt64 kernelNonPaged, UInt64 driverTotal, UInt64 systemCodeTotal)
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

		public UInt64 Available
		{
			get { return available; }
		}

		public UInt64 Used
		{
			get { return committed + systemCache + kernelPaged + KernelNonPaged + DriverTotal + SystemCodeTotal; }
		}

		public UInt64 Committed
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

		public UInt64 SystemCache
		{
			get { return systemCache; }
		}

        public UInt64 KernelTotal
        {
            get { return KernelNonPaged + KernelPaged + DriverTotal + SystemCodeTotal; }
        }

        public UInt64 KernelPaged
		{
			get { return kernelPaged; }
		}

		public UInt64 KernelNonPaged
		{
			get { return kernelNonPaged; }
		}

		public UInt64 DriverTotal
		{
			get { return driverTotal; }
		}

		public UInt64 SystemCodeTotal
		{
			get { return systemCodeTotal; }
		}
	}
}
