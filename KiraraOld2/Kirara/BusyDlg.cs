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

namespace Charlotte
{
	public partial class BusyDlg : Form
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

		public BusyDlg(operation_d operation)
		{
			_operation = operation;

			InitializeComponent();
		}

		private void BusyDlg_Load(object sender, EventArgs e)
		{
			// noop
		}

		private void BusyDlg_Shown(object sender, EventArgs e)
		{
			this.startTh();
			this.mtEnabled = true;
		}

		private void BusyDlg_FormClosing(object sender, FormClosingEventArgs e)
		{
			// noop
		}

		private void BusyDlg_FormClosed(object sender, FormClosedEventArgs e)
		{
			this.mtEnabled = false;
		}

		public static bool performing = false;

		public static void perform(operation_d operation, IWin32Window owner = null)
		{
			if (performing)
			{
				operation();
			}
			else
			{
				using (BusyDlg f = new BusyDlg(operation))
				{
					performing = true;
					f.ShowDialog(owner);
					performing = false;

					if (f._e != null)
					{
						throw new ExceptionCarrier(f._e);
					}
				}
			}
		}

		public delegate void operation_d();
		private operation_d _operation;
		private Exception _e;
		private Thread _th;

		private void startTh()
		{
			_e = null;
			_th = new Thread((ThreadStart)delegate
			{
				try
				{
					_operation();
				}
				catch (Exception e)
				{
					_e = e;
				}
			});
			_th.Start();
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
				{
					Image img = this.pbCyclone.Image;
					img.RotateFlip(RotateFlipType.Rotate270FlipNone);
					this.pbCyclone.Image = img;
				}

				if (5 < this.mtCount && _th.IsAlive == false)
				{
					this.mtEnabled = false;
					this.Close();
					return;
				}
			}
			finally
			{
				this.mtBusy = false;
				this.mtCount++;
			}
		}
	}
}
