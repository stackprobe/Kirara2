using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Charlotte.Tools;

namespace Charlotte
{
	public static class Utils
	{
		public static string getFile(string fileNoExt, Consts.MediaType_e type)
		{
			return fileNoExt + getExt(type);
		}

		public static string getExt(Consts.MediaType_e type)
		{
			switch (type)
			{
				case Consts.MediaType_e.AUDIO: return ".ogg";
				case Consts.MediaType_e.MOVIE: return ".ogv";
			}
			throw null;
		}

		public static bool equalsExt(string a, string b)
		{
			return StringTools.equalsIgnoreCase(Path.GetExtension(a), Path.GetExtension(b));
		}

		public static void textFileToLog(string file, Encoding encoding)
		{
			Gnd.i.logger.writeLine("TEXT-FILE-BEGIN");

			foreach (string line in FileTools.readAllLines(file, encoding))
				Gnd.i.logger.writeLine(line);

			Gnd.i.logger.writeLine("TEXT-FILE-END");
		}
	}
}
