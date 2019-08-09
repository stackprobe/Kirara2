using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Charlotte
{
	public static class Consts
	{
		public const string EV_CANCEL = "{ea121286-cae7-437b-9604-8f59b48e4ac9}"; // shared_uuid

		public enum MediaType_e
		{
			UNKNOWN,
			AUDIO,
			MOVIE,
		}

		public const int VIDEO_W_MIN = 10;
		public const int VIDEO_H_MIN = 10;
	}
}
