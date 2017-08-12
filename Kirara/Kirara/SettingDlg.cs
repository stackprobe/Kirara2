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
	public partial class SettingDlg : Form
	{
		public SettingDlg()
		{
			InitializeComponent();

			loadData();
		}

		private void loadData()
		{
			cbInstantMessagesDisabled.Checked = Gnd.i.instantMessagesDisabled;
			cbAutoPlayTop.Checked = Gnd.i.autoPlayTop;
			cbConvWavMastering.Checked = Gnd.i.convWavMastering;
			cbReportToLogDisabled.Checked = Gnd.i.reportToLogDisabled;
			cbConvBypassまとめて実行.Checked = Gnd.i.convBypassまとめて実行;
		}

		private void saveData()
		{
			Gnd.i.instantMessagesDisabled = cbInstantMessagesDisabled.Checked;
			Gnd.i.autoPlayTop = cbAutoPlayTop.Checked;
			Gnd.i.convWavMastering = cbConvWavMastering.Checked;
			Gnd.i.reportToLogDisabled = cbReportToLogDisabled.Checked;
			Gnd.i.convBypassまとめて実行 = cbConvBypassまとめて実行.Checked;
		}

		private void SettingDlg_Load(object sender, EventArgs e)
		{
			// noop
		}

		private void SettingDlg_Shown(object sender, EventArgs e)
		{
			// noop
		}

		private void SettingDlg_FormClosing(object sender, FormClosingEventArgs e)
		{
			// noop
		}

		private void SettingDlg_FormClosed(object sender, FormClosedEventArgs e)
		{
			// noop
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void btnOk_Click(object sender, EventArgs e)
		{
			saveData();
			this.Close();
		}
	}
}
