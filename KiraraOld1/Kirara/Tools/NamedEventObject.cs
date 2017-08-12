using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Charlotte.Tools
{
	public class NamedEventObject : IDisposable
	{
		private EventWaitHandle _ev;

		public NamedEventObject(string name)
		{
			_ev = new EventWaitHandle(false, EventResetMode.AutoReset, name);
		}

		public void set()
		{
			_ev.Set();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="millis">-1 == INFINITE</param>
		/// <returns>シグナルを受け取った。</returns>
		public bool waitForMillis(int millis)
		{
			return _ev.WaitOne(millis);
		}

		public void waitForever()
		{
			waitForMillis(-1);
		}

		public void Dispose()
		{
			if (_ev != null)
			{
				_ev.Dispose();
				_ev = null;
			}
		}
	}
}
