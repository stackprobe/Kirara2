using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Tools;
using System.IO;

namespace Charlotte
{
	public class MediaInfo
	{
		public string file;
		public string ogxFile = ""; // "" == 未定義
		public int serial;
		public Consts.MediaType_e type = Consts.MediaType_e.UNKNOWN;
		public Consts.MediaStatus_e status = Consts.MediaStatus_e.UNKNOWN;
		public int secLength = -1; // -1 == 未定義
		public string errorMessage = ""; // "" == no error
		public int w = -1; // -1 == 映像無し, VIDEO_W_MIN <=
		public int h = -1; // -1 == 映像無し, VIDEO_H_MIN <=

		public static MediaInfo create(string file)
		{
			return new MediaInfo()
			{
				file = file,
				serial = Utils.getSerial(),
			};
		}

		public static MediaInfo decode(string line)
		{
			string[] tokens = StringTools.decodeLines(line);

			return new MediaInfo()
			{
				file = tokens[0],
				ogxFile = tokens[1],
				serial = int.Parse(tokens[2]),
				type = (Consts.MediaType_e)int.Parse(tokens[3]),
				status = (Consts.MediaStatus_e)int.Parse(tokens[4]),
				secLength = int.Parse(tokens[5]),
				errorMessage = tokens[6],
				w = int.Parse(tokens[7]),
				h = int.Parse(tokens[8]),
			};
		}

		public string encode()
		{
			return StringTools.encodeLines(
				file,
				ogxFile,
				"" + serial,
				"" + (int)type,
				"" + (int)status,
				"" + secLength,
				errorMessage,
				"" + w,
				"" + h
				);
		}

		private MediaInfo()
		{ }

		public string title
		{
			get
			{
				return Path.GetFileNameWithoutExtension(file);
			}
		}

		public int time
		{
			get
			{
				return secLength * 1000;
			}
		}
	}
}
