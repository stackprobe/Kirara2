using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Charlotte
{
	public class Monitors
	{
		private List<Monitor> _mons = new List<Monitor>();

		/// <summary>
		/// ロードに失敗したら例外を投げる。
		/// </summary>
		public Monitors()
		{
			foreach (Screen s in Screen.AllScreens)
			{
				Monitor m = new Monitor()
				{
					l = s.Bounds.Left,
					t = s.Bounds.Top,
					w = s.Bounds.Width,
					h = s.Bounds.Height,
				};

				if (m.w < Consts.MONITOR_W_MIN)
					continue;

				if (m.h < Consts.MONITOR_H_MIN)
					continue;

				_mons.Add(m);
			}

			if (_mons.Count < 1)
				throw new Exception("モニタのサイズが小さすぎるか、モニタを検出できません。");
		}

		public int getCount()
		{
			return _mons.Count;
		}

		public Monitor get(int index)
		{
			return _mons[index];
		}

		public class Monitor
		{
			public int l;
			public int t;
			public int w;
			public int h;

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

		public int whereIs(int l, int t, int w, int h)
		{
			return whereIs(l + w / 2, t + h / 2);
		}

		public int whereIs(int x, int y)
		{
			for (int index = 0; index < _mons.Count; index++)
			{
				Monitor m = _mons[index];

				if (
					m.l <= x && x < m.r &&
					m.t <= y && y < m.b
					)
					return index;
			}
			return -1; // not found
		}

		public int indexOf(int l, int t, int w, int h)
		{
			for (int index = 0; index < _mons.Count; index++)
			{
				Monitor m = _mons[index];

				if (
					m.l == l && m.t == t &&
					m.w == w && m.h == h
					)
					return index;
			}
			return -1; // not found
		}

		public bool contains(int l, int t, int w, int h)
		{
			return indexOf(l, t, w, h) != -1;
		}
	}
}
