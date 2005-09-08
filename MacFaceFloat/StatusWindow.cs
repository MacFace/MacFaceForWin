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
		internal System.Windows.Forms.PictureBox memoryGraphPicBox;
		internal System.Windows.Forms.PictureBox cpuGraphPicBox;
		/// <summary>
		/// 必要なデザイナ変数です。
		/// </summary>
		private System.ComponentModel.Container components = null;

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

			cpuGraph = new Bitmap(5*60, 100);
			memoryGraph = new Bitmap(5*60, 100);
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

			g.FillRectangle(new SolidBrush(Color.White), 0, 0, 300, 100);
			Pen pen = new Pen(Color.FromArgb(220, 220, 220), 1F);
			for (int y = 0; y < 100; y += 10) 
			{
				g.DrawLine(pen, 0, y, 300, y);
			}
			g.DrawLine(Pens.Gray, 0, 50, 300, 50);

			int count = cpuStats.Count;

			if (count >= 2) 
			{
				Point[] userGraph = new Point[count+2];
				Point[] sysGraph = new Point[count+2];

				userGraph[count+0].X = 300 - (count-1) * 5;
				userGraph[count+0].Y = 100 - 0;
				userGraph[count+1].X = 300 - 0 * 5;
				userGraph[count+1].Y = 100 - 0;

				sysGraph[count+0].X = 300 - (count-1) * 5;
				sysGraph[count+0].Y = 100 - 0;
				sysGraph[count+1].X = 300 - 0 * 5;
				sysGraph[count+1].Y = 100 - 0;

				for (int i = 0; i < count; i++) 
				{
					CPUUsage usage = cpuStats[i];
					userGraph[i].X = sysGraph[i].X = 300 - i * 5;
					userGraph[i].Y = 100 - usage.Active;
					sysGraph[i].Y = 100 - usage.System;
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

			int totalMemory = (int)memStats.TotalVisibleMemorySize * 1024;
			double rate = 100.0 / memStats.CommitLimit;
			int border = (int)(totalMemory * rate);

			g.FillRectangle(new SolidBrush(Color.White), 0, 0, 300, 100);
			Pen pen = new Pen(Color.FromArgb(220, 220, 220), 1F);
			for (int y = 100; y > 0; y -= (int)(128*1024*1024 * rate)) 
			{
				g.DrawLine(pen, 0, y, 300, y);
			}

			g.SmoothingMode = SmoothingMode.None;
			Brush availableBrush = new SolidBrush(Color.FromArgb(180, 100, 100, 255));
			Brush kernelBrush = new SolidBrush(Color.FromArgb(180, 255, 0, 0));
			Brush commitedBrush = new SolidBrush(Color.FromArgb(180, 255, 145, 0));
			Brush systemCacheBrush = new SolidBrush(Color.FromArgb(50, 255, 0, 0));
			//			Brush spaceBrush = new SolidBrush(Color.FromArgb(180, 240, 230, 255));
			Brush spaceBrush = new SolidBrush(Color.FromArgb(100, 100, 100, 255));

			int count = memStats.Count;

			for (int i = 0; i < count; i++) 
			{
				MemoryUsage usage = memStats[i];

				int x = 300 - i * 5 - 5;
				int y = 100;
				int w = 5;
				int h = 0;

				int kernelTotal = usage.KernelNonPaged + usage.KernelPaged + usage.DriverTotal + usage.SystemCodeTotal;
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


				x = 300 - i * 5 - 5;
				w = 2;
				h = (int)(usage.Pagein);
				y = 100 - h;
				g.FillRectangle(Brushes.LightGray, x, y, w, h);

				x = 303 - i * 5 - 5;
				w = 2;
				h = (int)(usage.Pageout);
				y = 100 - h;
				g.FillRectangle(Brushes.Black, x, y, w, h);
			}
			Pen borderPen = new Pen(Color.Blue);
			borderPen.DashStyle = DashStyle.Dash;
			g.DrawLine(borderPen, 0, 100-border, 300, 100-border);

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
			this.memoryGraphPicBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.memoryGraphPicBox.Location = new System.Drawing.Point(6, 8);
			this.memoryGraphPicBox.Name = "memoryGraphPicBox";
			this.memoryGraphPicBox.Size = new System.Drawing.Size(302, 102);
			this.memoryGraphPicBox.TabIndex = 0;
			this.memoryGraphPicBox.TabStop = false;
			// 
			// cpuGraphPicBox
			// 
			this.cpuGraphPicBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.cpuGraphPicBox.Location = new System.Drawing.Point(6, 120);
			this.cpuGraphPicBox.Name = "cpuGraphPicBox";
			this.cpuGraphPicBox.Size = new System.Drawing.Size(302, 102);
			this.cpuGraphPicBox.TabIndex = 1;
			this.cpuGraphPicBox.TabStop = false;
			// 
			// StatusWindow
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 12);
			this.ClientSize = new System.Drawing.Size(314, 232);
			this.Controls.Add(this.cpuGraphPicBox);
			this.Controls.Add(this.memoryGraphPicBox);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "StatusWindow";
			this.Text = "ステータス";
			this.ResumeLayout(false);

		}
		#endregion
	}
}
