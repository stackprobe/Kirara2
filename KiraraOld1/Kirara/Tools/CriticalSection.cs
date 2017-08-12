using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Tools;

namespace Charlotte.Tools
{
	public class CriticalSection
	{
		private static NamedEventPair _enterEv = new NamedEventPair();
		private static NamedEventPair _approveEv = new NamedEventPair();
		private static NamedEventPair _leaveEv = new NamedEventPair();

		public void enter()
		{
			_enterEv.set();
			_approveEv.waitForever();
		}

		public void leave()
		{
			_leaveEv.set();
		}

		public void approve()
		{
			if (_enterEv.waitForMillis(0))
			{
				_approveEv.set();
				_leaveEv.waitForever();
			}
		}

		public EnterLeave critical()
		{
			return new EnterLeave(enter, leave);
		}

		public EnterLeave parallel()
		{
			return new EnterLeave(leave, enter);
		}
	}
}
