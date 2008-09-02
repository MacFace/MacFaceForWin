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
	/// Configuration の概要の説明です。
	/// </summary>
	public class Configuration
	{
		public static readonly String ConfigurationPath;
		private static Configuration _configInstance = null;

		private StringDictionary _config = new StringDictionary();


		//
		// プロパティにすべきな気がするけどとりあえずフィールドで。
		//
		[AutoConfigure("general.facedefpath")]
		[Category("全般"), Description("前回利用していた顔パターンのディレクトリ")]
		public String FaceDefPath;

		[AutoConfigure("form.faceform.opacity")]
		[Category("パターンウインドウ"), Description("透明度0-100の中で指定します。")]
		public Int32 Opacity = 100;

        [AutoConfigure("form.faceform.patternsize")]
        [Category("パターンウインドウ"), Description("パターンの大きさ10-100の中で指定します。")]
        public Int32 PatternSize = 100;

        [AutoConfigure("form.faceform.updatespeed")]
        [Category("パターンウインドウ"), Description("更新速度1-10の中で指定します。")]
        public Int32 UpdateSpeed = 1;

        [AutoConfigure("form.faceform.transparentmousemessage")]
		[Category("パターンウインドウ"), Description("マウスメッセージを透過させるかどうかを指定します。")]
		public bool TransparentMouseMessage;

		[AutoConfigure("form.faceform.point")]
		[Category("パターンウインドウ"), Description("表示位置")]
		public Point Location;

		[AutoConfigure("form.faceform.show")]
		[Category("パターンウインドウ"), Description("表示状態")]
		public bool ShowPatternWindow = true;

		[AutoConfigure("form.status.point")]
		[Category("ステータスウインドウ"), Description("表示位置")]
		public Point StatusWindowLocation;

		[AutoConfigure("form.status.show")]
		[Category("ステータスウインドウ"), Description("表示状態")]
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


		// 保存、読み込み
		public void Load()
		{
			// StringDictionaryからプロパティへもどす。
			_config = Setting.GetSettings(ConfigurationPath + @"\AppSettings.xml");
			AutoConfigurator.Configure(this,
					new AutoConfigurator.ConfigurationHandler(LoadConfigHandler));
		}

		public void Save()
		{
			if (!System.IO.Directory.Exists(ConfigurationPath))
				System.IO.Directory.CreateDirectory(ConfigurationPath);
			
			// プロパティをStringDictionaryに格納。
			AutoConfigurator.GetConfiguration(this,
				new AutoConfigurator.GetConfigurationHandler(SaveConfigHandler));

			// XML に書き出す。
			Setting.SaveSettings(ConfigurationPath + @"\AppSettings.xml", _config);

		}



		#region AutoConfiguration Handlers
		//
		// Saveで呼び出されるハンドラ
		//
		private void SaveConfigHandler(String key, Object value, Type propertyType)
		{
			TypeConverter tconv = TypeDescriptor.GetConverter(propertyType);
			_config[key] = tconv.ConvertToString(value);
		}

		//
		// Loadで呼び出されるハンドラ
		// HashtableのStringをTypeConverterで元の形式に戻す。(自動的にプロパティにセット)
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
