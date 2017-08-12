using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Tools;
using System.Threading;
using System.Windows.Forms;

namespace Charlotte
{
	public class Client : IDisposable
	{
		private Nectar2.Wrapper _n2w = new Nectar2.Wrapper(
			new Nectar2.Sender(Consts.N2_SEND_IDENT),
			new Nectar2.Recver(Consts.N2_RECV_IDENT),
			StringTools.ENCODING_SJIS
			);

		public Client()
		{
			Gnd.i.bootScreen();
			//Gnd.i.n2Delay = false;
			Gnd.i.ignoreBRTimeSec = -1L;
		}

		public void Dispose()
		{
			dispose();
		}

		public void dispose(IWin32Window owner = null, bool suspending = false)
		{
			if (_n2w != null)
			{
				BusyDlg.perform(delegate
				{
#if true
					if (suspending)
					{
						_n2w.sendLine("D"); // 動画を直ちに停止
					}
					else
					{
						_n2w.sendLine("F"); // 動画・音楽フェードアウト
						_n2w.sendLine("-"); // 壁紙非表示

						Thread.Sleep(2000);
					}
					_n2w.sendLine("X");

					Gnd.i.waitForScreenRunning(5000); // スクリーンが完全に終了するまで待つ。
#else // old
					_n2w.sendLine("F"); // 動画・音楽フェードアウト
					_n2w.sendLine("-"); // 壁紙非表示

					Thread.Sleep(2000);

					_n2w.sendLine("X");
					_n2w.waitForBusySending();

					Thread.Sleep(2000); // スクリーンが完全に終了するまで待つ。
#endif

					_n2w.Dispose();
					_n2w = null;
				},
				owner
				);
			}
		}

		private int _maximizeSerial = -1;
		private double _seekRate = 0.0;

		public void eachTimerTick()
		{
#if false // old
			if (Gnd.i.n2Delay)
			{
				Gnd.i.bootScreen();
				Gnd.i.n2Delay = false;
			}
#endif

			{
				string line = _n2w.recvLine(null);

				if (line != null)
				{
					recvedEvent(line);
				}
			}
		}

