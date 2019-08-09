using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Charlotte.Tools
{
	public static class DoubleTools
	{
		public static double toRange(double value, double minval, double maxval)
		{
			return Math.Min(
				Math.Max(value, minval),
				maxval
				);
		}

		public static bool isRange(double value, double minval, double maxval)
		{
			return minval <= value && value <= maxval;
		}
	}
}
