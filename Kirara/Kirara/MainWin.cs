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
using System.Threading;
using System.IO;

namespace Charlotte
{
	public partial class MainWin : Form
	{
		#region ALT_F4 抑止

		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected override void WndProc(ref Message m)
		{
			const int WM_SYSCOMMAND = 0x112;
			const long SC_CLOSE = 0xF060L;

			if (m.Msg == WM_SYSCOMMAND && (m.WParam.ToInt64() & 0xFFF0L) == SC_CLOSE)
				return;

			base.WndProc(ref m);
		}

		#endregion

		public MainWin()
		{
			InitializeComponent();
		}

		private void MainWin_Load(object sender, EventArgs e)
		{
			// noop
		}

		private void MainWin_Shown(object sender, EventArgs e)
		{
			new Thread((ThreadStart)delegate
			{
				Thread.Sleep(100);

				this.BeginInvoke((MethodInvoker)delegate
				{
					try
					{
						perform();
					}
					catch (Exception ex)
					{
						FailedOperation.caught(ex);
					}
					postPerform();
					this.Close();
				});
			})
			.Start();
		}

		private void MainWin_FormClosing(object sender, FormClosingEventArgs e)
		{
			// noop
		}

		private void MainWin_FormClosed(object sender, FormClosedEventArgs e)
		{
			// noop
		}

		private void perform()
		{
			Gnd.i.ffmpeg = new FFmpeg();
			Gnd.i.wavMaster = new WavMaster();
			Gnd.i.monitors = new Monitors();

			if (Gnd.i.screen_w == -1)
			{
				Monitors.Monitor m = Gnd.i.monitors.get(0);

				if (800 < m.w && 600 < m.h)
				{
					Gnd.i.screen_w = (m.w / 4) * 3;
					Gnd.i.screen_h = (m.h / 4) * 3;
					Gnd.i.screen_l = m.l + (m.w - Gnd.i.screen_w) / 2;
					Gnd.i.screen_t = m.t + (m.h - Gnd.i.screen_h) / 2;
				}
			}

			// set Gnd.i.mediaDir
			{
				string dir = Environment.GetEnvironmentVariable("TMP");

				if (dir == null || dir == "" || Directory.Exists(dir) == false)
					throw new Exception("Wrong TMP env");

				Gnd.i.mediaDir = Path.Combine(dir, Consts.MEDIA_DIR_ID);
			}

			FileTools.deletePath(Gnd.i.mediaDir);
			Directory.CreateDirectory(Gnd.i.mediaDir);

			// スクリーン_既に起動しているかチェック
			{
				using (Nectar2.Recver recver = new Nectar2.Recver(Consts.N2_RECV_IDENT))
				{
					Thread.Sleep(2000); // 受信待ち。

					if (recver.recv() != null) // ? 何かを受信した。== 既に起動している。-> 停止する。
					{
						using (Nectar2.Sender sender = new Nectar2.Sender(Consts.N2_SEND_IDENT))
						{
							sender.send(new byte[] { 0x58, 0x00 }); // send "X"

							Thread.Sleep(5000); // 送信完了待ち。+ スクリーンが完全に終了するまで待つ。
						}
					}
				}
			}
			Gnd.i.bootScreen(); // スクリーン_起動

			this.Visible = false;

			using (PlayListWin f = new PlayListWin())
			{
				f.ShowDialog();
			}
			this.BackColor = Color.FromArgb(50, 50, 50);
			this.Visible = true;
			Application.DoEvents();
			Thread.Sleep(500); // 一瞬で消えるとキモいので、ちょっと待つ。

			// この時点でスクリーンは終了していると想定する！

			FileTools.deletePath(Gnd.i.mediaDir);
		}

		private void postPerform()
		{
			Gnd.i.saveData();

			// release Gnd.i
			{
				Gnd.i.tc.Dispose();
				Gnd.i.tc = null;
			}
		}
	}
}
