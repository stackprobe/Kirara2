using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Charlotte.Tools
{
	public class EnterLeave : IDisposable
	{
		public delegate void EnterLeave_d();

		private EnterLeave_d _leave;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="enter">null ok</param>
		/// <param name="leave">null ok</param>
		public EnterLeave(EnterLeave_d enter, EnterLeave_d leave)
		{
			if (enter != null)
				enter();

			_leave = leave;
		}

		public void Dispose()
		{
			if (_leave != null)
			{
				_leave();
				_leave = null;
			}
		}
	}
}
