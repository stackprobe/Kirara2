using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Security.Permissions;
using Charlotte.Tools;
using System.IO;
using System.Threading;

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
			Utils.enableDoubleBuffer(plSheet);

			lblStatus.Text = "";
			lblEast.Text = "";

#if DEBUG == false
			this.テストToolStripMenuItem.Visible = false;
#endif
		}

		private void PlayListWin_Load(object sender, EventArgs e)
		{
			// noop
		}

		private void PlayListWin_Shown(object sender, EventArgs e)
		{
			loadLTWH();
			plSheetInit();
			Gnd.i.client = new Client();
			Gnd.i.client.x = closeWin;
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

		private void 終了XToolStripMenuItem_Click(object sender, EventArgs e)
		{
			_xPressed = true;
		}

		private void closeWin()
		{
			this.mtEnabled = false;
			Gnd.i.client.dispose(this);
			Gnd.i.client = null;
			this.Close();
		}

		private void suspendWin()
		{
			this.mtEnabled = false;
			Gnd.i.client.dispose(this, true);
			Gnd.i.client = null;
			this.Visible = false;
		}

		private void resumeWin()
		{
			this.Visible = true;
			Gnd.i.client = new Client();
			Gnd.i.client.x = closeWin;
			this.mtEnabled = true;

			// 再生中 -> READY
			{
#if true
				Gnd.i.client.sendLine("ES/");
#else // old
				int playingIndex = getPlayingIndex();

				if (playingIndex != -1)
				{
					MediaInfo mi = plSheetGetRow(playingIndex);
					mi.status = Consts.MediaStatus_e.READY;
					plSheetSetRow(playingIndex, mi);
				}
#endif
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
				if (_xPressed)
				{
					Gnd.i.client.x = closeWin;
					Gnd.i.client.sendLineEXP();
					_xPressed = false;
					return;
				}
				Gnd.i.oc.eachTimerTick();
				Gnd.i.convOc.eachTimerTick();
				Gnd.i.client.eachTimerTick();

				if (Gnd.i.client.xRecved)
				{
					Gnd.i.client.xRecved = false; // 2bs

					if (Gnd.i.client.x != null)
						Gnd.i.client.x();

					return;
				}
				if (Gnd.i.oc.getCount() == 0) // ? ocが暇なら、
				{
					plSheet_eachTimerTickEx();
					plSheet_eachTimerTick_B();
					plSheet_checkOverflow();
					bgConvInfos_eachTimerTick();
				}

				// このフォームでは mainTimer == 20 ms ですよ！

				if (mtCount % 100 == 0) // 2 sec
				{
					Gnd.i.bootScreen();

					{
						List<string> tokens = new List<string>();

						if (Gnd.i.conv != null)
						{
							tokens.Add("Converting=" + Path.GetFileName(Gnd.i.conv.rFile));
						}
						string state = string.Join(" ", tokens);

						if (lblStatus.Text != state)
							lblStatus.Text = state;
					}

					lblEast.Text = Gnd.i.oc.getCount() + "_" + Gnd.i.convOc.getCount();
				}
				if (mtCount % 1000 == 0) // 20 sec
				{
					GC.Collect();
				}
			}
			finally
			{
				this.mtBusy = false;
				this.mtCount++;
			}
		}

		private int bcie_index = -1;

		private void bgConvInfos_eachTimerTick()
		{
			if (Gnd.i.bgConvInfos.Count == 0)
			{
				bcie_index = -1; // 何となく..
				return;
			}
			bcie_index++;
			bcie_index %= Gnd.i.bgConvInfos.Count;

			BgConvInfo bci = Gnd.i.bgConvInfos[bcie_index];

			switch (bci.status)
			{
				case Consts.ConvStatus_e.READY:
					if (Gnd.i.conv == null)
					{
						Gnd.i.conv = new Conv(bci.rFile, bci.wFileNoExt);
						bci.status = Consts.ConvStatus_e.CONVERTING;
					}
					break;

				case Consts.ConvStatus_e.CONVERTING:
					// conv == null って事は無いはずだけど、念のため..
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
					}
					break;

				case Consts.ConvStatus_e.COMPLETED:
					// noop
					break;

				case Consts.ConvStatus_e.ERROR:
					// noop
					break;

				default:
					throw null;
			}
		}

		private void plSheet_checkOverflow()
		{
			if (Gnd.i.plCountMax < plSheet.RowCount) // ? overflow
			{
				MediaInfo mi = plSheetGetRow(plSheet.RowCount - 1);

				if (
					mi.status == Consts.MediaStatus_e.CONVERTING ||
					mi.status == Consts.MediaStatus_e.PLAYING
					)
					return;

				plSheet.RowCount--;
			}
		}

		private int plAdjColsCountDown = 0;

		private void plSheet_eachTimerTick_B()
		{
			if (0 < plAdjColsCountDown)
			{
				plAdjColsCountDown--;

				if (plAdjColsCountDown == 0)
				{
					Utils.adjustColumnsWidth(plSheet);
				}
			}
		}

		private void plSheet_eachTimerTickEx()
		{
			for (int c = 0; c < 10 && plSheet_eachTimerTick(); c++)
			{ }
		}

		private int pse_index = -1;
		private long pse_convBypassFreezeMtCount = -1L;

		private bool plSheet_eachTimerTick() // ret: ? 繰り返し呼び出すべき
		{
			if (plSheet.RowCount == 0)
			{
				pse_index = -1; // コンバートを先頭から行わせるため..
				return false;
			}
			pse_index++;
			pse_index %= plSheet.RowCount;

			MediaInfo mi = plSheetGetRow(pse_index);
			bool modified = false;

			switch (mi.status)
			{
				case Consts.MediaStatus_e.UNKNOWN:
					mi.status = Consts.MediaStatus_e.NEED_CONVERSION;
					modified = true;
					break;

				case Consts.MediaStatus_e.NEED_CONVERSION:
					if (Utils.isOgxPath(mi.file))
					{
						if (pse_convBypassFreezeMtCount < mtCount)
						{
							//pse_convBypassFreezeMtCount = mtCount + 50; // + convBypass_抑止期間(mtCount)

							convBypass(pse_index);
							return false; // plSheet は mi も含めて既に変更されているはずなので、
						}
					}
					else if (Gnd.i.conv == null)
					{
						Gnd.i.conv = new Conv(mi.file, Path.Combine(Gnd.i.mediaDir, StringTools.zPad(mi.serial, 10)));
						mi.status = Consts.MediaStatus_e.CONVERTING;
						modified = true;
					}
					break;

				case Consts.MediaStatus_e.CONVERTING:
					// conv == null ってことは無いはずだけど一応..
					if (Gnd.i.conv != null && Gnd.i.conv.completed)
					{
						mi.status = Consts.MediaStatus_e.READY;
						completedConvToMediaInfo(Gnd.i.conv, mi);

						Gnd.i.conv.Dispose();
						Gnd.i.conv = null;
						modified = true;

						if (pse_index == 0 && Gnd.i.autoPlayTop) // 自動再生
						{
							int playingIndex = getPlayingIndex();

							if (playingIndex == -1) // ? 未再生
							{
								Gnd.i.lastPlayedSerial = mi.serial;
								Gnd.i.client.sendLine("ES!");
							}
						}
					}
					break;

				case Consts.MediaStatus_e.READY:
					// noop
					break;

				case Consts.MediaStatus_e.PLAYING:
					// noop
					break;

				case Consts.MediaStatus_e.ERROR:
					// noop
					break;

				default:
					throw null;
			}
			if (modified)
			{
				plSheetSetRow(pse_index, mi);

				//Utils.adjustColumnsWidth(plSheet); // ここでやると、超ちらつく
				plAdjColsCountDown = 10;

				return false;
			}
			return true;
		}

		private class ConvBypassEntry
		{
			public int rowidx;
			public MediaInfo mi;
			public bool proced = false;
		};

		/// <summary>
		/// プレイリスト中の未変換 ogg, ogv を一気に変換する。
		/// mainTimerTick の中から呼ばれること！
		/// </summary>
		private void convBypass(int needConvOcxIndex)
		{
			List<ConvBypassEntry> entries = new List<ConvBypassEntry>();

			if (Gnd.i.convBypassまとめて実行)
			{
				for (int rowidx = 0; rowidx < plSheet.RowCount; rowidx++)
				{
					MediaInfo mi = plSheetGetRow(rowidx);

					if (mi.status == Consts.MediaStatus_e.NEED_CONVERSION && Utils.isOgxPath(mi.file))
					{
						entries.Add(new ConvBypassEntry()
						{
							rowidx = rowidx,
							mi = mi,
						});
					}
				}
			}
			else
			{
				entries.Add(new ConvBypassEntry()
				{
					rowidx = needConvOcxIndex,
					mi = plSheetGetRow(needConvOcxIndex),
				});
			}

			{
				OperationCenter convOcBk = Gnd.i.convOc;
				Gnd.i.convOc = new OperationCenter();

				BusyDlg.perform(delegate
				{
					foreach (ConvBypassEntry cbe in entries)
					{
						using (Conv conv = new Conv(cbe.mi.file, Path.Combine(Gnd.i.mediaDir, StringTools.zPad(cbe.mi.serial, 10))))
						{
							while (conv.completed == false)
							{
								Gnd.i.convOc.eachTimerTick();
							}
							cbe.mi.status = Consts.MediaStatus_e.READY;
							completedConvToMediaInfo(conv, cbe.mi);
							cbe.proced = true;
						}
					}
				},
				this,
				entries.Count == 1
				);

				Gnd.i.convOc = convOcBk;
			}

			foreach (ConvBypassEntry cbe in entries)
			{
				if (cbe.proced)
				{
					plSheetSetRow(cbe.rowidx, cbe.mi);

					if (cbe.rowidx == 0 && Gnd.i.autoPlayTop) // 自動再生
					{
						int playingIndex = getPlayingIndex();

						if (playingIndex == -1) // ? 未再生
						{
							Gnd.i.lastPlayedSerial = cbe.mi.serial;
							Gnd.i.client.sendLine("ES!");
						}
					}
				}
			}
		}

		private void completedConvToMediaInfo(Conv conv, MediaInfo mi)
		{
			if (conv.errorMessage == null) // ? 成功った。
			{
				mi.ogxFile = conv.wFile;
				mi.secLength = conv.secLength;

				if (conv.hasVideoStream)
				{
					mi.type = Consts.MediaType_e.MOVIE;
					mi.w = conv.w;
					mi.h = conv.h;
				}
				else
				{
					mi.type = Consts.MediaType_e.AUDIO;
				}
			}
			else // ? エラーった。
			{
				mi.status = Consts.MediaStatus_e.ERROR;
				mi.errorMessage = Gnd.i.conv.errorMessage;
			}
		}

		private int getPlayingIndex()
		{
			for (int rowidx = 0; rowidx < plSheet.RowCount; rowidx++)
			{
				MediaInfo mi = plSheetGetRow(rowidx);

				if (mi.status == Consts.MediaStatus_e.PLAYING)
					return rowidx;
			}
			return -1; // not playing
		}

		private int getFirstReadyIndex()
		{
			for (int rowidx = 0; rowidx < plSheet.RowCount; rowidx++)
			{
				MediaInfo mi = plSheetGetRow(rowidx);

				if (mi.status == Consts.MediaStatus_e.READY)
					return rowidx;
			}
			return -1; // not found
		}

		private int getFirstConvertingIndex()
		{
			for (int rowidx = 0; rowidx < plSheet.RowCount; rowidx++)
			{
				MediaInfo mi = plSheetGetRow(rowidx);

				if (mi.status == Consts.MediaStatus_e.CONVERTING)
					return rowidx;
			}
			return -1; // not found
		}

		private void plSheetInit()
		{
			plSheet.RowCount = 0;
			plSheet.ColumnCount = 0;
			plSheet.ColumnCount = 5;

			plSheet.RowHeadersVisible = false; // 行ヘッダ_非表示

			foreach (DataGridViewColumn column in this.plSheet.Columns) // 列ソート_禁止
			{
				column.SortMode = DataGridViewColumnSortMode.NotSortable;
			}

			int colidx = 0;

			Utils.addColumn(plSheet, colidx++, "Status");
			Utils.addColumn(plSheet, colidx++, "名前");
			Utils.addColumn(plSheet, colidx++, "長さ");
			Utils.addColumn(plSheet, colidx++, "Extra");
			Utils.addColumn(plSheet, colidx++, "Arguments", true);
		}

		public void plSheetSetRow(int rowidx, MediaInfo mi)
		{
			DataGridViewRow row = plSheet.Rows[rowidx];
			int colidx = 0;

			row.Cells[colidx++].Value = Consts.mediaStatuses[(int)mi.status];
			row.Cells[colidx++].Value = mi.title;
			row.Cells[colidx++].Value = Utils.secToUIStamp(mi.secLength);
			row.Cells[colidx++].Value = mi.errorMessage;
			row.Cells[colidx++].Value = mi.encode();
		}

		public MediaInfo plSheetGetRow(int rowidx)
		{
			return MediaInfo.decode(plSheet.Rows[rowidx].Cells[plSheet.ColumnCount - 1].Value.ToString());
		}

		private void plSheet_CellContentClick(object sender, DataGridViewCellEventArgs e)
		{
			// noop
		}

		private void plSheet_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
		{
			// noop -- 文字の部分をdcしないと起きない。
		}

		private void plSheet_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
		{
			try
			{
				Gnd.i.lastPlayedSerial = plSheetGetRow(e.RowIndex).serial;
				Gnd.i.client.sendLine("ES!");
			}
			catch (Exception ex)
			{
				Gnd.i.logger.writeLine("plSheet-DblClk-error: " + ex);
			}
		}

		// 要 plSheet.AllowDrop = true;

		private void plSheet_DragEnter(object sender, DragEventArgs e)
		{
			try
			{
				if (e.Data.GetDataPresent(DataFormats.FileDrop))
					e.Effect = DragDropEffects.Copy;
			}
			catch (Exception ex)
			{
				Gnd.i.logger.writeLine("plSheet-de-error: " + ex);
			}
		}

		private void plSheet_DragDrop(object sender, DragEventArgs e)
		{
			try
			{
				int droppedRowIndex;

				{
					Point pt = this.plSheet.PointToClient(new Point(e.X, e.Y));
					DataGridView.HitTestInfo hit = this.plSheet.HitTest(pt.X, pt.Y);
					droppedRowIndex = hit.RowIndex;
				}

				string[] files = (string[])e.Data.GetData(DataFormats.FileDrop, false);

				if (Gnd.i.plCountMax < files.Length)
					throw new Exception("入力ファイル大杉.1");

				files = Utils.droppedFilesFilter(files);

				if (Gnd.i.plCountMax < files.Length)
					throw new Exception("入力ファイル大杉.2");

				if (files.Length == 0)
					throw new Exception("入力ファイル無し");

				List<MediaInfo> mis = new List<MediaInfo>();

				foreach (string file in files)
					mis.Add(MediaInfo.create(file));

				Gnd.i.oc.add(delegate
				{
					if (droppedRowIndex < 0 || plSheet.RowCount < droppedRowIndex)
						droppedRowIndex = plSheet.RowCount;

					plSheet.RowCount += mis.Count;

					for (int rowidx = plSheet.RowCount - 1; ; rowidx--)
					{
						int rRowidx = rowidx - mis.Count;

						if (rRowidx < droppedRowIndex)
							break;

						plSheetSetRow(rowidx, plSheetGetRow(rRowidx));
					}
					for (int index = 0; index < mis.Count; index++)
					{
						plSheetSetRow(droppedRowIndex + index, mis[index]);
					}
					Utils.adjustColumnsWidth(plSheet);
				});
			}
			catch (Exception ex)
			{
				Gnd.i.logger.writeLine("plSheet-dd-error: " + ex);
			}
		}

		private void PlayListWin_Move(object sender, EventArgs e)
		{
			saveLTWH();
		}

		private void PlayListWin_Resize(object sender, EventArgs e)
		{
			saveLTWH();
		}

		private void 列幅調整WToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Utils.adjustColumnsWidth(plSheet);
		}

		private void 選択解除KToolStripMenuItem_Click(object sender, EventArgs e)
		{
			plSheet.ClearSelection();
		}

		private void 選択されている項目をクリアSToolStripMenuItem_Click(object sender, EventArgs e)
		{
			for (int rowidx = plSheet.RowCount - 1; 0 <= rowidx; rowidx--)
				if (plSheet.Rows[rowidx].Selected)
					tryDeleteRow(rowidx);
		}

		private void エラーになった項目をクリアEToolStripMenuItem_Click(object sender, EventArgs e)
		{
			for (int rowidx = plSheet.RowCount - 1; 0 <= rowidx; rowidx--)
			{
				MediaInfo mi = plSheetGetRow(rowidx);

				if (mi.status == Consts.MediaStatus_e.ERROR)
					tryDeleteRow(rowidx);
			}
		}

		private void 全てクリアCToolStripMenuItem_Click(object sender, EventArgs e)
		{
#if true
			Gnd.i.oc.add(delegate
			{
				List<MediaInfo> mis = new List<MediaInfo>();

				for (int rowidx = 0; rowidx < plSheet.RowCount; rowidx++)
				{
					MediaInfo mi = plSheetGetRow(rowidx);

					if (mi.status == Consts.MediaStatus_e.CONVERTING)
					{
						mis.Add(mi);
					}
					else
					{
						if (mi.status == Consts.MediaStatus_e.PLAYING)
							beforeDeletePlayingRow();

						beforeDeleteRow(mi);
					}
				}
				plSheet.RowCount = mis.Count;

				for (int index = 0; index < mis.Count; index++)
					plSheetSetRow(index, mis[index]);
			});
#else // same
			for (int rowidx = plSheet.RowCount - 1; 0 <= rowidx; rowidx--)
				tryDeleteRow(rowidx);
#endif
		}

		private void tryDeleteRow(int rowidx)
		{
			Gnd.i.oc.add(delegate
			{
				if (rowidx < 0 || plSheet.RowCount <= rowidx)
					return;

				MediaInfo mi = plSheetGetRow(rowidx);

				if (mi.status == Consts.MediaStatus_e.CONVERTING)
					return;

				if (mi.status == Consts.MediaStatus_e.PLAYING)
					beforeDeletePlayingRow();

				beforeDeleteRow(mi);

				for (int r = rowidx + 1; r < plSheet.RowCount; r++)
				{
					plSheetSetRow(r - 1, plSheetGetRow(r));
				}
				plSheet.RowCount--;

				//Utils.adjustColumnsWidth(plSheet); // ちょっと重い。
			});
		}

		private void beforeDeletePlayingRow()
		{
			// oc の中なので mtBusy == true であるはず！
			BusyDlg.perform(delegate
			{
				Gnd.i.client.sendLine("D");
				Gnd.i.client.sendLine("+");

				Thread.Sleep(2000); // 確実に停止するのを待つ。XXX ちゃんと同期したい。
			});
		}

		private void beforeDeleteRow(MediaInfo mi)
		{
			if (mi.ogxFile != "")
			{
				Gnd.i.logger.writeLine("TRY-DELETE-FILE: " + mi.ogxFile);

				try
				{
					File.Delete(mi.ogxFile);
				}
				catch
				{ }
			}
		}

		private void サイズ変更SToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Gnd.i.oc.add(delegate
			{
				Gnd.i.client.x = delegate
				{
					suspendWin();

					using (ScreenSizeWin f = new ScreenSizeWin())
					{
						f.ShowDialog();
					}
					resumeWin();
				};
				Gnd.i.client.sendLineEXP();
			});
		}

		private void モニタ選択MToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (Gnd.i.monitors.getCount() < 2)
			{
				this.mtEnabled = false;
				FailedOperation.caught(new FailedOperation("複数のモニタが必要です。"));
				this.mtEnabled = true;
				return;
			}

			Gnd.i.oc.add(delegate
			{
				Gnd.i.client.x = delegate
				{
					suspendWin();

					using (SelectMonitorDlg f = new SelectMonitorDlg())
					{
						f.ShowDialog();

						if (f.okPressed)
						{
							Gnd.i.oc.add(delegate
							{
								this.Left = Gnd.i.retPlWin_l;
								this.Top = Gnd.i.retPlWin_t;
								this.Width = Gnd.i.retPlWin_w;
								this.Height = Gnd.i.retPlWin_h;
							});
						}
					}
					resumeWin();
				};
				Gnd.i.client.sendLineEXP();
			});
		}

		private void コンバートするファイルを追加AToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Gnd.i.oc.add(delegate
			{
				Gnd.i.client.x = delegate
				{
					bool plConverting = getFirstConvertingIndex() != -1;

					suspendWin();

					using (BgConvWin f = new BgConvWin(plConverting))
					{
						f.ShowDialog();
					}
					resumeWin();
				};
				Gnd.i.client.sendLineEXP();
			});
		}

		private void 設定SToolStripMenuItem1_Click(object sender, EventArgs e)
		{
			Gnd.i.oc.add(delegate
			{
				Gnd.i.client.x = delegate
				{
					suspendWin();

					using (SettingDlg f = new SettingDlg())
					{
						f.ShowDialog();
					}
					resumeWin();
				};
				Gnd.i.client.sendLineEXP();
			});
		}

		public DataGridView getPlSheet()
		{
			return plSheet;
		}

		private void テストToolStripMenuItem1_Click(object sender, EventArgs e)
		{
			new TestWin().Show();
		}

		private void ファイルに保存AToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.mtEnabled = false;
			try
			{
				List<string> pl = new List<string>();

				pl.Add("; きららプレイリスト");
				pl.Add("; 空行と ; で始まる行は無視します。");
				pl.Add("; エンコード = UTF-8");

				for (int rowidx = 0; rowidx < plSheet.RowCount; rowidx++)
				{
					pl.Add(plSheetGetRow(rowidx).file);
				}

				string initDir = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
				string initLFile = "きららプレイリスト.krrpl";

				if (Gnd.i.lastPlayListFile != "")
				{
					initDir = Path.GetDirectoryName(Gnd.i.lastPlayListFile);
					initLFile = Path.GetFileName(Gnd.i.lastPlayListFile);
				}

				//SaveFileDialogクラスのインスタンスを作成
				using (SaveFileDialog sfd = new SaveFileDialog())
				{
					//はじめのファイル名を指定する
					//はじめに「ファイル名」で表示される文字列を指定する
					//sfd.FileName = "きららプレイリスト.krrpl";
					sfd.FileName = initLFile;
					//はじめに表示されるフォルダを指定する
					//sfd.InitialDirectory = @"C:\";
					//sfd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
					sfd.InitialDirectory = initDir;
					//[ファイルの種類]に表示される選択肢を指定する
					//指定しない（空の文字列）の時は、現在のディレクトリが表示される
					//sfd.Filter = "HTMLファイル(*.html;*.htm)|*.html;*.htm|すべてのファイル(*.*)|*.*";
					sfd.Filter = "きららプレイリスト(*.krrpl)|*.krrpl|すべてのファイル(*.*)|*.*";
					//[ファイルの種類]ではじめに選択されるものを指定する
					//2番目の「すべてのファイル」が選択されているようにする
					//sfd.FilterIndex = 2;
					sfd.FilterIndex = 1;
					//タイトルを設定する
					sfd.Title = "プレイリストの保存先を選択してください";
					//ダイアログボックスを閉じる前に現在のディレクトリを復元するようにする
					sfd.RestoreDirectory = true;
					//既に存在するファイル名を指定したとき警告する
					//デフォルトでTrueなので指定する必要はない
					sfd.OverwritePrompt = true;
					//存在しないパスが指定されたとき警告を表示する
					//デフォルトでTrueなので指定する必要はない
					sfd.CheckPathExists = true;

					//ダイアログを表示する
					if (sfd.ShowDialog() == DialogResult.OK)
					{
						//OKボタンがクリックされたとき、選択されたファイル名を表示する
						//Console.WriteLine(sfd.FileName);

						string plFile = sfd.FileName;
						plFile = FileTools.toFullPath(plFile);
						Gnd.i.lastPlayListFile = plFile;

						File.WriteAllLines(plFile, pl, Encoding.UTF8);

						MessageBox.Show(
							"プレイリストを保存しました。",
							"情報",
							MessageBoxButtons.OK,
							MessageBoxIcon.Information
							);
					}
				}
			}
			catch (Exception ex)
			{
				FailedOperation.caught(ex);
			}
			finally
			{
				this.mtEnabled = true;
			}
		}

		private void ファイルから読み込みRToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.mtEnabled = false;
			try
			{
				string initDir = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
				string initLFile = "きららプレイリスト.krrpl";

				if (Gnd.i.lastPlayListFile != "")
				{
					initDir = Path.GetDirectoryName(Gnd.i.lastPlayListFile);
					initLFile = Path.GetFileName(Gnd.i.lastPlayListFile);
				}

				//OpenFileDialogクラスのインスタンスを作成
				using (OpenFileDialog ofd = new OpenFileDialog())
				{
					//はじめのファイル名を指定する
					//はじめに「ファイル名」で表示される文字列を指定する
					//ofd.FileName = "きららプレイリスト.krrpl";
					ofd.FileName = initLFile;
					//はじめに表示されるフォルダを指定する
					//指定しない（空の文字列）の時は、現在のディレクトリが表示される
					//ofd.InitialDirectory = @"C:\";
					//ofd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
					ofd.InitialDirectory = initDir;
					//[ファイルの種類]に表示される選択肢を指定する
					//指定しないとすべてのファイルが表示される
					//ofd.Filter = "HTMLファイル(*.html;*.htm)|*.html;*.htm|すべてのファイル(*.*)|*.*";
					ofd.Filter = "きららプレイリスト(*.krrpl)|*.krrpl|すべてのファイル(*.*)|*.*";
					//[ファイルの種類]ではじめに選択されるものを指定する
					//2番目の「すべてのファイル」が選択されているようにする
					//ofd.FilterIndex = 2;
					ofd.FilterIndex = 1;
					//タイトルを設定する
					ofd.Title = "開くプレイリスト・ファイルを選択してください";
					//ダイアログボックスを閉じる前に現在のディレクトリを復元するようにする
					ofd.RestoreDirectory = true;
					//存在しないファイルの名前が指定されたとき警告を表示する
					//デフォルトでTrueなので指定する必要はない
					ofd.CheckFileExists = true;
					//存在しないパスが指定されたとき警告を表示する
					//デフォルトでTrueなので指定する必要はない
					ofd.CheckPathExists = true;

					//ダイアログを表示する
					if (ofd.ShowDialog() == DialogResult.OK)
					{
						//OKボタンがクリックされたとき、選択されたファイル名を表示する
						//Console.WriteLine(ofd.FileName);

						string plFile = ofd.FileName;
						plFile = FileTools.toFullPath(plFile);
						Gnd.i.lastPlayListFile = plFile;

						List<MediaInfo> mis = new List<MediaInfo>();

						foreach (string file in FileTools.readAllLines(plFile, Encoding.UTF8))
						{
							if (
								file != "" &&
								file[0] != ';'
								)
								mis.Add(MediaInfo.create(file));
						}
						int startPos = plSheet.RowCount;

						plSheet.RowCount += mis.Count;

						for (int index = 0; index < mis.Count; index++)
						{
							plSheetSetRow(startPos + index, mis[index]);
						}
					}
				}
			}
			catch (Exception ex)
			{
				FailedOperation.caught(ex);
			}
			finally
			{
				this.mtEnabled = true;
			}
		}

		private void 再生PToolStripMenuItem_Click(object sender, EventArgs e)
		{
			int rowidx = getTopSelectRowIndex();

			if (rowidx == -1)
				return;

			Gnd.i.lastPlayedSerial = plSheetGetRow(rowidx).serial;
			Gnd.i.client.sendLine("ES!");
		}

		private int getTopSelectRowIndex()
		{
			for (int rowidx = 0; rowidx < plSheet.RowCount; rowidx++)
				if (plSheet.Rows[rowidx].Selected)
					return rowidx;

			return -1; // not found
		}

		private void 停止SToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Gnd.i.client.sendLine("ES/");
		}

		private void psm選択されている項目をクリアToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.選択されている項目をクリアSToolStripMenuItem_Click(null, null);
		}

		private void psmエラーになった項目をクリアToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.エラーになった項目をクリアEToolStripMenuItem_Click(null, null);
		}

		private void psm全てクリアToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.全てクリアCToolStripMenuItem_Click(null, null);
		}

		private SortedList<int> _mvSrcSerials = new SortedList<int>(IntTools.comp);

		private void 選択されている項目を移動元として記憶MToolStripMenuItem_Click(object sender, EventArgs e)
		{
			_mvSrcSerials.clear();

			for (int rowidx = 0; rowidx < plSheet.RowCount; rowidx++)
				if (plSheet.Rows[rowidx].Selected)
					_mvSrcSerials.add(plSheetGetRow(rowidx).serial);
		}

		private void ここへ移動VToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.mtEnabled = false;
			try
			{
				int destRowidx = getFirstSelectedRowIndex();

				if (destRowidx == -1)
					throw new FailedOperation("移動先の項目を選択してね。");

				moveItems(destRowidx);
			}
			catch (Exception ex)
			{
				FailedOperation.caught(ex);
			}
			finally
			{
				this.mtEnabled = true;
			}
		}

		private int getFirstSelectedRowIndex()
		{
			for (int rowidx = 0; rowidx < plSheet.RowCount; rowidx++)
				if (plSheet.Rows[rowidx].Selected)
					return rowidx;

			return -1; // not found
		}

		private void プレイリストの終端へ移動WToolStripMenuItem_Click(object sender, EventArgs e)
		{
			moveItems(plSheet.RowCount);
		}

		private void moveItems(int destRowidx)
		{
			List<MediaInfo> mis = new List<MediaInfo>();

			for (int rowidx = 0; rowidx < plSheet.RowCount; rowidx++)
			{
				MediaInfo mi = plSheetGetRow(rowidx);

				if (_mvSrcSerials.contains(mi.serial))
				{
					mis.Add(mi);
					killRow(rowidx);
				}
			}
			if (1 <= mis.Count)
			{
				plSheet.Rows.Insert(destRowidx, mis.Count);

				for (int index = 0; index < mis.Count; index++)
				{
					plSheetSetRow(destRowidx + index, mis[index]);
				}
			}
			removeAllKilledRow();
			_mvSrcSerials.clear();
		}

		private void killRow(int rowidx)
		{
			plSheet.Rows[rowidx].Cells[0].Value = "";
		}

		private bool isKilledRow(int rowidx)
		{
			return plSheet.Rows[rowidx].Cells[0].Value.ToString() == "";
		}

		private void removeAllKilledRow()
		{
			for (int rowidx = plSheet.RowCount - 1; 0 <= rowidx; rowidx--)
				if (isKilledRow(rowidx))
					plSheet.Rows.RemoveAt(rowidx);
		}

		private void _400x300ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			doResizeScreen(400, 300);
		}

		private void _600x450ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			doResizeScreen(600, 450);
		}

		private void _800x600ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			doResizeScreen(800, 600);
		}

		private void _1000x750ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			doResizeScreen(1000, 750);
		}

		private void _1200x900ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			doResizeScreen(1200, 900);
		}

		private void _1400x1050ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			doResizeScreen(1400, 1050);
		}

		private void _1600x1200ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			doResizeScreen(1600, 1200);
		}

		private void _800x450ToolStripMenuItem1_Click(object sender, EventArgs e)
		{
			doResizeScreen(800, 450);
		}

		private void _1200x675ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			doResizeScreen(1200, 675);
		}

		private void _1600x900ToolStripMenuItem1_Click(object sender, EventArgs e)
		{
			doResizeScreen(1600, 900);
		}

		private void doResizeScreen(int w, int h)
		{
			this.mtEnabled = false;
			try
			{
				Gnd.i.client.doResizeScreen(w, h);
			}
			catch (Exception e)
			{
				FailedOperation.caught(e);
			}
			this.mtEnabled = true;
		}

		private void plSheet_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == (char)32) // space
			{
				e.Handled = true;

				try
				{
					int rowidx = getFirstSelectedRowIndex();

					if (rowidx == -1)
						return;

					Gnd.i.lastPlayedSerial = plSheetGetRow(rowidx).serial;
					Gnd.i.client.sendLine("ES!");
				}
				catch
				{ }

				return;
			}
		}
	}
}
