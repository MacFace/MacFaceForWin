/*
 * MacFace パターンウインドウクラス
 *
 * $Id$
 * 
 */

using System;
using System.Windows.Forms;
using System.Drawing;

namespace MacFace.FloatApp
{
	public class PatternWindow : Misuzilla.Windows.Forms.AlphaForm
	{
		private FaceDef curFaceDef;
		private float patternSize;

		private FaceDef.PatternSuite curSuite;
		private int curPattern;
		private int curMarkers;

		// コンストラクタ
		public PatternWindow()
		{
			InitializeComponent();

			this.TransparentMouseMessage = false;
			this.MoveAtFormDrag = true;

			curSuite   = FaceDef.PatternSuite.Normal;
			curPattern = 0;
			curMarkers = 0;
		}

		void InitializeComponent()
		{
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 12);
			this.ClientSize = new System.Drawing.Size(120, 101);
			this.ControlBox = false;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "PatternWindow";
			this.Opacity = 0.75F;
			this.ShowInTaskbar = false;
			this.Text = "MacFace For Windows";
			this.TopMost = true;
		}

		public float PatternSize
		{
			get { return patternSize; }
			set { patternSize = value; }
		}

		public FaceDef FaceDef
		{
			get { return curFaceDef; }
			set { curFaceDef = value; }
		}

		public void UpdatePattern(FaceDef.PatternSuite suite, int patternNo, int markers)
		{
			if (curSuite != suite || curPattern != patternNo || curMarkers != markers) 
			{
				curSuite   = suite;
				curPattern = patternNo;
				curMarkers = markers;
				RefreshPattern();
			}
		}

		public void RefreshPattern()
		{
			Graphics g = this.Graphics;
			g.Clear(Color.FromArgb(0, 0, 0, 0));
			curFaceDef.DrawPatternImage(g, curSuite, curPattern, curMarkers, patternSize);
			this.Update();
		}
	}
}
