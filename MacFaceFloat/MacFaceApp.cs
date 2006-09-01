/*
 * MacFace �A�v���P�[�V�����N���X
 *
 * $Id$
 * 
 * project created on 2004/06/02 at 2:43
 * 
 */
using System;
using System.Windows.Forms;
using System.Drawing;
using System.Data;
using System.IO;
using System.Reflection;

namespace MacFace.FloatApp
{
	/// <summary>
	/// MacFaceApp �̊T�v�̐����ł��B
	/// </summary>
	public class MacFaceApp : ApplicationContext
	{
		private const string MES_OPEN_PATTERN_WINDOW = "�p�^�[���E�C���h�E���J��(&P)";
		private const string MES_CLOSE_PATTERN_WINDOW = "�p�^�[���E�C���h�E�����(&P)";
		private const string MES_OPEN_STATUS_WINDOW = "�X�e�[�^�X�E�C���h�E���J��(&S)";
		private const string MES_CLOSE_STATUS_WINDOW = "�X�e�[�^�X�E�C���h�E�����(&S)";

		private Configuration config;

		private CPUStatistics cpuStats;
		private MemoryStatistics memStats;
		private int pageio_count;

		private System.Windows.Forms.Timer updateTimer;

		private NotifyIcon notifyIcon;
		private PatternWindow patternWindow;
		private StatusWindow statusWindow;
		private MenuItem menuItemTogglePatternWindow;
		private MenuItem menuItemToggleStatusWindow;
		private MacFace.FaceDef curFaceDef;

		[STAThread]
		public static void Main(string[] args)
		{
			AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
			MacFaceApp app = new MacFaceApp();
			app.StartApplication();
		}

		/// <summary>
		/// Windows 2000 �̓p�t�H�[�}���X���j�^���_���Ȃ炻�ꂼ��API�ł���΂�
		/// </summary>
		public void SetupStatisticsForWindows2000()
		{
//			cpuStats = new CPUStatistics(81);
//			memStats = new MemoryStatistics(81);
//
//			try 
//			{
//				// �����ɃJ�E���^�����s���Ă݂�
//				cpuStats.Update();
//				memStats.Update();
//			} 
//			catch (System.ComponentModel.Win32Exception) 
//			{
//				// �_���������̂Ńp�t�H�[�}���X�J�E���^���g��Ȃ����@��
//				cpuStats = new CPUStatisticsNtQuerySystemInformation(61);
//				memStats = new MemoryStatisticsGlobalMemoryStatusEx(61);
//			}
//			catch (System.InvalidOperationException) 
//			{
//				// �_���������̂Ńp�t�H�[�}���X�J�E���^���g��Ȃ����@��
//				cpuStats = new CPUStatisticsNtQuerySystemInformation(61);
//				memStats = new MemoryStatisticsGlobalMemoryStatusEx(61);
//			}

			cpuStats = new CPUStatisticsNtQuerySystemInformation(61);
			memStats = new MemoryStatisticsNtQuerySystemInformation(61);
		}

		public void SetupStatisticsForWindowsXP()
		{
//			cpuStats = new CPUStatistics(81);
//			memStats = new MemoryStatistics(81);
//			try 
//			{
//				// �����ɃJ�E���^�����s���Ă݂�
//				cpuStats.Update();
//				memStats.Update();
//			} 
//			catch (System.ComponentModel.Win32Exception) 
//			{
//				// �_���������̂Ńp�t�H�[�}���X�J�E���^���g��Ȃ����@��
//				cpuStats = new CPUStatisticsGetSystemTime(61);
//				memStats = new MemoryStatisticsNtQuerySystemInformation(61);
//			}
//			catch (System.InvalidOperationException) 
//			{
//				// �_���������̂Ńp�t�H�[�}���X�J�E���^���g��Ȃ����@��
//				cpuStats = new CPUStatisticsGetSystemTime(61);
//				memStats = new MemoryStatisticsPSAPI(61);
//			}

			cpuStats = new CPUStatisticsGetSystemTime(61);
			memStats = new MemoryStatisticsNtQuerySystemInformation(61);
		}

