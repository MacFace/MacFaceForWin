/*
 * FacePattern.cs
 * $Id$
 * 
 */

using System;
using System.Collections;

namespace MacFace
{
	public enum PageOutFlag
	{
		Normal,
		PageOut,
		PageOutLarge
	}

	/// <summary>
	/// FacePattern の概要の説明です。
	/// </summary>
	public class FacePattern
	{
		private PartList[] _partsMemNormal   = new PartList[11];
		private PartList[] _partsMemPageOut  = new PartList[11];
		private PartList[] _partsMemPageOutL = new PartList[11];

		protected PartList[] MemoryNormal { get { return _partsMemNormal; } }
		protected PartList[] MemoryPageOut { get { return _partsMemPageOut; } }
		protected PartList[] MemoryPageOutLarge { get { return _partsMemPageOutL; } }

		public PartList GetPartList(int cpuUsagePct, PageOutFlag pageOut)
		{
			PartList[] partlist = null;
			switch (pageOut) 
			{
				case PageOutFlag.Normal: partlist = _partsMemNormal; break;
				case PageOutFlag.PageOut: partlist = _partsMemPageOut; break;
				case PageOutFlag.PageOutLarge: partlist = _partsMemPageOutL; break;
			}
			return partlist[cpuUsagePct/10];
		}

		public FacePattern(PartList faceParts, ArrayList patterns)
		{
			// メモリの3パターン
			for (int i = 0; i < 3; i++) 
			{
				PartList[] partlist = null;
				switch (i) 
				{
					case 0: partlist = _partsMemNormal; break;
					case 1: partlist = _partsMemPageOut; break;
					case 2: partlist = _partsMemPageOutL; break;
					default: break;
				}

				ArrayList patternByMem = patterns[i] as ArrayList;
				// CPU の11パターン。
				for (int j = 0; j < 11; j++) 
				{
					partlist[j] = new PartList();
					ArrayList facePattern = patternByMem[j] as ArrayList;
					foreach (int patNo in facePattern) 
					{
						partlist[j].Add(faceParts[patNo]);
					}
				}
			}
		}
	}
}
