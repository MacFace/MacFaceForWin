/*
 * MacFace アプリケーションクラス
 *
 * $Id$
 * 
 * project created on 2004/06/02 at 2:43
 * 
 */
using System;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Data;
using System.IO;
using System.Reflection;

namespace MacFace.FloatApp
{
	/// <summary>
	/// MacFaceApp の概要の説明です。
	/// </summary>
	public class MacFaceApp : ApplicationContext
	{

		private Configuration config;

		private NotifyIcon notifyIcon;
		private PatternWindow patternWindow;
		
		private System.Windows.Forms.Timer updateTimer;
		private CPUUsageCounter cpuCounter;
		private MemoryUsageCounter memoryCounter;

		private StatusWindow statusWindow;
		private Bitmap cpuGraph;
		private Bitmap memoryGraph;

		private CPUUsage[] cpuHistory;
		private int cpuHistoryHead;
		private int cpuHistoryCount;

		private MemoryUsage[] memHistory;
		private int memHistoryHead;
		private int memHistoryCount;

		[STAThread]
		public static void Main(string[] args)
		{
			MacFaceApp app = new MacFaceApp();
			app.StartApplication();
		}

		public MacFaceApp()
		{
			config = Configuration.GetInstance();
			config.Load();

			cpuHistory = new CPUUsage[61];
			cpuHistoryCount = 0;
			cpuHistoryHead = 0;

			memHistory = new MemoryUsage[61];
			memHistoryCount = 0;
			memHistoryHead = 0;

			cpuCounter = new CPUUsageCounter();
			memoryCounter = new MemoryUsageCounter();

			updateTimer = new System.Windows.Forms.Timer();
			updateTimer.Enabled = false;
			updateTimer.Interval = 1000;
			updateTimer.Tick += new EventHandler(this.CountProcessorUsage);

			patternWindow = null;

			cpuGraph = null;
			memoryGraph = null;
			statusWindow = null;

			InitializeComponent();
		}

		void InitializeComponent() 
		{
			// コンテキストメニュー
			ContextMenu contextMenu = new System.Windows.Forms.ContextMenu();
			MenuItem menuItemShowPatternWindow = new System.Windows.Forms.MenuItem();
			MenuItem menuItemShowStatusWindow = new System.Windows.Forms.MenuItem();
			MenuItem menuItemConfigure = new System.Windows.Forms.MenuItem();
			MenuItem menuItemExit = new System.Windows.Forms.MenuItem();

			menuItemShowPatternWindow.Text = "パターンウインドウを開く(&P)";
			menuItemShowPatternWindow.Click +=new EventHandler(menuItemShowPatternWindow_Click);

			menuItemShowStatusWindow.Text = "ステータスウインドウを開く(&S)";
			menuItemShowStatusWindow.Click +=new EventHandler(menuItemShowStatusWindow_Click);

			menuItemConfigure.Text = "MacFace の設定(&C)...";
			menuItemConfigure.Click +=new EventHandler(menuItemConfigure_Click);

			menuItemExit.Index = 0;
			menuItemExit.Text = "終了(&X)";
			menuItemExit.Click += new System.EventHandler(menuItemExit_Click);

			contextMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
					menuItemShowPatternWindow, menuItemShowStatusWindow, new MenuItem("-"), menuItemConfigure, new MenuItem("-"), menuItemExit});

