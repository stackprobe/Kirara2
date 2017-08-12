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
using System.Threading;

namespace Charlotte
{
	public partial class MainWin : Form
	{
		public static MainWin self;

		public MainWin()
		{
			self = this;

			InitializeComponent();
		}

		private void MainWin_Load(object sender, EventArgs e)
		{
			// noop
		}

		private void MainWin_Shown(object sender, EventArgs e)
		{
			this.Visible = false;

			this.BeginInvoke((MethodInvoker)delegate
			{
				try
				{
					main2();
				}
				catch (Exception ex)
				{
					FaultOperation.caught(ex);
				}
				this.Close();
			});
		}

		private void MainWin_FormClosing(object sender, FormClosingEventArgs e)
		{
			// noop
		}

		private void MainWin_FormClosed(object sender, FormClosedEventArgs e)
		{
			self = null;
		}

		private void main2()
		{
			// set Gnd.i.mediaDir
			{
				string dir = Environment.GetEnvironmentVariable("TMP");

				if (dir == null || dir == "" || Directory.Exists(dir) == false)
					throw new Exception("Wrong TMP env");

				Gnd.i.mediaDir = Path.Combine(dir, Consts.MEDIA_DIR_ID);
			}

			FileTools.deletePath(Gnd.i.mediaDir);
			Directory.CreateDirectory(Gnd.i.mediaDir);

			Gnd.i.ffmpge = new FFmpeg();
			Gnd.i.wavMaster = new WavMaster();
			Gnd.i.monitors = new Monitors();

			BusyDlg.perform(delegate
			{
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
			});

			Gnd.i.bootScreen(); // スクリーン_起動

			using (PlayListWin f = new PlayListWin())
			{
				f.ShowDialog();
			}
		}
	}
}
