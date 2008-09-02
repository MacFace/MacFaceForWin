/*
 * Configuration.cs
 * $Id$
 * 
 */

using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Specialized;
using System.ComponentModel;

using Misuzilla.Utilities;

namespace MacFace.FloatApp
{
	/// <summary>
	/// Configuration �̊T�v�̐����ł��B
	/// </summary>
	public class Configuration
	{
		public static readonly String ConfigurationPath;
		private static Configuration _configInstance = null;

		private StringDictionary _config = new StringDictionary();


		//
		// �v���p�e�B�ɂ��ׂ��ȋC�����邯�ǂƂ肠�����t�B�[���h�ŁB
		//
		[AutoConfigure("general.facedefpath")]
		[Category("�S��"), Description("�O�񗘗p���Ă�����p�^�[���̃f�B���N�g��")]
		public String FaceDefPath;

		[AutoConfigure("form.faceform.opacity")]
		[Category("�p�^�[���E�C���h�E"), Description("�����x0-100�̒��Ŏw�肵�܂��B")]
		public Int32 Opacity = 100;

        [AutoConfigure("form.faceform.patternsize")]
        [Category("�p�^�[���E�C���h�E"), Description("�p�^�[���̑傫��10-100�̒��Ŏw�肵�܂��B")]
        public Int32 PatternSize = 100;

        [AutoConfigure("form.faceform.updatespeed")]
        [Category("�p�^�[���E�C���h�E"), Description("�X�V���x1-10�̒��Ŏw�肵�܂��B")]
        public Int32 UpdateSpeed = 1;

        [AutoConfigure("form.faceform.transparentmousemessage")]
		[Category("�p�^�[���E�C���h�E"), Description("�}�E�X���b�Z�[�W�𓧉߂����邩�ǂ������w�肵�܂��B")]
		public bool TransparentMouseMessage;

		[AutoConfigure("form.faceform.point")]
		[Category("�p�^�[���E�C���h�E"), Description("�\���ʒu")]
		public Point Location;

		[AutoConfigure("form.faceform.show")]
		[Category("�p�^�[���E�C���h�E"), Description("�\�����")]
		public bool ShowPatternWindow = true;

		[AutoConfigure("form.status.point")]
		[Category("�X�e�[�^�X�E�C���h�E"), Description("�\���ʒu")]
		public Point StatusWindowLocation;

		[AutoConfigure("form.status.show")]
		[Category("�X�e�[�^�X�E�C���h�E"), Description("�\�����")]
		public bool ShowStatusWindow = true;


		public static Configuration GetInstance()
		{
			if (_configInstance == null) 
				_configInstance = new Configuration();

			return _configInstance;
		}
		private Configuration()
		{
			FaceDefPath = System.IO.Path.Combine(Application.StartupPath, "default.mcface");
		}
		static Configuration()
		{
			ConfigurationPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\MacFace";
		}


		// �ۑ��A�ǂݍ���
		public void Load()
		{
			// StringDictionary����v���p�e�B�ւ��ǂ��B
			_config = Setting.GetSettings(ConfigurationPath + @"\AppSettings.xml");
			AutoConfigurator.Configure(this,
					new AutoConfigurator.ConfigurationHandler(LoadConfigHandler));
		}

		public void Save()
		{
			if (!System.IO.Directory.Exists(ConfigurationPath))
				System.IO.Directory.CreateDirectory(ConfigurationPath);
			
			// �v���p�e�B��StringDictionary�Ɋi�[�B
			AutoConfigurator.GetConfiguration(this,
				new AutoConfigurator.GetConfigurationHandler(SaveConfigHandler));

			// XML �ɏ����o���B
			Setting.SaveSettings(ConfigurationPath + @"\AppSettings.xml", _config);

		}



		#region AutoConfiguration Handlers
		//
		// Save�ŌĂяo�����n���h��
		//
		private void SaveConfigHandler(String key, Object value, Type propertyType)
		{
			TypeConverter tconv = TypeDescriptor.GetConverter(propertyType);
			_config[key] = tconv.ConvertToString(value);
		}

		//
		// Load�ŌĂяo�����n���h��
		// Hashtable��String��TypeConverter�Ō��̌`���ɖ߂��B(�����I�Ƀv���p�e�B�ɃZ�b�g)
		//
		private Object LoadConfigHandler(String key, Type propertyType)
		{
			if (!_config.ContainsKey(key)) return null;

			TypeConverter tconv = TypeDescriptor.GetConverter(propertyType);
			try
			{
				Object o = tconv.ConvertFromString(_config[key] as String);
				return o;
			} 
			catch (NotSupportedException) 
			{
				return null;
			}
		}
		#endregion

	}
}