		private void recvedEvent(string line)
		{
			if (line == "X")
			{
				xRecved = true;
				return;
			}

			// ---- 最大化 ----

			if (line == "M")
			{
				int playingIndex = getPlayingIndex();
				bool moviePlaying = false;

				if (playingIndex != -1)
				{
					MediaInfo mi = PlayListWin.self.plSheetGetRow(playingIndex);

					if (mi.type == Consts.MediaType_e.MOVIE)
					{
						moviePlaying = true;
						_maximizeSerial = mi.serial;
					}
				}
				if (moviePlaying)
				{
					_n2w.sendLine("C");
					_n2w.sendLine("D");
				}
				_n2w.sendLine("R");
				_n2w.sendLine("EM-2");

				if (moviePlaying)
					_n2w.sendLine("EM-3");
				else
					_n2w.sendLine("EM-3.2");

				return;
			}
			if (line.StartsWith("Curr="))
			{
				_seekRate = (double)int.Parse(line.Substring(5)) / IntTools.IMAX;
				return;
			}
			if (line.StartsWith("Rect="))
			{
				List<string> tokens = StringTools.tokenize(line.Substring(5), ",");
				int c = 0;

				Gnd.i.screen_l = int.Parse(tokens[c++]);
				Gnd.i.screen_t = int.Parse(tokens[c++]);
				Gnd.i.screen_w = int.Parse(tokens[c++]);
				Gnd.i.screen_h = int.Parse(tokens[c++]);

				// adjust
				{
					Gnd.i.screen_w = Math.Max(Gnd.i.screen_w, Consts.SCREEN_W_MIN);
					Gnd.i.screen_h = Math.Max(Gnd.i.screen_h, Consts.SCREEN_H_MIN);
				}
				return;
			}
			if (line == "M-2")
			{
				for (int index = 0; index < Gnd.i.monitors.getCount(); index++)
				{
					Monitors.Monitor m = Gnd.i.monitors.get(index);

					if (
						Gnd.i.screen_l == m.l &&
						Gnd.i.screen_t == m.t &&
						Gnd.i.screen_w == m.w &&
						Gnd.i.screen_h == m.h
						)
					{
						int l = Gnd.i.normScreen_l;
						int t = Gnd.i.normScreen_t;
						int w = Gnd.i.normScreen_w;
						int h = Gnd.i.normScreen_h;

						// old
						/*
						w = Math.Min(w, Gnd.i.screen_w - 1);
						h = Math.Min(h, Gnd.i.screen_h - 1);

						int l = m.l + (m.w - w) / 2;
						int t = m.t + (m.h - h) / 2;

						t = Math.Max(m.t + 30, t); // ウィンドウがデカすぎても上部バーが見えるように。
						*/

						_n2w.sendLine("L" + l);
						_n2w.sendLine("Y" + t);
						_n2w.sendLine("W" + w);
						_n2w.sendLine("H" + h);
						_n2w.sendLine("M");
						_n2w.sendLine("r1");

						// 2回目 -- r1によって位置が正しく設定されないことがあるため。
						{
							_n2w.sendLine("L" + l);
							_n2w.sendLine("Y" + t);
							_n2w.sendLine("W" + w);
							_n2w.sendLine("H" + h);
							_n2w.sendLine("M");
						}
						return;
					}
				}
				Monitors.Monitor currMon = null;

				for (int index = 0; index < Gnd.i.monitors.getCount(); index++)
				{
					Monitors.Monitor m = Gnd.i.monitors.get(index);

					if (
						Gnd.i.screen_l < m.r &&
						Gnd.i.screen_t < m.b &&
						m.l < Gnd.i.screen_r &&
						m.t < Gnd.i.screen_b
						)
					{
						currMon = m;
						break;
					}
				}
				if (currMon == null)
					currMon = Gnd.i.monitors.get(0);

				if (Gnd.i.screen_w != -1) // 2bs? -- 未設定ってことは無いと思うけど..
				{
					Gnd.i.normScreen_l = Gnd.i.screen_l;
					Gnd.i.normScreen_t = Gnd.i.screen_t;
					Gnd.i.normScreen_w = Gnd.i.screen_w;
					Gnd.i.normScreen_h = Gnd.i.screen_h;
				}
				_n2w.sendLine("r0");
				_n2w.sendLine("L" + currMon.l);
				_n2w.sendLine("Y" + currMon.t);
				_n2w.sendLine("W" + currMon.w);
				_n2w.sendLine("H" + currMon.h);
				_n2w.sendLine("M");
				return;
			}
			if (line == "M-3")
			{
				int miIndex = getIndexBySerial(_maximizeSerial);
				_maximizeSerial = -1;

				if (miIndex == -1)
					return; // エラー

				MediaInfo mi = PlayListWin.self.plSheetGetRow(miIndex);

				if (mi.type != Consts.MediaType_e.MOVIE)
					return; // エラー

				int startTime = IntTools.toInt(_seekRate * mi.time);
				startTime = IntTools.toRange(startTime, 0, mi.time - 2000); // ２秒の余裕 <- 動画の長さより長いと不安定になる。

				_n2w.sendLine("I" + mi.serial);
				_n2w.sendLine("W" + mi.w);
				_n2w.sendLine("H" + mi.h);
				_n2w.sendLine("T" + startTime);
				_n2w.sendLine("t" + mi.time);
				_n2w.sendLine("P");
				return;
			}
			if (line == "M-3.2")
			{
				_n2w.sendLine("+");
				return;
			}

			// ---- 連続再生 ----

			if (line == "B" || line == "R")
			{
				int playingIndex = getPlayingIndex();

				if (playingIndex != -1)
				{
					MediaInfo mi = PlayListWin.self.plSheetGetRow(playingIndex);

					if (mi.type == Consts.MediaType_e.AUDIO ? line == "B" : line == "R")
					{
						if (Gnd.i.ignoreBRTimeSec < DateTimeToSec.Now.getSec())
						{
							Gnd.i.ignoreBRTimeSec = DateTimeToSec.Now.getSec() + 20L; // マージン適当, 20秒以上もBR受信し続けたら、もう事故だろう..
							_n2w.sendLine("E-IgnBR");

							doPlayNext(playingIndex);
						}
					}
				}
				return;
			}
			if (line == "-IgnBR")
			{
				Gnd.i.ignoreBRTimeSec = -1L;
				return;
			}

			// ---- 再生ボタン ----

			if (line == "S" || line == "S!" || line == "S/")
			{
				int playingIndex = getPlayingIndex();

				if (line == "S/") // 強制的に停止する。
				{
					if (playingIndex == -1)
					{
						// 2bs -- 停止
						{
							_n2w.sendLine("F");
							_n2w.sendLine("+");
						}
						return;
					}
				}
				if (line == "S!") // 強制的に再生する。
					playingIndex = -1;

				if (playingIndex != -1) // ? 再生中 -> 停止
				{
					{
						MediaInfo mi = PlayListWin.self.plSheetGetRow(playingIndex);
						mi.status = Consts.MediaStatus_e.READY;
						PlayListWin.self.plSheetSetRow(playingIndex, mi);
					}

					_n2w.sendLine("F");
					_n2w.sendLine("+");
				}
				else // ? 停止中 -> 再生
				{
					int index = getIndexBySerial(Gnd.i.lastPlayedSerial);

					if (index == -1)
					{
						for (index = 0; index < PlayListWin.self.getPlSheet().RowCount; index++)
						{
							MediaInfo mi = PlayListWin.self.plSheetGetRow(index);

							if (mi.status == Consts.MediaStatus_e.READY)
								break;
						}
					}
					doPlay(index);
				}
				return;
			}

			// ---- シークバー操作 ----

			if (line.StartsWith("Seek="))
			{
				int playingIndex = getPlayingIndex();

				if (playingIndex != -1)
				{
					MediaInfo mi = PlayListWin.self.plSheetGetRow(playingIndex);

					if (mi.type == Consts.MediaType_e.MOVIE)
					{
						double rate = (double)int.Parse(line.Substring(5)) / IntTools.IMAX;
						int startTime = IntTools.toInt(rate * mi.time);

						_n2w.sendLine("I" + mi.serial);
						_n2w.sendLine("W" + mi.w);
						_n2w.sendLine("H" + mi.h);
						_n2w.sendLine("T" + startTime);
						_n2w.sendLine("t" + mi.time);
						_n2w.sendLine("P");
					}
				}
				return;
			}

			// ---- 情報レスポンス ----

			if (line.StartsWith("Volume="))
			{
				Gnd.i.volume = int.Parse(line.Substring(7));
				return;
			}

			// スクリーンのサイズ Rect= は上の方で、、

			// ----

			if (line.StartsWith("!")) // エラーの通知
			{
				Gnd.i.logger.writeLine("SCREEN_ERROR: " + line.Substring(1));
				return;
			}
			if (line == "Booting")
			{
				_n2w.sendLine("i" + (Gnd.i.instantMessagesDisabled ? 1 : 0));

				if (Gnd.i.monitors.contains(Gnd.i.screen_l, Gnd.i.screen_t, Gnd.i.screen_w, Gnd.i.screen_h))
					_n2w.sendLine("r0");

				_n2w.sendLine("L" + Gnd.i.screen_l);
				_n2w.sendLine("Y" + Gnd.i.screen_t);
				_n2w.sendLine("W" + Gnd.i.screen_w);
				_n2w.sendLine("H" + Gnd.i.screen_h);
				_n2w.sendLine("M");

				_n2w.sendLine("v" + Gnd.i.volume);

				_n2w.sendLine("+"); // 壁紙表示
				return;
			}
			if (line == "Resized")
			{
				int playingIndex = getPlayingIndex();
				bool moviePlaying = false;

				if (playingIndex != -1)
				{
					MediaInfo mi = PlayListWin.self.plSheetGetRow(playingIndex);

					if (mi.type == Consts.MediaType_e.MOVIE)
					{
						moviePlaying = true;
						_maximizeSerial = mi.serial;
					}
				}
				if (moviePlaying)
				{
					_n2w.sendLine("C");
					_n2w.sendLine("D");
				}

				// resize screen
				{
					_n2w.sendLine("L" + Gnd.i.screen_l);
					_n2w.sendLine("Y" + Gnd.i.screen_t);
					_n2w.sendLine("W" + Gnd.i.screen_w);
					_n2w.sendLine("H" + Gnd.i.screen_h);
					_n2w.sendLine("M");
				}

				if (moviePlaying)
					_n2w.sendLine("EM-3");
				else
					_n2w.sendLine("EM-3.2");

				return;
			}
			if (line == "XP") // 終了
			{
				_n2w.sendLine("V");
				_n2w.sendLine("R");
				_n2w.sendLine("EX");
				return;
			}
		}

