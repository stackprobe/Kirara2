namespace Charlotte
{
	partial class SettingDlg
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingDlg));
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnOk = new System.Windows.Forms.Button();
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabPage3 = new System.Windows.Forms.TabPage();
			this.cbDoubleMovie = new System.Windows.Forms.CheckBox();
			this.cbInstantMessagesDisabled = new System.Windows.Forms.CheckBox();
			this.tabPage2 = new System.Windows.Forms.TabPage();
			this.cbConvBypassまとめて実行 = new System.Windows.Forms.CheckBox();
			this.cbAutoPlayTop = new System.Windows.Forms.CheckBox();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.cbReportToLogDisabled = new System.Windows.Forms.CheckBox();
			this.cbConvWavMastering = new System.Windows.Forms.CheckBox();
			this.doubleMovieOption = new System.Windows.Forms.GroupBox();
			this.label1 = new System.Windows.Forms.Label();
			this.doubleMovie_darknessPct = new System.Windows.Forms.NumericUpDown();
			this.label2 = new System.Windows.Forms.Label();
			this.doubleMovie_bokashiLevel = new System.Windows.Forms.NumericUpDown();
			this.tabControl1.SuspendLayout();
			this.tabPage3.SuspendLayout();
			this.tabPage2.SuspendLayout();
			this.tabPage1.SuspendLayout();
			this.doubleMovieOption.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.doubleMovie_darknessPct)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.doubleMovie_bokashiLevel)).BeginInit();
			this.SuspendLayout();
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.Location = new System.Drawing.Point(436, 476);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(101, 39);
			this.btnCancel.TabIndex = 2;
			this.btnCancel.Text = "キャンセル";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// btnOk
			// 
			this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnOk.Location = new System.Drawing.Point(329, 476);
			this.btnOk.Name = "btnOk";
			this.btnOk.Size = new System.Drawing.Size(101, 39);
			this.btnOk.TabIndex = 1;
			this.btnOk.Text = "OK";
			this.btnOk.UseVisualStyleBackColor = true;
			this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
			// 
			// tabControl1
			// 
			this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tabControl1.Controls.Add(this.tabPage3);
			this.tabControl1.Controls.Add(this.tabPage2);
			this.tabControl1.Controls.Add(this.tabPage1);
			this.tabControl1.Location = new System.Drawing.Point(12, 12);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(525, 458);
			this.tabControl1.TabIndex = 0;
			// 
			// tabPage3
			// 
			this.tabPage3.Controls.Add(this.doubleMovieOption);
			this.tabPage3.Controls.Add(this.cbDoubleMovie);
			this.tabPage3.Controls.Add(this.cbInstantMessagesDisabled);
			this.tabPage3.Location = new System.Drawing.Point(4, 29);
			this.tabPage3.Name = "tabPage3";
			this.tabPage3.Size = new System.Drawing.Size(517, 425);
			this.tabPage3.TabIndex = 2;
			this.tabPage3.Text = "スクリーン";
			this.tabPage3.UseVisualStyleBackColor = true;
			// 
			// cbDoubleMovie
			// 
			this.cbDoubleMovie.AutoSize = true;
			this.cbDoubleMovie.Location = new System.Drawing.Point(20, 50);
			this.cbDoubleMovie.Name = "cbDoubleMovie";
			this.cbDoubleMovie.Size = new System.Drawing.Size(444, 24);
			this.cbDoubleMovie.TabIndex = 1;
			this.cbDoubleMovie.Text = "映像を二重に表示して、余白の部分にも映像が表示されるようにする。";
			this.cbDoubleMovie.UseVisualStyleBackColor = true;
			this.cbDoubleMovie.CheckedChanged += new System.EventHandler(this.cbDoubleMovie_CheckedChanged);
			// 
			// cbInstantMessagesDisabled
			// 
			this.cbInstantMessagesDisabled.AutoSize = true;
			this.cbInstantMessagesDisabled.Location = new System.Drawing.Point(20, 20);
			this.cbInstantMessagesDisabled.Name = "cbInstantMessagesDisabled";
			this.cbInstantMessagesDisabled.Size = new System.Drawing.Size(275, 24);
			this.cbInstantMessagesDisabled.TabIndex = 0;
			this.cbInstantMessagesDisabled.Text = "インスタント・メッセージを表示しない。";
			this.cbInstantMessagesDisabled.UseVisualStyleBackColor = true;
			// 
			// tabPage2
			// 
			this.tabPage2.Controls.Add(this.cbConvBypassまとめて実行);
			this.tabPage2.Controls.Add(this.cbAutoPlayTop);
			this.tabPage2.Location = new System.Drawing.Point(4, 29);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Size = new System.Drawing.Size(517, 425);
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "プレイリスト";
			this.tabPage2.UseVisualStyleBackColor = true;
			// 
			// cbConvBypassまとめて実行
			// 
			this.cbConvBypassまとめて実行.AutoSize = true;
			this.cbConvBypassまとめて実行.Location = new System.Drawing.Point(20, 50);
			this.cbConvBypassまとめて実行.Name = "cbConvBypassまとめて実行";
			this.cbConvBypassまとめて実行.Size = new System.Drawing.Size(270, 24);
			this.cbConvBypassまとめて実行.TabIndex = 1;
			this.cbConvBypassまとめて実行.Text = ".ogX のコンバートをまとめて実行する。";
			this.cbConvBypassまとめて実行.UseVisualStyleBackColor = true;
			// 
			// cbAutoPlayTop
			// 
			this.cbAutoPlayTop.AutoSize = true;
			this.cbAutoPlayTop.Location = new System.Drawing.Point(20, 20);
			this.cbAutoPlayTop.Name = "cbAutoPlayTop";
			this.cbAutoPlayTop.Size = new System.Drawing.Size(366, 24);
			this.cbAutoPlayTop.TabIndex = 0;
			this.cbAutoPlayTop.Text = "最初のアイテムが再生可能になったら自動的に再生する。";
			this.cbAutoPlayTop.UseVisualStyleBackColor = true;
			// 
			// tabPage1
			// 
			this.tabPage1.Controls.Add(this.cbReportToLogDisabled);
			this.tabPage1.Controls.Add(this.cbConvWavMastering);
			this.tabPage1.Location = new System.Drawing.Point(4, 29);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage1.Size = new System.Drawing.Size(517, 425);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "コンバート";
			this.tabPage1.UseVisualStyleBackColor = true;
			// 
			// cbReportToLogDisabled
			// 
			this.cbReportToLogDisabled.AutoSize = true;
			this.cbReportToLogDisabled.Location = new System.Drawing.Point(20, 50);
			this.cbReportToLogDisabled.Name = "cbReportToLogDisabled";
			this.cbReportToLogDisabled.Size = new System.Drawing.Size(327, 24);
			this.cbReportToLogDisabled.TabIndex = 1;
			this.cbReportToLogDisabled.Text = "コマンドの出力やレポートをログに書き出さない。";
			this.cbReportToLogDisabled.UseVisualStyleBackColor = true;
			// 
			// cbConvWavMastering
			// 
			this.cbConvWavMastering.AutoSize = true;
			this.cbConvWavMastering.Location = new System.Drawing.Point(20, 20);
			this.cbConvWavMastering.Name = "cbConvWavMastering";
			this.cbConvWavMastering.Size = new System.Drawing.Size(314, 24);
			this.cbConvWavMastering.TabIndex = 0;
			this.cbConvWavMastering.Text = "ついでに音量のノーマライズ（均一化）も行う。";
			this.cbConvWavMastering.UseVisualStyleBackColor = true;
			// 
			// doubleMovieOption
			// 
			this.doubleMovieOption.Controls.Add(this.doubleMovie_bokashiLevel);
			this.doubleMovieOption.Controls.Add(this.label2);
			this.doubleMovieOption.Controls.Add(this.doubleMovie_darknessPct);
			this.doubleMovieOption.Controls.Add(this.label1);
			this.doubleMovieOption.Location = new System.Drawing.Point(20, 80);
			this.doubleMovieOption.Name = "doubleMovieOption";
			this.doubleMovieOption.Size = new System.Drawing.Size(470, 130);
			this.doubleMovieOption.TabIndex = 2;
			this.doubleMovieOption.TabStop = false;
			this.doubleMovieOption.Text = "背後の映像";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(20, 40);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(261, 20);
			this.label1.TabIndex = 0;
			this.label1.Text = "背景の暗さ ( 0 ～ 100 : 明るい ～ 暗い ) :";
			// 
			// doubleMovie_darkness
			// 
			this.doubleMovie_darknessPct.Location = new System.Drawing.Point(313, 38);
			this.doubleMovie_darknessPct.Name = "doubleMovie_darkness";
			this.doubleMovie_darknessPct.Size = new System.Drawing.Size(70, 27);
			this.doubleMovie_darknessPct.TabIndex = 1;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(20, 80);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(287, 20);
			this.label2.TabIndex = 2;
			this.label2.Text = "ぼかしレベル ( 0 ～ 100 : ぼかし無し ～ 強 ) :";
			// 
			// doubleMovie_bokashiLevel
			// 
			this.doubleMovie_bokashiLevel.Location = new System.Drawing.Point(313, 78);
			this.doubleMovie_bokashiLevel.Name = "doubleMovie_bokashiLevel";
			this.doubleMovie_bokashiLevel.Size = new System.Drawing.Size(70, 27);
			this.doubleMovie_bokashiLevel.TabIndex = 3;
			// 
			// SettingDlg
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(549, 527);
			this.Controls.Add(this.tabControl1);
			this.Controls.Add(this.btnOk);
			this.Controls.Add(this.btnCancel);
			this.Font = new System.Drawing.Font("メイリオ", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "SettingDlg";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Kirara / Setting";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SettingDlg_FormClosing);
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.SettingDlg_FormClosed);
			this.Load += new System.EventHandler(this.SettingDlg_Load);
			this.Shown += new System.EventHandler(this.SettingDlg_Shown);
			this.tabControl1.ResumeLayout(false);
			this.tabPage3.ResumeLayout(false);
			this.tabPage3.PerformLayout();
			this.tabPage2.ResumeLayout(false);
			this.tabPage2.PerformLayout();
			this.tabPage1.ResumeLayout(false);
			this.tabPage1.PerformLayout();
			this.doubleMovieOption.ResumeLayout(false);
			this.doubleMovieOption.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.doubleMovie_darknessPct)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.doubleMovie_bokashiLevel)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnOk;
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tabPage1;
		private System.Windows.Forms.CheckBox cbConvWavMastering;
		private System.Windows.Forms.TabPage tabPage2;
		private System.Windows.Forms.CheckBox cbAutoPlayTop;
		private System.Windows.Forms.TabPage tabPage3;
		private System.Windows.Forms.CheckBox cbInstantMessagesDisabled;
		private System.Windows.Forms.CheckBox cbReportToLogDisabled;
		private System.Windows.Forms.CheckBox cbConvBypassまとめて実行;
		private System.Windows.Forms.CheckBox cbDoubleMovie;
		private System.Windows.Forms.GroupBox doubleMovieOption;
		private System.Windows.Forms.NumericUpDown doubleMovie_bokashiLevel;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.NumericUpDown doubleMovie_darknessPct;
		private System.Windows.Forms.Label label1;
	}
}
