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
	public partial class SelectMonitorDlg : Form
	{
		public bool okPressed = false;

		public SelectMonitorDlg()
		{
			InitializeComponent();

			initCmbMonNo(this.cmbMonNoScreen, 0);
			initCmbMonNo(this.cmbMonNoPlayList, 1);
		}

		private void initCmbMonNo(ComboBox cmb, int initSelectIndex)
		{
			cmb.Items.Clear();

			for (int no = 1; no <= Gnd.i.monitors.getCount(); no++)
			{
				cmb.Items.Add("" + no);
			}
			cmb.SelectedIndex = initSelectIndex;
		}

		private void SelectMonitorDlg_Load(object sender, EventArgs e)
		{
			// noop
		}

		private void SelectMonitorDlg_Shown(object sender, EventArgs e)
		{
			// noop
		}

		private void SelectMonitorDlg_FormClosing(object sender, FormClosingEventArgs e)
		{
			// noop
		}

		private void SelectMonitorDlg_FormClosed(object sender, FormClosedEventArgs e)
		{
			// noop
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void btnOk_Click(object sender, EventArgs e)
		{
			// 適用
			{
				{
					Monitors.Monitor m = Gnd.i.monitors.get(this.cmbMonNoScreen.SelectedIndex);

					Gnd.i.screen_l = m.l;
					Gnd.i.screen_t = m.t;
					Gnd.i.screen_w = m.w;
					Gnd.i.screen_h = m.h;
				}

				{
					Monitors.Monitor m = Gnd.i.monitors.get(this.cmbMonNoPlayList.SelectedIndex);

					Gnd.i.retPlWin_l = m.l;
					Gnd.i.retPlWin_t = m.t;
					Gnd.i.retPlWin_w = m.w / 2;
					Gnd.i.retPlWin_h = m.h;
				}
			}
			okPressed = true;
			this.Close();
		}

		private void btnモニタ番号確認_Click(object sender, EventArgs e)
		{
			this.Visible = false;

			using (DispMonitorNoMainWin f = new DispMonitorNoMainWin())
			{
				f.ShowDialog();
			}
			this.Visible = true;
		}

		private void cmbMonNoScreen_SelectedIndexChanged(object sender, EventArgs e)
		{
			cmbMonNoChanged(cmbMonNoScreen, cmbMonNoPlayList);
		}

		private void cmbMonNoPlayList_SelectedIndexChanged(object sender, EventArgs e)
		{
			cmbMonNoChanged(cmbMonNoPlayList, cmbMonNoScreen);
		}

		private void cmbMonNoChanged(ComboBox cmb, ComboBox otherCmb)
		{
			if (cmb.SelectedIndex == otherCmb.SelectedIndex)
			{
				if (otherCmb.SelectedIndex + 1 < otherCmb.Items.Count)
					otherCmb.SelectedIndex++;
				else
					otherCmb.SelectedIndex--;
			}
		}
	}
}
