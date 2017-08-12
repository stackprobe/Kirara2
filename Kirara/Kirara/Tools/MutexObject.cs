using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Charlotte.Tools
{
	public class MutexObject : IDisposable
	{
		private Mutex _m;

		public MutexObject(string name)
		{
			_m = new Mutex(false, name);
		}

		public bool waitForMillis(int millis)
		{
			return _m.WaitOne(millis);
		}

		public void waitForever()
		{
			_m.WaitOne();
		}

		public void release()
		{
			_m.ReleaseMutex();
		}

		public void Dispose()
		{
			if (_m != null)
			{
				_m.Dispose();
				_m = null;
			}
		}

		public static EnterLeave section(string name)
		{
			MutexObject m = new MutexObject(name);

			return new EnterLeave(delegate
			{
				m.waitForever();
			},
			delegate
			{
				m.release();
				m.Dispose();
			});
		}

		public static EnterLeave section(MutexObject m)
		{
			return new EnterLeave(delegate
			{
				m.waitForever();
			},
			delegate
			{
				m.release();
			});
		}

		public static EnterLeave deadSection(MutexObject m)
		{
			return new EnterLeave(delegate
			{
				m.release();
			},
			delegate
			{
				m.waitForever();
			});
		}
	}
}
