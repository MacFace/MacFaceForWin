/*
 * $Id$
 */
using System;

namespace MacFace
{
	/// <summary>
	/// MemoryUsage ‚ÌŠT—v‚Ìà–¾‚Å‚·B
	/// </summary>
	public class MemoryUsage
	{
		private int available;
		private int committed;
		private int pagein;
		private int pageout;

		public MemoryUsage(int available, int committed, int pagein, int pageout)
		{
			this.available = available;
			this.committed = committed;
			this.pagein = pagein;
			this.pageout = pageout;
		}

		public int Available
		{
			get { return available; }
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
	}
}
