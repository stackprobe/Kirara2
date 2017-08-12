using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Charlotte
{
	public partial class BgConvWin : Form
	{
		public BgConvWin()
		{
			InitializeComponent();

			this.MinimumSize = this.Size;

			this.txtDestDir.ForeColor = new TextBox().ForeColor;
			this.txtDestDir.BackColor = new TextBox().BackColor;

			bciSheetInit();

			// load
			{
				this.txtDestDir.Text = Gnd.i.bgConvDestDir;
			}
		}

		private void BgConvWin_Load(object sender, EventArgs e)
		{
			// noop
		}

		private void BgConvWin_Shown(object sender, EventArgs e)
		{
			loadLTWH();
			this.mtEnabled = true;
		}

		private void loadLTWH()
		{
			if (Gnd.i.bgConvWin_w != -1)
			{
				this.Left = Gnd.i.bgConvWin_l;
				this.Top = Gnd.i.bgConvWin_t;
				this.Width = Gnd.i.bgConvWin_w;
				this.Height = Gnd.i.bgConvWin_h;
			}
		}

		private void saveLTWH()
		{
			if (this.mtCount < 2) // ? まだフォームを開いたばかり
				return;

			if (this.WindowState != FormWindowState.Normal)
				return;

			Gnd.i.bgConvWin_l = this.Left;
			Gnd.i.bgConvWin_t = this.Top;
			Gnd.i.bgConvWin_w = this.Width;
			Gnd.i.bgConvWin_h = this.Height;
		}

		private void BgConvWin_FormClosing(object sender, FormClosingEventArgs e)
		{
			saveLTWH();
		}

		private void BgConvWin_FormClosed(object sender, FormClosedEventArgs e)
		{
			// save
			{
				Gnd.i.bgConvDestDir = this.txtDestDir.Text;
			}
			this.mtEnabled = true;
		}

		private void 閉じるXToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.Close();
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
				// noop
			}
			finally
			{
				this.mtBusy = false;
				this.mtCount++;
			}
		}

		private void 全てクリアCToolStripMenuItem_Click(object sender, EventArgs e)
		{
			// TODO
		}

		private void エラー完了した項目をクリアDToolStripMenuItem_Click(object sender, EventArgs e)
		{
			// TODO
		}

		private void エラーになった項目をクリアEToolStripMenuItem_Click(object sender, EventArgs e)
		{
			// TODO
		}

		private void 完了した項目をクリアFToolStripMenuItem_Click(object sender, EventArgs e)
		{
			// TODO
		}

		private void 選択されている項目をクリアLToolStripMenuItem_Click(object sender, EventArgs e)
		{
			// TODO
		}

		private void 選択解除KToolStripMenuItem_Click(object sender, EventArgs e)
		{
			// TODO
		}

		private void btnDestDir_Click(object sender, EventArgs e)
		{
			// TODO
		}

		private void txtDestDir_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == (char)1) // ctrl_a
			{
				this.txtDestDir.SelectAll();
				e.Handled = true;
			}
		}

		private void txtDestDir_TextChanged(object sender, EventArgs e)
		{
			// noop
		}

		// 要 this.bciSheet.AllowDrop = true;

		private void bciSheet_DragEnter(object sender, DragEventArgs e)
		{
			try
			{
				if (e.Data.GetDataPresent(DataFormats.FileDrop))
					e.Effect = DragDropEffects.Copy;
			}
			catch
			{ }
		}

		private void bciSheet_DragDrop(object sender, DragEventArgs e)
		{
			try
			{
				int rowidx;

				{
					Point pt = this.bciSheet.PointToClient(new Point(e.X, e.Y));
					DataGridView.HitTestInfo hit = this.bciSheet.HitTest(pt.X, pt.Y);
					rowidx = hit.RowIndex;
				}

				string[] files = (string[])e.Data.GetData(DataFormats.FileDrop, false);

				// TODO
			}
			catch
			{ }
		}

		private void bciSheet_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
		{
			// noop
		}

		private void bciSheet_CellContentClick(object sender, DataGridViewCellEventArgs e)
		{
			// noop
		}

		private void bciSheetInit()
		{
			this.bciSheet.RowCount = 0;
			this.bciSheet.ColumnCount = 0;
			this.bciSheet.ColumnCount = 5;

			this.bciSheet.RowHeadersVisible = false; // 行ヘッダ_非表示

			foreach (DataGridViewColumn column in this.bciSheet.Columns) // 列ソート_禁止
			{
				column.SortMode = DataGridViewColumnSortMode.NotSortable;
			}

			int colidx = 0;

			Utils.addColumn(this.bciSheet, colidx++, "Status");
			Utils.addColumn(this.bciSheet, colidx++, "入力ファイル");
			Utils.addColumn(this.bciSheet, colidx++, "出力ファイル");
			Utils.addColumn(this.bciSheet, colidx++, "Extra");
			Utils.addColumn(this.bciSheet, colidx++, "Arguments", true);
		}

		private void bciSheetSetRow(int rowidx, BgConvInfo bci)
		{
			DataGridViewRow row = this.bciSheet.Rows[rowidx];
			int colidx = 0;

			row.Cells[colidx++].Value = Consts.convStatuses[(int)bci.status];
			row.Cells[colidx++].Value = bci.rFile;
			row.Cells[colidx++].Value = bci.wFileOrDummy;
			row.Cells[colidx++].Value = bci.errorMessage;
			row.Cells[colidx++].Value = bci.encode();
		}

		private BgConvInfo bciSheetGetRow(int rowidx)
		{
			DataGridViewRow row = this.bciSheet.Rows[rowidx];

			return BgConvInfo.decode(row.Cells[4].Value.ToString());
		}

		private void BgConvWin_Move(object sender, EventArgs e)
		{
			saveLTWH();
		}

		private void BgConvWin_ResizeEnd(object sender, EventArgs e)
		{
			saveLTWH();
		}
	}
}
