using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Charlotte.Tools
{
	/// <summary>
	/// thread safe
	/// </summary>
	public class NamedEventPair : IDisposable
	{
		private object SYNCROOT_evForSet = new object();
		private object SYNCROOT_evForWait = new object();
		private NamedEventObject _evForSet;
		private NamedEventObject _evForWait;

		public NamedEventPair()
			: this(StringTools.getUUID())
		{ }

		public NamedEventPair(string name)
		{
			_evForSet = new NamedEventObject(name);
			_evForWait = new NamedEventObject(name);
		}

		public void set()
		{
			lock (SYNCROOT_evForSet)
			{
				_evForSet.set();
			}
		}

		public bool waitForMillis(int millis)
		{
			lock (SYNCROOT_evForWait)
			{
				return _evForWait.waitForMillis(millis);
			}
		}

		public void waitForever()
		{
			waitForMillis(-1);
		}

		public void Dispose()
		{
			if (_evForSet != null)
			{
				_evForSet.Dispose();
				_evForSet = null;
				_evForWait.Dispose();
				_evForWait = null;
			}
		}
	}
}
