using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Charlotte.Tools
{
	public class Cancelled : FailedOperation
	{
		public Cancelled(Exception e = null)
			: this("キャンセルしました。", e)
		{ }

		public Cancelled(string message, Exception e = null)
			: base(message, e)
		{ }
	}
}
