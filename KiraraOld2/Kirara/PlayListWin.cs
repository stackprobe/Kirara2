using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Security.Permissions;

namespace Charlotte
{
	public partial class PlayListWin : Form
	{
		#region ALT_F4 抑止

		private static bool _xPressed = false;

		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected override void WndProc(ref Message m)
		{
			const int WM_SYSCOMMAND = 0x112;
			const long SC_CLOSE = 0xF060L;

			if (m.Msg == WM_SYSCOMMAND && (m.WParam.ToInt64() & 0xFFF0L) == SC_CLOSE)
			{
				_xPressed = true;
				return;
			}
			base.WndProc(ref m);
		}

		#endregion

		public static PlayListWin self;

		public PlayListWin()
		{
			self = this;

			InitializeComponent();

			this.MinimumSize = new Size(300, 300);
			//this.MinimumSize = this.Size;
			this.lblStatus.Text = "";

			plSheetInit();

			if (Gnd.i.productMode)
			{
				this.テストToolStripMenuItem.Visible = false;
			}
		}

		private void PlayListWin_Load(object sender, EventArgs e)
		{
			// noop
		}

		private void PlayListWin_Shown(object sender, EventArgs e)
		{
			loadLTWH();
			Gnd.i.startTh(true);
			this.mtEnabled = true;
		}

		private void loadLTWH()
		{
			if (Gnd.i.plWin_w != -1)
			{
				this.Left = Gnd.i.plWin_l;
				this.Top = Gnd.i.plWin_t;
				this.Width = Gnd.i.plWin_w;
				this.Height = Gnd.i.plWin_h;
			}
		}

		private void saveLTWH()
		{
			if (this.mtCount < 2) // ? まだフォームを開いた直後
				return;

			if (this.WindowState != FormWindowState.Normal)
				return;

			Gnd.i.plWin_l = this.Left;
			Gnd.i.plWin_t = this.Top;
			Gnd.i.plWin_w = this.Width;
			Gnd.i.plWin_h = this.Height;
		}

		private void PlayListWin_FormClosing(object sender, FormClosingEventArgs e)
		{
			saveLTWH();
		}

		private void PlayListWin_FormClosed(object sender, FormClosedEventArgs e)
		{
			this.mtEnabled = false; // 2bs
			self = null;
		}

		private void closeWin()
		{
			this.mtEnabled = false;
			Gnd.i.endTh(true);
			this.Close();
		}

		private void 終了XToolStripMenuItem_Click(object sender, EventArgs e)
		{
			closeWin();
		}

		private bool mtEnabled;
		private bool mtBusy;
		private long mtCount;

		private void mainTimer_Tick(object sender, EventArgs e)
		{
			if (this.mtEnabled == false || this.mtBusy)
				return;

			this.mtBusy = true;

			try
			{
				if (_xPressed)
				{
					closeWin();
					return;
				}
				if (Gnd.i.xRequested)
				{
					Gnd.i.xRequested = false;
					closeWin();
					return;
				}

				{
					EngineTh.Operation_d operation = Gnd.i.invokers.dequeue();

					if (operation != null)
						operation();
				}
			}
			finally
			{
				this.mtBusy = false;
				this.mtCount++;
			}
		}

		private void コンバートするファイルを追加AToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Gnd.i.endTh();
			this.mtEnabled = false;
			this.Visible = false;

			using (BgConvWin f = new BgConvWin())
			{
				f.ShowDialog();
			}
			this.Visible = true;
			this.mtEnabled = true;
			Gnd.i.startTh();
		}

		private void その他の設定SToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Gnd.i.endTh();
			this.mtEnabled = false;
			this.Visible = false;

