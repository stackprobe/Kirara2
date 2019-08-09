using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Charlotte
{
	public static class Consts
	{
		public const string MEDIA_DIR_ID = "{80b5a52a-7cc7-4875-9980-452b0aa95fac}"; // shared_uuid

		public enum MediaType_e
		{
			UNKNOWN,
			AUDIO,
			MOVIE,
		}

		public static readonly string[] mediaTypes = new string[]
		{
			"不明",
			"音楽",
			"動画",
		};

		public enum MediaStatus_e
		{
			UNKNOWN,
			NEED_CONVERSION,
			CONVERTING,
			READY,
			PLAYING,
			ERROR,
		}

		public static readonly string[] mediaStatuses = new string[]
		{
			"確認中",
			"変換待ち",
			"変換中",
			"Ready",
			"再生中",
			"Error",
		};

		public const int VIDEO_W_MIN = 10;
		public const int VIDEO_H_MIN = 10;
	}
}
