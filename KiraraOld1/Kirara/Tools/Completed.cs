using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Charlotte.Tools
{
	public class Completed : Ended
	{
		public Completed(Exception e = null)
			: this("完了しました。", e)
		{ }

		public Completed(string message, Exception e = null)
			: base(message, e)
		{ }
	}
}
