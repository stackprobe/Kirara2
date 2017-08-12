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
	public partial class ScreenSizeWin : Form
	{
		private string messageFormat;

		public ScreenSizeWin()
		{
			InitializeComponent();

			this.MinimumSize = new Size(Consts.SCREEN_W_MIN, Consts.SCREEN_H_MIN);
			messageFormat = lblMessage.Text;
		}

		private void ScreenSizeWin_Load(object sender, EventArgs e)
		{
			// noop
		}

		private void ScreenSizeWin_Shown(object sender, EventArgs e)
		{
			this.Left = Gnd.i.screen_l;
			this.Top = Gnd.i.screen_t;
			this.Width = Gnd.i.screen_w;
			this.Height = Gnd.i.screen_h;

			refreshUI();
		}

		private void ScreenSizeWin_FormClosing(object sender, FormClosingEventArgs e)
		{
			Gnd.i.screen_l = this.Left;
			Gnd.i.screen_t = this.Top;
			Gnd.i.screen_w = this.Width;
			Gnd.i.screen_h = this.Height;
		}

		private void ScreenSizeWin_FormClosed(object sender, FormClosedEventArgs e)
		{
			// noop
		}

		private void ScreenSizeWin_DoubleClick(object sender, EventArgs e)
		{
			this.Close();
		}

		private void ScreenSizeWin_ResizeBegin(object sender, EventArgs e)
		{
			refreshUI();
		}

		private void ScreenSizeWin_Resize(object sender, EventArgs e)
		{
			refreshUI();
		}

		private void ScreenSizeWin_ResizeEnd(object sender, EventArgs e)
		{
			refreshUI();
		}

		private void ScreenSizeWin_Move(object sender, EventArgs e)
		{
			refreshUI();
		}

		private void refreshUI()
		{
			lblMessage.Left = (mainPanel.Width - lblMessage.Width) / 2;
			lblMessage.Top = (mainPanel.Height - lblMessage.Height) / 2;

			{
				string message = messageFormat;

				message = message.Replace("$L", "" + this.Left);
				message = message.Replace("$T", "" + this.Top);
				message = message.Replace("$W", "" + this.Width);
				message = message.Replace("$H", "" + this.Height);

				lblMessage.Text = message;
			}
		}

		private void mainPanel_Paint(object sender, PaintEventArgs e)
		{
			// noop
		}

		private void mainPanel_DoubleClick(object sender, EventArgs e)
		{
			ScreenSizeWin_DoubleClick(null, null);
		}

		private void lblMessage_Click(object sender, EventArgs e)
		{
			// noop
		}

		private void lblMessage_DoubleClick(object sender, EventArgs e)
		{
			ScreenSizeWin_DoubleClick(null, null);
		}
	}
}
