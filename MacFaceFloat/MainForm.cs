/*
 * MainForm.cs
 * $Id$
 * 
 * project created on 2004/06/02 at 2:43
 * 
 */

using System;
using System.Windows.Forms;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Diagnostics;
using System.Threading;

namespace MacFace.FloatApp
{
	public class MainForm : Misuzilla.Windows.Forms.AlphaForm
	{
		private System.Windows.Forms.NotifyIcon notifyIcon;
		private System.Windows.Forms.ContextMenu contextMenu;
		private System.Windows.Forms.MenuItem menuItemPatternSelect;
		private System.Windows.Forms.MenuItem menuItemConfigure;
		private System.Windows.Forms.MenuItem menuItemExit;

		private Configuration _config;

		private System.Windows.Forms.Timer _updateTimer;
		private CPUUsageCounter cpuCounter;
		private MemoryUsageCounter memoryCounter;

		private FaceDef _currentFaceDef;
		private float _patternSize;
		private FaceDef.PatternSuite curSuite;
		private int curPattern;
		private int curMarkers;

		// コンストラクタ
		public MainForm()
		{
			InitializeComponent();
			this.TransparentMouseMessage = false;
			this.MoveAtFormDrag = true;

			curSuite   = FaceDef.PatternSuite.Normal;
			curPattern = 0;
			curMarkers = 0;

			cpuCounter = new CPUUsageCounter();
			memoryCounter = new MemoryUsageCounter();

			_updateTimer = new System.Windows.Forms.Timer();
			_updateTimer.Enabled = false;
			_updateTimer.Interval = 1000;
			_updateTimer.Tick += new EventHandler(this.CountProcessorUsage);
			_updateTimer.Stop();
		}

