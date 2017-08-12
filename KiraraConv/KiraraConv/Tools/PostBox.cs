using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Charlotte.Tools
{
	/// <summary>
	/// thread safe
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class PostBox<T>
	{
		private object SYNCROOT = new object();
		private T _value;

		public PostBox()
		{ }

		public PostBox(T value)
		{
			_value = value;
		}

		public T get()
		{
			lock (SYNCROOT)
			{
				return _value;
			}
		}

		public void post(T value)
		{
			lock (SYNCROOT)
			{
				_value = value;
			}
		}

		public T getPost(T value)
		{
			lock (SYNCROOT)
			{
				T ret = _value;
				_value = value;
				return ret;
			}
		}
	}
}
