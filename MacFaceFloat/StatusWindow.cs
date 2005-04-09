using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace MacFace.FloatApp
{
	/// <summary>
	/// StatusWindow の概要の説明です。
	/// </summary>
	public class StatusWindow : System.Windows.Forms.Form
	{
		internal System.Windows.Forms.PictureBox memoryGraphPicBox;
		internal System.Windows.Forms.PictureBox cpuGraphPicBox;
		/// <summary>
		/// 必要なデザイナ変数です。
		/// </summary>
		private System.ComponentModel.Container components = null;

		public StatusWindow()
		{
			//
			// Windows フォーム デザイナ サポートに必要です。
			//
			InitializeComponent();

			//
			// TODO: InitializeComponent 呼び出しの後に、コンストラクタ コードを追加してください。
			//
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

		#region Windows フォーム デザイナで生成されたコード 
		/// <summary>
		/// デザイナ サポートに必要なメソッドです。このメソッドの内容を
		/// コード エディタで変更しないでください。
		/// </summary>
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(StatusWindow));
			this.memoryGraphPicBox = new System.Windows.Forms.PictureBox();
			this.cpuGraphPicBox = new System.Windows.Forms.PictureBox();
			this.SuspendLayout();
			// 
			// memoryGraphPicBox
			// 
			this.memoryGraphPicBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.memoryGraphPicBox.Location = new System.Drawing.Point(6, 8);
			this.memoryGraphPicBox.Name = "memoryGraphPicBox";
			this.memoryGraphPicBox.Size = new System.Drawing.Size(302, 102);
			this.memoryGraphPicBox.TabIndex = 0;
			this.memoryGraphPicBox.TabStop = false;
			// 
			// cpuGraphPicBox
			// 
			this.cpuGraphPicBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.cpuGraphPicBox.Location = new System.Drawing.Point(6, 120);
			this.cpuGraphPicBox.Name = "cpuGraphPicBox";
			this.cpuGraphPicBox.Size = new System.Drawing.Size(302, 102);
			this.cpuGraphPicBox.TabIndex = 1;
			this.cpuGraphPicBox.TabStop = false;
			// 
			// StatusWindow
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 12);
			this.ClientSize = new System.Drawing.Size(314, 232);
			this.ControlBox = false;
			this.Controls.Add(this.cpuGraphPicBox);
			this.Controls.Add(this.memoryGraphPicBox);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "StatusWindow";
			this.Text = "ステータス";
			this.ResumeLayout(false);

		}
		#endregion
	}
}
