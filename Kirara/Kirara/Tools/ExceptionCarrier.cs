using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Charlotte.Tools
{
	public class ExceptionCarrier : Exception
	{
		public ExceptionCarrier(Exception e)
			: base("Carrier", e)
		{ }
	}
}
