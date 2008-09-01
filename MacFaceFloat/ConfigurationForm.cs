/*
 * $Id$
 */
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
	/// ConfigurationForm �̊T�v�̐����ł��B
	/// </summary>
	public class ConfigurationForm : System.Windows.Forms.Form
	{
		private Configuration _config;
		private System.Windows.Forms.ImageList imageListFacePreviews;
		private System.Windows.Forms.ToolTip toolTipPreviewDetail;
		private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox checkMouseMessage;
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
        private LinkLabel linkWebSite;
        private Label label1;
        private Label label3;
        private Label label4;
        private Label lblAuthor;
        private Label lblVersion;
        private Label label9;
        private Label lblTitle;
		private System.ComponentModel.IContainer components;

		public event ConfigChangedEvent ConfigChanged;

		public ConfigurationForm()
		{
			//
			// Windows �t�H�[�� �f�U�C�i �T�|�[�g�ɕK�v�ł��B
			//
			InitializeComponent();

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConfigurationForm));
            this.imageListFacePreviews = new System.Windows.Forms.ImageList(this.components);
            this.label6 = new System.Windows.Forms.Label();
            this.toolTipPreviewDetail = new System.Windows.Forms.ToolTip(this.components);
            this.checkMouseMessage = new System.Windows.Forms.CheckBox();
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
            this.linkWebSite = new System.Windows.Forms.LinkLabel();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lblAuthor = new System.Windows.Forms.Label();
            this.lblVersion = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
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
            this.label6.Text = "�傫��:";
            // 
            // checkMouseMessage
            // 
            this.checkMouseMessage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.checkMouseMessage.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.checkMouseMessage.Location = new System.Drawing.Point(216, 255);
            this.checkMouseMessage.Name = "checkMouseMessage";
            this.checkMouseMessage.Size = new System.Drawing.Size(266, 16);
            this.checkMouseMessage.TabIndex = 6;
            this.checkMouseMessage.Text = "�}�E�X�̓����w�ʂ̃E�B���h�E�֓`����(&T)";
            this.checkMouseMessage.Click += new System.EventHandler(this.checkMouseMessage_Click);
            // 
            // buttonBrowse
            // 
            this.buttonBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonBrowse.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.buttonBrowse.Location = new System.Drawing.Point(631, 98);
            this.buttonBrowse.Name = "buttonBrowse";
            this.buttonBrowse.Size = new System.Drawing.Size(95, 24);
            this.buttonBrowse.TabIndex = 13;
            this.buttonBrowse.Text = "�Q��(&B)...";
            this.buttonBrowse.Click += new System.EventHandler(this.buttonBrowse_Click);
            // 
            // listViewFaces
            // 
            this.listViewFaces.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.listViewFaces.LargeImageList = this.imageListFacePreviews;
            this.listViewFaces.Location = new System.Drawing.Point(12, 12);
            this.listViewFaces.MultiSelect = false;
            this.listViewFaces.Name = "listViewFaces";
            this.listViewFaces.Size = new System.Drawing.Size(198, 325);
            this.listViewFaces.TabIndex = 12;
            this.listViewFaces.UseCompatibleStateImageBehavior = false;
            this.listViewFaces.SelectedIndexChanged += new System.EventHandler(this.listViewFaces_SelectedIndexChanged);
            this.listViewFaces.MouseMove += new System.Windows.Forms.MouseEventHandler(this.listViewFaces_MouseMove);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.trackBarOpacity);
            this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox1.Location = new System.Drawing.Point(216, 123);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(266, 60);
            this.groupBox1.TabIndex = 19;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "�����x";
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.Location = new System.Drawing.Point(210, 24);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(48, 11);
            this.label5.TabIndex = 12;
            this.label5.Text = "�s����";
            this.label5.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(8, 24);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(40, 11);
            this.label2.TabIndex = 11;
            this.label2.Text = "����";
            // 
            // trackBarOpacity
            // 
            this.trackBarOpacity.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.trackBarOpacity.LargeChange = 10;
            this.trackBarOpacity.Location = new System.Drawing.Point(54, 12);
            this.trackBarOpacity.Maximum = 100;
            this.trackBarOpacity.Minimum = 5;
            this.trackBarOpacity.Name = "trackBarOpacity";
            this.trackBarOpacity.Size = new System.Drawing.Size(158, 45);
            this.trackBarOpacity.TabIndex = 9;
            this.trackBarOpacity.TickFrequency = 10;
            this.trackBarOpacity.Value = 100;
            this.trackBarOpacity.ValueChanged += new System.EventHandler(this.trackBarOpacity_ValueChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.trackBarPatternSize);
            this.groupBox2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox2.Location = new System.Drawing.Point(216, 189);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(266, 60);
            this.groupBox2.TabIndex = 20;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "�p�^�[���̑傫��";
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label7.Location = new System.Drawing.Point(218, 24);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(40, 11);
            this.label7.TabIndex = 21;
            this.label7.Text = "�ő�";
            this.label7.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(8, 24);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(40, 11);
            this.label8.TabIndex = 20;
            this.label8.Text = "�ŏ�";
            // 
            // trackBarPatternSize
            // 
            this.trackBarPatternSize.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.trackBarPatternSize.LargeChange = 10;
            this.trackBarPatternSize.Location = new System.Drawing.Point(54, 12);
            this.trackBarPatternSize.Maximum = 100;
            this.trackBarPatternSize.Minimum = 10;
            this.trackBarPatternSize.Name = "trackBarPatternSize";
            this.trackBarPatternSize.Size = new System.Drawing.Size(158, 45);
            this.trackBarPatternSize.TabIndex = 19;
            this.trackBarPatternSize.TickFrequency = 10;
            this.trackBarPatternSize.Value = 100;
            this.trackBarPatternSize.ValueChanged += new System.EventHandler(this.trackBarPatternSize_ValueChanged);
            // 
            // linkWebSite
            // 
            this.linkWebSite.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.linkWebSite.AutoEllipsis = true;
            this.linkWebSite.Location = new System.Drawing.Point(283, 66);
            this.linkWebSite.Name = "linkWebSite";
            this.linkWebSite.Size = new System.Drawing.Size(199, 12);
            this.linkWebSite.TabIndex = 14;
            this.linkWebSite.TabStop = true;
            this.linkWebSite.Text = "--";
            this.linkWebSite.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkWebSite_LinkClicked);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(216, 66);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 17);
            this.label1.TabIndex = 15;
            this.label1.Text = "�E�F�u�T�C�g:";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(216, 32);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(61, 17);
            this.label3.TabIndex = 22;
            this.label3.Text = "�����:";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(216, 49);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(61, 17);
            this.label4.TabIndex = 22;
            this.label4.Text = "�o�[�W����:";
            // 
            // lblAuthor
            // 
            this.lblAuthor.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblAuthor.Location = new System.Drawing.Point(283, 32);
            this.lblAuthor.Name = "lblAuthor";
            this.lblAuthor.Size = new System.Drawing.Size(199, 12);
            this.lblAuthor.TabIndex = 23;
            this.lblAuthor.Text = "--";
            // 
            // lblVersion
            // 
            this.lblVersion.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblVersion.Location = new System.Drawing.Point(283, 49);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(199, 12);
            this.lblVersion.TabIndex = 24;
            this.lblVersion.Text = "--";
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(216, 15);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(61, 17);
            this.label9.TabIndex = 22;
            this.label9.Text = "����:";
            // 
            // lblTitle
            // 
            this.lblTitle.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblTitle.Location = new System.Drawing.Point(283, 15);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(199, 12);
            this.lblTitle.TabIndex = 23;
            this.lblTitle.Text = "--";
            // 
            // ConfigurationForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 12);
            this.ClientSize = new System.Drawing.Size(494, 349);
            this.Controls.Add(this.lblVersion);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.lblAuthor);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.linkWebSite);
            this.Controls.Add(this.buttonBrowse);
            this.Controls.Add(this.listViewFaces);
            this.Controls.Add(this.checkMouseMessage);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(500, 300);
            this.Name = "ConfigurationForm";
            this.Text = "MacFace �̐ݒ�";
            this.Load += new System.EventHandler(this.ConfigurationForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarOpacity)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarPatternSize)).EndInit();
            this.ResumeLayout(false);

		}
		#endregion

		private void ConfigurationForm_Load(object sender, System.EventArgs e)
		{
			// �����l�̃Z�b�g
			trackBarOpacity.Value = _config.Opacity;
			trackBarPatternSize.Value = _config.PatternSize;
			checkMouseMessage.Checked = _config.TransparentMouseMessage;

			// ��p�^�[�����X�g�����B
			// TODO: ���̂܂܂��ƒx���̂ŋ߂������ɔ񓯊��ɂ��悤�B
			using (Bitmap tmpImage = new Bitmap(128, 128)) 
			{
				// �A�v���P�[�V�����f�B���N�g���̉���T��
				foreach (DirectoryInfo dir in (new DirectoryInfo(Application.StartupPath)).GetDirectories("*.mcface"))
				{
					AddPreviewListItem(dir.FullName);
				}
				// %USERPROFILE%\Application Data\MacFace\Faces �̉���T��
				string userFaceDefPath = Path.Combine(Configuration.ConfigurationPath, "Faces");
				if (Directory.Exists(userFaceDefPath)) 
				{
					foreach (DirectoryInfo dir in (new DirectoryInfo(userFaceDefPath).GetDirectories("*.mcface")))
					{
						AddPreviewListItem(dir.FullName);
					}
				}
			}
            listViewFaces.Focus();
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

                if (_config.FaceDefPath == path)
                {
                    item.Selected = true;
                }
			} 
			catch (Exception ex) 
			{
				// TODO: Exception ��������Ƌ��߂�B
				MessageBox.Show(ex.ToString(), "MacFace for Windows", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
			if (listViewFaces.SelectedItems.Count != 0) 
			{
				ListViewItem item = listViewFaces.SelectedItems[0];
                lblTitle.Text = item.Text;
                lblAuthor.Text = item.SubItems[2].Text;
                lblVersion.Text = item.SubItems[3].Text;
                linkWebSite.Text = item.SubItems[4].Text;

                _config.FaceDefPath = item.SubItems[1].Text;
				ConfigChanged();
            }
			else 
			{
                lblTitle.Text = "";
                lblAuthor.Text = "";
                lblVersion.Text = "";
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
