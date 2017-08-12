using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Charlotte.Tools;

namespace Charlotte
{
	public class Monitors
	{
		private List<MonitorInfo> _list = new List<MonitorInfo>();

		public Monitors()
		{
			int index = 0;

			foreach (Screen s in Screen.AllScreens)
			{
				int w = s.Bounds.Width;
				int h = s.Bounds.Height;

				if (w < Consts.MONITOR_W_MIN)
					continue;

				if (h < Consts.MONITOR_H_MIN)
					continue;

				_list.Add(new MonitorInfo(index, s));
				index++;
			}
			if (_list.Count < 1)
				throw new FaultOperation("モニタが小さすぎるか、モニタが見つかりません。");
		}

		public int getCount()
		{
			return _list.Count;
		}

		public MonitorInfo get(int index)
		{
			return _list[index];
		}
	}
}
