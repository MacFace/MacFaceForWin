using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Reflection;
using System.IO;

namespace MacFace.FloatApp
{
	/// <summary>
	/// InfoWindow の概要の説明です。
	/// </summary>
	public class InfoWindow : System.Windows.Forms.Form
	{
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.Label lblAppName;
		private System.Windows.Forms.Label lblAppVersion;
		private System.Windows.Forms.Label lblAppCopyright;
		/// <summary>
		/// 必要なデザイナ変数です。
		/// </summary>
		private System.ComponentModel.Container components = null;

		public InfoWindow()
		{
			//
			// Windows フォーム デザイナ サポートに必要です。
			//
			InitializeComponent();

			Assembly asm = Assembly.GetEntryAssembly();
			lblAppName.Text = Application.ProductName;
			lblAppVersion.Text = ((ApplicationVersionStringAttribute)GetAssemblyAttribute(asm, typeof(ApplicationVersionStringAttribute))).Version;
			lblAppCopyright.Text = ((AssemblyCopyrightAttribute)GetAssemblyAttribute(asm, typeof(AssemblyCopyrightAttribute))).Copyright;
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
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		private Object GetAssemblyAttribute(Assembly asm, Type type)
		{
			object[] attrs = asm.GetCustomAttributes(type, false);
			if (attrs != null && attrs.Length > 0) 
			{
				return attrs[0];
			}
			return null;
		}

        
		#region Windows フォーム デザイナで生成されたコード 
		/// <summary>
		/// デザイナ サポートに必要なメソッドです。このメソッドの内容を
		/// コード エディタで変更しないでください。
		/// </summary>
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(InfoWindow));
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.lblAppName = new System.Windows.Forms.Label();
			this.lblAppVersion = new System.Windows.Forms.Label();
			this.lblAppCopyright = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// pictureBox1
			// 
			this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
			this.pictureBox1.Location = new System.Drawing.Point(137, 16);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(56, 48);
			this.pictureBox1.TabIndex = 0;
			this.pictureBox1.TabStop = false;
			// 
			// lblAppName
			// 
			this.lblAppName.Location = new System.Drawing.Point(8, 80);
			this.lblAppName.Name = "lblAppName";
			this.lblAppName.Size = new System.Drawing.Size(320, 16);
			this.lblAppName.TabIndex = 2;
			this.lblAppName.Text = "label1";
			this.lblAppName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblAppVersion
			// 
			this.lblAppVersion.Location = new System.Drawing.Point(8, 104);
			this.lblAppVersion.Name = "lblAppVersion";
			this.lblAppVersion.Size = new System.Drawing.Size(320, 16);
			this.lblAppVersion.TabIndex = 3;
			this.lblAppVersion.Text = "label2";
			this.lblAppVersion.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// lblAppCopyright
			// 
			this.lblAppCopyright.Location = new System.Drawing.Point(8, 128);
			this.lblAppCopyright.Name = "lblAppCopyright";
			this.lblAppCopyright.Size = new System.Drawing.Size(320, 16);
			this.lblAppCopyright.TabIndex = 4;
			this.lblAppCopyright.Text = "label2";
			this.lblAppCopyright.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// InfoWindow
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 12);
			this.ClientSize = new System.Drawing.Size(330, 149);
			this.Controls.Add(this.lblAppCopyright);
			this.Controls.Add(this.lblAppVersion);
			this.Controls.Add(this.lblAppName);
			this.Controls.Add(this.pictureBox1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "InfoWindow";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "バージョン情報";
			this.ResumeLayout(false);

		}
		#endregion

	}
}
