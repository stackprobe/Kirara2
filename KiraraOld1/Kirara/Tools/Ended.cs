using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Charlotte.Tools
{
	public class Ended : Cancelled
	{
		public Ended(Exception e = null)
			: this("Ended", e)
		{ }

		public Ended(string message, Exception e = null)
			: base(message, e)
		{ }
	}
}
