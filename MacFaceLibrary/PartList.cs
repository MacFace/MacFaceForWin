/*
 * PartList.cs
 * $Id$
 * 
 */

using System;
using System.Collections;

namespace MacFace
{
	/// <summary>
	/// PartList ‚ÌŠT—v‚Ìà–¾‚Å‚·B
	/// </summary>
	public class PartList : ArrayList
	{
		public PartList()
		{
		}
		public PartList(ArrayList parts)
		{
			base.AddRange(parts);
		}
		public PartList(Part[] parts)
		{
			this.AddRange(parts);
		}

		//
		// overloads
		//
		public int Add(Part value)
		{
			return base.Add (value);
		}

		public void AddRange(Part[] c)
		{
			base.AddRange (c);
		}

		public void Remove(Part obj)
		{
			base.Remove (obj);
		}

		public bool Contains(Part item)
		{
			return base.Contains (item);
		}

		public int IndexOf(Part value)
		{
			return base.IndexOf (value);
		}

		public int IndexOf(Part value, int startIndex)
		{
			return base.IndexOf (value, startIndex);
		}
		
		public void CopyTo(Part[] array)
		{
			base.CopyTo (array);
		}
	}
}
