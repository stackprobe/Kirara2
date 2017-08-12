using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using Charlotte.Tools;
using System.IO;

namespace Charlotte
{
	public partial class BgConvWin : Form
	{
		private bool _plConverting;

		public BgConvWin(bool plConverting)
		{
			_plConverting = plConverting;

			InitializeComponent();

			this.MinimumSize = this.Size;
			Utils.enableDoubleBuffer(blSheet);

			this.txtDestDir.ForeColor = new TextBox().ForeColor;
			this.txtDestDir.BackColor = new TextBox().BackColor;

			this.lblStatus.Text = "";
		}

		private void BgConvWin_Load(object sender, EventArgs e)
		{
			// noop
		}

		private bool _winOpened = false;

		private void BgConvWin_Shown(object sender, EventArgs e)
		{
			blSheetInit();

			// load
			{
				if (Gnd.i.bgConvDestDir == "")
					Gnd.i.bgConvDestDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "KiraraConvOut");

				this.txtDestDir.Text = Gnd.i.bgConvDestDir;

				blSheet.RowCount = Gnd.i.bgConvInfos.Count;

				for (int index = 0; index < Gnd.i.bgConvInfos.Count; index++)
				{
					blSheetSetRow(index, Gnd.i.bgConvInfos[index]);
				}
				Utils.adjustColumnsWidth(blSheet);
			}

			loadLTWH();

			if (_plConverting)
			{
				MessageBox.Show(
					"プレイリストに変換中の項目があるため、Conversion Dialog を開いている間は変換を開始できません。",
					"情報",
					MessageBoxButtons.OK,
					MessageBoxIcon.Information
					);
			}

			Gnd.i.tc.add(delegate
			{
				Thread.Sleep(100);
				_winOpened = true;
				this.mtEnabled = true;
			});
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
			if (_winOpened == false) // ? まだフォームを開いた直後
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
			this.mtEnabled = false;

			// save
			{
				Gnd.i.bgConvDestDir = this.txtDestDir.Text;

				Gnd.i.bgConvInfos.Clear();

				for (int rowidx = 0; rowidx < blSheet.RowCount; rowidx++)
				{
					Gnd.i.bgConvInfos.Add(blSheetGetRow(rowidx));
				}
			}
		}

