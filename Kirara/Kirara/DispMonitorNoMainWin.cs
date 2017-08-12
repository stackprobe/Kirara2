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
	public partial class DispMonitorNoMainWin : Form
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

		public static DispMonitorNoMainWin self;

		public DispMonitorNoMainWin()
		{
			self = this;

			InitializeComponent();
		}

		private void DispMonitorNoMainWin_Load(object sender, EventArgs e)
		{
			// noop
		}

		private List<DispMonitorNoWin> _wins = new List<DispMonitorNoWin>();

		private void DispMonitorNoMainWin_Shown(object sender, EventArgs e)
		{
			int ll = 0;
			int tt = 0;

			for (int index = 0; index < Gnd.i.monitors.getCount(); index++)
			{
				Monitors.Monitor m = Gnd.i.monitors.get(index);
				DispMonitorNoWin w = new DispMonitorNoWin(index + 1, m.l, m.t, m.w, m.h);
				w.Show();
				_wins.Add(w);

				ll = Math.Min(ll, m.l);
				tt = Math.Min(tt, m.t);
			}
			this.Left = ll - 400;
			this.Top = tt - 400;
		}

		private void DispMonitorNoMainWin_FormClosing(object sender, FormClosingEventArgs e)
		{
			// noop
		}

		private void DispMonitorNoMainWin_FormClosed(object sender, FormClosedEventArgs e)
		{
			foreach (DispMonitorNoWin w in _wins)
			{
				w.Close();
				w.Dispose();
			}
			self = null;
		}

		public void doClose()
		{
			this.Close();
		}
	}
}
