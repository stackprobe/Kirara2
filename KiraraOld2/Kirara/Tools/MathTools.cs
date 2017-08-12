using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Charlotte.Tools
{
	public class MathTools
	{
		public static Random random = new Random();

		/// <summary>
		/// thread safe
		/// </summary>
		public class Random
		{
			public delegate byte NextByte_d();

			private NextByte_d _nextByte;

			public Random()
				: this(SecurityTools.getCRandByte)
			{ }

			public Random(NextByte_d nextByte)
			{
				_nextByte = nextByte;
			}

			public ulong getULong()
			{
				return
					((ulong)_nextByte() << 56) |
					((ulong)_nextByte() << 48) |
					((ulong)_nextByte() << 40) |
					((ulong)_nextByte() << 32) |
					((ulong)_nextByte() << 24) |
					((ulong)_nextByte() << 16) |
					((ulong)_nextByte() << 8) |
					((ulong)_nextByte() << 0);
			}

			public uint getUInt()
			{
				return
					((uint)_nextByte() << 24) |
					((uint)_nextByte() << 16) |
					((uint)_nextByte() << 8) |
					((uint)_nextByte() << 0);
			}

			public uint getUInt(uint modulo)
			{
				if (modulo == 0u)
					throw new ArgumentException("modulo is zero");

				uint minval = (uint)(0x100000000ul % (ulong)modulo);
				uint ret;

				do
				{
					ret = getUInt();
				}
				while (ret < minval);

				return ret % modulo;
			}

			public int getInt(int minval, int maxval)
			{
				if (maxval < minval)
					throw new ArgumentException("minval, maxval: " + minval + ", " + maxval);

				ulong modulo = (ulong)((long)maxval - (long)minval + 1L);
				uint ret;

				if (modulo < 0x100000000ul)
					ret = getUInt((uint)modulo);
				else
					ret = getUInt();

				return (int)((long)ret - (long)minval);
			}

			public int getInt(int modulo)
			{
				if (modulo < 1)
					throw new ArgumentException("modulo: " + modulo);

				return (int)getUInt((uint)modulo);
				//return getInt(0, modulo - 1); // same
			}
		}
	}
}
