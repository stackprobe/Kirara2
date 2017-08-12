using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Charlotte
{
	public class Consts
	{
		public const string MEDIA_DIR_ID = "{80b5a52a-7cc7-4875-9980-452b0aa95fac}"; // shared_uuid
		public const string MTX_ENGINE_TH = "{e1edf7ff-b80e-455a-8558-ced03096cbaf}";
		public const string N2_SEND_IDENT = "{a86e1bc6-907e-4c08-9633-4bf6f61c2b4f}"; // shared_uuid
		public const string N2_RECV_IDENT = "{4155c94a-b0aa-4738-bf59-b444b755ed81}"; // shared_uuid

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

		public enum ConvStatus_e
		{
			READY,
			CONVERTING,
			COMPLETED,
			ERROR,
		}

		public static readonly string[] convStatuses = new string[]
		{
			"変換待ち",
			"変換中",
			"完了",
			"Error",
		};

		public const int VIDEO_W_MIN = 10;
		public const int VIDEO_H_MIN = 10;
		public const int SCREEN_W_MIN = 400;
		public const int SCREEN_H_MIN = 300;
		public const int MONITOR_W_MIN = SCREEN_W_MIN + 1;
		public const int MONITOR_H_MIN = SCREEN_H_MIN + 1;
	}
}
