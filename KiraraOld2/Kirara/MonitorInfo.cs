using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Charlotte
{
	public class MonitorInfo
	{
		public int index;
		public int l;
		public int t;
		public int w;
		public int h;

		public MonitorInfo(int i, Screen s)
		{
			index = i;
			l = s.Bounds.Left;
			t = s.Bounds.Top;
			w = s.Bounds.Width;
			h = s.Bounds.Height;
		}

		public int r
		{
			get
			{
				return l + w;
			}
		}

		public int b
		{
			get
			{
				return t + h;
			}
		}
	}
}
