using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Charlotte.Tools
{
	public class SortedList<T>
	{
		private List<T> _list = new List<T>();
		private bool _sorted = true;
		private Comparison<T> _comp;

		public SortedList(Comparison<T> comp)
		{
			_comp = comp;
		}

		public void clear()
		{
			_list.Clear();
			_sorted = true;
		}

		public void add(T element)
		{
			_list.Add(element);
			_sorted = false;
		}

		public int size()
		{
			return _list.Count;
		}

		public T get(int index)
		{
			sortIfNeed();
			return _list[index];
		}

		private void sortIfNeed()
		{
			if (_sorted == false)
			{
				sort();
				_sorted = true;
			}
		}

		private void sort()
		{
			ArrayTools.sort<T>(_list, _comp);
		}

		public bool contains(T ferret)
		{
			return indexOf(ferret) != -1;
		}

		public int indexOf(T ferret)
		{
			sortIfNeed();

			int l = -1;
			int r = _list.Count;

			while (l + 1 < r)
			{
				int m = (l + r) / 2;
				int ret = _comp(_list[m], ferret);

				if (ret < 0)
				{
					l = m;
				}
				else if (0 < ret)
				{
					r = m;
				}
				else
				{
					return m;
				}
			}
			return -1; // not found
		}

		public int leftIndexOf(T ferret)
		{
			sortIfNeed();

			int l = 0;
			int r = _list.Count;

			while (l < r)
			{
				int m = (l + r) / 2;
				int ret = _comp(_list[m], ferret);

				if (ret < 0)
				{
					l = m + 1;
				}
				else
				{
					r = m;
				}
			}
			return l;
		}

		public int rightIndexOf(T ferret)
		{
			sortIfNeed();

			int l = -1;
			int r = _list.Count - 1;

			while (l < r)
			{
				int m = (l + r + 1) / 2;
				int ret = _comp(_list[m], ferret);

				if (0 < ret)
				{
					r = m - 1;
				}
				else
				{
					l = m;
				}
			}
			return r;
		}

		public List<T> getMatch(T ferret)
		{
			//sortIfNeed(); // -> leftIndexOf

			List<T> dest = new List<T>();

			for (int index = leftIndexOf(ferret); index <= rightIndexOf(ferret); index++)
			{
				dest.Add(_list[index]);
			}
			return dest;
		}
	}
}
