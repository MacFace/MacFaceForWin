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

		private Configuration config;

		private NotifyIcon notifyIcon;
		private PatternWindow patternWindow;
		
		private System.Windows.Forms.Timer updateTimer;
		private CPUUsageCounter cpuCounter;
		private MemoryUsageCounter memoryCounter;


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

			cpuCounter = new CPUUsageCounter();
			memoryCounter = new MemoryUsageCounter();

			updateTimer = new System.Windows.Forms.Timer();
			updateTimer.Enabled = false;
			updateTimer.Interval = 1000;
			updateTimer.Tick += new EventHandler(this.CountProcessorUsage);

			InitializeComponent();
		}

		void InitializeComponent() 
		{
			// �R���e�L�X�g���j���[
			ContextMenu contextMenu = new System.Windows.Forms.ContextMenu();
			MenuItem menuItemPatternSelect = new System.Windows.Forms.MenuItem();
			MenuItem menuItemConfigure = new System.Windows.Forms.MenuItem();
			MenuItem menuItemExit = new System.Windows.Forms.MenuItem();

			menuItemPatternSelect.Text = "��p�^�[���̑I��(&S)";
			menuItemPatternSelect.Click += new System.EventHandler(menuItemPatternSelect_Click);

			menuItemConfigure.Text = "MacFace �̐ݒ�(&C)...";
			menuItemConfigure.Click +=new EventHandler(menuItemConfigure_Click);

			menuItemExit.Index = 0;
			menuItemExit.Text = "�I��(&X)";
			menuItemExit.Click += new System.EventHandler(menuItemExit_Click);

			contextMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
					menuItemPatternSelect, menuItemConfigure, new MenuItem("-"), menuItemExit});

			// �ʒm�A�C�R��
			Assembly asm = Assembly.GetExecutingAssembly();
			this.notifyIcon = new System.Windows.Forms.NotifyIcon();
			this.notifyIcon.Text = "MacFace";
			this.notifyIcon.Icon = new Icon(asm.GetManifestResourceStream("MacFace.FloatApp.App.ico"));
			this.notifyIcon.Visible = true;
			this.notifyIcon.ContextMenu = contextMenu;

			// �p�^�[���E�C���h�E
			this.patternWindow = new PatternWindow();
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
				if (!SelectFaceDefine(Application.StartupPath))
				{
					Application.Exit();
					return;
				}
			}

			Application.ApplicationExit += new EventHandler(Application_ApplicationExit);

			patternWindow.Location = config.Location;
			ApplyConfiguration();

			patternWindow.Show();
			updateTimer.Start();

			Application.Run(this);
		}

		void Application_ApplicationExit(object sender, EventArgs e)
		{
			notifyIcon.Visible = false;

			// �ۑ�
			config.FaceDefPath = patternWindow.FaceDef.Path;
			config.Location = patternWindow.Location;
			config.Save();
		}

		/*
		 * ��p�^�[����`�t�H���_�I��
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
				folderBrowser.Description = "��p�^�[���t�@�C���̑��݂���t�H���_��I�����Ă��������B";
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

			// ��p�^�[�������ւ����͍X�V���~�߂Ă���
			if (updateTimer != null) updateTimer.Stop();

			patternWindow.FaceDef = newFaceDef;
			patternWindow.Refresh();

			notifyIcon.Text = "MacFace - " + patternWindow.FaceDef.Title;

			// �X�V�ĊJ
			if (updateTimer != null) updateTimer.Start();

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

			patternWindow.UpdatePattern(suite, pattern, markers);
		}

		// TODO:���[�f�B���O���̏����ݒ�Ɛݒ�ύX�̔��f�͕�����ׂ�
		private void ApplyConfiguration()
		{
			patternWindow.Opacity = (float)config.Opacity / 100;
			patternWindow.PatternSize = (float)config.PatternSize / 100;
			patternWindow.TransparentMouseMessage = config.TransparentMouseMessage;

			patternWindow.Refresh();
		}

		/*
		 * ���j���[�N���b�N�C�x���g
		 */
		public void menuItemPatternSelect_Click(object sender, System.EventArgs e)
		{
			SelectFaceDefine(patternWindow.FaceDef.Path);	
		}

		public void menuItemExit_Click(object sender, System.EventArgs e)
		{
			updateTimer.Stop();
			patternWindow.Close();
			ExitThread();
		}

		private void menuItemConfigure_Click(object sender, EventArgs e)
		{
			ConfigurationForm configForm = new ConfigurationForm(this);
			if (configForm.ShowDialog() == DialogResult.OK) 
			{
				ApplyConfiguration();
			}
		}
	}
}