			// 通知アイコン
			Assembly asm = Assembly.GetExecutingAssembly();
			this.notifyIcon = new System.Windows.Forms.NotifyIcon();
			this.notifyIcon.Text = "MacFace";
			this.notifyIcon.Icon = new Icon(asm.GetManifestResourceStream("MacFace.FloatApp.App.ico"));
			this.notifyIcon.Visible = true;
			this.notifyIcon.ContextMenu = contextMenu;
		}

		public void StartApplication()
		{
			// 顔パターン読み込み
			bool result = false;
			if (Directory.Exists(config.FaceDefPath))
			{
				result = LoadFaceDefine(config.FaceDefPath);
			}

			if (!result)
			{
				string path = Path.Combine(Application.StartupPath, "default.plist");

				if (!LoadFaceDefine(path))
				{
					Application.Exit();
					return;
				}
			}

			openPatternWindow();
			openStatusWindow();
			updateTimer.Start();

			Application.ApplicationExit += new EventHandler(Application_ApplicationExit);
			Application.Run(this);
		}

		void Application_ApplicationExit(object sender, EventArgs e)
		{
			notifyIcon.Visible = false;

			config.Save();
		}

		public bool LoadFaceDefine(string path)
		{
			FaceDef newFaceDef = null;
			string plistPath = Path.Combine(path, "faceDef.plist");

			if (!File.Exists(plistPath))
			{
				System.Windows.Forms.MessageBox.Show(
					String.Format("指定されたフォルダに顔パターン定義XMLファイル \"faceDef.plist\" が存在しません。\n\nフォルダ:\n{0}", path),
					"MacFace for Windows", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return false;
			}
			
			try 
			{
				newFaceDef = new MacFace.FaceDef(path);
			} 
			catch (System.IO.IOException ie) 
			{
				System.Windows.Forms.MessageBox.Show(
					String.Format("顔パターン定義XMLファイルを読み込む際にエラーが発生しました。\n\n原因:\n{0}",
					ie.ToString()), "MacFace for Windows", MessageBoxButtons.OK, MessageBoxIcon.Error);
				
				return false;
			}
			catch (System.Xml.XmlException xe) 
			{
				System.Windows.Forms.MessageBox.Show(
					String.Format("顔パターン定義XMLファイルを読込み中にエラーが発生しました。\n\n原因:\n{0}",
					xe.ToString()), "MacFace for Windows", MessageBoxButtons.OK, MessageBoxIcon.Error);
				
				return false;
			}

			if (patternWindow != null) 
			{
				// 顔パターン差し替え中は更新を止めておく
				if (updateTimer != null) updateTimer.Stop();

				patternWindow.FaceDef = newFaceDef;
				patternWindow.Refresh();

				notifyIcon.Text = "MacFace - " + patternWindow.FaceDef.Title;

				// 更新再開
				if (updateTimer != null) updateTimer.Start();
			}

			return true;
		}

		public void CountProcessorUsage(object sender, EventArgs e)
		{
			CPUUsage cpuUsage = cpuCounter.CurrentUsage();
			MemoryUsage memUsage = memoryCounter.CurrentUsage();

			cpuHistory[cpuHistoryHead++] = cpuUsage;
			if (cpuHistoryHead >= cpuHistory.Length) cpuHistoryHead = 0;
			if (cpuHistoryCount < cpuHistory.Length) cpuHistoryCount++;

			memHistory[memHistoryHead++] = memUsage;
			if (memHistoryHead >= memHistory.Length) memHistoryHead = 0;
			if (memHistoryCount < memHistory.Length) memHistoryCount++;

			if (patternWindow != null) 
			{
				FaceDef.PatternSuite suite = FaceDef.PatternSuite.Normal;

				int pattern = cpuUsage.Active / 10;
				int avilable = (int)MemoryUsageCounter.TotalVisibleMemorySize * 1024 - memUsage.Committed;
				if (avilable < (10 * 1024 *1024)) 
				{
					suite = FaceDef.PatternSuite.MemoryInsufficient;
				} 
				else if (memUsage.Available < (30 * 1024 *1024)) 
				{
					suite = FaceDef.PatternSuite.MemoryDecline;
				}

				int markers = FaceDef.MarkerNone;
				if (memUsage.Pagein > 0) markers += FaceDef.MarkerPageIn;
				if (memUsage.Pageout > 0) markers += FaceDef.MarkerPageOut;

				patternWindow.UpdatePattern(suite, pattern, markers);
			}

			if (statusWindow != null) 
			{
				drawCPUGraph();
				drawMemoryGraph();
				statusWindow.cpuGraphPicBox.Invalidate();
				statusWindow.memoryGraphPicBox.Invalidate();
			}
		}

		public void openPatternWindow()
		{
			// パターンウインドウ
			patternWindow = new PatternWindow();
			patternWindow.Closed += new EventHandler(patternWindow_Closed);
			patternWindow.Move +=new EventHandler(patternWindow_Move);

			patternWindow.Location = config.Location;
			patternWindow.Opacity = (float)config.Opacity / 100;
			patternWindow.PatternSize = (float)config.PatternSize / 100;
			patternWindow.TransparentMouseMessage = config.TransparentMouseMessage;

			LoadFaceDefine(config.FaceDefPath);

			patternWindow.Show();
		}

		public void openStatusWindow()
		{
			statusWindow = new StatusWindow();
			statusWindow.Closed += new EventHandler(statusWindow_Closed);
			statusWindow.Move +=new EventHandler(statusWindow_Move);

			FormBorderStyle orgStyle = statusWindow.FormBorderStyle;
			statusWindow.FormBorderStyle = FormBorderStyle.Sizable;
			statusWindow.StartPosition = FormStartPosition.Manual;
			statusWindow.Location = config.StatusWindowLocation;
			statusWindow.FormBorderStyle = orgStyle;

			cpuGraph = new Bitmap(5*60, 100);
			memoryGraph = new Bitmap(5*60, 100);
			statusWindow.cpuGraphPicBox.Image = cpuGraph;
			statusWindow.memoryGraphPicBox.Image = memoryGraph;
			drawCPUGraph();
			drawMemoryGraph();

			statusWindow.Show();
		}

		/*
		 * メニュークリックイベント
		 */

		public void menuItemExit_Click(object sender, System.EventArgs e)
		{
			updateTimer.Stop();

			if (patternWindow != null)
			{
				patternWindow.Close();
			}

			if (statusWindow != null) 
			{
				statusWindow.Close();
			}

			ExitThread();
		}

		private void menuItemConfigure_Click(object sender, EventArgs e)
		{
			ConfigurationForm configForm = new ConfigurationForm();
			configForm.ConfigChanged += new ConfigChangedEvent(configForm_ConfigChanged);
			configForm.Show();
		}

		private void configForm_ConfigChanged()
		{
			if (patternWindow.FaceDef.Path != config.FaceDefPath) 
			{
				bool result = LoadFaceDefine(config.FaceDefPath);
				// パターン変更に失敗したら設定を元に戻す
				if (!result) 
				{
					config.FaceDefPath = patternWindow.FaceDef.Path;
				}
			}

			if (patternWindow != null) 
			{
				patternWindow.Opacity = (float)config.Opacity / 100;
				patternWindow.PatternSize = (float)config.PatternSize / 100;
				patternWindow.TransparentMouseMessage = config.TransparentMouseMessage;
				patternWindow.Refresh();
			}
		}

		private void statusWindow_Paint(object sender, PaintEventArgs e)
		{
			Graphics g = e.Graphics;

			g.DrawImage(cpuGraph, 5,5);
			g.DrawRectangle(Pens.Black, 4, 4, 301, 101);

			g.DrawImage(memoryGraph, 5,110);
			g.DrawRectangle(Pens.Black, 4, 109, 301, 101);
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

			if (cpuHistoryCount >= 2) 
			{
				Point[] userGraph = new Point[cpuHistoryCount+2];
				Point[] sysGraph = new Point[cpuHistoryCount+2];

				userGraph[cpuHistoryCount+0].X = 300 - (cpuHistoryCount-1) * 5;
				userGraph[cpuHistoryCount+0].Y = 100 - 0;
				userGraph[cpuHistoryCount+1].X = 300 - 0 * 5;
				userGraph[cpuHistoryCount+1].Y = 100 - 0;

				sysGraph[cpuHistoryCount+0].X = 300 - (cpuHistoryCount-1) * 5;
				sysGraph[cpuHistoryCount+0].Y = 100 - 0;
				sysGraph[cpuHistoryCount+1].X = 300 - 0 * 5;
				sysGraph[cpuHistoryCount+1].Y = 100 - 0;

				int pos = cpuHistoryHead - 1;
				for (int i = 0; i < cpuHistoryCount; i++) 
				{
					if (pos < 0) pos = cpuHistory.Length - 1;
					CPUUsage usage = cpuHistory[pos];
					userGraph[i].X = sysGraph[i].X = 300 - i * 5;
					userGraph[i].Y = 100 - usage.Active;
					sysGraph[i].Y = 100 - usage.System;
					pos--;
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

			int totalMemory = (int)MemoryUsageCounter.TotalVisibleMemorySize * 1024;
			double rate = 100.0 / MemoryUsageCounter.CommitLimit;
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

			int pos = memHistoryHead - 1;
			for (int i = 0; i < memHistoryCount; i++) 
			{
				if (pos < 0) pos = memHistory.Length - 1;
				MemoryUsage usage = memHistory[pos];

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

				pos--;
			}
			Pen borderPen = new Pen(Color.Blue);
			borderPen.DashStyle = DashStyle.Dash;
			g.DrawLine(borderPen, 0, 100-border, 300, 100-border);

			g.Dispose();
		}

		private void patternWindow_Closed(object sender, EventArgs e)
		{
			patternWindow.Dispose();
			patternWindow = null;
		}

		private void statusWindow_Closed(object sender, EventArgs e)
		{
			cpuGraph.Dispose();
			cpuGraph = null;
			memoryGraph.Dispose();
			memoryGraph = null;
			statusWindow.Dispose();
			statusWindow = null;
		}

		private void menuItemShowPatternWindow_Click(object sender, EventArgs e)
		{
			if (patternWindow == null) 
			{
				openPatternWindow();
			}
		}

		private void menuItemShowStatusWindow_Click(object sender, EventArgs e)
		{
			if (statusWindow == null) 
			{
				openStatusWindow();
			}
		}

		private void patternWindow_Move(object sender, EventArgs e)
		{
			config.Location = patternWindow.Location;
		}

		private void statusWindow_Move(object sender, EventArgs e)
		{
			config.StatusWindowLocation = statusWindow.Location;
		}
	}
}