		public MacFaceApp()
		{
			config = Configuration.GetInstance();
			config.Load();
			
			pageio_count = 0;

			// OS ���ƂɎ擾������@��ύX����
			if (Environment.OSVersion.Version.Major == 5 && Environment.OSVersion.Version.Minor == 0)
			{
				SetupStatisticsForWindows2000();
			}
			else
			{
				// XP / 2003 / Vista
				SetupStatisticsForWindowsXP();
			}

			updateTimer = new System.Windows.Forms.Timer();
			updateTimer.Enabled = false;
			updateTimer.Interval = 1000;
			updateTimer.Tick += new EventHandler(this.CountProcessorUsage);

			patternWindow = null;
			statusWindow = null;

			InitializeComponent();
		}

		void InitializeComponent() 
		{
			// �R���e�L�X�g���j���[
			ContextMenu contextMenu = new System.Windows.Forms.ContextMenu();
			menuItemTogglePatternWindow = new System.Windows.Forms.MenuItem();
			menuItemToggleStatusWindow = new System.Windows.Forms.MenuItem();
			MenuItem menuItemConfigure = new System.Windows.Forms.MenuItem();
			MenuItem menuItemExit = new System.Windows.Forms.MenuItem();
			MenuItem menuVersionInfo = new System.Windows.Forms.MenuItem();

			menuItemTogglePatternWindow.Text = MES_OPEN_PATTERN_WINDOW;
			menuItemTogglePatternWindow.Click +=new EventHandler(menuItemTogglePatternWindow_Click);

			menuItemToggleStatusWindow.Text = MES_OPEN_STATUS_WINDOW;
			menuItemToggleStatusWindow.Click +=new EventHandler(menuItemToggleStatusWindow_Click);

			menuItemConfigure.Text = "MacFace �̐ݒ�(&O)...";
			menuItemConfigure.Click +=new EventHandler(menuItemConfigure_Click);

			menuVersionInfo.Index = 0;
			menuVersionInfo.Text = "�o�[�W�������(&A)";
			menuVersionInfo.Click +=new EventHandler(menuVersionInfo_Click);

			menuItemExit.Index = 0;
			menuItemExit.Text = "�I��(&X)";
			menuItemExit.Click += new System.EventHandler(menuItemExit_Click);

			contextMenu.MenuItems.AddRange(new MenuItem[] {
					menuItemTogglePatternWindow,
					menuItemToggleStatusWindow,
					new MenuItem("-"),
					menuItemConfigure,
					menuVersionInfo,
					new MenuItem("-"),
					menuItemExit}
				);

			// �ʒm�A�C�R��
			Assembly asm = Assembly.GetExecutingAssembly();
			this.notifyIcon = new System.Windows.Forms.NotifyIcon();
			this.notifyIcon.Text = "MacFace";
			this.notifyIcon.Icon = new Icon(asm.GetManifestResourceStream("MacFace.FloatApp.App.ico"));
			this.notifyIcon.Visible = true;
			this.notifyIcon.ContextMenu = contextMenu;
		}

		public void StartApplication()
		{
			// ��p�^�[���ǂݍ���
			bool result = false;
			if (Directory.Exists(config.FaceDefPath))
			{
				result = LoadFaceDefine(config.FaceDefPath);
			}

			if (!result)
			{
				string path = Path.Combine(Application.StartupPath, "default.mcface");

				if (!LoadFaceDefine(path))
				{
					Application.Exit();
					return;
				}
			}

			if (config.ShowPatternWindow) 
			{
				openPatternWindow();
			}

			if (config.ShowStatusWindow) 
			{
				openStatusWindow();
			}

			updateTimer.Start();

			Microsoft.Win32.SystemEvents.SessionEnding += new Microsoft.Win32.SessionEndingEventHandler(SystemEvents_SessionEnding);
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
					String.Format("�w�肳�ꂽ�t�H���_�Ɋ�p�^�[����`XML�t�@�C�� \"faceDef.plist\" �����݂��܂���B\n\n�t�H���_:\n{0}", path),
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
					String.Format("��p�^�[����`XML�t�@�C����ǂݍ��ލۂɃG���[���������܂����B\n\n����:\n{0}",
					ie.ToString()), "MacFace for Windows", MessageBoxButtons.OK, MessageBoxIcon.Error);
				
