using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace MacFace.FloatApp
{
	/// <summary>
	/// StatusWindow �̊T�v�̐����ł��B
	/// </summary>
	public class StatusWindow : System.Windows.Forms.Form
	{
		internal System.Windows.Forms.PictureBox memoryGraphPicBox;
		internal System.Windows.Forms.PictureBox cpuGraphPicBox;
		/// <summary>
		/// �K�v�ȃf�U�C�i�ϐ��ł��B
		/// </summary>
		private System.ComponentModel.Container components = null;

		public StatusWindow()
		{
			//
			// Windows �t�H�[�� �f�U�C�i �T�|�[�g�ɕK�v�ł��B
			//
			InitializeComponent();

			//
			// TODO: InitializeComponent �Ăяo���̌�ɁA�R���X�g���N�^ �R�[�h��ǉ����Ă��������B
			//
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
			this.Text = "�X�e�[�^�X";
			this.ResumeLayout(false);

		}
		#endregion
	}
}