		// ----

		private void doPlayNext(int index)
		{
			if (doPlayNextMain(index + 1))
				doPlayNextMain(0);
		}

		private bool doPlayNextMain(int index)
		{
			for (; index < PlayListWin.self.getPlSheet().RowCount; index++)
			{
				MediaInfo mi = PlayListWin.self.plSheetGetRow(index);

				if (
					mi.status == Consts.MediaStatus_e.READY ||
					mi.status == Consts.MediaStatus_e.PLAYING
					)
				{
					doPlay(index);
					return false;
				}
			}
			return true;
		}

		private void doPlay(int index)
		{
			if (index < 0 || PlayListWin.self.getPlSheet().RowCount <= index)
				return;

			MediaInfo mi = PlayListWin.self.plSheetGetRow(index);

			if (mi.status == Consts.MediaStatus_e.PLAYING)
			{
				// noop
			}
			else
			{
				if (mi.status != Consts.MediaStatus_e.READY)
					return;

				{
					int playingIndex = getPlayingIndex();

					if (playingIndex != -1)
					{
						MediaInfo pMi = PlayListWin.self.plSheetGetRow(playingIndex);
						pMi.status = Consts.MediaStatus_e.READY;
						PlayListWin.self.plSheetSetRow(playingIndex, pMi);
					}
				}

				mi.status = Consts.MediaStatus_e.PLAYING;
				PlayListWin.self.plSheetSetRow(index, mi);
			}

			if (mi.type == Consts.MediaType_e.AUDIO)
			{
				_n2w.sendLine("F");
				_n2w.sendLine("+");
				_n2w.sendLine("I" + mi.serial);
				_n2w.sendLine("t" + mi.time);
				_n2w.sendLine("B");
			}
			else
			{
				_n2w.sendLine("-");
				_n2w.sendLine("F");
				_n2w.sendLine("I" + mi.serial);
				_n2w.sendLine("W" + mi.w);
				_n2w.sendLine("H" + mi.h);
				_n2w.sendLine("T0");
				_n2w.sendLine("t" + mi.time);
				_n2w.sendLine("P");
			}
			Gnd.i.lastPlayedSerial = mi.serial;
		}

