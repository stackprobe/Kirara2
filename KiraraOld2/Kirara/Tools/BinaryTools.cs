using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Charlotte.Tools
{
	public static class BinaryTools
	{
		public static Comparison<byte> comp = delegate(byte a, byte b)
		{
			return (int)a - (int)b;
		};

		public static byte[] getSubBytes(byte[] src, int startPos)
		{
			return getSubBytes(src, startPos, src.Length - startPos);
		}

		public static byte[] getSubBytes(byte[] src, int startPos, int count)
		{
			byte[] dest = new byte[count];
			Array.Copy(src, startPos, dest, 0, count);
			return dest;
		}

		public static byte[] join(params byte[][] blocks)
		{
			int count = 0;

			foreach (byte[] block in blocks)
				count += block.Length;

			byte[] dest = new byte[count];
			count = 0;

			foreach (byte[] block in blocks)
			{
				Array.Copy(block, 0, dest, count, block.Length);
				count += block.Length;
			}
			return dest;
		}

		public static byte[] makeSq(int startChr, int endChr)
		{
			List<byte> dest = new List<byte>();

			for (int chr = startChr; chr <= endChr; chr++)
				dest.Add((byte)chr);

			return dest.ToArray();
		}
	}
}
