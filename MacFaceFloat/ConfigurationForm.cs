using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;


namespace MacFace.FloatApp
{
	/// <summary>
	/// ConfigurationForm の概要の説明です。
	/// </summary>
	public class ConfigurationForm : System.Windows.Forms.Form
	{
		private MainForm _mainForm;
		private Configuration _config;

		private System.Windows.Forms.TreeView treeCategory;
		private System.Windows.Forms.Button buttonCancel;
		private System.Windows.Forms.Button buttonApply;
		private System.Windows.Forms.Button buttonOK;
		private System.Windows.Forms.Label labelTitle;
		private System.Windows.Forms.Panel panelContainer;
		private System.Windows.Forms.Panel panelAppearance;
		private System.Windows.Forms.TrackBar trackBarOpacity;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Button buttonBrowse;
		private System.Windows.Forms.Panel panelFacePatternList;
		private System.Windows.Forms.ListView listViewFaces;
		private System.Windows.Forms.ImageList imageListFacePreviews;
		private System.Windows.Forms.ToolTip toolTipPreviewDetail;
		private System.Windows.Forms.LinkLabel linkWebSite;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.CheckBox checkMouseMessage;
		private System.ComponentModel.IContainer components;

		public ConfigurationForm(MainForm mainForm)
		{
			//
			// Windows フォーム デザイナ サポートに必要です。
			//
			InitializeComponent();

			_mainForm = mainForm;
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
			this.treeCategory = new System.Windows.Forms.TreeView();
			this.buttonCancel = new System.Windows.Forms.Button();
			this.buttonApply = new System.Windows.Forms.Button();
			this.buttonOK = new System.Windows.Forms.Button();
			this.labelTitle = new System.Windows.Forms.Label();
			this.panelContainer = new System.Windows.Forms.Panel();
			this.panelFacePatternList = new System.Windows.Forms.Panel();
			this.label4 = new System.Windows.Forms.Label();
			this.linkWebSite = new System.Windows.Forms.LinkLabel();
			this.buttonBrowse = new System.Windows.Forms.Button();
			this.listViewFaces = new System.Windows.Forms.ListView();
			this.imageListFacePreviews = new System.Windows.Forms.ImageList(this.components);
			this.panelAppearance = new System.Windows.Forms.Panel();
			this.checkMouseMessage = new System.Windows.Forms.CheckBox();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.trackBarOpacity = new System.Windows.Forms.TrackBar();
			this.toolTipPreviewDetail = new System.Windows.Forms.ToolTip(this.components);
			this.panelContainer.SuspendLayout();
			this.panelFacePatternList.SuspendLayout();
			this.panelAppearance.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.trackBarOpacity)).BeginInit();
			this.SuspendLayout();
			// 
			// treeCategory
			// 
			this.treeCategory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left)));
			this.treeCategory.ImageIndex = -1;
			this.treeCategory.Location = new System.Drawing.Point(0, 0);
			this.treeCategory.Name = "treeCategory";
			this.treeCategory.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
																					 new System.Windows.Forms.TreeNode("全般"),
																					 new System.Windows.Forms.TreeNode("顔パターン")});
			this.treeCategory.SelectedImageIndex = -1;
			this.treeCategory.Size = new System.Drawing.Size(193, 449);
			this.treeCategory.TabIndex = 0;
			this.treeCategory.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeCategory_AfterSelect);
			// 
			// buttonCancel
			// 
			this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonCancel.Location = new System.Drawing.Point(427, 420);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new System.Drawing.Size(87, 21);
			this.buttonCancel.TabIndex = 1;
			this.buttonCancel.Text = "キャンセル";
			// 
			// buttonApply
			// 
			this.buttonApply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonApply.Enabled = false;
			this.buttonApply.Location = new System.Drawing.Point(521, 420);
			this.buttonApply.Name = "buttonApply";
			this.buttonApply.Size = new System.Drawing.Size(87, 21);
			this.buttonApply.TabIndex = 2;
			this.buttonApply.Text = "適用(&A)";
			// 
			// buttonOK
			// 
			this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.buttonOK.Location = new System.Drawing.Point(332, 420);
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.Size = new System.Drawing.Size(87, 21);
			this.buttonOK.TabIndex = 3;
			this.buttonOK.Text = "OK";
			this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
			// 
			// labelTitle
			// 
			this.labelTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.labelTitle.BackColor = System.Drawing.SystemColors.ActiveCaption;
			this.labelTitle.Font = new System.Drawing.Font("MS UI Gothic", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(128)));
			this.labelTitle.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
			this.labelTitle.Location = new System.Drawing.Point(199, 5);
			this.labelTitle.Name = "labelTitle";
			this.labelTitle.Size = new System.Drawing.Size(412, 27);
			this.labelTitle.TabIndex = 4;
			this.labelTitle.Text = "全般";
			this.labelTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// panelContainer
			// 
			this.panelContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.panelContainer.Controls.Add(this.panelFacePatternList);
			this.panelContainer.Controls.Add(this.panelAppearance);
			this.panelContainer.Location = new System.Drawing.Point(198, 38);
			this.panelContainer.Name = "panelContainer";
			this.panelContainer.Size = new System.Drawing.Size(411, 371);
			this.panelContainer.TabIndex = 5;
			// 
			// panelFacePatternList
			// 
			this.panelFacePatternList.Controls.Add(this.label4);
			this.panelFacePatternList.Controls.Add(this.linkWebSite);
			this.panelFacePatternList.Controls.Add(this.buttonBrowse);
			this.panelFacePatternList.Controls.Add(this.listViewFaces);
			this.panelFacePatternList.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panelFacePatternList.Location = new System.Drawing.Point(0, 0);
			this.panelFacePatternList.Name = "panelFacePatternList";
			this.panelFacePatternList.Size = new System.Drawing.Size(411, 371);
			this.panelFacePatternList.TabIndex = 7;
			// 
			// label4
			// 
			this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label4.Location = new System.Drawing.Point(4, 350);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(63, 17);
			this.label4.TabIndex = 3;
			this.label4.Text = "ウェブサイト:";
			// 
			// linkWebSite
			// 
			this.linkWebSite.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.linkWebSite.Location = new System.Drawing.Point(67, 350);
			this.linkWebSite.Name = "linkWebSite";
			this.linkWebSite.Size = new System.Drawing.Size(338, 16);
			this.linkWebSite.TabIndex = 2;
			this.linkWebSite.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkWebSite_LinkClicked);
			// 
			// buttonBrowse
			// 
			this.buttonBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonBrowse.Location = new System.Drawing.Point(322, 323);
			this.buttonBrowse.Name = "buttonBrowse";
			this.buttonBrowse.Size = new System.Drawing.Size(87, 21);
			this.buttonBrowse.TabIndex = 1;
			this.buttonBrowse.Text = "参照(&B)...";
			this.buttonBrowse.Click += new System.EventHandler(this.buttonBrowse_Click);
			// 
			// listViewFaces
			// 
			this.listViewFaces.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.listViewFaces.LargeImageList = this.imageListFacePreviews;
			this.listViewFaces.Location = new System.Drawing.Point(0, 0);
			this.listViewFaces.Name = "listViewFaces";
			this.listViewFaces.Size = new System.Drawing.Size(410, 318);
			this.listViewFaces.TabIndex = 0;
			this.listViewFaces.DoubleClick += new System.EventHandler(this.listViewFaces_DoubleClick);
			this.listViewFaces.MouseMove += new System.Windows.Forms.MouseEventHandler(this.listViewFaces_MouseMove);
			this.listViewFaces.SelectedIndexChanged += new System.EventHandler(this.listViewFaces_SelectedIndexChanged);
			// 
			// imageListFacePreviews
			// 
			this.imageListFacePreviews.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
			this.imageListFacePreviews.ImageSize = new System.Drawing.Size(128, 128);
			this.imageListFacePreviews.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// panelAppearance
			// 
			this.panelAppearance.Controls.Add(this.checkMouseMessage);
			this.panelAppearance.Controls.Add(this.label3);
			this.panelAppearance.Controls.Add(this.label2);
			this.panelAppearance.Controls.Add(this.label1);
			this.panelAppearance.Controls.Add(this.trackBarOpacity);
			this.panelAppearance.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panelAppearance.Location = new System.Drawing.Point(0, 0);
			this.panelAppearance.Name = "panelAppearance";
			this.panelAppearance.Size = new System.Drawing.Size(411, 371);
			this.panelAppearance.TabIndex = 0;
			// 
			// checkMouseMessage
			// 
			this.checkMouseMessage.Enabled = false;
			this.checkMouseMessage.Location = new System.Drawing.Point(5, 115);
			this.checkMouseMessage.Name = "checkMouseMessage";
			this.checkMouseMessage.Size = new System.Drawing.Size(400, 16);
			this.checkMouseMessage.TabIndex = 4;
			this.checkMouseMessage.Text = "マウスの動作を背面のウィンドウへ伝える(&T)";
			// 
			// label3
			// 
			this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label3.Location = new System.Drawing.Point(364, 73);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(45, 11);
			this.label3.TabIndex = 3;
			this.label3.Text = "不透明";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(4, 73);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(31, 11);
			this.label2.TabIndex = 2;
			this.label2.Text = "透明";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(2, 5);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(196, 18);
			this.label1.TabIndex = 1;
			this.label1.Text = "透明度:";
			// 
			// trackBarOpacity
			// 
			this.trackBarOpacity.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.trackBarOpacity.LargeChange = 10;
			this.trackBarOpacity.Location = new System.Drawing.Point(8, 26);
			this.trackBarOpacity.Maximum = 100;
			this.trackBarOpacity.Name = "trackBarOpacity";
			this.trackBarOpacity.Size = new System.Drawing.Size(400, 42);
			this.trackBarOpacity.TabIndex = 0;
			this.trackBarOpacity.TickFrequency = 10;
			// 
			// ConfigurationForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 12);
			this.ClientSize = new System.Drawing.Size(616, 450);
			this.Controls.Add(this.panelContainer);
			this.Controls.Add(this.labelTitle);
			this.Controls.Add(this.buttonOK);
			this.Controls.Add(this.buttonApply);
			this.Controls.Add(this.buttonCancel);
			this.Controls.Add(this.treeCategory);
			this.MinimumSize = new System.Drawing.Size(500, 140);
			this.Name = "ConfigurationForm";
			this.Text = "MacFace の設定";
			this.Load += new System.EventHandler(this.ConfigurationForm_Load);
			this.panelContainer.ResumeLayout(false);
			this.panelFacePatternList.ResumeLayout(false);
			this.panelAppearance.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.trackBarOpacity)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		private void ConfigurationForm_Load(object sender, System.EventArgs e)
		{
			// 全般を表示。
			foreach (Panel pane in panelContainer.Controls)
				pane.Visible = false;

			panelAppearance.Show();

			// 初期値をセット。
			trackBarOpacity.Value = (int) (_mainForm.Opacity * 100);
			checkMouseMessage.Checked = Configuration.GetInstance().TransparentMouseMessage;
		}

		private void buttonOK_Click(object sender, System.EventArgs e)
		{
			Configuration config = Configuration.GetInstance();
			config.Opacity = trackBarOpacity.Value;
			config.TransparentMouseMessage = checkMouseMessage.Checked;

			this.Close();
		}

		private void treeCategory_AfterSelect(object sender, System.Windows.Forms.TreeViewEventArgs e)
		{
			Panel container;

			// てきとう
			if (e.Node.Text == labelTitle.Text) return;
			foreach (Panel pane in panelContainer.Controls)
				pane.Visible = false;
			switch (e.Node.Text) 
			{
				case "全般":
					panelAppearance.Show();
					break;
				case "顔パターン":
					listViewFaces.Items.Clear();
					imageListFacePreviews.Images.Clear();
					int i = 0;

					using (Bitmap tmpImage = new Bitmap(128, 128)) 
					{
						// 顔パターンリストを作る。
						// TODO: このままだと遅いので近いうちに非同期にしよう。
						// アプリケーションディレクトリの下。
						foreach (DirectoryInfo dir in (new DirectoryInfo(Application.StartupPath)).GetDirectories("*.mcface"))
						{
							AddPreviewListItem(dir.FullName);
						}
						// %USERPROFILE%\Application Data\MacFace\Faces 下。
						string userFaceDefPath = Path.Combine(Configuration.ConfigurationPath, "Faces");
						if (Directory.Exists(userFaceDefPath)) 
						{
							foreach (DirectoryInfo dir in (new DirectoryInfo(userFaceDefPath).GetDirectories("*.mcface")))
							{
								AddPreviewListItem(dir.FullName);
							}
						}
					}

					panelFacePatternList.Show();
					break;
			}

			labelTitle.Text = e.Node.Text;
		}


		private void AddPreviewListItem(string path)
		{
			using (Bitmap tmpImage = new Bitmap(128, 128)) 
			{
				try 
				{
					FaceDef faceDef = new FaceDef(path);
					PartList partList = faceDef.Pattern(FaceDef.PatternSuite.Normal, 10);
					
					using (Graphics g = Graphics.FromImage(tmpImage)) 
					{
						g.Clear(Color.White);
						g.DrawRectangle(new Pen(Color.LightGray), 0, 0, 127, 127);
						foreach (Part part in partList) 
						{
							g.DrawImage(part.Image,
								part.Point.X, part.Point.Y,
								part.Image.Size.Width, part.Image.Size.Height);
						}
					}
					imageListFacePreviews.Images.Add(tmpImage);

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
			if (listViewFaces.SelectedItems.Count == 0) 
			{
				linkWebSite.Text = "";
			} 
			else 
			{
				ListViewItem item = listViewFaces.SelectedItems[0];
				linkWebSite.Text = item.SubItems[4].Text;
			}
		}

		private void listViewFaces_DoubleClick(object sender, System.EventArgs e)
		{
			Point pos = listViewFaces.PointToClient(Cursor.Position);
			ListViewItem item = listViewFaces.GetItemAt(pos.X, pos.Y);
			
			if (item != null) 
			{
				_mainForm.LoadFaceDefine(item.SubItems[1].Text);
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

		private void linkWebSite_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
		{
			try 
			{
				System.Diagnostics.Process.Start(linkWebSite.Text);
			} 
			catch (Win32Exception) {}
		}
	}
}
