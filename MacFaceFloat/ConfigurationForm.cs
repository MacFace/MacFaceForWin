using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;


namespace MacFace.FloatApp
{
	/// <summary>
	/// ConfigurationForm �̊T�v�̐����ł��B
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
		private System.Windows.Forms.ImageList imageListConfigTreeIcon;
		private System.Windows.Forms.Label label5;
		private System.ComponentModel.IContainer components;

		public ConfigurationForm(MainForm mainForm)
		{
			//
			// Windows �t�H�[�� �f�U�C�i �T�|�[�g�ɕK�v�ł��B
			//
			InitializeComponent();

			_mainForm = mainForm;
			_config = Configuration.GetInstance();

		}

		/// <summary>
		/// �g�p����Ă��郊�\�[�X�Ɍ㏈�������s���܂��B
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

		#region Windows �t�H�[�� �f�U�C�i�Ő������ꂽ�R�[�h 
		/// <summary>
		/// �f�U�C�i �T�|�[�g�ɕK�v�ȃ��\�b�h�ł��B���̃��\�b�h�̓��e��
		/// �R�[�h �G�f�B�^�ŕύX���Ȃ��ł��������B
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(ConfigurationForm));
			this.treeCategory = new System.Windows.Forms.TreeView();
			this.imageListConfigTreeIcon = new System.Windows.Forms.ImageList(this.components);
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
			this.label5 = new System.Windows.Forms.Label();
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
			this.treeCategory.FullRowSelect = true;
			this.treeCategory.HideSelection = false;
			this.treeCategory.HotTracking = true;
			this.treeCategory.ImageList = this.imageListConfigTreeIcon;
			this.treeCategory.Location = new System.Drawing.Point(0, 0);
			this.treeCategory.Name = "treeCategory";
			this.treeCategory.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
																					 new System.Windows.Forms.TreeNode("�S��", 1, 1),
																					 new System.Windows.Forms.TreeNode("��p�^�[��")});
			this.treeCategory.ShowLines = false;
			this.treeCategory.ShowPlusMinus = false;
			this.treeCategory.ShowRootLines = false;
			this.treeCategory.Size = new System.Drawing.Size(101, 296);
			this.treeCategory.TabIndex = 0;
			this.treeCategory.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeCategory_AfterSelect);
			// 
			// imageListConfigTreeIcon
			// 
			this.imageListConfigTreeIcon.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
			this.imageListConfigTreeIcon.ImageSize = new System.Drawing.Size(32, 32);
			this.imageListConfigTreeIcon.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListConfigTreeIcon.ImageStream")));
			this.imageListConfigTreeIcon.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// buttonCancel
			// 
			this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonCancel.Location = new System.Drawing.Point(303, 267);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new System.Drawing.Size(87, 21);
			this.buttonCancel.TabIndex = 1;
			this.buttonCancel.Text = "�L�����Z��";
			// 
			// buttonApply
			// 
			this.buttonApply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonApply.Enabled = false;
			this.buttonApply.Location = new System.Drawing.Point(397, 267);
			this.buttonApply.Name = "buttonApply";
			this.buttonApply.Size = new System.Drawing.Size(87, 21);
			this.buttonApply.TabIndex = 2;
			this.buttonApply.Text = "�K�p(&A)";
			// 
			// buttonOK
			// 
			this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.buttonOK.Location = new System.Drawing.Point(208, 267);
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
			this.labelTitle.Location = new System.Drawing.Point(107, 5);
			this.labelTitle.Name = "labelTitle";
			this.labelTitle.Size = new System.Drawing.Size(380, 27);
			this.labelTitle.TabIndex = 4;
			this.labelTitle.Text = "�S��";
			this.labelTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// panelContainer
			// 
			this.panelContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.panelContainer.Controls.Add(this.panelAppearance);
			this.panelContainer.Controls.Add(this.panelFacePatternList);
			this.panelContainer.Location = new System.Drawing.Point(106, 38);
			this.panelContainer.Name = "panelContainer";
			this.panelContainer.Size = new System.Drawing.Size(380, 218);
			this.panelContainer.TabIndex = 5;
			// 
			// panelFacePatternList
			// 
			this.panelFacePatternList.Controls.Add(this.label4);
			this.panelFacePatternList.Controls.Add(this.linkWebSite);
			this.panelFacePatternList.Controls.Add(this.buttonBrowse);
			this.panelFacePatternList.Controls.Add(this.listViewFaces);
			this.panelFacePatternList.Location = new System.Drawing.Point(2, 1);
			this.panelFacePatternList.Name = "panelFacePatternList";
			this.panelFacePatternList.Size = new System.Drawing.Size(367, 213);
			this.panelFacePatternList.TabIndex = 7;
			// 
			// label4
			// 
			this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label4.Location = new System.Drawing.Point(4, 165);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(63, 17);
			this.label4.TabIndex = 3;
			this.label4.Text = "�E�F�u�T�C�g:";
			// 
			// linkWebSite
			// 
			this.linkWebSite.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.linkWebSite.Location = new System.Drawing.Point(67, 165);
			this.linkWebSite.Name = "linkWebSite";
			this.linkWebSite.Size = new System.Drawing.Size(388, 16);
			this.linkWebSite.TabIndex = 2;
			this.linkWebSite.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkWebSite_LinkClicked);
			// 
			// buttonBrowse
			// 
			this.buttonBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonBrowse.Location = new System.Drawing.Point(267, 187);
			this.buttonBrowse.Name = "buttonBrowse";
			this.buttonBrowse.Size = new System.Drawing.Size(95, 24);
			this.buttonBrowse.TabIndex = 1;
			this.buttonBrowse.Text = "�Q��(&B)...";
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
			this.listViewFaces.Size = new System.Drawing.Size(367, 161);
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
			this.panelAppearance.Controls.Add(this.label5);
			this.panelAppearance.Controls.Add(this.checkMouseMessage);
			this.panelAppearance.Controls.Add(this.label3);
			this.panelAppearance.Controls.Add(this.label2);
			this.panelAppearance.Controls.Add(this.label1);
			this.panelAppearance.Controls.Add(this.trackBarOpacity);
			this.panelAppearance.Location = new System.Drawing.Point(4, 3);
			this.panelAppearance.Name = "panelAppearance";
			this.panelAppearance.Size = new System.Drawing.Size(366, 194);
			this.panelAppearance.TabIndex = 0;
			// 
			// checkMouseMessage
			// 
			this.checkMouseMessage.Location = new System.Drawing.Point(5, 115);
			this.checkMouseMessage.Name = "checkMouseMessage";
			this.checkMouseMessage.Size = new System.Drawing.Size(400, 16);
			this.checkMouseMessage.TabIndex = 4;
			this.checkMouseMessage.Text = "�}�E�X�̓����w�ʂ̃E�B���h�E�֓`����(&T)";
			// 
			// label3
			// 
			this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label3.Location = new System.Drawing.Point(524, 73);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(45, 11);
			this.label3.TabIndex = 3;
			this.label3.Text = "�s����";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(19, 67);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(31, 11);
			this.label2.TabIndex = 2;
			this.label2.Text = "����";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(2, 5);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(196, 18);
			this.label1.TabIndex = 1;
			this.label1.Text = "�����x:";
			// 
			// trackBarOpacity
			// 
			this.trackBarOpacity.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.trackBarOpacity.LargeChange = 10;
			this.trackBarOpacity.Location = new System.Drawing.Point(15, 24);
			this.trackBarOpacity.Maximum = 100;
			this.trackBarOpacity.Name = "trackBarOpacity";
			this.trackBarOpacity.Size = new System.Drawing.Size(338, 37);
			this.trackBarOpacity.TabIndex = 0;
			this.trackBarOpacity.TickFrequency = 10;
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(309, 67);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(42, 11);
			this.label5.TabIndex = 5;
			this.label5.Text = "�s����";
			// 
			// ConfigurationForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 12);
			this.ClientSize = new System.Drawing.Size(492, 297);
			this.Controls.Add(this.panelContainer);
			this.Controls.Add(this.labelTitle);
			this.Controls.Add(this.buttonOK);
			this.Controls.Add(this.buttonApply);
			this.Controls.Add(this.buttonCancel);
			this.Controls.Add(this.treeCategory);
			this.MinimumSize = new System.Drawing.Size(500, 140);
			this.Name = "ConfigurationForm";
			this.Text = "MacFace �̐ݒ�";
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
			// �S�ʂ�\���B
			foreach (Panel pane in panelContainer.Controls) 
			{
				pane.Visible = false;
				pane.Dock = DockStyle.Fill;
			}

			panelAppearance.Show();

			// �����l���Z�b�g�B
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
			// �Ă��Ƃ�
			if (e.Node.Text == labelTitle.Text) return;
			foreach (Panel pane in panelContainer.Controls)
				pane.Visible = false;
			switch (e.Node.Text) 
			{
				case "�S��":
					panelAppearance.Show();
					break;
				case "��p�^�[��":
					listViewFaces.Items.Clear();
					imageListFacePreviews.Images.Clear();
					panelFacePatternList.Show();

					using (Bitmap tmpImage = new Bitmap(128, 128)) 
					{
						// ��p�^�[�����X�g�����B
						// TODO: ���̂܂܂��ƒx���̂ŋ߂������ɔ񓯊��ɂ��悤�B
						// �A�v���P�[�V�����f�B���N�g���̉��B
						foreach (DirectoryInfo dir in (new DirectoryInfo(Application.StartupPath)).GetDirectories("*.mcface"))
						{
							AddPreviewListItem(dir.FullName);
						}
						// %USERPROFILE%\Application Data\MacFace\Faces ���B
						string userFaceDefPath = Path.Combine(Configuration.ConfigurationPath, "Faces");
						if (Directory.Exists(userFaceDefPath)) 
						{
							foreach (DirectoryInfo dir in (new DirectoryInfo(userFaceDefPath).GetDirectories("*.mcface")))
							{
								AddPreviewListItem(dir.FullName);
							}
						}
					}
					break;
			}

			labelTitle.Text = e.Node.Text;
		}


		private void AddPreviewListItem(string path)
		{
			try 
			{
				FaceDef faceDef = new FaceDef(path);
				// �\��/�I�������ۂɉ����Ȃ�Ȃ��悤�ɂ��炩���ߔ��h�肵�ĕ`�悵���摜��p�ӂ���B
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
				item.SubItems.Add(path);    // 0: �p�X
				item.SubItems.Add(faceDef.Author);  // 1: �����
				item.SubItems.Add(faceDef.Version); // 2: �o�[�W����
				item.SubItems.Add(
					(faceDef.WebSite != null ?
					faceDef.WebSite.ToString() : "")); // 3: �E�F�u�T�C�g
			} 
			catch (Exception ex) 
			{
				// TODO: Exception ��������Ƌ��߂�B
				MessageBox.Show(ex.ToString(), "MacFace for Windows", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void listViewFaces_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			ListViewItem item = listViewFaces.GetItemAt(e.X, e.Y);
			
			if (item != null) 
			{
				string tooltipText = String.Format("�^�C�g��: {0}\n�����: {1}\n�o�[�W����: {2}",
					item.Text, item.SubItems[2].Text, item.SubItems[3].Text);
                
				if (toolTipPreviewDetail.GetToolTip(listViewFaces) != tooltipText)
					toolTipPreviewDetail.SetToolTip(listViewFaces, tooltipText);
			} 
			else 
			{
				// �����B
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
			folderBrowser.Description = "��p�^�[���t�@�C���̑��݂���t�H���_��I�����Ă��������B";
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
