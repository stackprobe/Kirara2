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
			this.plSheet = new System.Windows.Forms.DataGridView();
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.アプリAToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.終了XToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.コンバートCToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.コンバートするファイルを追加AToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.設定SToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.その他の設定SToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.テストToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.statusStrip1 = new System.Windows.Forms.StatusStrip();
			this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
			this.mainTimer = new System.Windows.Forms.Timer(this.components);
			this.プレイリストLToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.選択解除KToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.全てクリアCToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.選択されている項目をクリアLToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
			this.エラーになった項目をクリアEToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.スクリーンSToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.スクリーン_サイズ変更SToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.モニタ選択MToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			((System.ComponentModel.ISupportInitialize)(this.plSheet)).BeginInit();
			this.menuStrip1.SuspendLayout();
			this.statusStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// plSheet
			// 
			this.plSheet.AllowDrop = true;
			this.plSheet.AllowUserToAddRows = false;
			this.plSheet.AllowUserToDeleteRows = false;
			this.plSheet.AllowUserToResizeRows = false;
			this.plSheet.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.plSheet.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.plSheet.Dock = System.Windows.Forms.DockStyle.Fill;
			this.plSheet.Location = new System.Drawing.Point(0, 26);
			this.plSheet.Name = "plSheet";
			this.plSheet.ReadOnly = true;
			this.plSheet.RowTemplate.Height = 21;
			this.plSheet.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.plSheet.Size = new System.Drawing.Size(608, 489);
			this.plSheet.TabIndex = 1;
			this.plSheet.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.plSheet_CellContentClick);
			this.plSheet.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.plSheet_CellContentDoubleClick);
			this.plSheet.DragDrop += new System.Windows.Forms.DragEventHandler(this.plSheet_DragDrop);
			this.plSheet.DragEnter += new System.Windows.Forms.DragEventHandler(this.plSheet_DragEnter);
			// 
			// menuStrip1
			// 
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.アプリAToolStripMenuItem,
            this.プレイリストLToolStripMenuItem,
            this.スクリーンSToolStripMenuItem,
            this.コンバートCToolStripMenuItem,
            this.設定SToolStripMenuItem,
            this.テストToolStripMenuItem});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(608, 26);
			this.menuStrip1.TabIndex = 0;
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
			this.終了XToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.終了XToolStripMenuItem.Text = "終了(&X)";
			this.終了XToolStripMenuItem.Click += new System.EventHandler(this.終了XToolStripMenuItem_Click);
			// 
			// コンバートCToolStripMenuItem
			// 
			this.コンバートCToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.コンバートするファイルを追加AToolStripMenuItem});
			this.コンバートCToolStripMenuItem.Name = "コンバートCToolStripMenuItem";
			this.コンバートCToolStripMenuItem.Size = new System.Drawing.Size(98, 22);
			this.コンバートCToolStripMenuItem.Text = "コンバート(&C)";
			// 
			// コンバートするファイルを追加AToolStripMenuItem
			// 
			this.コンバートするファイルを追加AToolStripMenuItem.Name = "コンバートするファイルを追加AToolStripMenuItem";
			this.コンバートするファイルを追加AToolStripMenuItem.Size = new System.Drawing.Size(262, 22);
			this.コンバートするファイルを追加AToolStripMenuItem.Text = "コンバートするファイルを追加(&A)";
			this.コンバートするファイルを追加AToolStripMenuItem.Click += new System.EventHandler(this.コンバートするファイルを追加AToolStripMenuItem_Click);
			// 
			// 設定SToolStripMenuItem
			// 
			this.設定SToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.その他の設定SToolStripMenuItem});
			this.設定SToolStripMenuItem.Name = "設定SToolStripMenuItem";
			this.設定SToolStripMenuItem.Size = new System.Drawing.Size(62, 22);
			this.設定SToolStripMenuItem.Text = "設定(&S)";
			// 
			// その他の設定SToolStripMenuItem
			// 
			this.その他の設定SToolStripMenuItem.Name = "その他の設定SToolStripMenuItem";
			this.その他の設定SToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.その他の設定SToolStripMenuItem.Text = "設定(&S)";
			this.その他の設定SToolStripMenuItem.Click += new System.EventHandler(this.その他の設定SToolStripMenuItem_Click);
			// 
			// テストToolStripMenuItem
			// 
			this.テストToolStripMenuItem.Name = "テストToolStripMenuItem";
			this.テストToolStripMenuItem.Size = new System.Drawing.Size(56, 22);
			this.テストToolStripMenuItem.Text = "テスト";
			// 
			// statusStrip1
			// 
			this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatus});
			this.statusStrip1.Location = new System.Drawing.Point(0, 515);
			this.statusStrip1.Name = "statusStrip1";
			this.statusStrip1.Size = new System.Drawing.Size(608, 23);
			this.statusStrip1.TabIndex = 2;
			this.statusStrip1.Text = "statusStrip1";
			// 
			// lblStatus
			// 
			this.lblStatus.Name = "lblStatus";
			this.lblStatus.Size = new System.Drawing.Size(593, 18);
			this.lblStatus.Spring = true;
			this.lblStatus.Text = "準備しています...";
			this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// mainTimer
			// 
			this.mainTimer.Enabled = true;
			this.mainTimer.Tick += new System.EventHandler(this.mainTimer_Tick);
			// 
			// プレイリストLToolStripMenuItem
			// 
			this.プレイリストLToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.選択解除KToolStripMenuItem,
            this.toolStripMenuItem1,
            this.選択されている項目をクリアLToolStripMenuItem,
            this.エラーになった項目をクリアEToolStripMenuItem,
            this.全てクリアCToolStripMenuItem});
			this.プレイリストLToolStripMenuItem.Name = "プレイリストLToolStripMenuItem";
			this.プレイリストLToolStripMenuItem.Size = new System.Drawing.Size(109, 22);
			this.プレイリストLToolStripMenuItem.Text = "プレイリスト(&L)";
			// 
			// 選択解除KToolStripMenuItem
			// 
			this.選択解除KToolStripMenuItem.Name = "選択解除KToolStripMenuItem";
			this.選択解除KToolStripMenuItem.Size = new System.Drawing.Size(249, 22);
			this.選択解除KToolStripMenuItem.Text = "選択解除(&K)";
			this.選択解除KToolStripMenuItem.Click += new System.EventHandler(this.選択解除KToolStripMenuItem_Click);
			// 
			// 全てクリアCToolStripMenuItem
			// 
			this.全てクリアCToolStripMenuItem.Name = "全てクリアCToolStripMenuItem";
			this.全てクリアCToolStripMenuItem.Size = new System.Drawing.Size(249, 22);
			this.全てクリアCToolStripMenuItem.Text = "全てクリア(&C)";
			this.全てクリアCToolStripMenuItem.Click += new System.EventHandler(this.全てクリアCToolStripMenuItem_Click);
			// 
			// 選択されている項目をクリアLToolStripMenuItem
			// 
			this.選択されている項目をクリアLToolStripMenuItem.Name = "選択されている項目をクリアLToolStripMenuItem";
			this.選択されている項目をクリアLToolStripMenuItem.Size = new System.Drawing.Size(249, 22);
			this.選択されている項目をクリアLToolStripMenuItem.Text = "選択されている項目をクリア(&L)";
			this.選択されている項目をクリアLToolStripMenuItem.Click += new System.EventHandler(this.選択されている項目をクリアLToolStripMenuItem_Click);
			// 
			// toolStripMenuItem1
			// 
			this.toolStripMenuItem1.Name = "toolStripMenuItem1";
			this.toolStripMenuItem1.Size = new System.Drawing.Size(246, 6);
			// 
			// エラーになった項目をクリアEToolStripMenuItem
			// 
			this.エラーになった項目をクリアEToolStripMenuItem.Name = "エラーになった項目をクリアEToolStripMenuItem";
			this.エラーになった項目をクリアEToolStripMenuItem.Size = new System.Drawing.Size(249, 22);
			this.エラーになった項目をクリアEToolStripMenuItem.Text = "エラーになった項目をクリア(&E)";
			this.エラーになった項目をクリアEToolStripMenuItem.Click += new System.EventHandler(this.エラーになった項目をクリアEToolStripMenuItem_Click);
			// 
			// スクリーンSToolStripMenuItem
			// 
			this.スクリーンSToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.スクリーン_サイズ変更SToolStripMenuItem,
            this.モニタ選択MToolStripMenuItem});
			this.スクリーンSToolStripMenuItem.Name = "スクリーンSToolStripMenuItem";
			this.スクリーンSToolStripMenuItem.Size = new System.Drawing.Size(98, 22);
			this.スクリーンSToolStripMenuItem.Text = "スクリーン(&S)";
			// 
			// スクリーン_サイズ変更SToolStripMenuItem
			// 
			this.スクリーン_サイズ変更SToolStripMenuItem.Name = "スクリーン_サイズ変更SToolStripMenuItem";
			this.スクリーン_サイズ変更SToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
			this.スクリーン_サイズ変更SToolStripMenuItem.Text = "サイズ変更(&S)";
			this.スクリーン_サイズ変更SToolStripMenuItem.Click += new System.EventHandler(this.スクリーン_サイズ変更SToolStripMenuItem_Click);
			// 
			// モニタ選択MToolStripMenuItem
			// 
			this.モニタ選択MToolStripMenuItem.Name = "モニタ選択MToolStripMenuItem";
			this.モニタ選択MToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
			this.モニタ選択MToolStripMenuItem.Text = "モニタ選択(&M)";
			this.モニタ選択MToolStripMenuItem.Click += new System.EventHandler(this.モニタ選択MToolStripMenuItem_Click);
			// 
			// PlayListWin
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(608, 538);
			this.Controls.Add(this.plSheet);
			this.Controls.Add(this.menuStrip1);
			this.Controls.Add(this.statusStrip1);
			this.Font = new System.Drawing.Font("メイリオ", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MainMenuStrip = this.menuStrip1;
			this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.Name = "PlayListWin";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "Kirara";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.PlayListWin_FormClosing);
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.PlayListWin_FormClosed);
			this.Load += new System.EventHandler(this.PlayListWin_Load);
			this.Shown += new System.EventHandler(this.PlayListWin_Shown);
			this.ResizeEnd += new System.EventHandler(this.PlayListWin_ResizeEnd);
			this.Move += new System.EventHandler(this.PlayListWin_Move);
			((System.ComponentModel.ISupportInitialize)(this.plSheet)).EndInit();
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.statusStrip1.ResumeLayout(false);
			this.statusStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.DataGridView plSheet;
		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem アプリAToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem 終了XToolStripMenuItem;
		private System.Windows.Forms.StatusStrip statusStrip1;
		private System.Windows.Forms.ToolStripStatusLabel lblStatus;
		private System.Windows.Forms.Timer mainTimer;
		private System.Windows.Forms.ToolStripMenuItem 設定SToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem その他の設定SToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem テストToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem コンバートCToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem コンバートするファイルを追加AToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem プレイリストLToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem 選択解除KToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem 選択されている項目をクリアLToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem 全てクリアCToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem エラーになった項目をクリアEToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem スクリーンSToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem スクリーン_サイズ変更SToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem モニタ選択MToolStripMenuItem;

	}
}
