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

namespace MacFace
{
	public class MainForm : Misuzilla.Windows.Forms.AlphaForm
	{
		private System.Windows.Forms.ContextMenu contextMenu;
		private System.Windows.Forms.MenuItem menuItemPatternSelect;
		private System.Windows.Forms.MenuItem menuItemExit;
		private Hashtable _property;
		private ArrayList _parts;
		private String _facePath;
		private System.Windows.Forms.Timer _updateTimer;
		
		Int32 prevUsage;
		PerformanceCounter cpuCount;
//		PerformanceCounter pageoutCount;
//		PerformanceCounter pageinCount;

		// コンストラクタ
		public MainForm()
		{
			InitializeComponent();
			this.TransparentMouseMessage = false;
			this.MoveAtFormDrag = true;

			prevUsage = -10;

			cpuCount = new PerformanceCounter();
			cpuCount.CategoryName = "Processor";
			cpuCount.CounterName  = "% Processor Time";
			cpuCount.InstanceName = "_Total";

//			pageoutCount = new PerformanceCounter();
//			pageoutCount.CategoryName = "Memory";
//			pageoutCount.CounterName  = "Pages Output/sec";
//
//			pageinCount = new PerformanceCounter();
//			pageinCount.CategoryName = "Memory";
//			pageinCount.CounterName  = "Pages Input/sec";

			_updateTimer = new System.Windows.Forms.Timer();
			_updateTimer.Enabled = false;
			_updateTimer.Interval = 1000;
			_updateTimer.Tick += new EventHandler(this.CountProcessorUsage);
		}

		// THIS METHOD IS MAINTAINED BY THE FORM DESIGNER
		// DO NOT EDIT IT MANUALLY! YOUR CHANGES ARE LIKELY TO BE LOST
		void InitializeComponent() {
			this.menuItemPatternSelect = new System.Windows.Forms.MenuItem();
			this.menuItemExit = new System.Windows.Forms.MenuItem();
			this.contextMenu = new System.Windows.Forms.ContextMenu();
			// 
			// menuItemPatternSelect
			// 
			this.menuItemPatternSelect.Index = 0;
			this.menuItemPatternSelect.Text = "顔パターンの選択(&S)";
			this.menuItemPatternSelect.Click += new System.EventHandler(this.PatternSelect_Click);

			// 
			// menuItemExit
			// 
			this.menuItemExit.Index = 0;
			this.menuItemExit.Shortcut = System.Windows.Forms.Shortcut.CtrlQ;
			this.menuItemExit.Text = "終了(&X)";
			this.menuItemExit.Click += new System.EventHandler(this.doQuit);
			// 
			// contextMenu
			// 
			this.contextMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
						this.menuItemPatternSelect, new MenuItem("-"), this.menuItemExit});
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


		bool LoadFaceDefine(string path)
		{
			string plistPath = Path.Combine(path, "faceDef.plist");

			if (!File.Exists(plistPath))
			{
				System.Windows.Forms.MessageBox.Show(
					String.Format("指定されたフォルダに顔パターン定義XMLファイル \"faceDef.plist\" が存在しません。\n\nフォルダ:\n{0}", path),
					"MacFace for Windows", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return false;
			}
			
			Hashtable property;
			try 
			{
				property = PropertyList.Load(plistPath);
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

			_property = property;
			_facePath = path;
			_parts = (ArrayList)_property["parts"];
			prevUsage = -10;

			// 更新再開
			if (_updateTimer != null) _updateTimer.Start();

			return true;
		}

		public void CountProcessorUsage(object sender, EventArgs e)
		{
			Int32 usage = (Int32)cpuCount.NextValue();
//			Int32 pagein = (Int32)pageinCount.NextValue();
//			Int32 pageout = (Int32)pageoutCount.NextValue();

//				Console.WriteLine("Processor: {0}% (pattern: {1}) {2} {3}", usage, usage/10, pagein, pageout);
			if (usage >= 100) {
				usage = 100;
			} else if (usage < 0) {
				usage = 0;
			}
				
			if (prevUsage/10 != usage/10) {
				ArrayList patterns = (ArrayList)_property["pattern"];
				ArrayList patternCpu = (ArrayList)patterns[0];
				ArrayList facePattern = (ArrayList)patternCpu[usage/10];
				
				this.Graphics.Clear(Color.FromArgb(0, 0, 0, 0));
				foreach (Int32 i in facePattern) {
					Hashtable part = _parts[i] as Hashtable;
					string filename = (string)part["filename"];
					string imgPath = Path.Combine(_facePath, filename);
					using (Bitmap bitmap = new Bitmap(imgPath)) {
						int x = (int)part["pos x"];
						int y = 128 - (int)part["pos y"] - bitmap.Size.Height;
						this.Graphics.DrawImage(bitmap,x,y,bitmap.Size.Width,bitmap.Size.Height);
					}
				}
				this.Update();
			}
				
			prevUsage = usage;			
		}
		

		public void MainForm_Load(object sender, System.EventArgs e)
		{
			string faceDefPath = Path.Combine(Application.StartupPath, "default.mcface");
			bool result = false;

			if (Directory.Exists(faceDefPath))
			{
				result = LoadFaceDefine(faceDefPath);
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

		/*
		 * メニュークリックイベント
		 */
		public void PatternSelect_Click(object sender, System.EventArgs e)
		{
			SelectFaceDefine(_facePath);	
		}

		public void doQuit(object sender, System.EventArgs e)
		{
			_updateTimer.Stop();
			Application.Exit();
		}
		
	}
}
