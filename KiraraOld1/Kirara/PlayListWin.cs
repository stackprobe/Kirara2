using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Charlotte.Tools;
using System.IO;

namespace Charlotte
{
	public partial class PlayListWin : Form
	{
		public PlayListWin()
		{
			InitializeComponent();

			this.MinimumSize = this.Size;
			mainSheetInit();
			this.lblStatus.Text = "";
		}

		private void PlayListWin_Load(object sender, EventArgs e)
		{
			// noop
		}

		private void PlayListWin_Shown(object sender, EventArgs e)
		{
			this.mtEnabled = true;
		}

		private void PlayListWin_FormClosing(object sender, FormClosingEventArgs e)
		{
			// noop
		}

		private void PlayListWin_FormClosed(object sender, FormClosedEventArgs e)
		{
			this.mtEnabled = false;
		}

		private void 終了XToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void mainSheetInit()
		{
			this.mainSheet.RowCount = 0;
			this.mainSheet.ColumnCount = 0;
			this.mainSheet.ColumnCount = 5;

			this.mainSheet.RowHeadersVisible = false; // 行ヘッダ_非表示

			foreach (DataGridViewColumn column in this.mainSheet.Columns) // 列ソート_禁止
			{
				column.SortMode = DataGridViewColumnSortMode.NotSortable;
			}

			int colidx = 0;

			Utils.addColumn(this.mainSheet, colidx++, "Status");
			Utils.addColumn(this.mainSheet, colidx++, "名前");
			Utils.addColumn(this.mainSheet, colidx++, "長さ");
			Utils.addColumn(this.mainSheet, colidx++, "Extra");
			Utils.addColumn(this.mainSheet, colidx++, "Arguments", true);
		}

		private void mainSheetSetRow(int rowidx, MediaInfo mi)
		{
			DataGridViewRow row = this.mainSheet.Rows[rowidx];
			int colidx = 0;

			row.Cells[colidx++].Value = Consts.mediaStatuses[(int)mi.status];
			row.Cells[colidx++].Value = mi.title;
			row.Cells[colidx++].Value = Utils.secLengthToStamp(mi.secLength);
			row.Cells[colidx++].Value = mi.errorMessage;
			row.Cells[colidx++].Value = mi.encode();
		}

		private MediaInfo mainSheetGetRow(int rowidx)
		{
			DataGridViewRow row = this.mainSheet.Rows[rowidx];

			return MediaInfo.decode(row.Cells[4].Value.ToString());
		}

		// 要 this.mainSheet.AllowDrop = true;

		private void mainSheet_DragEnter(object sender, DragEventArgs e)
		{
			try
			{
				if (e.Data.GetDataPresent(DataFormats.FileDrop))
					e.Effect = DragDropEffects.Copy;
			}
			catch
			{ }
		}

		private void mainSheet_DragDrop(object sender, DragEventArgs e)
		{
			try
			{
				int rowidx;

				{
					Point pt = this.mainSheet.PointToClient(new Point(e.X, e.Y));
					DataGridView.HitTestInfo hit = this.mainSheet.HitTest(pt.X, pt.Y);
					rowidx = hit.RowIndex;
				}

				string[] files = (string[])e.Data.GetData(DataFormats.FileDrop, false);

				files = droppedFilesFltr(files);

				if (rowidx == -1)
					rowidx = this.mainSheet.RowCount;

				this.mainSheet.Rows.Insert(rowidx, files.Length);

				foreach (string file in files)
				{
					mainSheetSetRow(rowidx++, MediaInfo.create(file));
				}
				Utils.adjustColumnsWidth(this.mainSheet);
			}
			catch
			{ }
		}

		private string[] droppedFilesFltr(string[] files)
		{
			Queue<string> rq = new Queue<string>(files);
			Queue<string> wq = new Queue<string>();

			while (1 <= rq.Count)
			{
				string file = rq.Dequeue();

				file = FileTools.toFullPath(file);

				if (Directory.Exists(file))
				{
					foreach (string subFile in FileTools.lssFiles(file))
						rq.Enqueue(subFile);
				}
				else if (File.Exists(file))
				{
					wq.Enqueue(file);
				}
			}
			files = wq.ToArray();
			ArrayTools.sort<string>(files, StringTools.compIgnoreCase);
			return files;
		}

