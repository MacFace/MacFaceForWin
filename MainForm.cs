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
	{
		private Hashtable _property;
		private ArrayList _parts;
		private String _facePath;
		
		public MainForm()
		{
			InitializeComponent();
//			this.Load += new EventHandler(this.Form1_Load);
			_facePath = Path.Combine(Application.StartupPath,"default.mcface");
			_property = PropertyList.load(Path.Combine(_facePath,"faceDef.plist"));
			_parts = (ArrayList)_property["parts"];

			(new Thread(new ThreadStart(CountProcessorUsage))).Start();
		}
	
		// THIS METHOD IS MAINTAINED BY THE FORM DESIGNER
		// DO NOT EDIT IT MANUALLY! YOUR CHANGES ARE LIKELY TO BE LOST
		void InitializeComponent()
		{
			// 
			//  Set up generated class MainForm
			// 
			this.SuspendLayout();
			this.Name = "MainForm";
			this.Text = "This is my form";
			this.Size = new System.Drawing.Size(300, 300);
			this.ResumeLayout(false);
			this.ShowInTaskbar = false;
			this.TopMost = true;
			this.FormBorderStyle = FormBorderStyle.None;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.ControlBox = false;
			this.TransparentMouseMessage = false;
			this.MoveAtFormDrag = true;
			this.Visible = true;
			this.Opacity = 0.75F;
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
				
				// 前のパターンと同じなら更新しない。
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
	}
}
