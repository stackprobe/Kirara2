namespace Charlotte
{
	partial class SelectMonitorDlg
	{
		/// <summary>
		/// 必要なデザイナー変数です。
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// 使用中のリソースをすべてクリーンアップします。
		/// </summary>
		/// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows フォーム デザイナーで生成されたコード

		/// <summary>
		/// デザイナー サポートに必要なメソッドです。このメソッドの内容を
		/// コード エディターで変更しないでください。
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SelectMonitorDlg));
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.cmbMonNoScreen = new System.Windows.Forms.ComboBox();
			this.cmbMonNoPlayList = new System.Windows.Forms.ComboBox();
			this.btnモニタ番号確認 = new System.Windows.Forms.Button();
			this.btnOk = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Controls.Add(this.cmbMonNoPlayList);
			this.groupBox1.Controls.Add(this.cmbMonNoScreen);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Location = new System.Drawing.Point(12, 12);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(460, 130);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "モニタ選択";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(16, 38);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(191, 20);
			this.label1.TabIndex = 0;
			this.label1.Text = "スクリーンにするモニタ番号：";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(16, 84);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(230, 20);
			this.label2.TabIndex = 2;
			this.label2.Text = "プレイリストを表示するモニタ番号：";
			// 
			// cmbMonNoScreen
			// 
			this.cmbMonNoScreen.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.cmbMonNoScreen.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbMonNoScreen.FormattingEnabled = true;
			this.cmbMonNoScreen.Location = new System.Drawing.Point(252, 35);
			this.cmbMonNoScreen.Name = "cmbMonNoScreen";
			this.cmbMonNoScreen.Size = new System.Drawing.Size(185, 28);
			this.cmbMonNoScreen.TabIndex = 1;
			this.cmbMonNoScreen.SelectedIndexChanged += new System.EventHandler(this.cmbMonNoScreen_SelectedIndexChanged);
			// 
			// cmbMonNoPlayList
			// 
			this.cmbMonNoPlayList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.cmbMonNoPlayList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbMonNoPlayList.FormattingEnabled = true;
			this.cmbMonNoPlayList.Location = new System.Drawing.Point(252, 81);
			this.cmbMonNoPlayList.Name = "cmbMonNoPlayList";
			this.cmbMonNoPlayList.Size = new System.Drawing.Size(185, 28);
			this.cmbMonNoPlayList.TabIndex = 3;
			this.cmbMonNoPlayList.SelectedIndexChanged += new System.EventHandler(this.cmbMonNoPlayList_SelectedIndexChanged);
			// 
			// btnモニタ番号確認
			// 
			this.btnモニタ番号確認.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.btnモニタ番号確認.Location = new System.Drawing.Point(32, 157);
			this.btnモニタ番号確認.Name = "btnモニタ番号確認";
			this.btnモニタ番号確認.Size = new System.Drawing.Size(417, 40);
			this.btnモニタ番号確認.TabIndex = 1;
			this.btnモニタ番号確認.Text = "モニタ番号を確認する";
			this.btnモニタ番号確認.UseVisualStyleBackColor = true;
			this.btnモニタ番号確認.Click += new System.EventHandler(this.btnモニタ番号確認_Click);
			// 
			// btnOk
			// 
			this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnOk.Location = new System.Drawing.Point(264, 211);
			this.btnOk.Name = "btnOk";
			this.btnOk.Size = new System.Drawing.Size(101, 39);
			this.btnOk.TabIndex = 2;
			this.btnOk.Text = "OK";
			this.btnOk.UseVisualStyleBackColor = true;
			this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.Location = new System.Drawing.Point(371, 211);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(101, 39);
			this.btnCancel.TabIndex = 3;
			this.btnCancel.Text = "キャンセル";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// SelectMonitorDlg
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(484, 262);
			this.Controls.Add(this.btnOk);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnモニタ番号確認);
			this.Controls.Add(this.groupBox1);
			this.Font = new System.Drawing.Font("メイリオ", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "SelectMonitorDlg";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Kirara / モニタ選択";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SelectMonitorDlg_FormClosing);
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.SelectMonitorDlg_FormClosed);
			this.Load += new System.EventHandler(this.SelectMonitorDlg_Load);
			this.Shown += new System.EventHandler(this.SelectMonitorDlg_Shown);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.ComboBox cmbMonNoPlayList;
		private System.Windows.Forms.ComboBox cmbMonNoScreen;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button btnモニタ番号確認;
		private System.Windows.Forms.Button btnOk;
		private System.Windows.Forms.Button btnCancel;
	}
}
