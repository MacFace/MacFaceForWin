/*
 * Part.cs
 * $Id$
 * 
 */
using System;
using System.Drawing;

namespace MacFace
{
	/// <summary>
	/// Part ÇÃäTóvÇÃê‡ñæÇ≈Ç∑ÅB
	/// </summary>
	public class Part
	{
		protected Image image;
		protected string filename;
		protected int x;
		protected int y;

		/*public Part(string basePath, Hashtable partDef)
		{
			this.path = (string)Path.Combine(partDef["filename"]);
			this.x = (int)partDef["pos x"];
			this.y = (int)partDef["pos y"];
		}*/
		
		public Part(String path, int x, int y)
		{
			this.filename = filename;
			this.image = Image.FromFile(path);
			this.x = x;
			this.y = y;
		}

		public Part(String path, Image image, int x, int y)
		{
			this.filename = filename;
			this.image = image;
			this.x = x;
			this.y = y;
		}

		public string FileName 
		{
			get { return filename; }
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

		/*public void Draw(Graphics g)
		{
			Image img = part.Image;
			g.DrawImage(img, x, drawY, img.Size.Width, img.Size.Height);
		}*/
	}
}
