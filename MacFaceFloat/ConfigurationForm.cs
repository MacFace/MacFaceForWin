using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;
using System.IO;


namespace MacFace.FloatApp
{
	public delegate void ConfigChangedEvent();

	/// <summary>
	/// ConfigurationForm の概要の説明です。
	/// </summary>
	public class ConfigurationForm : System.Windows.Forms.Form
	{
		private Configuration _config;
		private System.Windows.Forms.ImageList imageListFacePreviews;
		private System.Windows.Forms.ToolTip toolTipPreviewDetail;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.CheckBox checkMouseMessage;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.LinkLabel linkWebSite;
		private System.Windows.Forms.Button buttonBrowse;
		private System.Windows.Forms.ListView listViewFaces;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.TrackBar trackBarOpacity;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.TrackBar trackBarPatternSize;
		private System.ComponentModel.IContainer components;

		public event ConfigChangedEvent ConfigChanged;

		public ConfigurationForm()
		{
			//
			// Windows フォーム デザイナ サポートに必要です。
			//
			InitializeComponent();

			_config = Configuration.GetInstance();
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
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(ConfigurationForm));
			this.imageListFacePreviews = new System.Windows.Forms.ImageList(this.components);
			this.label6 = new System.Windows.Forms.Label();
			this.toolTipPreviewDetail = new System.Windows.Forms.ToolTip(this.components);
			this.checkMouseMessage = new System.Windows.Forms.CheckBox();
			this.label4 = new System.Windows.Forms.Label();
			this.linkWebSite = new System.Windows.Forms.LinkLabel();
			this.buttonBrowse = new System.Windows.Forms.Button();
			this.listViewFaces = new System.Windows.Forms.ListView();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.label5 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.trackBarOpacity = new System.Windows.Forms.TrackBar();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.label7 = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.trackBarPatternSize = new System.Windows.Forms.TrackBar();
			this.groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.trackBarOpacity)).BeginInit();
			this.groupBox2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.trackBarPatternSize)).BeginInit();
			this.SuspendLayout();
			// 
			// imageListFacePreviews
			// 
			this.imageListFacePreviews.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
			this.imageListFacePreviews.ImageSize = new System.Drawing.Size(128, 128);
			this.imageListFacePreviews.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(5, 93);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(196, 18);
			this.label6.TabIndex = 7;
			this.label6.Text = "大きさ:";
			// 
			// checkMouseMessage
			// 
			this.checkMouseMessage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.checkMouseMessage.Location = new System.Drawing.Point(8, 352);
			this.checkMouseMessage.Name = "checkMouseMessage";
			this.checkMouseMessage.Size = new System.Drawing.Size(339, 16);
			this.checkMouseMessage.TabIndex = 6;
			this.checkMouseMessage.Text = "マウスの動作を背面のウィンドウへ伝える(&T)";
			this.checkMouseMessage.Click += new System.EventHandler(this.checkMouseMessage_Click);
			// 
			// label4
			// 
			this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label4.Location = new System.Drawing.Point(8, 176);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(63, 17);
			this.label4.TabIndex = 15;
			this.label4.Text = "ウェブサイト:";
			// 
			// linkWebSite
			// 
			this.linkWebSite.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.linkWebSite.Location = new System.Drawing.Point(72, 176);
			this.linkWebSite.Name = "linkWebSite";
			this.linkWebSite.Size = new System.Drawing.Size(322, 16);
			this.linkWebSite.TabIndex = 14;
			this.linkWebSite.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkWebSite_LinkClicked);
			// 
			// buttonBrowse
			// 
			this.buttonBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonBrowse.Location = new System.Drawing.Point(272, 192);
			this.buttonBrowse.Name = "buttonBrowse";
			this.buttonBrowse.Size = new System.Drawing.Size(95, 24);
			this.buttonBrowse.TabIndex = 13;
			this.buttonBrowse.Text = "参照(&B)...";
			this.buttonBrowse.Click += new System.EventHandler(this.buttonBrowse_Click);
			// 
			// listViewFaces
			// 
			this.listViewFaces.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.listViewFaces.LargeImageList = this.imageListFacePreviews;
			this.listViewFaces.Location = new System.Drawing.Point(8, 8);
			this.listViewFaces.Name = "listViewFaces";
			this.listViewFaces.Size = new System.Drawing.Size(362, 160);
			this.listViewFaces.TabIndex = 12;
			this.listViewFaces.MouseMove += new System.Windows.Forms.MouseEventHandler(this.listViewFaces_MouseMove);
			this.listViewFaces.SelectedIndexChanged += new System.EventHandler(this.listViewFaces_SelectedIndexChanged);
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Controls.Add(this.label5);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.trackBarOpacity);
			this.groupBox1.Location = new System.Drawing.Point(10, 224);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(360, 56);
			this.groupBox1.TabIndex = 19;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "透明度";
			// 
			// label5
			// 
			this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label5.Location = new System.Drawing.Point(304, 24);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(48, 11);
			this.label5.TabIndex = 12;
			this.label5.Text = "不透明";
			this.label5.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(8, 24);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(40, 11);
			this.label2.TabIndex = 11;
			this.label2.Text = "透明";
			// 
			// trackBarOpacity
			// 
			this.trackBarOpacity.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.trackBarOpacity.LargeChange = 10;
			this.trackBarOpacity.Location = new System.Drawing.Point(64, 13);
			this.trackBarOpacity.Maximum = 100;
			this.trackBarOpacity.Minimum = 5;
			this.trackBarOpacity.Name = "trackBarOpacity";
			this.trackBarOpacity.Size = new System.Drawing.Size(232, 37);
			this.trackBarOpacity.TabIndex = 9;
			this.trackBarOpacity.TickFrequency = 10;
			this.trackBarOpacity.Value = 100;
			this.trackBarOpacity.ValueChanged += new System.EventHandler(this.trackBarOpacity_ValueChanged);
			// 
			// groupBox2
			// 
			this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox2.Controls.Add(this.label7);
			this.groupBox2.Controls.Add(this.label8);
			this.groupBox2.Controls.Add(this.trackBarPatternSize);
			this.groupBox2.Location = new System.Drawing.Point(10, 288);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(360, 56);
			this.groupBox2.TabIndex = 20;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "パターンの大きさ";
			// 
			// label7
			// 
			this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label7.Location = new System.Drawing.Point(312, 24);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(40, 11);
			this.label7.TabIndex = 21;
			this.label7.Text = "最大";
			this.label7.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(8, 24);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(40, 11);
			this.label8.TabIndex = 20;
			this.label8.Text = "最小";
			// 
			// trackBarPatternSize
			// 
			this.trackBarPatternSize.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.trackBarPatternSize.LargeChange = 10;
			this.trackBarPatternSize.Location = new System.Drawing.Point(64, 12);
			this.trackBarPatternSize.Maximum = 100;
			this.trackBarPatternSize.Minimum = 10;
			this.trackBarPatternSize.Name = "trackBarPatternSize";
			this.trackBarPatternSize.Size = new System.Drawing.Size(232, 37);
			this.trackBarPatternSize.TabIndex = 19;
			this.trackBarPatternSize.TickFrequency = 10;
			this.trackBarPatternSize.Value = 100;
			this.trackBarPatternSize.ValueChanged += new System.EventHandler(this.trackBarPatternSize_ValueChanged);
			// 
			// ConfigurationForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 12);
			this.ClientSize = new System.Drawing.Size(378, 376);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.linkWebSite);
			this.Controls.Add(this.buttonBrowse);
			this.Controls.Add(this.listViewFaces);
			this.Controls.Add(this.checkMouseMessage);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(384, 400);
			this.Name = "ConfigurationForm";
			this.Text = "MacFace の設定";
			this.Load += new System.EventHandler(this.ConfigurationForm_Load);
			this.groupBox1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.trackBarOpacity)).EndInit();
			this.groupBox2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.trackBarPatternSize)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		private void ConfigurationForm_Load(object sender, System.EventArgs e)
		{
			// 初期値のセット
			trackBarOpacity.Value = _config.Opacity;
			trackBarPatternSize.Value = _config.PatternSize;
			checkMouseMessage.Checked = _config.TransparentMouseMessage;

			// 顔パターンリストを作る。
			// TODO: このままだと遅いので近いうちに非同期にしよう。
			using (Bitmap tmpImage = new Bitmap(128, 128)) 
			{
				// アプリケーションディレクトリの下を探す
				foreach (DirectoryInfo dir in (new DirectoryInfo(Application.StartupPath)).GetDirectories("*.mcface"))
				{
					AddPreviewListItem(dir.FullName);
				}
				// %USERPROFILE%\Application Data\MacFace\Faces の下を探す
				string userFaceDefPath = Path.Combine(Configuration.ConfigurationPath, "Faces");
				if (Directory.Exists(userFaceDefPath)) 
				{
					foreach (DirectoryInfo dir in (new DirectoryInfo(userFaceDefPath).GetDirectories("*.mcface")))
					{
						AddPreviewListItem(dir.FullName);
					}
				}
			}
		}

		private void AddPreviewListItem(string path)
		{
			try 
			{
				FaceDef faceDef = new FaceDef(path);
				// 表示/選択した際に汚くならないようにあらかじめ白塗りして描画した画像を用意する。
				using (Image titleImage = faceDef.TitleImage) 
				{
					using (Bitmap titlePreviewImage = new Bitmap(titleImage.Width, titleImage.Height)) 
					{
						using (Graphics g = Graphics.FromImage(titlePreviewImage))
						{
							g.Clear(Color.White);
							g.DrawRectangle(new Pen(Color.LightGray), 0, 0, 127, 127);
							g.DrawImage(titleImage, 0, 0);
						}
						imageListFacePreviews.Images.Add(titlePreviewImage);
					}
				}


				ListViewItem item = listViewFaces.Items.Add(faceDef.Title, imageListFacePreviews.Images.Count-1);
				item.SubItems.Add(path);    // 0: パス
				item.SubItems.Add(faceDef.Author);  // 1: 製作者
				item.SubItems.Add(faceDef.Version); // 2: バージョン
				item.SubItems.Add(
					(faceDef.WebSite != null ?
					faceDef.WebSite.ToString() : "")); // 3: ウェブサイト
			} 
			catch (Exception ex) 
			{
				// TODO: Exception からもっと狭める。
				MessageBox.Show(ex.ToString(), "MacFace for Windows", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void buttonBrowse_Click(object sender, System.EventArgs e)
		{
			FolderBrowserDialog folderBrowser = new FolderBrowserDialog();
			folderBrowser.SelectedPath = Application.StartupPath;
			folderBrowser.Description = "顔パターンファイルの存在するフォルダを選択してください。";
			if (folderBrowser.ShowDialog() == DialogResult.OK) 
			{
				AddPreviewListItem(folderBrowser.SelectedPath);
			} 
		}

		private void listViewFaces_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			ListViewItem item = listViewFaces.GetItemAt(e.X, e.Y);
			
			if (item != null) 
			{
				string tooltipText = String.Format("タイトル: {0}\n製作者: {1}\nバージョン: {2}",
					item.Text, item.SubItems[2].Text, item.SubItems[3].Text);
                
				if (toolTipPreviewDetail.GetToolTip(listViewFaces) != tooltipText)
					toolTipPreviewDetail.SetToolTip(listViewFaces, tooltipText);
			} 
			else 
			{
				// 消す。
				toolTipPreviewDetail.SetToolTip(listViewFaces, "");
			}
		
		}

		private void listViewFaces_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (listViewFaces.SelectedItems.Count != 0) 
			{
				ListViewItem item = listViewFaces.SelectedItems[0];
				linkWebSite.Text = item.SubItems[4].Text;

				_config.FaceDefPath = item.SubItems[1].Text;
				ConfigChanged();
			}
			else 
			{
				linkWebSite.Text = "";
			} 
		}

		private void trackBarOpacity_ValueChanged(object sender, System.EventArgs e)
		{
			_config.Opacity = trackBarOpacity.Value;
			ConfigChanged();
		}

		private void trackBarPatternSize_ValueChanged(object sender, System.EventArgs e)
		{
			_config.PatternSize = trackBarPatternSize.Value;
			ConfigChanged();		
		}

		private void checkMouseMessage_Click(object sender, System.EventArgs e)
		{
			_config.TransparentMouseMessage = checkMouseMessage.Checked;
			ConfigChanged();	
		}

		private void linkWebSite_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
		{
			if (linkWebSite.Text != "") {
				Process.Start(linkWebSite.Text);
			} 
		}

	}
}
