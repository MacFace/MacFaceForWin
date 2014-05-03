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
        private Label label1;
        private Label lblFree;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label lblCommited;
        private Label lblSysCache;
        private Label lblSystem;
        private Label label2;
        private Label label6;
        private Label label7;
        private Label lblCPUUser;
        private Label lblCPUSystem;
        private Label lblCPUIdle;
        private Label label8;
        private Label lblTotal;
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

            if (cpuStats.Count > 0)
            {
                CPUUsage u = cpuStats[0];
                lblCPUUser.Text = u.User.ToString("##0\\%");
                lblCPUSystem.Text = u.System.ToString("##0\\%");
                lblCPUIdle.Text = u.Idle.ToString("##0\\%");
            }

            g.Dispose();
		}

		private void drawMemoryGraph()
		{
			Graphics g = Graphics.FromImage(memoryGraph);

			int fw = memoryGraph.Width;
			int fh = memoryGraph.Height;

            ulong totalMemory = (ulong)memStats.TotalVisibleMemorySize * 1024;
            double rate;

			MemoryUsage latestUsage = memStats.Latest;
            if (latestUsage.Used + latestUsage.Available > totalMemory * 1.5)
            {
                rate = (double)fh / (totalMemory * 3.0);// (double)memStats.CommitLimit;
            }
            else
            {
                rate = (double)fh / (totalMemory * 1.5);
            }

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

				h = (int)((usage.KernelTotal) * rate);
				y -= h;
				g.FillRectangle(kernelBrush, x, y, w, h);

				h = (int)(usage.SystemCache * rate);
				y -= h;
				g.FillRectangle(systemCacheBrush, x, y, w, h);

                h = (int)((usage.Committed - usage.KernelTotal) * rate);
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

            if (memStats.Count > 0)
            {
                MemoryUsage u = memStats[0];
                lblTotal.Text = (memStats.TotalVisibleMemorySize / (1024.0)).ToString("#####0.0MB");
                lblFree.Text = (u.Available / (1048576.0)).ToString("#####0.0MB");
                lblCommited.Text = (u.Committed / (1048576.0)).ToString("#####0.0MB");
                lblSysCache.Text = (u.SystemCache / (1048576.0)).ToString("#####0.0MB");
                lblSystem.Text = (u.KernelTotal / (1048576.0)).ToString("#####0.0MB");
            }
		}

		#region Windows フォーム デザイナで生成されたコード 
		/// <summary>
		/// デザイナ サポートに必要なメソッドです。このメソッドの内容を
		/// コード エディタで変更しないでください。
		/// </summary>
		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StatusWindow));
            this.memoryGraphPicBox = new System.Windows.Forms.PictureBox();
            this.cpuGraphPicBox = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lblFree = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lblCommited = new System.Windows.Forms.Label();
            this.lblSysCache = new System.Windows.Forms.Label();
            this.lblSystem = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.lblCPUUser = new System.Windows.Forms.Label();
            this.lblCPUSystem = new System.Windows.Forms.Label();
            this.lblCPUIdle = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.lblTotal = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.memoryGraphPicBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cpuGraphPicBox)).BeginInit();
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
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(313, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(28, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "Free";
            // 
            // lblFree
            // 
            this.lblFree.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblFree.Location = new System.Drawing.Point(393, 26);
            this.lblFree.Name = "lblFree";
            this.lblFree.Size = new System.Drawing.Size(62, 12);
            this.lblFree.TabIndex = 3;
            this.lblFree.Text = "00000.0MB";
            this.lblFree.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(313, 44);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 12);
            this.label3.TabIndex = 2;
            this.label3.Text = "Commited";
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(313, 62);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(79, 12);
            this.label4.TabIndex = 2;
            this.label4.Text = "System Cache";
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(313, 80);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(43, 12);
            this.label5.TabIndex = 2;
            this.label5.Text = "System";
            // 
            // lblCommited
            // 
            this.lblCommited.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblCommited.Location = new System.Drawing.Point(393, 44);
            this.lblCommited.Name = "lblCommited";
            this.lblCommited.Size = new System.Drawing.Size(62, 12);
            this.lblCommited.TabIndex = 3;
            this.lblCommited.Text = "00000.0MB";
            this.lblCommited.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblSysCache
            // 
            this.lblSysCache.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblSysCache.Location = new System.Drawing.Point(393, 62);
            this.lblSysCache.Name = "lblSysCache";
            this.lblSysCache.Size = new System.Drawing.Size(62, 12);
            this.lblSysCache.TabIndex = 3;
            this.lblSysCache.Text = "00000.0MB";
            this.lblSysCache.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblSystem
            // 
            this.lblSystem.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblSystem.Location = new System.Drawing.Point(393, 80);
            this.lblSystem.Name = "lblSystem";
            this.lblSystem.Size = new System.Drawing.Size(62, 12);
            this.lblSystem.TabIndex = 3;
            this.lblSystem.Text = "00000.0MB";
            this.lblSystem.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(313, 116);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "User";
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(313, 134);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(43, 12);
            this.label6.TabIndex = 5;
            this.label6.Text = "System";
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(313, 152);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(23, 12);
            this.label7.TabIndex = 6;
            this.label7.Text = "Idle";
            // 
            // lblCPUUser
            // 
            this.lblCPUUser.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblCPUUser.Location = new System.Drawing.Point(393, 116);
            this.lblCPUUser.Name = "lblCPUUser";
            this.lblCPUUser.Size = new System.Drawing.Size(62, 12);
            this.lblCPUUser.TabIndex = 7;
            this.lblCPUUser.Text = "100%";
            this.lblCPUUser.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblCPUSystem
            // 
            this.lblCPUSystem.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblCPUSystem.Location = new System.Drawing.Point(393, 134);
            this.lblCPUSystem.Name = "lblCPUSystem";
            this.lblCPUSystem.Size = new System.Drawing.Size(62, 12);
            this.lblCPUSystem.TabIndex = 8;
            this.lblCPUSystem.Text = "100%";
            this.lblCPUSystem.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblCPUIdle
            // 
            this.lblCPUIdle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblCPUIdle.Location = new System.Drawing.Point(393, 152);
            this.lblCPUIdle.Name = "lblCPUIdle";
            this.lblCPUIdle.Size = new System.Drawing.Size(62, 12);
            this.lblCPUIdle.TabIndex = 9;
            this.lblCPUIdle.Text = "100%";
            this.lblCPUIdle.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label8
            // 
            this.label8.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(313, 8);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(31, 12);
            this.label8.TabIndex = 10;
            this.label8.Text = "Total";
            // 
            // lblTotal
            // 
            this.lblTotal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTotal.Location = new System.Drawing.Point(393, 9);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(62, 12);
            this.lblTotal.TabIndex = 11;
            this.lblTotal.Text = "00000.0MB";
            this.lblTotal.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // StatusWindow
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 12);
            this.ClientSize = new System.Drawing.Size(460, 223);
            this.Controls.Add(this.lblTotal);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.lblCPUIdle);
            this.Controls.Add(this.lblCPUSystem);
            this.Controls.Add(this.lblCPUUser);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblSystem);
            this.Controls.Add(this.lblSysCache);
            this.Controls.Add(this.lblCommited);
            this.Controls.Add(this.lblFree);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cpuGraphPicBox);
            this.Controls.Add(this.memoryGraphPicBox);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(476, 260);
            this.Name = "StatusWindow";
            this.Text = "ステータス";
            this.Load += new System.EventHandler(this.StatusWindow_Load);
            this.SizeChanged += new System.EventHandler(this.StatusWindow_SizeChanged);
            ((System.ComponentModel.ISupportInitialize)(this.memoryGraphPicBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cpuGraphPicBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

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

        private void StatusWindow_Load(object sender, EventArgs e)
        {

        }
	}
}