		private int getIndexBySerial(int serial)
		{
			if (serial == -1)
				return -1;

			for (int index = 0; index < PlayListWin.self.getPlSheet().RowCount; index++)
			{
				MediaInfo mi = PlayListWin.self.plSheetGetRow(index);

				if (mi.serial == serial)
					return index;
			}
			return -1;
		}

		private int getPlayingIndex()
		{
			for (int index = 0; index < PlayListWin.self.getPlSheet().RowCount; index++)
			{
				MediaInfo mi = PlayListWin.self.plSheetGetRow(index);

				if (mi.status == Consts.MediaStatus_e.PLAYING)
					return index;
			}
			return -1;
		}

		public void sendLine(string line)
		{
			_n2w.sendLine(line);
		}

		public void sendLineEXP()
		{
			_n2w.sendLine("EXP");
		}

		public delegate void operation_d();

		public operation_d x = null;
		public bool xRecved = false;

		public void doResizeScreen(int w, int h)
		{
			int monIndex = Gnd.i.monitors.whereIs(
				Gnd.i.screen_l,
				Gnd.i.screen_t,
				Gnd.i.screen_w,
				Gnd.i.screen_h
				);

			if (monIndex == -1)
				monIndex = 0;

			Monitors.Monitor m = Gnd.i.monitors.get(monIndex);

			// ウィンドウをリサイズ可能にしたら、モニタより大きく設定できなくなった。ので、その対応。適当だけど..
			{
				w = Math.Min(w, m.w - Consts.SCREEN_MARGIN);
				h = Math.Min(h, m.h - Consts.SCREEN_MARGIN);
			}

			Gnd.i.screen_l = m.l;
			Gnd.i.screen_t = m.t + 30; // ウィンドウ上部のバーが見えるように..
			Gnd.i.screen_w = w;
			Gnd.i.screen_h = h;

			// ----

			int playingIndex = getPlayingIndex();
			bool moviePlaying = false;

			if (playingIndex != -1)
			{
				MediaInfo mi = PlayListWin.self.plSheetGetRow(playingIndex);

				if (mi.type == Consts.MediaType_e.MOVIE)
				{
					moviePlaying = true;
					_maximizeSerial = mi.serial;
				}
			}
			if (moviePlaying)
			{
				_n2w.sendLine("C");
				_n2w.sendLine("D");
			}

			// resize screen
			{
				_n2w.sendLine("L" + Gnd.i.screen_l);
				_n2w.sendLine("Y" + Gnd.i.screen_t);
				_n2w.sendLine("W" + Gnd.i.screen_w);
				_n2w.sendLine("H" + Gnd.i.screen_h);
				_n2w.sendLine("M");
				_n2w.sendLine("r1"); // フルスクリーンの時にやられる可能性があるので、、

				// 2回目 -- r1によって位置が正しく設定されないことがあるため。
				{
					_n2w.sendLine("L" + Gnd.i.screen_l);
					_n2w.sendLine("Y" + Gnd.i.screen_t);
					_n2w.sendLine("W" + Gnd.i.screen_w);
					_n2w.sendLine("H" + Gnd.i.screen_h);
					_n2w.sendLine("M");
				}
			}

			if (moviePlaying)
				_n2w.sendLine("EM-3");
			else
				_n2w.sendLine("EM-3.2");
		}
	}
}
