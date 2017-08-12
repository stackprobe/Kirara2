using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Charlotte.Tools
{
	public class StringTools
	{
		public const string DIGIT = "0123456789";
		public const string ALPHA = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
		public const string alpha = "abcdefghijklmnopqrstuvwxyz";
		public const string HEXADECIMAL = "0123456789ABCDEF";
		public const string hexadecimal = "0123456789abcdef";
		public const string OCTODECIMAL = "01234567";
		public const string BINADECIMAL = "01";

		public static readonly Encoding ENCODING_SJIS = Encoding.GetEncoding(932);

		public static List<string> tokenize(string str, string delimiters, bool meaningFlag = false, bool ignoreEmpty = false)
		{
			StringBuilder buff = new StringBuilder();
			List<string> tokens = new List<string>();

			foreach (char chr in str)
			{
				if (delimiters.Contains(chr) == meaningFlag)
				{
					buff.Append(chr);
				}
				else
				{
					if (ignoreEmpty == false || buff.Length != 0)
						tokens.Add(buff.ToString());

					buff = new StringBuilder();
				}
			}
			if (ignoreEmpty == false || buff.Length != 0)
				tokens.Add(buff.ToString());

			return tokens;
		}

		public static string replace(string str, string fromChrs, char toChr)
		{
			foreach (char fromChr in fromChrs)
			{
				str = str.Replace(fromChr, toChr);
			}
			return str;
		}

		public static string replaceLoop(string str, string fromPtn, string toPtn, int maxCount)
		{
			for (int count = 0; count < maxCount; count++)
			{
				str = str.Replace(fromPtn, toPtn);
			}
			return str;
		}

		public static string zPad(int value, int minlen, string padding = "0")
		{
			return zPad("" + value, minlen, padding);
		}

		public static string zPad(string str, int minlen, string padding = "0")
		{
			while (str.Length < minlen)
			{
				str = padding + str;
			}
			return str;
		}

		public static bool startsWithIgnoreCase(string str, string ptn)
		{
			return str.ToLower().StartsWith(ptn.ToLower());
		}

		public static bool endsWithIgnoreCase(string str, string ptn)
		{
			return str.ToLower().EndsWith(ptn.ToLower());
		}

		public static bool equalsIgnoreCase(string a, string b)
		{
			return a.ToLower() == b.ToLower();
		}

		public static string getUUID()
		{
			return Guid.NewGuid().ToString("B");
		}

		public const string S_TRUE = "true";
		public const string S_FALSE = "false";

		public static bool toFlag(string str)
		{
			return equalsIgnoreCase(str, S_TRUE) || str == "1";
		}

		public static string toString(bool flag)
		{
			return flag ? S_TRUE : S_FALSE;
		}

		public static string toBase64(byte[] data)
		{
			return Convert.ToBase64String(data);
		}

		public static byte[] decodeBase64(string str)
		{
			return Convert.FromBase64String(str);
		}

		public static string encode(string str)
		{
			return toBase64(Encoding.UTF8.GetBytes(str));
		}

		public static string decode(string str)
		{
			return Encoding.UTF8.GetString(decodeBase64(str));
		}

		public static string encodeLines(params string[] lines)
		{
			List<string> tokens = new List<string>();

			foreach (string line in lines)
				tokens.Add(encode(line));

			tokens.Add("");
			return string.Join(":", tokens);
		}

		public static string[] decodeLines(string line)
		{
			List<string> tokens = tokenize(line, ":");
			int count = tokens.Count - 1;
			string[] lines = new string[count];

			for (int index = 0; index < count; index++)
				lines[index] = decode(tokens[index]);

			return lines;
		}

		public static string escape(string str)
		{
			StringBuilder buff = new StringBuilder();

			foreach (char chr in str)
			{
				if (chr <= ' ' || chr == '$')
				{
					buff.Append('$');
					buff.Append(hexadecimal[(int)chr / 16]);
					buff.Append(hexadecimal[(int)chr % 16]);
				}
				else
				{
					buff.Append(chr);
				}
			}
			return buff.ToString();
		}

		public static string unescape(string str)
		{
			StringBuilder buff = new StringBuilder();

			for (int index = 0; index < str.Length; index++)
			{
				if (str[index] == '$')
				{
					buff.Append((char)(
						hexadecimal.IndexOf(str[index + 1]) * 16 +
						hexadecimal.IndexOf(str[index + 2])
						));
					index += 2;
				}
				else
				{
					buff.Append(str[index]);
				}
			}
			return buff.ToString();
		}

		public static Comparison<string> comp = delegate(string a, string b)
		{
#if true
			return eComp(a, b);
#else
			return a.CompareTo(b); // "X" < "x-" < "-x" < "X"
#endif
		};

		public static Comparison<string> compIgnoreCase = delegate(string a, string b)
		{
			return comp(a.ToLower(), b.ToLower());
		};

		public static int eComp(string a, string b)
		{
			return eComp(a, b, Encoding.UTF8);
		}

		public static int eComp(string a, string b, Encoding encoding)
		{
			return ArrayTools.arrComp<byte>(encoding.GetBytes(a), encoding.GetBytes(b), BinaryTools.comp);
		}

		public static string toFormat(string str, bool antiRepeat = false)
		{
			str = replace(str, DIGIT, '9');
			str = replace(str, ALPHA, 'A');
			str = replace(str, alpha, 'a');

			if (antiRepeat)
			{
				str = replaceLoop(str, "99", "9", 20);
				str = replaceLoop(str, "AA", "A", 20);
				str = replaceLoop(str, "aa", "a", 20);
			}
			return str;
		}

		public static string toDigitFormat(string str, bool antiRepeat = false)
		{
			str = replace(str, DIGIT, '9');

			if (antiRepeat)
				str = replaceLoop(str, "99", "9", 20);

			return str;
		}

		public static string toHexFormat(string str, bool antiRepeat = false)
		{
			str = replace(str, HEXADECIMAL + hexadecimal, '9');

			if (antiRepeat)
				str = replaceLoop(str, "99", "9", 20);

			return str;
		}

		public static string toHex(byte[] src)
		{
			StringBuilder buff = new StringBuilder();

			foreach (byte chr in src)
			{
				buff.Append(hexadecimal[(int)chr / 16]);
				buff.Append(hexadecimal[(int)chr % 16]);
			}
			return buff.ToString();
		}

		public static byte[] hex(string src)
		{
			if (src.Length % 2 != 0)
				throw new FormatException("hex-string length error");

			byte[] dest = new byte[src.Length / 2];

			for (int index = 0; index < dest.Length; index++)
			{
				int v1 = hexadecimal.IndexOf(src[index * 2]);
				int v2 = hexadecimal.IndexOf(src[index * 2 + 1]);

				if (v1 == -1 || v2 == -1)
					throw new FormatException("hex-string char error");

				dest[index] = (byte)((v1 << 4) | v2);
			}
			return dest;
		}

		public static string repeat(string ptn, int count)
		{
			StringBuilder buff = new StringBuilder();

			while (0 < count)
			{
				buff.Append(ptn);
				count--;
			}
			return buff.ToString();
		}

		public static readonly string ASCII = Encoding.ASCII.GetString(BinaryTools.makeSq(0x20, 0x7e));
		public static readonly string HAN_KANA = Encoding.ASCII.GetString(BinaryTools.makeSq(0xa1, 0xdf));
	}
}