		void InitializeComponent() {
			this.notifyIcon = new System.Windows.Forms.NotifyIcon();
			this.menuItemPatternSelect = new System.Windows.Forms.MenuItem();
			this.menuItemConfigure = new System.Windows.Forms.MenuItem();
			this.menuItemExit = new System.Windows.Forms.MenuItem();
			this.contextMenu = new System.Windows.Forms.ContextMenu();

			//
			// notifyIcon
			//
			this.notifyIcon.Text = "MacFace";
			this.notifyIcon.Icon = this.Icon;
			this.notifyIcon.Visible = true;
			this.notifyIcon.ContextMenu = this.contextMenu;

			// 
			// menuItemPatternSelect
			// 
			this.menuItemPatternSelect.Text = "顔パターンの選択(&S)";
			this.menuItemPatternSelect.Click += new System.EventHandler(this.menuItemPatternSelect_Click);

			// 
			// menuItemConfigure
			// 
			this.menuItemConfigure.Text = "MacFace の設定(&C)...";
			this.menuItemConfigure.Click +=new EventHandler(menuItemConfigure_Click);
			// 
			// menuItemExit
			// 
			this.menuItemExit.Index = 0;
			this.menuItemExit.Shortcut = System.Windows.Forms.Shortcut.CtrlQ;
			this.menuItemExit.Text = "終了(&X)";
			this.menuItemExit.Click += new System.EventHandler(this.menuItemExit_Click);
			// 
			// contextMenu
			// 
			this.contextMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
						this.menuItemPatternSelect, this.menuItemConfigure, new MenuItem("-"), this.menuItemExit});
			// 
			// MainForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 12);
			this.ClientSize = new System.Drawing.Size(120, 101);
			this.ContextMenu = this.contextMenu;
			this.ControlBox = false;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "MainForm";
			this.Opacity = 0.75F;
			this.ShowInTaskbar = false;
			this.Text = "MacFace For Windows";
			this.TopMost = true;
			this.Load += new EventHandler(MainForm_Load);
			this.Closing += new CancelEventHandler(MainForm_Closing);
		}
			

		[STAThread]
		public static void Main(string[] args)
		{
			Application.Run(new MainForm());
		}
		

		/*
		 * 顔パターン定義フォルダ選択。
		 */
		public bool SelectFaceDefine()
		{
			return SelectFaceDefine(Application.StartupPath);
		}

		public bool SelectFaceDefine(string defaultPath)
		{
			while (true) 
			{
				FolderBrowserDialog folderBrowser = new FolderBrowserDialog();
				folderBrowser.SelectedPath = defaultPath;
				folderBrowser.Description = "顔パターンファイルの存在するフォルダを選択してください。";
				if (folderBrowser.ShowDialog() == DialogResult.OK) 
				{
					if (LoadFaceDefine(folderBrowser.SelectedPath)) 
					{
						return true;
					}
				}
				else 
				{
					return false;
				}
			}

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

			// 顔パターン差し替え中は更新を止めておく
			if (_updateTimer != null) _updateTimer.Stop();

			this.FaceDef = newFaceDef;
			RefreshPattern();

			notifyIcon.Text = "MacFace - " + _currentFaceDef.Title;

			// 更新再開
			if (_updateTimer != null) _updateTimer.Start();

			return true;
		}

		public void CountProcessorUsage(object sender, EventArgs e)
		{
			CPUUsage cpuUsage = cpuCounter.CurrentUsage();
			MemoryUsage memUsage = memoryCounter.CurrentUsage();

			int pattern = cpuUsage.Active / 10;

			FaceDef.PatternSuite suite = FaceDef.PatternSuite.Normal;
			if (memUsage.Available < (10 * 1024 *1024)) 
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

			UpdatePattern(suite, pattern, markers);
		}

		public float PatternSize
		{
			get { return _patternSize; }
			set { _patternSize = value; }
		}

		public FaceDef FaceDef
		{
			get { return _currentFaceDef; }
			set { _currentFaceDef = value; }
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
			_currentFaceDef.DrawPatternImage(g, curSuite, curPattern, curMarkers, _patternSize);
			this.Update();
		}

		//
		// 起動
		//
		public void MainForm_Load(object sender, System.EventArgs e)
		{
			// 設定
			_config = Configuration.GetInstance();
			_config.Load();


			// 顔パターン読み込み
			bool result = false;
			if (Directory.Exists(_config.FaceDefPath))
			{
				result = LoadFaceDefine(_config.FaceDefPath);
				ApplyConfiguration();
				_updateTimer.Start();
			}

			if (!result)
			{
				if (!SelectFaceDefine(Application.StartupPath))
				{
					Application.Exit();
					return;
				}

			}
		}

		// 
		// 終了
		//
		private void MainForm_Closing(object sender, CancelEventArgs e)
		{
			notifyIcon.Visible = false;

			// 保存
			_config.Opacity = (int) (this.Opacity * 100);
			_config.FaceDefPath = (_currentFaceDef != null ? _currentFaceDef.Path : Path.Combine(Application.StartupPath, "default.mcface"));
			_config.Location = this.Location;
			_config.TransparentMouseMessage = this.TransparentMouseMessage;

			_config.Save();
		}


		/*
		 * メニュークリックイベント
		 */
		public void menuItemPatternSelect_Click(object sender, System.EventArgs e)
		{
			SelectFaceDefine(FaceDef.Path);	
		}

		public void menuItemExit_Click(object sender, System.EventArgs e)
		{
			_updateTimer.Stop();
			this.Close();
		}

		private void menuItemConfigure_Click(object sender, EventArgs e)
		{
			ConfigurationForm configForm = new ConfigurationForm(this);
			if (configForm.ShowDialog() == DialogResult.OK) 
			{
				ApplyConfiguration();
			}
		}

		private void ApplyConfiguration()
		{
			this.Opacity = (float)_config.Opacity / 100;
			this.PatternSize = (float)_config.PatternSize / 100;
			this.Location = _config.Location;
			this.TransparentMouseMessage = _config.TransparentMouseMessage;

			RefreshPattern();
		}
	}
}
