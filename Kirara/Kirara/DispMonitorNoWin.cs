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
	public partial class DispMonitorNoWin : Form
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

		private int _no;
		private int _l;
		private int _t;
		private int _w;
		private int _h;

		public DispMonitorNoWin(int no, int l, int t, int w, int h)
		{
			InitializeComponent();

			_no = no;
			_l = l;
			_t = t;
			_w = w;
			_h = h;
		}

		private void DispMonitorNoWin_Load(object sender, EventArgs e)
		{
			// noop
		}

		private void DispMonitorNoWin_Shown(object sender, EventArgs e)
		{
			this.Left = _l;
			this.Top = _t;
			this.Width = _w;
			this.Height = _h;

			if (800 <= _w && 600 <= _h)
				lblMonitorNo.Font = new Font("メイリオ", 200f, FontStyle.Regular);

			lblMonitorNo.Text = "" + _no;

			lblMonitorNo.Left = (this.Width - lblMonitorNo.Width) / 2;
			lblMonitorNo.Top = (this.Height - lblMonitorNo.Height) / 2;

			lblMessage.Left = (this.Width - lblMessage.Width) / 2;
			lblMessage.Top = lblMonitorNo.Top + lblMonitorNo.Height + 10;
		}

		private void DispMonitorNoWin_FormClosing(object sender, FormClosingEventArgs e)
		{
			// noop
		}

		private void DispMonitorNoWin_FormClosed(object sender, FormClosedEventArgs e)
		{
			// noop
		}

		private void lblMonitorNo_Click(object sender, EventArgs e)
		{
			DispMonitorNoWin_Click(null, null);
		}

		private void lblMessage_Click(object sender, EventArgs e)
		{
			DispMonitorNoWin_Click(null, null);
		}

		private void DispMonitorNoWin_Click(object sender, EventArgs e)
		{
			DispMonitorNoMainWin.self.doClose();
		}
	}
}