		private void 閉じるXToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.mtEnabled = false;
			this.Close();
		}

		private void blSheetInit()
		{
			blSheet.RowCount = 0;
			blSheet.ColumnCount = 0;
			blSheet.ColumnCount = 5;

			blSheet.RowHeadersVisible = false; // 行ヘッダ_非表示

			foreach (DataGridViewColumn column in this.blSheet.Columns) // 列ソート_禁止
			{
				column.SortMode = DataGridViewColumnSortMode.NotSortable;
			}

			int colidx = 0;

			Utils.addColumn(blSheet, colidx++, "Status");
			Utils.addColumn(blSheet, colidx++, "入力ファイル");
			Utils.addColumn(blSheet, colidx++, "出力ファイル");
			Utils.addColumn(blSheet, colidx++, "Extra");
			Utils.addColumn(blSheet, colidx++, "Arguments", true);
		}

		private void blSheetSetRow(int rowidx, BgConvInfo bci)
		{
			DataGridViewRow row = blSheet.Rows[rowidx];
			int colidx = 0;

			row.Cells[colidx++].Value = Consts.convStatuses[(int)bci.status];
			row.Cells[colidx++].Value = bci.rFile;
			row.Cells[colidx++].Value = bci.wFileOrProvisionalWFile;
			row.Cells[colidx++].Value = bci.errorMessage;
			row.Cells[colidx++].Value = bci.encode();
		}

		private BgConvInfo blSheetGetRow(int rowidx)
		{
			return BgConvInfo.decode(blSheet.Rows[rowidx].Cells[blSheet.ColumnCount - 1].Value.ToString());
		}

		private void blSheet_CellContentClick(object sender, DataGridViewCellEventArgs e)
		{
			// noop
		}

		// 要 this.blSheet.AllowDrop = true;

		private void blSheet_DragEnter(object sender, DragEventArgs e)
		{
			try
			{
				if (e.Data.GetDataPresent(DataFormats.FileDrop))
					e.Effect = DragDropEffects.Copy;
			}
			catch (Exception ex)
			{
				Gnd.i.logger.writeLine("blSheet-de-error: " + ex);
			}
		}

		private void blSheet_DragDrop(object sender, DragEventArgs e)
		{
			this.mtEnabled = false;
			try
			{
#if false // 不要
				int droppedRowIndex;

				{
					Point pt = this.blSheet.PointToClient(new Point(e.X, e.Y));
					DataGridView.HitTestInfo hit = this.blSheet.HitTest(pt.X, pt.Y);
					droppedRowIndex = hit.RowIndex;
				}
#endif

				string[] files = (string[])e.Data.GetData(DataFormats.FileDrop, false);

				if (Gnd.i.bciCountMax < files.Length)
					throw new FailedOperation("入力ファイルが多すぎます。(展開前)");

				List<string> relFiles = new List<string>();
				files = Utils.droppedFilesFilter(files, relFiles);

				if (Gnd.i.bciCountMax < files.Length)
					throw new FailedOperation("入力ファイルが多すぎます。(展開後)");

				if (files.Length == 0)
					throw new FailedOperation("no files");

				if (Gnd.i.bciCountMax < files.Length + blSheet.RowCount)
					throw new FailedOperation("入力ファイルが多すぎます。\nエラー・完了した項目をクリアすると解決するかもしれません。");

				List<BgConvInfo> bcis = new List<BgConvInfo>();

				for (int index = 0; index < files.Length; index++)
				{
					string wFile = Path.Combine(this.txtDestDir.Text, relFiles[index]);
					string wFileNoExt = Path.Combine(Path.GetDirectoryName(wFile), Path.GetFileNameWithoutExtension(wFile));

					bcis.Add(BgConvInfo.create(
						files[index],
						wFileNoExt
						));
				}
				int startPos = blSheet.RowCount;

				blSheet.RowCount += bcis.Count;

				for (int index = 0; index < bcis.Count; index++)
				{
					blSheetSetRow(startPos + index, bcis[index]);
				}
				Utils.adjustColumnsWidth(blSheet);
			}
			catch (Exception ex)
			{
				Gnd.i.logger.writeLine("blSheet-dd-error: " + ex);

				FailedOperation.caught(ex);
			}
			finally
			{
				this.mtEnabled = true;
			}
		}

		private void BgConvWin_Move(object sender, EventArgs e)
		{
			saveLTWH();
		}

		private void BgConvWin_ResizeEnd(object sender, EventArgs e)
		{
			saveLTWH();
		}

		private void 選択解除KToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.blSheet.ClearSelection();
		}

		private void 選択されている項目をクリアCToolStripMenuItem_Click(object sender, EventArgs e)
		{
			for (int rowidx = blSheet.RowCount - 1; 0 <= rowidx; rowidx--)
				if (blSheet.Rows[rowidx].Selected)
					tryDeleteRow(rowidx);
		}

		private void エラーand完了した項目をクリアDToolStripMenuItem_Click(object sender, EventArgs e)
		{
			for (int rowidx = blSheet.RowCount - 1; 0 <= rowidx; rowidx--)
			{
				BgConvInfo bci = blSheetGetRow(rowidx);

				if (
					bci.status == Consts.ConvStatus_e.COMPLETED ||
					bci.status == Consts.ConvStatus_e.ERROR
					)
					tryDeleteRow(rowidx);
			}
		}

		private void エラーになった項目をクリアEToolStripMenuItem_Click(object sender, EventArgs e)
		{
			for (int rowidx = blSheet.RowCount - 1; 0 <= rowidx; rowidx--)
			{
				BgConvInfo bci = blSheetGetRow(rowidx);

				if (bci.status == Consts.ConvStatus_e.ERROR)
					tryDeleteRow(rowidx);
			}
		}

		private void 完了した項目をクリアFToolStripMenuItem_Click(object sender, EventArgs e)
		{
			for (int rowidx = blSheet.RowCount - 1; 0 <= rowidx; rowidx--)
			{
				BgConvInfo bci = blSheetGetRow(rowidx);

				if (bci.status == Consts.ConvStatus_e.COMPLETED)
					tryDeleteRow(rowidx);
			}
		}

		private void 全てクリアCToolStripMenuItem_Click(object sender, EventArgs e)
		{
			for (int rowidx = blSheet.RowCount - 1; 0 <= rowidx; rowidx--)
				tryDeleteRow(rowidx);
		}

		private void tryDeleteRow(int rowidx)
		{
			if (rowidx < 0 || blSheet.RowCount <= rowidx)
				return;

			BgConvInfo bci = blSheetGetRow(rowidx);

			if (bci.status == Consts.ConvStatus_e.CONVERTING)
				return;

			blSheet.Rows.RemoveAt(rowidx);
		}

		private void btnDestDir_Click(object sender, EventArgs e)
		{
			this.mtEnabled = false;
			try
			{
				//FolderBrowserDialogクラスのインスタンスを作成
				using (FolderBrowserDialog fbd = new FolderBrowserDialog())
				{
					//上部に表示する説明テキストを指定する
					fbd.Description = "出力先フォルダを指定してください。";
					//ルートフォルダを指定する
					//デフォルトでDesktop
					fbd.RootFolder = Environment.SpecialFolder.Desktop;
					//fbd.RootFolder = Environment.SpecialFolder.MyComputer;
					//最初に選択するフォルダを指定する
					//RootFolder以下にあるフォルダである必要がある
					//fbd.SelectedPath = @"C:\Windows";
					fbd.SelectedPath = this.txtDestDir.Text;
					//ユーザーが新しいフォルダを作成できるようにする
					//デフォルトでTrue
					fbd.ShowNewFolderButton = true;

					//ダイアログを表示する
					if (fbd.ShowDialog(this) == DialogResult.OK)
					{
						//選択されたフォルダを表示する
						//Console.WriteLine(fbd.SelectedPath);

						string dir = fbd.SelectedPath;
						dir = FileTools.toFullPath(dir);
						this.txtDestDir.Text = dir;
					}
				}
			}
			catch
			{ }
			finally
			{
				this.mtEnabled = true;
			}
		}

		private void txtDestDir_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == (char)1) // ctrl_a
			{
				txtDestDir.SelectAll();
				e.Handled = true;
			}
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
				Gnd.i.convOc.eachTimerTick();

				for (int c = 0; c < 10 && eachTimerTick(); c++)
				{ }

				// このフォームでは mainTimer == 100 ms ですよ！

				if (mtCount % 20 == 0) // 2 sec
				{
					List<string> tokens = new List<string>();

					if (_plConverting)
					{
						tokens.Add("変換を開始できません！");
					}
					else if (Gnd.i.conv != null)
					{
						tokens.Add("Converting=" + Path.GetFileName(Gnd.i.conv.rFile));
					}
					string state = string.Join(" ", tokens);

					if (lblStatus.Text != state)
						lblStatus.Text = state;
				}
			}
			finally
			{
				this.mtBusy = false;
				this.mtCount++;
			}
		}

		private int ettRowIndex = -1;

		private bool eachTimerTick() // ret: ? 継続すべき
		{
			if (blSheet.RowCount == 0)
			{
				ettRowIndex = -1;
				return false;
			}
			ettRowIndex++;
			ettRowIndex %= blSheet.RowCount;

			BgConvInfo bci = blSheetGetRow(ettRowIndex);

			switch (bci.status)
			{
				case Consts.ConvStatus_e.READY:
					if (Gnd.i.conv == null)
					{
						Gnd.i.conv = new Conv(bci.rFile, bci.wFileNoExt);
						bci.status = Consts.ConvStatus_e.CONVERTING;

						// UIに反映
						blSheetSetRow(ettRowIndex, bci);
						Utils.adjustColumnsWidth(blSheet);
						return false;
					}
					break;

				case Consts.ConvStatus_e.CONVERTING:
					// Gnd.i.conv == null ってことは無いけど、念のため
					if (Gnd.i.conv != null && Gnd.i.conv.completed)
					{
						if (Gnd.i.conv.errorMessage == null) // ? 成功
						{
							bci.status = Consts.ConvStatus_e.COMPLETED;
							bci.wFile = Gnd.i.conv.wFile;
						}
						else // ? エラー
						{
							bci.status = Consts.ConvStatus_e.ERROR;
							bci.errorMessage = Gnd.i.conv.errorMessage;
						}
						Gnd.i.conv.Dispose();
						Gnd.i.conv = null;

						// UIに反映
						blSheetSetRow(ettRowIndex, bci);
						Utils.adjustColumnsWidth(blSheet);
						return false;
					}
					break;
			}
			return true;
		}
	}
}
