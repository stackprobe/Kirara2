namespace Charlotte
{
	partial class DispMonitorNoWin
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DispMonitorNoWin));
			this.lblMonitorNo = new System.Windows.Forms.Label();
			this.lblMessage = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// lblMonitorNo
			// 
			this.lblMonitorNo.AutoSize = true;
			this.lblMonitorNo.Font = new System.Drawing.Font("メイリオ", 99.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.lblMonitorNo.ForeColor = System.Drawing.Color.White;
			this.lblMonitorNo.Location = new System.Drawing.Point(12, 9);
			this.lblMonitorNo.Name = "lblMonitorNo";
			this.lblMonitorNo.Size = new System.Drawing.Size(167, 200);
			this.lblMonitorNo.TabIndex = 0;
			this.lblMonitorNo.Text = "9";
			this.lblMonitorNo.Click += new System.EventHandler(this.lblMonitorNo_Click);
			// 
			// lblMessage
			// 
			this.lblMessage.AutoSize = true;
			this.lblMessage.ForeColor = System.Drawing.Color.White;
			this.lblMessage.Location = new System.Drawing.Point(12, 216);
			this.lblMessage.Name = "lblMessage";
			this.lblMessage.Size = new System.Drawing.Size(360, 20);
			this.lblMessage.TabIndex = 1;
			this.lblMessage.Text = "このウィンドウを閉じるには、どこかクリックして下さい。";
			this.lblMessage.Click += new System.EventHandler(this.lblMessage_Click);
			// 
			// DispMonitorNoWin
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
			this.ClientSize = new System.Drawing.Size(400, 300);
			this.Controls.Add(this.lblMessage);
			this.Controls.Add(this.lblMonitorNo);
			this.Font = new System.Drawing.Font("メイリオ", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.Name = "DispMonitorNoWin";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "Kirara_DispMonitorNoWin";
			this.TopMost = true;
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DispMonitorNoWin_FormClosing);
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.DispMonitorNoWin_FormClosed);
			this.Load += new System.EventHandler(this.DispMonitorNoWin_Load);
			this.Shown += new System.EventHandler(this.DispMonitorNoWin_Shown);
			this.Click += new System.EventHandler(this.DispMonitorNoWin_Click);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label lblMonitorNo;
		private System.Windows.Forms.Label lblMessage;
	}
}
