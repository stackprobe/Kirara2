namespace Charlotte
{
	partial class BgConvWin
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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BgConvWin));
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.リストLToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.選択解除KToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
			this.選択されている項目をクリアLToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.完了した項目をクリアFToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.エラーになった項目をクリアEToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.エラー完了した項目をクリアDToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.全てクリアCToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
			this.閉じるXToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.statusStrip1 = new System.Windows.Forms.StatusStrip();
			this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.btnDestDir = new System.Windows.Forms.Button();
			this.txtDestDir = new System.Windows.Forms.TextBox();
			this.bciSheet = new System.Windows.Forms.DataGridView();
			this.mainTimer = new System.Windows.Forms.Timer(this.components);
			this.menuStrip1.SuspendLayout();
			this.statusStrip1.SuspendLayout();
			this.groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.bciSheet)).BeginInit();
			this.SuspendLayout();
			// 
			// menuStrip1
			// 
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.リストLToolStripMenuItem});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(539, 26);
			this.menuStrip1.TabIndex = 0;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// リストLToolStripMenuItem
			// 
			this.リストLToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.選択解除KToolStripMenuItem,
            this.toolStripMenuItem1,
            this.選択されている項目をクリアLToolStripMenuItem,
            this.完了した項目をクリアFToolStripMenuItem,
            this.エラーになった項目をクリアEToolStripMenuItem,
            this.エラー完了した項目をクリアDToolStripMenuItem,
            this.全てクリアCToolStripMenuItem,
            this.toolStripMenuItem2,
            this.閉じるXToolStripMenuItem});
			this.リストLToolStripMenuItem.Name = "リストLToolStripMenuItem";
			this.リストLToolStripMenuItem.Size = new System.Drawing.Size(73, 22);
			this.リストLToolStripMenuItem.Text = "リスト(&L)";
			// 
			// 選択解除KToolStripMenuItem
			// 
			this.選択解除KToolStripMenuItem.Name = "選択解除KToolStripMenuItem";
			this.選択解除KToolStripMenuItem.Size = new System.Drawing.Size(263, 22);
			this.選択解除KToolStripMenuItem.Text = "選択解除(&K)";
			this.選択解除KToolStripMenuItem.Click += new System.EventHandler(this.選択解除KToolStripMenuItem_Click);
			// 
			// toolStripMenuItem1
			// 
			this.toolStripMenuItem1.Name = "toolStripMenuItem1";
			this.toolStripMenuItem1.Size = new System.Drawing.Size(260, 6);
			// 
			// 選択されている項目をクリアLToolStripMenuItem
			// 
			this.選択されている項目をクリアLToolStripMenuItem.Name = "選択されている項目をクリアLToolStripMenuItem";
			this.選択されている項目をクリアLToolStripMenuItem.Size = new System.Drawing.Size(263, 22);
			this.選択されている項目をクリアLToolStripMenuItem.Text = "選択されている項目をクリア(&L)";
			this.選択されている項目をクリアLToolStripMenuItem.Click += new System.EventHandler(this.選択されている項目をクリアLToolStripMenuItem_Click);
			// 
			// 完了した項目をクリアFToolStripMenuItem
			// 
			this.完了した項目をクリアFToolStripMenuItem.Name = "完了した項目をクリアFToolStripMenuItem";
			this.完了した項目をクリアFToolStripMenuItem.Size = new System.Drawing.Size(263, 22);
			this.完了した項目をクリアFToolStripMenuItem.Text = "完了した項目をクリア(&F)";
			this.完了した項目をクリアFToolStripMenuItem.Click += new System.EventHandler(this.完了した項目をクリアFToolStripMenuItem_Click);
			// 
			// エラーになった項目をクリアEToolStripMenuItem
			// 
			this.エラーになった項目をクリアEToolStripMenuItem.Name = "エラーになった項目をクリアEToolStripMenuItem";
			this.エラーになった項目をクリアEToolStripMenuItem.Size = new System.Drawing.Size(263, 22);
			this.エラーになった項目をクリアEToolStripMenuItem.Text = "エラーになった項目をクリア(&E)";
			this.エラーになった項目をクリアEToolStripMenuItem.Click += new System.EventHandler(this.エラーになった項目をクリアEToolStripMenuItem_Click);
			// 
			// エラー完了した項目をクリアDToolStripMenuItem
			// 
			this.エラー完了した項目をクリアDToolStripMenuItem.Name = "エラー完了した項目をクリアDToolStripMenuItem";
			this.エラー完了した項目をクリアDToolStripMenuItem.Size = new System.Drawing.Size(263, 22);
			this.エラー完了した項目をクリアDToolStripMenuItem.Text = "エラー・完了した項目をクリア(&D)";
			this.エラー完了した項目をクリアDToolStripMenuItem.Click += new System.EventHandler(this.エラー完了した項目をクリアDToolStripMenuItem_Click);
			// 
			// 全てクリアCToolStripMenuItem
			// 
			this.全てクリアCToolStripMenuItem.Name = "全てクリアCToolStripMenuItem";
			this.全てクリアCToolStripMenuItem.Size = new System.Drawing.Size(263, 22);
			this.全てクリアCToolStripMenuItem.Text = "全てクリア(&C)";
			this.全てクリアCToolStripMenuItem.Click += new System.EventHandler(this.全てクリアCToolStripMenuItem_Click);
			// 
			// toolStripMenuItem2
			// 
			this.toolStripMenuItem2.Name = "toolStripMenuItem2";
			this.toolStripMenuItem2.Size = new System.Drawing.Size(260, 6);
			// 
			// 閉じるXToolStripMenuItem
			// 
			this.閉じるXToolStripMenuItem.Name = "閉じるXToolStripMenuItem";
			this.閉じるXToolStripMenuItem.Size = new System.Drawing.Size(263, 22);
			this.閉じるXToolStripMenuItem.Text = "閉じる(&X)";
			this.閉じるXToolStripMenuItem.Click += new System.EventHandler(this.閉じるXToolStripMenuItem_Click);
			// 
			// statusStrip1
			// 
			this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatus});
			this.statusStrip1.Location = new System.Drawing.Point(0, 494);
			this.statusStrip1.Name = "statusStrip1";
			this.statusStrip1.Size = new System.Drawing.Size(539, 23);
			this.statusStrip1.TabIndex = 3;
			this.statusStrip1.Text = "statusStrip1";
			// 
			// lblStatus
			// 
			this.lblStatus.Name = "lblStatus";
			this.lblStatus.Size = new System.Drawing.Size(524, 18);
			this.lblStatus.Spring = true;
			this.lblStatus.Text = "準備しています...";
			this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Controls.Add(this.btnDestDir);
			this.groupBox1.Controls.Add(this.txtDestDir);
			this.groupBox1.Location = new System.Drawing.Point(12, 27);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(515, 59);
			this.groupBox1.TabIndex = 1;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "出力先パス";
			// 
			// btnDestDir
			// 
			this.btnDestDir.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnDestDir.Location = new System.Drawing.Point(454, 26);
			this.btnDestDir.Name = "btnDestDir";
			this.btnDestDir.Size = new System.Drawing.Size(55, 27);
			this.btnDestDir.TabIndex = 1;
			this.btnDestDir.Text = "変更";
			this.btnDestDir.UseVisualStyleBackColor = true;
			this.btnDestDir.Click += new System.EventHandler(this.btnDestDir_Click);
			// 
			// txtDestDir
			// 
			this.txtDestDir.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtDestDir.Location = new System.Drawing.Point(6, 26);
			this.txtDestDir.Name = "txtDestDir";
			this.txtDestDir.ReadOnly = true;
			this.txtDestDir.Size = new System.Drawing.Size(449, 27);
			this.txtDestDir.TabIndex = 0;
			this.txtDestDir.TextChanged += new System.EventHandler(this.txtDestDir_TextChanged);
			this.txtDestDir.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtDestDir_KeyPress);
			// 
			// bciSheet
			// 
			this.bciSheet.AllowDrop = true;
			this.bciSheet.AllowUserToAddRows = false;
			this.bciSheet.AllowUserToDeleteRows = false;
			this.bciSheet.AllowUserToResizeRows = false;
			this.bciSheet.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.bciSheet.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.bciSheet.Location = new System.Drawing.Point(12, 92);
			this.bciSheet.Name = "bciSheet";
			this.bciSheet.ReadOnly = true;
			this.bciSheet.RowTemplate.Height = 21;
			this.bciSheet.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.bciSheet.Size = new System.Drawing.Size(515, 400);
			this.bciSheet.TabIndex = 2;
			this.bciSheet.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.bciSheet_CellContentClick);
			this.bciSheet.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.bciSheet_CellContentDoubleClick);
			this.bciSheet.DragDrop += new System.Windows.Forms.DragEventHandler(this.bciSheet_DragDrop);
			this.bciSheet.DragEnter += new System.Windows.Forms.DragEventHandler(this.bciSheet_DragEnter);
			// 
			// mainTimer
			// 
			this.mainTimer.Enabled = true;
			this.mainTimer.Tick += new System.EventHandler(this.mainTimer_Tick);
			// 
			// BgConvWin
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(539, 517);
			this.Controls.Add(this.bciSheet);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.statusStrip1);
			this.Controls.Add(this.menuStrip1);
			this.Font = new System.Drawing.Font("メイリオ", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MainMenuStrip = this.menuStrip1;
			this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.Name = "BgConvWin";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "Kirara / Conversion Dialog";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.BgConvWin_FormClosing);
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.BgConvWin_FormClosed);
			this.Load += new System.EventHandler(this.BgConvWin_Load);
			this.Shown += new System.EventHandler(this.BgConvWin_Shown);
			this.ResizeEnd += new System.EventHandler(this.BgConvWin_ResizeEnd);
			this.Move += new System.EventHandler(this.BgConvWin_Move);
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.statusStrip1.ResumeLayout(false);
			this.statusStrip1.PerformLayout();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.bciSheet)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.StatusStrip statusStrip1;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Button btnDestDir;
		private System.Windows.Forms.TextBox txtDestDir;
		private System.Windows.Forms.DataGridView bciSheet;
		private System.Windows.Forms.ToolStripMenuItem リストLToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem 選択解除KToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem 選択されている項目をクリアLToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem 完了した項目をクリアFToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem エラーになった項目をクリアEToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem エラー完了した項目をクリアDToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem 全てクリアCToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
		private System.Windows.Forms.ToolStripMenuItem 閉じるXToolStripMenuItem;
		private System.Windows.Forms.ToolStripStatusLabel lblStatus;
		private System.Windows.Forms.Timer mainTimer;
	}
}
