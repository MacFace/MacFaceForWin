/*
 * $Id$
 */
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace MacFace.FloatApp
{
	/// <summary>
	/// StatusWindow の概要の説明です。
	/// </summary>
	public class StatusWindow : System.Windows.Forms.Form
	{
		/// <summary>
		/// 必要なデザイナ変数です。
		/// </summary>
		private System.ComponentModel.Container components = null;

		internal System.Windows.Forms.PictureBox memoryGraphPicBox;
		internal System.Windows.Forms.PictureBox cpuGraphPicBox;

		private CPUStatistics cpuStats;
		private MemoryStatistics memStats;

		private Bitmap cpuGraph;
		private Bitmap memoryGraph;

		public StatusWindow(CPUStatistics cpuStats, MemoryStatistics memStats)
		{
			//
			// Windows フォーム デザイナ サポートに必要です。
			//
			InitializeComponent();

			this.cpuStats = cpuStats;
			this.memStats = memStats;

			cpuGraph = new Bitmap(cpuGraphPicBox.Width, cpuGraphPicBox.Height);
			memoryGraph = new Bitmap(memoryGraphPicBox.Width, memoryGraphPicBox.Height);
			cpuGraphPicBox.Image = cpuGraph;
			memoryGraphPicBox.Image = memoryGraph;
		}

		/// <summary>
		/// 使用されているリソースに後処理を実行します。
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					cpuGraph.Dispose();
					memoryGraph.Dispose();
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		public void UpdateGraph()
		{
			drawCPUGraph();
			drawMemoryGraph();
			cpuGraphPicBox.Invalidate();
			memoryGraphPicBox.Invalidate();
		}

		private void drawCPUGraph()
		{
			Graphics g = Graphics.FromImage(cpuGraph);
			g.SmoothingMode = SmoothingMode.AntiAlias;

			int fw = cpuGraph.Width;
			int fh = cpuGraph.Height;
			int m = fh / 10;

			g.FillRectangle(new SolidBrush(Color.White), 0, 0, fw, fh);
			Pen pen = new Pen(Color.FromArgb(220, 220, 220), 1F);
			for (int y = 0; y < fh; y += m) 
			{
				g.DrawLine(pen, 0, y, fw, y);
			}
			g.DrawLine(Pens.Gray, 0, fh/2, fw, fh/2);

			int count = cpuStats.Count;

			if (count >= 2) 
			{
				int bw = (fw + (60-1)) / 60;
				Point[] userGraph = new Point[count+2];
				Point[] sysGraph = new Point[count+2];

				userGraph[count+0].X = fw - (count-1) * bw;
				userGraph[count+0].Y = fh - 0;
				userGraph[count+1].X = fw - 0 * bw;
				userGraph[count+1].Y = fh - 0;

				sysGraph[count+0].X = fw - (count-1) * bw;
				sysGraph[count+0].Y = fh - 0;
				sysGraph[count+1].X = fw - 0 * bw;
				sysGraph[count+1].Y = fh - 0;

				for (int i = 0; i < count; i++) 
				{
					CPUUsage usage = cpuStats[i];
					userGraph[i].X = sysGraph[i].X = fw - i * bw;
					userGraph[i].Y = fh - usage.Active;
					sysGraph[i].Y = fh - usage.System;
				}

				g.FillPolygon(new SolidBrush(Color.FromArgb(50, 0, 0, 255)), userGraph);
				g.DrawPolygon(new Pen(Color.FromArgb(0, 0, 255), 1F), userGraph);
				g.FillPolygon(new SolidBrush(Color.FromArgb(50, 255, 0, 0)), sysGraph);
			}

			g.Dispose();
		}

		private void drawMemoryGraph()
		{
			Graphics g = Graphics.FromImage(memoryGraph);

			int fw = memoryGraph.Width;
			int fh = memoryGraph.Height;

			int totalMemory = (int)memStats.TotalVisibleMemorySize * 1024;
			double rate = (double)fh / (totalMemory * 1.5);
			int border = (int)(totalMemory * rate);

			g.FillRectangle(new SolidBrush(Color.White), 0, 0, fw, fh);
			Pen pen = new Pen(Color.FromArgb(220, 220, 220), 1F);
			for (int y = fh; y > 0; y -= (int)(256*1024*1024 * rate)) 
			{
				g.DrawLine(pen, 0, y, fw, y);
			}

			g.SmoothingMode = SmoothingMode.None;
			Brush availableBrush = new SolidBrush(Color.FromArgb(180, 100, 100, 255));
			Brush kernelBrush = new SolidBrush(Color.FromArgb(180, 255, 0, 0));
			Brush commitedBrush = new SolidBrush(Color.FromArgb(180, 255, 145, 0));
			Brush systemCacheBrush = new SolidBrush(Color.FromArgb(50, 255, 0, 0));
			//			Brush spaceBrush = new SolidBrush(Color.FromArgb(180, 240, 230, 255));
			Brush spaceBrush = new SolidBrush(Color.FromArgb(100, 100, 100, 255));

			int count = memStats.Count;
			int bw = (fw + (60-1)) / 60;

			for (int i = 0; i < count; i++) 
			{
				MemoryUsage usage = memStats[i];

				int x = fw - i * bw - bw;
				int y = fh;
				int w = bw;
				int h = 0;

				long kernelTotal = usage.KernelNonPaged + usage.KernelPaged + usage.DriverTotal + usage.SystemCodeTotal;
				h = (int)((kernelTotal) * rate);
				y -= h;
				g.FillRectangle(kernelBrush, x, y, w, h);

				h = (int)(usage.SystemCache * rate);
				y -= h;
				g.FillRectangle(systemCacheBrush, x, y, w, h);

				h = (int)(usage.Committed * rate);
				y -= h;
				g.FillRectangle(commitedBrush, x, y, w, h);

				h = (int)(usage.Available * rate);
				y -= h;
				g.FillRectangle(availableBrush, x, y, w, h);

				h = y;
				y = 0;
				g.FillRectangle(spaceBrush, x, y, w, h);


				x = fw - i * bw - bw;
				w = bw/2;
				h = (int)(usage.Pagein);
				y = fh - h;
				g.FillRectangle(Brushes.LightGray, x, y, w, h);

				x = fw + bw/2 - i * bw - bw;
				w = bw/2;
				h = (int)(usage.Pageout);
				y = fh - h;
				g.FillRectangle(Brushes.Black, x, y, w, h);
			}
			Pen borderPen = new Pen(Color.Blue);
			borderPen.DashStyle = DashStyle.Dash;
			g.DrawLine(borderPen, 0, fh-border, fw, fh-border);

			g.Dispose();
		}

		#region Windows フォーム デザイナで生成されたコード 
		/// <summary>
		/// デザイナ サポートに必要なメソッドです。このメソッドの内容を
		/// コード エディタで変更しないでください。
		/// </summary>
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(StatusWindow));
			this.memoryGraphPicBox = new System.Windows.Forms.PictureBox();
			this.cpuGraphPicBox = new System.Windows.Forms.PictureBox();
			this.SuspendLayout();
			// 
			// memoryGraphPicBox
			// 
			this.memoryGraphPicBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.memoryGraphPicBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.memoryGraphPicBox.Location = new System.Drawing.Point(7, 8);
			this.memoryGraphPicBox.Name = "memoryGraphPicBox";
			this.memoryGraphPicBox.Size = new System.Drawing.Size(300, 100);
			this.memoryGraphPicBox.TabIndex = 0;
			this.memoryGraphPicBox.TabStop = false;
			// 
			// cpuGraphPicBox
			// 
			this.cpuGraphPicBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.cpuGraphPicBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.cpuGraphPicBox.Location = new System.Drawing.Point(7, 116);
			this.cpuGraphPicBox.Name = "cpuGraphPicBox";
			this.cpuGraphPicBox.Size = new System.Drawing.Size(300, 100);
			this.cpuGraphPicBox.TabIndex = 1;
			this.cpuGraphPicBox.TabStop = false;
			// 
			// StatusWindow
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 12);
			this.ClientSize = new System.Drawing.Size(314, 223);
			this.Controls.Add(this.cpuGraphPicBox);
			this.Controls.Add(this.memoryGraphPicBox);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(322, 250);
			this.Name = "StatusWindow";
			this.Text = "ステータス";
			this.SizeChanged += new System.EventHandler(this.StatusWindow_SizeChanged);
			this.ResumeLayout(false);

		}
		#endregion

		private void StatusWindow_SizeChanged(object sender, System.EventArgs e)
		{
			cpuGraph.Dispose();
			memoryGraph.Dispose();
			cpuGraph = new Bitmap(cpuGraphPicBox.Width, cpuGraphPicBox.Height);
			memoryGraph = new Bitmap(memoryGraphPicBox.Width, memoryGraphPicBox.Height);
			cpuGraphPicBox.Image = cpuGraph;
			memoryGraphPicBox.Image = memoryGraph;
		}
	}
}
