namespace Charlotte
{
	partial class PlayListWin
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PlayListWin));
			this.statusStrip1 = new System.Windows.Forms.StatusStrip();
			this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.アプリAToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.終了XToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.プレイリストLToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.クリアCToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.エラーになったアイテムをクリアEToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.mainSheet = new System.Windows.Forms.DataGridView();
			this.mainTimer = new System.Windows.Forms.Timer(this.components);
			this.statusStrip1.SuspendLayout();
			this.menuStrip1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.mainSheet)).BeginInit();
			this.SuspendLayout();
			// 
			// statusStrip1
			// 
			this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatus});
			this.statusStrip1.Location = new System.Drawing.Point(0, 439);
			this.statusStrip1.Name = "statusStrip1";
			this.statusStrip1.Size = new System.Drawing.Size(384, 23);
			this.statusStrip1.TabIndex = 0;
			this.statusStrip1.Text = "statusStrip1";
			// 
			// lblStatus
			// 
			this.lblStatus.Name = "lblStatus";
			this.lblStatus.Size = new System.Drawing.Size(369, 18);
			this.lblStatus.Spring = true;
			this.lblStatus.Text = "準備しています...";
			this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// menuStrip1
			// 
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.アプリAToolStripMenuItem,
            this.プレイリストLToolStripMenuItem});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(384, 26);
			this.menuStrip1.TabIndex = 1;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// アプリAToolStripMenuItem
			// 
			this.アプリAToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.終了XToolStripMenuItem});
			this.アプリAToolStripMenuItem.Name = "アプリAToolStripMenuItem";
			this.アプリAToolStripMenuItem.Size = new System.Drawing.Size(74, 22);
			this.アプリAToolStripMenuItem.Text = "アプリ(&A)";
			// 
			// 終了XToolStripMenuItem
			// 
			this.終了XToolStripMenuItem.Name = "終了XToolStripMenuItem";
			this.終了XToolStripMenuItem.Size = new System.Drawing.Size(118, 22);
			this.終了XToolStripMenuItem.Text = "終了(&X)";
			this.終了XToolStripMenuItem.Click += new System.EventHandler(this.終了XToolStripMenuItem_Click);
			// 
			// プレイリストLToolStripMenuItem
			// 
			this.プレイリストLToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.クリアCToolStripMenuItem,
            this.エラーになったアイテムをクリアEToolStripMenuItem});
			this.プレイリストLToolStripMenuItem.Name = "プレイリストLToolStripMenuItem";
			this.プレイリストLToolStripMenuItem.Size = new System.Drawing.Size(109, 22);
			this.プレイリストLToolStripMenuItem.Text = "プレイリスト(&L)";
			// 
			// クリアCToolStripMenuItem
			// 
			this.クリアCToolStripMenuItem.Name = "クリアCToolStripMenuItem";
			this.クリアCToolStripMenuItem.Size = new System.Drawing.Size(273, 22);
			this.クリアCToolStripMenuItem.Text = "クリア(&C)";
			// 
			// エラーになったアイテムをクリアEToolStripMenuItem
			// 
			this.エラーになったアイテムをクリアEToolStripMenuItem.Name = "エラーになったアイテムをクリアEToolStripMenuItem";
			this.エラーになったアイテムをクリアEToolStripMenuItem.Size = new System.Drawing.Size(273, 22);
			this.エラーになったアイテムをクリアEToolStripMenuItem.Text = "エラーになったアイテムをクリア(&E)";
			// 
			// mainSheet
			// 
			this.mainSheet.AllowDrop = true;
			this.mainSheet.AllowUserToAddRows = false;
			this.mainSheet.AllowUserToDeleteRows = false;
			this.mainSheet.AllowUserToResizeRows = false;
			this.mainSheet.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.mainSheet.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.mainSheet.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mainSheet.Location = new System.Drawing.Point(0, 26);
			this.mainSheet.Name = "mainSheet";
			this.mainSheet.ReadOnly = true;
			this.mainSheet.RowTemplate.Height = 21;
			this.mainSheet.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.mainSheet.Size = new System.Drawing.Size(384, 413);
			this.mainSheet.TabIndex = 2;
			this.mainSheet.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.mainSheet_CellContentClick);
			this.mainSheet.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.mainSheet_CellContentDoubleClick);
			this.mainSheet.DragDrop += new System.Windows.Forms.DragEventHandler(this.mainSheet_DragDrop);
			this.mainSheet.DragEnter += new System.Windows.Forms.DragEventHandler(this.mainSheet_DragEnter);
			// 
			// mainTimer
			// 
			this.mainTimer.Enabled = true;
			this.mainTimer.Tick += new System.EventHandler(this.mainTimer_Tick);
			// 
			// PlayListWin
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(384, 462);
			this.Controls.Add(this.mainSheet);
			this.Controls.Add(this.menuStrip1);
			this.Controls.Add(this.statusStrip1);
			this.Font = new System.Drawing.Font("メイリオ", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MainMenuStrip = this.menuStrip1;
			this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.Name = "PlayListWin";
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "Kirara";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.PlayListWin_FormClosing);
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.PlayListWin_FormClosed);
			this.Load += new System.EventHandler(this.PlayListWin_Load);
			this.Shown += new System.EventHandler(this.PlayListWin_Shown);
			this.statusStrip1.ResumeLayout(false);
			this.statusStrip1.PerformLayout();
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.mainSheet)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.StatusStrip statusStrip1;
		private System.Windows.Forms.ToolStripStatusLabel lblStatus;
		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem アプリAToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem 終了XToolStripMenuItem;
		private System.Windows.Forms.DataGridView mainSheet;
		private System.Windows.Forms.ToolStripMenuItem プレイリストLToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem クリアCToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem エラーになったアイテムをクリアEToolStripMenuItem;
		private System.Windows.Forms.Timer mainTimer;
	}
}
