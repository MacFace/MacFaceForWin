using System;
using System.Drawing;
using System.Xml;
using System.Collections;

namespace MacFace
{
	/// <summary>
	/// FaceDef ÇÃäTóvÇÃê‡ñæÇ≈Ç∑ÅB
	/// </summary>
	public class FaceDef
	{
		public static const string FACEDEF_NAME = "faceDef.plist";
		public static const int PATTERN_COLS = 11;
		public static const int PATTERN_ROWS = 3;

		protected string path;
		protected string title;
		protected string auther;
		protected string version;
		protected string webSite;

		protected Part[] parts;
		protected int[][,] patterns;
		protected Part[] makers;

		public FaceDef(string path)
		{
			this.path = path;
		
			//
			string defFile = Path.Combine(path,FACEDEF_NAME);
			XmlTextReader reader = new XmlTextReader(path);
			reader.XmlResolver = null;
			doc.Load(reader);

			Hashtable def = ReadDictionary(doc.DocumentElement.FirstChild);

			ArrayList partDefList = def["parts"];
			parts = new Part[partDefList.Size];
			for (int i = 0; i < partDefList.Size; i++)
			{
				Hashtable partDef = partDefList[i];
				string filnemae = Path.Combine(path,partDef["filename"]);
				int x = partDef["pos x"];
				int y = partDef["pos y"];
				parts[i] = new Part(filename,x,y);
			}

			ArrayList partDefList = def["pattern"];
			parts = new Part[partDefList.Size];
			for (int i = 0; i < partDefList.Size; i++)
			{
				Hashtable partDef = partDefList[i];
				string filnemae = Path.Combine(path,partDef["filename"]);
				int x = partDef["pos x"];
				int y = partDef["pos y"];
				parts[i] = new Part(filename,x,y);
			}
		}


		protected static Part[] LoadPartList(string basePath,Hashtable def)
		{
			ArrayList partDefList = def["pattern"];
			ArrayList list = new ArrayList();
			foreach (Hashtable partDef in partDefList)
			{
				list.Add(new Part(partDef));
			}

			return list.ToArray();
		}

		protected static Hashtable LoadFaceDef(string path)
		{
			string defFile = Path.Combine(path,FACEDEF_NAME);
		}
	}

	public class Pattern
	{
		Part[] parts;

		public Pattern(Part[] parts)
		{
			this.parts = parts;
		}

		public Part this [int index]
		{
			get { return part[index]; }
		}

		public void Draw(Graphics g)
		{
			foreach (Part part in parts) 
			{
				part.Draw(g);
			}
		}
	}

	public class Part
	{
		protected string path;
		protected int x;
		protected int y;

		public Part(string basePath,Hashtable partDef)
		{
			this.path = (string)Path.Combine(partDef["filename"]);
			this.x = (int)partDef["pos x"];
			this.y = (int)partDef["pos y"];
		}
		
		public Part(String path,int x,int y)
		{
			this.path = path;
			this.image = Image.FromFile(path);
			this.x = x;
			this.y = y;
		}

		public string Path 
		{
			get { return path; }
		}

		public Image Image 
		{
			get { return image; }
		}

		public int X
		{
			get { return x; }
		}

		public int Y
		{
			get { return y; }
		}

		public void Draw(Graphics g)
		{
			Image img = part.Image;
			g.DrawImage(img, x, drawY, img.Size.Width, img.Size.Height);
		}
	}
}