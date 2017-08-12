using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Tools;

namespace Charlotte
{
	public class BgConvInfo
	{
		public string rFile;
		public string wFileNoExt;
		public string wFile = ""; // "" == 未確定
		public Consts.ConvStatus_e status = Consts.ConvStatus_e.READY;
		public string errorMessage = ""; // "" == no error

		public static BgConvInfo create(string rFile, string wFileNoExt)
		{
			return new BgConvInfo()
			{
				rFile = rFile,
				wFileNoExt = wFileNoExt,
			};
		}

		public static BgConvInfo decode(string line)
		{
			string[] tokens = StringTools.decodeLines(line);

			return new BgConvInfo()
			{
				rFile = tokens[0],
				wFileNoExt = tokens[1],
				wFile = tokens[2],
				status = (Consts.ConvStatus_e)int.Parse(tokens[3]),
				errorMessage = tokens[4],
			};
		}

		private BgConvInfo()
		{ }

		public string encode()
		{
			return StringTools.encodeLines(
				rFile,
				wFileNoExt,
				wFile,
				"" + (int)status,
				errorMessage
				);
		}

		public string wFileOrDummy
		{
			get
			{
				if (wFile == "")
					return wFileNoExt + ".ogx";
				else
					return wFile;
			}
		}
	}
}