		private void mainSheet_CellContentClick(object sender, DataGridViewCellEventArgs e)
		{
			// noop
		}

		private void mainSheet_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
		{
			// TODO 再生
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
				if (Conv.curr != null)
					Conv.curr.critSect.approve();

				playListMonitorEachTimer();
			}
			finally
			{
				this.mtBusy = false;
				this.mtCount++;
			}
		}

		private int plmIndex = -1;

		private void playListMonitorEachTimer()
		{
			if (this.mainSheet.RowCount == 0)
				return;

			plmIndex++;
			plmIndex %= this.mainSheet.RowCount;

			MediaInfo mi = mainSheetGetRow(plmIndex);

			switch (mi.status)
			{
				case Consts.MediaStatus_e.UNKNOWN:
					{
						string ext = Path.GetExtension(mi.file);

						if (StringTools.equalsIgnoreCase(ext, ".ogg")) // ? そのまま行ける音楽ファイル
						{
							mi.ogxFile = Utils.getOgxFile(mi.index, Consts.MediaType_e.AUDIO);

							try
							{
								File.Copy(mi.file, mi.ogxFile);

								if (File.Exists(mi.ogxFile) == false)
									throw new Exception("ファイルにアクセス出来ません。(音楽ファイル_Direct)");
							}
							catch (Exception e)
							{
								Gnd.i.logger.writeLine("そのまま行ける音楽ファイルのコピーに失敗しました。" + e);

								mi.status = Consts.MediaStatus_e.ERROR;
								mi.errorMessage = Utils.getMessage(e);
							}
						}
						else if (StringTools.equalsIgnoreCase(ext, ".ogv")) // ? そのまま行ける動画ファイル
						{
							mi.ogxFile = Utils.getOgxFile(mi.index, Consts.MediaType_e.MOVIE);

							try
							{
								File.Copy(mi.file, mi.ogxFile);

								if (File.Exists(mi.ogxFile) == false)
									throw new Exception("ファイルにアクセス出来ません。(動画ファイル_Direct)");
							}
							catch (Exception e)
							{
								Gnd.i.logger.writeLine("そのまま行ける動画ファイルのコピーに失敗しました。" + e);

								mi.status = Consts.MediaStatus_e.ERROR;
								mi.errorMessage = Utils.getMessage(e);
							}
						}
						else if (Conv.curr == null)
						{
							Conv.curr = new Conv(mi.file, mi.index);

							mi.status = Consts.MediaStatus_e.CONVERTING;
						}
						else
						{
							mi.status = Consts.MediaStatus_e.NEED_CONVERSION;
						}
					}
					break;

				case Consts.MediaStatus_e.NEED_CONVERSION:
					{
						if (Conv.curr == null)
						{
							Conv.curr = new Conv(mi.file, mi.index);

							mi.status = Consts.MediaStatus_e.CONVERTING;
						}
					}
					break;

				case Consts.MediaStatus_e.CONVERTING:
					{
						if (Conv.curr != null && Conv.curr.isEnded())
						{
							if (Conv.curr.hasError())
							{
								mi.status = Consts.MediaStatus_e.ERROR;
								mi.errorMessage = Conv.curr.getErrorMessage();
							}
							else
							{
								mi.status = Consts.MediaStatus_e.READY;
								mi.ogxFile = Conv.curr.getOgxFile();
								mi.type = Conv.curr.getMediaType();
							}
							Conv.curr = null;
						}
					}
					break;

				case Consts.MediaStatus_e.READY:
					{
						// noop
					}
					break;

				case Consts.MediaStatus_e.PLAYING:
					{
						// noop
					}
					break;

				case Consts.MediaStatus_e.ERROR:
					{
						// noop
					}
					break;

				default:
					throw null;
			}
			mainSheetSetRow(plmIndex, mi);
		}
	}
}
