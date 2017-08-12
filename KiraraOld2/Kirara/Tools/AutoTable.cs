using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Charlotte.Tools
{
	public class AutoTable<T>
	{
		private List<List<T>> _table = new List<List<T>>();
		private T _defval;

		public AutoTable(T defval)
		{
			_defval = defval;
		}

		public void set(int x, int y, T value)
		{
			while (_table.Count <= y)
			{
				_table.Add(new List<T>());
			}
			List<T> row = _table[y];

			while (row.Count <= x)
			{
				row.Add(_defval);
			}
			row[x] = value;
		}

		public T get(int x, int y)
		{
			if (_table.Count <= y)
			{
				return _defval;
			}
			List<T> row = _table[y];

			if (row.Count <= x)
			{
				return _defval;
			}
			return row[x];
		}
	}
}
