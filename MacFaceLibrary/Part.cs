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
	public class Part : IDisposable
	{
		private Image _image;
		private string _filename;
		private Point _point;

		/*public Part(string basePath, Hashtable partDef)
		{
			this.path = (string)Path.Combine(partDef["filename"]);
			this.x = (int)partDef["pos x"];
			this.y = (int)partDef["pos y"];
		}*/
		
		public Part(String path, int x, int y)
		{
			_filename = System.IO.Path.GetFileName(path);
			_image = Image.FromFile(path);
			this.X = x;
			this.Y = y;
		}

		public Part(String path, Image image, int x, int y)
		{
			_filename = System.IO.Path.GetFileName(path);
			_image = image;
			this.X = x;
			this.Y = y;
		}

		public string FileName 
		{
			get { return _filename; }
		}

		public Image Image 
		{
			get { return _image; }
		}

		public Point Point
		{
			get { return _point; }
			set { _point = value; }
		}
		public int X
		{
			get { return _point.X; }
			set { _point.X = value; }
		}

		public int Y
		{
			get { return 128 - _point.Y - _image.Height; }
			set { _point.Y = 128 - value - _image.Height; }
		}

		/*public void Draw(Graphics g)
		{
			Image img = part.Image;
			g.DrawImage(img, x, drawY, img.Size.Width, img.Size.Height);
		}*/

		#region IDisposable ÉÅÉìÉo

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing) 
		{
			if (disposing)
			{
			}
			// çÌèú
			if (_image != null) { _image.Dispose(); }
		}

		~Part()
		{
			Dispose(false);
		}

		#endregion
	}
}
