// project created on 2004/06/02 at 2:43
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
	class MainForm : Misuzilla.Windows.Forms.AlphaForm
//	class MainForm : System.Windows.Forms.Form
	{
		private System.Windows.Forms.ContextMenu contextMenu1;
		private System.Windows.Forms.MenuItem menuItem1;
		private Hashtable _property;
		private ArrayList _parts;
		private String _facePath;
		
		public MainForm()
		{
			InitializeComponent();
			this.TransparentMouseMessage = false;
			this.MoveAtFormDrag = true;

			_facePath = Path.Combine(Application.StartupPath,"default.mcface");
			_property = PropertyList.load(Path.Combine(_facePath,"faceDef.plist"));
			_parts = (ArrayList)_property["parts"];

			(new Thread(new ThreadStart(CountProcessorUsage))).Start();
		}

		// THIS METHOD IS MAINTAINED BY THE FORM DESIGNER
		// DO NOT EDIT IT MANUALLY! YOUR CHANGES ARE LIKELY TO BE LOST
		void InitializeComponent() {
			this.menuItem1 = new System.Windows.Forms.MenuItem();
			this.contextMenu1 = new System.Windows.Forms.ContextMenu();
			// 
			// menuItem1
			// 
			this.menuItem1.Index = 0;
			this.menuItem1.Shortcut = System.Windows.Forms.Shortcut.CtrlQ;
			this.menuItem1.Text = "終了";
			this.menuItem1.Click += new System.EventHandler(this.doQuit);
			// 
			// contextMenu1
			// 
			this.contextMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
						this.menuItem1});
			// 
			// MainForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 12);
			this.ClientSize = new System.Drawing.Size(120, 101);
			this.ContextMenu = this.contextMenu1;
			this.ControlBox = false;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "MainForm";
			this.Opacity = 0.75F;
			this.ShowInTaskbar = false;
			this.Text = "MacFace For Windows";
			this.TopMost = true;
		}
			
		[STAThread]
		public static void Main(string[] args)
		{
			Application.Run(new MainForm());
		}
		
		private void CountProcessorUsage()
		{
			PerformanceCounter perfCount = new PerformanceCounter();
			perfCount.CategoryName = "Processor";
			perfCount.CounterName  = "% Processor Time";
			perfCount.InstanceName = "_Total";

			PerformanceCounter pageoutCount = new PerformanceCounter();
			pageoutCount.CategoryName = "Memory";
			pageoutCount.CounterName  = "Pages Output/sec";

			PerformanceCounter pageinCount = new PerformanceCounter();
			pageinCount.CategoryName = "Memory";
			pageinCount.CounterName  = "Pages Input/sec";

			Int32 prevUsage = -10;
			while (true) {
				Int32 usage = (Int32)perfCount.NextValue();
				Int32 pagein = (Int32)pageinCount.NextValue();
				Int32 pageout = (Int32)pageoutCount.NextValue();

				Console.WriteLine("Processor: {0}% (pattern: {1}) {2} {3}", usage, usage/10, pagein, pageout);
				if (usage >= 100) {
					usage = 100;
				} else if (usage < 0) {
					usage = 0;
				}
				
				// Õp^[ƓȂXVȂB
				if (prevUsage/10 != usage/10) {
					ArrayList patterns = (ArrayList)_property["pattern"];
					ArrayList patternCpu = (ArrayList)patterns[0];
					ArrayList facePattern = (ArrayList)patternCpu[usage/10];
					
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
				Thread.Sleep(1000);
			}
			
		}
		void doQuit(object sender, System.EventArgs e)
		{
			Application.Exit();
		}
		
	}
}
