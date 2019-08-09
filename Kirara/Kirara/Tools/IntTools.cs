using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Charlotte.Tools
{
	public static class IntTools
	{
		public const int IMAX = 1000000000;

		public static int toInt(string str, int minval = 0, int maxval = IMAX)
		{
			return toRange(int.Parse(str), minval, maxval);
		}

		public static int toRange(int value, int minval = 0, int maxval = IMAX)
		{
			return Math.Min(
				Math.Max(value, minval),
				maxval
				);
		}

		public static int toInt(string str, int minval, int maxval, int defval)
		{
			try
			{
				return toRange(int.Parse(str), minval, maxval, defval);
			}
			catch
			{
				return defval;
			}
		}

		public static int toRange(int value, int minval, int maxval, int defval)
		{
			return isRange(value, minval, maxval) ? value : defval;
		}

		public static bool isRange(int value, int minval, int maxval)
		{
			return minval <= value && value <= maxval;
		}

		public static int toInt(Color color)
		{
			return (int)(
				((uint)color.A << 24) |
				((uint)color.R << 16) |
				((uint)color.G << 8) |
				((uint)color.B << 0));
		}

		public static Color toColor(int color)
		{
			return Color.FromArgb(
				(int)(((uint)color >> 24) & 0xff),
				(int)(((uint)color >> 16) & 0xff),
				(int)(((uint)color >> 8) & 0xff),
				(int)(((uint)color >> 0) & 0xff)
				);
		}

		public static int toInt(double value)
		{
			return value < 0.0 ? (int)(value - 0.5) : (int)(value + 0.5);
		}

		public static Comparison<int> comp = delegate(int a, int b)
		{
			if (a < b)
			{
				return -1;
			}
			if (a > b)
			{
				return 1;
			}
			return 0;
		};
	}
}
