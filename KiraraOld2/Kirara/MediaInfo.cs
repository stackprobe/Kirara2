using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Charlotte.Tools;

namespace Charlotte
{
	public class MediaInfo
	{
		public string file;
		public string ogxFile = ""; // "" == 未定義
		public int index;
		public Consts.MediaType_e type = Consts.MediaType_e.UNKNOWN;
		public Consts.MediaStatus_e status = Consts.MediaStatus_e.UNKNOWN;
		public int sec = -1; // -1 == 不明
		public string errorMessage = ""; // "" == no error
		public bool lastPlayed = false;
		public int w = -1; // -1 == 映像無し, VIDEO_W_MIN <=
		public int h = -1; // -1 == 映像無し, VIDEO_H_MIN <=

		public static MediaInfo create(string file)
		{
			return new MediaInfo()
			{
				file = file,
				index = Utils.nextIndex(),
			};
		}

		public static MediaInfo decode(string line)
		{
			string[] tokens = StringTools.decodeLines(line);

			return new MediaInfo()
			{
				file = tokens[0],
				ogxFile = tokens[1],
				index = int.Parse(tokens[2]),
				type = (Consts.MediaType_e)int.Parse(tokens[3]),
				status = (Consts.MediaStatus_e)int.Parse(tokens[4]),
				sec = int.Parse(tokens[5]),
				errorMessage = tokens[6],
				lastPlayed = StringTools.toFlag(tokens[7]),
				w = int.Parse(tokens[8]),
				h = int.Parse(tokens[9]),
			};
		}

		private MediaInfo()
		{ }

		public string encode()
		{
			return StringTools.encodeLines(
				file,
				ogxFile,
				"" + index,
				"" + (int)type,
				"" + (int)status,
				"" + sec,
				errorMessage,
				StringTools.toString(lastPlayed),
				"" + w,
				"" + h
				);
		}

		public string title
		{
			get
			{
				return Path.GetFileNameWithoutExtension(file);
			}
		}
	}
}
