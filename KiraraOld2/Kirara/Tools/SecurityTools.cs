using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace Charlotte.Tools
{
	public class SecurityTools
	{
		private static object GCRB_SYNCROOT = new object();
		private const int GCRB_BUFF_SIZE = 1024;
		private static byte[] _gcrbBuff;
		private static int _gcrbIndex = GCRB_BUFF_SIZE;

		public static byte getCRandByte()
		{
			if (GCRB_BUFF_SIZE <= _gcrbIndex)
			{
				_gcrbBuff = getCRand(GCRB_BUFF_SIZE);
				_gcrbIndex = 0;
			}
			return _gcrbBuff[_gcrbIndex++];
		}

		private static RandomNumberGenerator _cRandom = RNGCryptoServiceProvider.Create();

		/// <summary>
		/// thread safe
		/// </summary>
		/// <param name="size"></param>
		/// <returns></returns>
		public static byte[] getCRand(int size)
		{
			byte[] dest = new byte[size];
			_cRandom.GetBytes(dest); // thread safe らしい。https://msdn.microsoft.com/ja-jp/library/wb9c8c67(v=vs.110).aspx
			return dest;
		}

		public static byte[] getSHA512(byte[] src)
		{
			using (SHA512 sha512 = SHA512.Create())
			{
				return sha512.ComputeHash(src);
			}
		}

		public static byte[] getSHA512File(string file)
		{
			using (SHA512 sha512 = SHA512.Create())
			using (FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read))
			{
				return sha512.ComputeHash(fs);
			}
		}
	}
}
