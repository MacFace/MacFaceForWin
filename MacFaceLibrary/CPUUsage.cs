/*
 * $Id$
 */
using System;

namespace MacFace
{
	/// <summary>
	/// CPUUsage ‚ÌŠT—v‚Ìà–¾‚Å‚·B
	/// </summary>
	public class CPUUsage
	{
		private int user;
		private int system;
		private int idle;

		public CPUUsage(int user, int system, int idle)
		{
			this.user = user;
			this.system = system;
			this.idle = idle;
		}

		public int User
		{
			get { return user; }
		}

		public int System
		{
			get { return system; }
		}

		public int Active
		{
			get { return 100 - idle; }
		}

		public int Idle
		{
			get { return idle; }
		}
	}
}