			using (SettingDlg f = new SettingDlg())
			{
				f.ShowDialog();
			}
			this.Visible = true;
			this.mtEnabled = true;
			Gnd.i.startTh();
		}

		// 要 this.plSheet.AllowDrop = true;

		private void plSheet_DragEnter(object sender, DragEventArgs e)
		{
			try
			{
				if (e.Data.GetDataPresent(DataFormats.FileDrop))
					e.Effect = DragDropEffects.Copy;
			}
			catch
			{ }
		}

		private void plSheet_DragDrop(object sender, DragEventArgs e)
		{
			try
			{
				int rowidx;

				{
					Point pt = this.plSheet.PointToClient(new Point(e.X, e.Y));
					DataGridView.HitTestInfo hit = this.plSheet.HitTest(pt.X, pt.Y);
					rowidx = hit.RowIndex;
				}

				string[] files = (string[])e.Data.GetData(DataFormats.FileDrop, false);

				// TODO ClientTh
			}
			catch
			{ }
		}

		private void plSheet_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
		{
			// TODO 再生
		}

		private void plSheet_CellContentClick(object sender, DataGridViewCellEventArgs e)
		{
			// noop
		}

		private void plSheetInit()
		{
			this.plSheet.RowCount = 0;
			this.plSheet.ColumnCount = 0;
			this.plSheet.ColumnCount = 5;

			this.plSheet.RowHeadersVisible = false; // 行ヘッダ_非表示

			foreach (DataGridViewColumn column in this.plSheet.Columns) // 列ソート_禁止
			{
				column.SortMode = DataGridViewColumnSortMode.NotSortable;
			}

			int colidx = 0;

			Utils.addColumn(this.plSheet, colidx++, "Status");
			Utils.addColumn(this.plSheet, colidx++, "名前");
			Utils.addColumn(this.plSheet, colidx++, "長さ");
			Utils.addColumn(this.plSheet, colidx++, "Extra");
			Utils.addColumn(this.plSheet, colidx++, "Arguments", true);
		}

		public void plSheetSetRow(int rowidx, MediaInfo mi)
		{
			DataGridViewRow row = this.plSheet.Rows[rowidx];
			int colidx = 0;

			row.Cells[colidx++].Value = Consts.mediaStatuses[(int)mi.status];
			row.Cells[colidx++].Value = mi.title;
			row.Cells[colidx++].Value = Utils.secToUIStamp(mi.sec);
			row.Cells[colidx++].Value = mi.errorMessage;
			row.Cells[colidx++].Value = mi.encode();
		}

		public MediaInfo plSheetGetRow(int rowidx)
		{
			DataGridViewRow row = this.plSheet.Rows[rowidx];

			return MediaInfo.decode(row.Cells[4].Value.ToString());
		}

		private void PlayListWin_Move(object sender, EventArgs e)
		{
			saveLTWH();
		}

		private void PlayListWin_ResizeEnd(object sender, EventArgs e)
		{
			saveLTWH();
		}

		private void 選択解除KToolStripMenuItem_Click(object sender, EventArgs e)
		{
			plSheet.ClearSelection();
		}

		private void 選択されている項目をクリアLToolStripMenuItem_Click(object sender, EventArgs e)
		{
			// TODO ClientTh
		}

		private void エラーになった項目をクリアEToolStripMenuItem_Click(object sender, EventArgs e)
		{
			// TODO ClientTh
		}

		private void 全てクリアCToolStripMenuItem_Click(object sender, EventArgs e)
		{
			// TODO ClientTh
		}

		private void スクリーン_サイズ変更SToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Gnd.i.endTh();
			this.mtEnabled = false;
			this.Visible = false;

			// TODO ResizeScreenWin_ShowDialog

			this.Visible = true;
			this.mtEnabled = true;
			Gnd.i.startTh();
		}

		private void モニタ選択MToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Gnd.i.endTh();
			this.mtEnabled = false;
			this.Visible = false;

			// TODO SelectMonitor_ShowDialog

			this.Visible = true;
			this.mtEnabled = true;
			Gnd.i.startTh();
		}

		public DataGridView getPLSheet()
		{
			return plSheet;
		}
	}
}
