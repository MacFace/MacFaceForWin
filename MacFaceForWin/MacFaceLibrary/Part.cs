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
	/// Part ‚ÌŠT—v‚Ìà–¾‚Å‚·B
	/// </summary>
	public class Part : IDisposable
	{
		private Image _image;
		private string _imagePath;
		private Point _point;

		public Part(String path, int x, int y)
		{
			_imagePath = path;
			_image = Image.FromFile(path);
			_point.X = x;
			_point.Y = y;
		}

		public Part(String path, Image image, int x, int y)
		{
            _imagePath = path;
            _image = image;
			_point.X = x;
			_point.Y = y;
		}

		public string FileName 
		{
            get
            {
                return System.IO.Path.GetFileName(_imagePath);
            }
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
			get { return _point.Y; }
			set { _point.Y = value; }
		}

		/*public void Draw(Graphics g)
		{
			Image img = part.Image;
			g.DrawImage(img, x, drawY, img.Size.Width, img.Size.Height);
		}*/

		#region IDisposable ƒƒ“ƒo

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
			// íœ
			if (_image != null) { _image.Dispose(); }
		}

		~Part()
		{
			Dispose(false);
		}

		#endregion
	}
}
