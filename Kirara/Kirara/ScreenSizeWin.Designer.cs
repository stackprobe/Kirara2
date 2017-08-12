namespace Charlotte
{
	partial class ScreenSizeWin
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ScreenSizeWin));
			this.lblMessage = new System.Windows.Forms.Label();
			this.mainPanel = new System.Windows.Forms.Panel();
			this.mainPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// lblMessage
			// 
			this.lblMessage.AutoSize = true;
			this.lblMessage.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
			this.lblMessage.Location = new System.Drawing.Point(5, 5);
			this.lblMessage.Name = "lblMessage";
			this.lblMessage.Size = new System.Drawing.Size(347, 80);
			this.lblMessage.TabIndex = 0;
			this.lblMessage.Text = "このウィンドウのサイズを\r\nスクリーン・サイズに調整して下さい。\r\n調整が終わったら、この辺をダブルクリックして下さい。\r\n(現在のサイズ: $L, $T, $W" +
    ", $H)";
			this.lblMessage.Click += new System.EventHandler(this.lblMessage_Click);
			this.lblMessage.DoubleClick += new System.EventHandler(this.lblMessage_DoubleClick);
			// 
			// mainPanel
			// 
			this.mainPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.mainPanel.Controls.Add(this.lblMessage);
			this.mainPanel.Location = new System.Drawing.Point(12, 12);
			this.mainPanel.Name = "mainPanel";
			this.mainPanel.Size = new System.Drawing.Size(460, 438);
			this.mainPanel.TabIndex = 0;
			this.mainPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.mainPanel_Paint);
			this.mainPanel.DoubleClick += new System.EventHandler(this.mainPanel_DoubleClick);
			// 
			// ScreenSizeWin
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(484, 462);
			this.Controls.Add(this.mainPanel);
			this.Font = new System.Drawing.Font("メイリオ", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.MinimizeBox = false;
			this.Name = "ScreenSizeWin";
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "Kirara / サイズ変更";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ScreenSizeWin_FormClosing);
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ScreenSizeWin_FormClosed);
			this.Load += new System.EventHandler(this.ScreenSizeWin_Load);
			this.Shown += new System.EventHandler(this.ScreenSizeWin_Shown);
			this.ResizeBegin += new System.EventHandler(this.ScreenSizeWin_ResizeBegin);
			this.ResizeEnd += new System.EventHandler(this.ScreenSizeWin_ResizeEnd);
			this.DoubleClick += new System.EventHandler(this.ScreenSizeWin_DoubleClick);
			this.Move += new System.EventHandler(this.ScreenSizeWin_Move);
			this.Resize += new System.EventHandler(this.ScreenSizeWin_Resize);
			this.mainPanel.ResumeLayout(false);
			this.mainPanel.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Label lblMessage;
		private System.Windows.Forms.Panel mainPanel;
	}
}