				return false;
			}
			catch (System.Xml.XmlException xe) 
			{
				System.Windows.Forms.MessageBox.Show(
					String.Format("��p�^�[����`XML�t�@�C����Ǎ��ݒ��ɃG���[���������܂����B\n\n����:\n{0}",
					xe.ToString()), "MacFace for Windows", MessageBoxButtons.OK, MessageBoxIcon.Error);
				
				return false;
			}
			
			curFaceDef = newFaceDef;

			if (patternWindow != null) 
			{
				// ��p�^�[�������ւ����͍X�V���~�߂Ă���
				if (updateTimer != null) updateTimer.Stop();

				patternWindow.FaceDef = newFaceDef;
				patternWindow.Refresh();

				notifyIcon.Text = "MacFace - " + patternWindow.FaceDef.Title;

				// �X�V�ĊJ
				if (updateTimer != null) updateTimer.Start();
			}

			return true;
		}

		public void CountProcessorUsage(object sender, EventArgs e)
		{
			cpuStats.Update();
			CPUUsage cpuUsage = cpuStats.Latest;

			memStats.Update();
			MemoryUsage memUsage = memStats.Latest;

			pageio_count += memUsage.Pageout;
			if (pageio_count > 0) pageio_count += memUsage.Pagein;
			pageio_count--;
			if (pageio_count < 0) pageio_count = 0;

			if (patternWindow != null) 
			{
				int pattern = cpuUsage.Active / 10;
				pattern += memUsage.Pageout / 15;
				pattern += memUsage.Pagein / 30;
				if (pattern > 10) pattern = 10;

				FaceDef.PatternSuite suite = FaceDef.PatternSuite.Normal;

				int avilable = (int)memStats.TotalVisibleMemorySize * 1024 - memUsage.Used;
				/*if (pageio_count > 100) 
				{
					suite = FaceDef.PatternSuite.MemoryInsufficient;
				}
				else */if (avilable < 0) 
				{
					suite = FaceDef.PatternSuite.MemoryInsufficient;
				} 
				else if (avilable < (10 * 1024 *1024)) 
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
				statusWindow.UpdateGraph();
			}
		}

		public void openPatternWindow()
		{
			// �p�^�[���E�C���h�E
			patternWindow = new PatternWindow();
			patternWindow.Closed += new EventHandler(patternWindow_Closed);
			patternWindow.Move +=new EventHandler(patternWindow_Move);

			patternWindow.Location = config.Location;
			patternWindow.Opacity = (float)config.Opacity / 100;
			patternWindow.PatternSize = (float)config.PatternSize / 100;
			patternWindow.TransparentMouseMessage = config.TransparentMouseMessage;

			//LoadFaceDefine(config.FaceDefPath);
			patternWindow.FaceDef = curFaceDef;

			patternWindow.Show();

			menuItemTogglePatternWindow.Text = MES_CLOSE_PATTERN_WINDOW;
			config.ShowPatternWindow = true;
		}

		public void openStatusWindow()
		{
			statusWindow = new StatusWindow(cpuStats, memStats);
			statusWindow.Closed += new EventHandler(statusWindow_Closed);
			statusWindow.Move +=new EventHandler(statusWindow_Move);

			statusWindow.StartPosition = FormStartPosition.Manual;
			statusWindow.Location = config.StatusWindowLocation;

			statusWindow.UpdateGraph();
			statusWindow.Show();

			menuItemToggleStatusWindow.Text = MES_CLOSE_STATUS_WINDOW;
			config.ShowStatusWindow = true;
		}

		/*
		 * ���j���[�N���b�N�C�x���g
		 */

		public void menuItemExit_Click(object sender, System.EventArgs e)
		{
			updateTimer.Stop();
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
				// �p�^�[���ύX�Ɏ��s������ݒ�����ɖ߂�
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

		private void patternWindow_Closed(object sender, EventArgs e)
		{
			patternWindow.Dispose();
			patternWindow = null;
			menuItemTogglePatternWindow.Text = MES_OPEN_PATTERN_WINDOW;
			config.ShowPatternWindow = false;
		}

		private void statusWindow_Closed(object sender, EventArgs e)
		{
			statusWindow.Dispose();
			statusWindow = null;
			menuItemToggleStatusWindow.Text = MES_OPEN_STATUS_WINDOW;
			config.ShowStatusWindow = false;
		}

		private void menuItemTogglePatternWindow_Click(object sender, EventArgs e)
		{
			if (patternWindow == null) 
			{
				openPatternWindow();
			} 
			else 
			{
				patternWindow.Close();
			}
		}

		private void menuItemToggleStatusWindow_Click(object sender, EventArgs e)
		{
			if (statusWindow == null) 
			{
				openStatusWindow();
			}
			else 
			{
				statusWindow.Close();
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

		private void menuVersionInfo_Click(object sender, EventArgs e)
		{
			InfoWindow window = new InfoWindow();
			window.Show();
		}

		/// <summary>
		/// �n���h������Ă��Ȃ���O���L���b�`���āA�X�^�b�N�g���[�X��ۑ����ăf�o�b�O�ɖ𗧂Ă�
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			Exception ex = e.ExceptionObject as Exception; // Exception �ȊO�����ł���̂͒�����ȏꍇ�̂݁B

			if (MessageBox.Show(
				String.Format("�A�v���P�[�V�����̎��s���ɗ\�����Ȃ��d��ȃG���[���������܂����B\n\n�G���[���e:\n{0}\n\n�G���[�����t�@�C���ɕۑ����A�񍐂��Ă����������Ƃŕs��̉����ɖ𗧂\��������܂��B�G���[�����t�@�C���ɕۑ����܂���?",
				((Exception)(e.ExceptionObject)).Message)
				, Application.ProductName
				, MessageBoxButtons.YesNo
				, MessageBoxIcon.Error
				, MessageBoxDefaultButton.Button1
				) == DialogResult.Yes)
			{
				using (SaveFileDialog saveFileDialog = new SaveFileDialog())
				{
					saveFileDialog.DefaultExt = "txt";
					saveFileDialog.Filter = "�e�L�X�g�t�@�C��|*.txt";
					saveFileDialog.FileName = String.Format("macface4win_stacktrace_{0:yyyyMMdd_HHmmss}.txt", DateTime.Now);
					if (saveFileDialog.ShowDialog() == DialogResult.OK)
					{
						using (Stream stream = saveFileDialog.OpenFile())
						using (StreamWriter sw = new StreamWriter(stream))
						{
							Assembly asm = Assembly.GetExecutingAssembly();

							sw.WriteLine("��������: {0}", DateTime.Now);
							sw.WriteLine();
							sw.WriteLine("MacFace for Windows:");
							sw.WriteLine("========================");
							sw.WriteLine("�o�[�W����: {0}", ((ApplicationVersionStringAttribute)(asm.GetCustomAttributes(typeof(ApplicationVersionStringAttribute), true))[0]).Version);
							sw.WriteLine("�A�Z���u��: {0}", Assembly.GetExecutingAssembly().FullName);
							sw.WriteLine();
							sw.WriteLine("�����:");
							sw.WriteLine("========================");
							sw.WriteLine("�I�y���[�e�B���O�V�X�e��: {0}", Environment.OSVersion);
							sw.WriteLine("Microsoft .NET Framework: {0}", Environment.Version);
							sw.WriteLine();
							sw.WriteLine("�n���h������Ă��Ȃ���O: ");
							sw.WriteLine("=========================");
							sw.WriteLine(ex.ToString());
						}
					}
				}
				
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void SystemEvents_SessionEnding(object sender, Microsoft.Win32.SessionEndingEventArgs e)
		{
			config.Save();
		}
	}
}
