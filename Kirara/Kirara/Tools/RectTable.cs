using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Charlotte.Tools
{
	public class RectTable<T>
	{
		public int w; // 0 <=
		public int h; // 0 <=

		private AutoTable<T> _table;

		public RectTable(int w, int h, T defval)
		{
			this.w = w;
			this.h = h;

			_table = new AutoTable<T>(defval);
		}

		public void set(int x, int y, T value)
		{
			w = Math.Max(w, x + 1);
			h = Math.Max(h, y + 1);

			_table.set(x, y, value);
		}

		public T get(int x, int y)
		{
			return _table.get(x, y);
		}
	}
}
