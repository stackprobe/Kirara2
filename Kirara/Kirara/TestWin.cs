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
	public partial class TestWin : Form
	{
		public TestWin()
		{
			InitializeComponent();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			Gnd.i.convWavMastering = true;
			Gnd.i.conv = new Conv(@"C:\var\mp4_test\GirlsUndPanzerPv4.m4v", @"C:\temp\GirlsUndPanzerPv4_mst");
		}

		private void button2_Click(object sender, EventArgs e)
		{
			Gnd.i.convWavMastering = false;
			Gnd.i.conv = new Conv(@"C:\var\mp4_test\GirlsUndPanzerPv4.m4v", @"C:\temp\GirlsUndPanzerPv4_no-mst");
		}

		private void button3_Click(object sender, EventArgs e)
		{
			Gnd.i.convWavMastering = true;
			Gnd.i.conv = new Conv(@"C:\var\月姫リメイクop.mp3", @"C:\temp\月姫リメイクop_mst");
		}

		private void button4_Click(object sender, EventArgs e)
		{
			Gnd.i.convWavMastering = false;
			Gnd.i.conv = new Conv(@"C:\var\月姫リメイクop.mp3", @"C:\temp\月姫リメイクop_no-mst");
		}

		private void button5_Click(object sender, EventArgs e)
		{
			string[] lines = new string[]
			{
				"completed: " + Gnd.i.conv.completed,
				"errorMessage: " + Gnd.i.conv.errorMessage,
				"wFile: " + Gnd.i.conv.wFile,
				"w: " + (Gnd.i.conv.hasVideoStream ? Gnd.i.conv.w : -1),
				"h: " + (Gnd.i.conv.hasVideoStream ? Gnd.i.conv.h : -1),
			};

			textBox1.Text = string.Join("\r\n", lines);
		}

		private void button6_Click(object sender, EventArgs e)
		{
			Gnd.i.conv.Dispose();
			Gnd.i.conv = null;
		}
	}
}
