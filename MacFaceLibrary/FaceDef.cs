/*
 * FaceDef.cs
 * $Id$
 * 
 */

using System;
using System.IO;
using System.Collections;

namespace MacFace
{
	/// <summary>
	/// FaceDef の概要の説明です。
	/// </summary>
	public class FaceDef
	{
		public const string FaceDefName = "faceDef.plist";
		public const int PatternCols = 11;
		public const int PatternRows = 3;

		protected string path;
		protected string title;
		protected string auther;
		protected string version;
		protected string webSite;

		protected PartList parts = new PartList();
		//protected int[][,] patterns;
		//protected PartList makers = new PartList();


		// ctor
		private FaceDef() {}


		// properties
		public PartList Parts
		{
			get { return parts; }
		}


		// methods
		public static FaceDef CreateFaceDefFromFile(string path)
		{
			FaceDef faceDef = new FaceDef();

			string defFile = Path.Combine(path, FaceDefName);
			Hashtable def = PropertyList.Load(path);

			// Part を読み込む。
			ArrayList partDefList = def["parts"] as ArrayList;
			if (partDefList == null) 
			{
				// throw new IOException();
				return null;
			}

			for (int i = 0; i < partDefList.Count; i++)
			{
				Hashtable partDef = partDefList[i] as Hashtable;
				string filename = partDef["filename"] as String;
				int x = (int)partDef["pos x"];
				int y = (int)partDef["pos y"];
				faceDef.Parts.Add(new Part(filename, x, y));
			}

			// TODO: pattern を読み込む。

			return faceDef;
		}

	}
}
