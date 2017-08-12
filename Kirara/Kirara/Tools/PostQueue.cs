using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Charlotte.Tools
{
	public class PostQueue<T>
	{
		private object SYNCROOT = new object();
		private Queue<T> _q = new Queue<T>();
		private T _defval;

		public PostQueue()
		{ }

		public PostQueue(T defval)
		{
			_defval = defval;
		}

		public void enqueue(T value)
		{
			lock (SYNCROOT)
			{
				_q.Enqueue(value);
			}
		}

		public T dequeue()
		{
			lock (SYNCROOT)
			{
				if (_q.Count == 0)
				{
					return _defval;
				}
				return _q.Dequeue();
			}
		}

		public int getCount()
		{
			lock (SYNCROOT)
			{
				return _q.Count;
			}
		}
	}
}
