/*
 * FaceDef.cs
 * $Id$
 * 
 */

using System;
using System.IO;
using System.Collections;
using System.Drawing;

namespace MacFace
{

	/// <summary>
	/// FaceDef の概要の説明です。
	/// </summary>
	public class FaceDef
	{
		// パターンスイートID
		public enum PatternSuite
		{
			Normal             = 0,
			MemoryDecline      = 1,
			MemoryInsufficient = 2
		}

		public const int PatternCount = 11;
		public const int ImageWidth   = 128;
		public const int ImageHeight  = 128;

		// マーカー
		public const int MarkerNone    = 0x00;
		public const int MarkerPageIn  = 0x01;
		public const int MarkerPageOut = 0x02;

		// 顔パターン定義プロパティリストファイル名
		private const string FaceDefName = "faceDef.plist";

		// 顔パターン定義のキー名称
		private const string KeyInfoTitle       = "title";
		private const string KeyInfoAuthor      = "author";
		private const string KeyInfoVersion     = "version";
		private const string KeyInfoSiteURL     = "web site";
		
		private const string KeyDefParts        = "parts";
		private const string KeyDefPatterns     = "pattern";
		private const string KeyDefMarkers      = "markers";
		private const string KeyDefTitlePattern = "title pattern";

		private const string KeyPartImage       = "filename";
		private const string KeyPartPosX        = "pos x";
		private const string KeyPartPosY        = "pos y";

		//
		private string _path;
		private string _title;
		private string _author;
		private string _version;
		private Uri    _webSite;

		private Part[] _parts;
		private Part[][][] _patternSuites;
		private Part[] _markers;
		private Part[] _titlePattern;


		/// <summary>
		/// コンストラクタ。
		/// </summary>
		/// <param name="path">顔パターンファイルのパス</param>
		public FaceDef(string path)
		{
			string defFile = System.IO.Path.Combine(path, FaceDefName);
			Hashtable def = PropertyList.Load(defFile);

			// 情報
			_path = path;
			_title = ReadInfoString(def, KeyInfoTitle);
			_author = ReadInfoString(def, KeyInfoAuthor);
			_version = ReadInfoString(def, KeyInfoVersion);

			if (def.ContainsKey(KeyInfoSiteURL))
			{
				try 
				{
					_webSite = new Uri(def[KeyInfoSiteURL] as string);
				} 
				catch (UriFormatException) {}
			}


			// パーツの読み込み
			ArrayList partDefList = (ArrayList)def[KeyDefParts];
			if (partDefList == null) 
			{
				// throw new IOException();
			}

			_parts = new Part[partDefList.Count];
			for (int i = 0; i < partDefList.Count; i++)
			{
				_parts[i] = ReadPart(path, (Hashtable)partDefList[i]);
            }

			// パターンの読み込み
            int suiteCount = PatternSuite.GetNames(typeof(PatternSuite)).Length;
            ;
            ArrayList suiteDefList = (ArrayList)def[KeyDefPatterns];
            _patternSuites = new Part[suiteCount][][];
            for (int i = 0; i < suiteCount; i++)
            {
				_patternSuites[i] = ReadPatternSuite(_parts, (ArrayList)suiteDefList[i]);
			}

			// マーカーの読み込み
			ArrayList markerDefList = (ArrayList)def[KeyDefMarkers];
			_markers = new Part[markerDefList.Count];
			for (int i = 0; i < markerDefList.Count; i++)
			{
				_markers[i] = _parts[(int)markerDefList[i]];
			}

			// タイトルパターンの読み込み
			_titlePattern = ReadPattern(_parts, (ArrayList)def[KeyDefTitlePattern]);
		}

		private string ReadInfoString(Hashtable def, string key)
		{
			string str = def[key] as string;
			return str != null ? str : "";
		}

		private Part ReadPart(string path, Hashtable partDef)
		{
			string filename = (String)partDef[KeyPartImage];
			string imgPath = System.IO.Path.Combine(path, filename);
			Image img = Image.FromFile(imgPath);

			int x = (int)partDef[KeyPartPosX];
			int y = ImageHeight - (int)partDef[KeyPartPosY] - img.Height;

			return new Part(imgPath, img, x, y);
		}

		private Part[] ReadPattern(Part[] parts, ArrayList patternDef)
		{
			Part[] pattern = new Part[patternDef.Count];
			for (int i = 0; i < patternDef.Count; i++) 
			{
				pattern[i] = _parts[(int)patternDef[i]];
			}
			return pattern;
		}

		private Part[][] ReadPatternSuite(Part[] parts, ArrayList suiteDef)
		{
			Part[][] suite = new Part[PatternCount][];
			for (int i = 0; i < PatternCount; i++) 
			{
				suite[i] = ReadPattern(_parts, (ArrayList)suiteDef[i]);
			}
			return suite;
		}


        public string Path
        {
            get { return _path; }
        }

        public string Title
        {
            get { return _title; }
        }

        public string Author
        {
            get { return _author; }
        }

        public string Version
        {
            get { return _version; }
        }

        public Uri WebSite
        {
			get { return _webSite; }
		}

		public Image TitleImage
		{
			get 
			{
				Bitmap image = new Bitmap(ImageWidth, ImageHeight);
				Graphics g = Graphics.FromImage(image);

				foreach (Part part in _titlePattern)
				{
					g.DrawImage(part.Image,
						part.Point.X, part.Point.Y,
						part.Image.Size.Width, part.Image.Size.Height);
				}

				g.Dispose();
				return image;
			}
		}

		public void DrawPatternImage(Graphics g, PatternSuite suite, int no, int markers, float rate)
		{
			Part[] pattern = _patternSuites[(int)suite][no];
			foreach (Part part in pattern)
			{
				g.DrawImage(part.Image,
					part.Point.X * rate, part.Point.Y * rate,
					part.Image.Size.Width * rate, part.Image.Size.Height * rate);
			}

			if (markers != 0)
			{
				for (int i = 0; i < 8; i++)
				{
					if ((markers & (1 << i)) != 0)
					{
						Part part = _markers[i];
						g.DrawImage(part.Image,
							part.Point.X * rate, part.Point.Y * rate,
							part.Image.Size.Width * rate, part.Image.Size.Height * rate);
					}
				}
			}
		}

		public void DrawPatternImage(Graphics g, PatternSuite suite, int no, int markers)
		{
			Part[] pattern = _patternSuites[(int)suite][no];
			foreach (Part part in pattern)
			{
				g.DrawImage(part.Image,
					part.Point.X, part.Point.Y,
					part.Image.Size.Width, part.Image.Size.Height);
			}

			if (markers != 0) {
				for (int i = 0; i < 8; i++)
				{
					if ((markers & (1 << i)) != 0)
					{
						Part part = _markers[i];
						g.DrawImage(part.Image,
							part.Point.X, part.Point.Y,
							part.Image.Size.Width, part.Image.Size.Height);
					}
				}
			}
		}

		public Image PatternImage(PatternSuite suite, int no, int markers)
		{
			Bitmap image = new Bitmap(ImageWidth, ImageHeight);
			Graphics g = Graphics.FromImage(image);
			DrawPatternImage(g, suite, no, markers);
			g.Dispose();
			return image;
		}
	}

	public class FaceDefFormatException : ApplicationException
	{
		// TODO: フォーマットが不正だったときに投げるExceptionを書いて、不正だったときに投げるようにする。
	}
}
