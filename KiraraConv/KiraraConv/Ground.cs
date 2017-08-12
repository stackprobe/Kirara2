using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Tools;
using System.IO;

namespace Charlotte
{
	public class Gnd
	{
		private static Gnd _i = null;

		public static Gnd i
		{
			get
			{
				if (_i == null)
					_i = new Gnd();

				return _i;
			}
		}

		private Gnd()
		{ }

		// ---- conf data ----

		public string ffmpegOptAudio = "-codec:a libvorbis -qscale:a 5 -ac 2";
		public string ffmpegOptVideo = "-codec:v libtheora -qscale:v 7";

		public void loadConf()
		{
			try
			{
				List<string> lines = new List<string>();

				foreach (string line in FileTools.readAllLines(getConfFile(), StringTools.ENCODING_SJIS))
					if (line != "" && line[0] != ';')
						lines.Add(line);

				int c = 0;

				// items >

				ffmpegOptAudio = lines[c++];
				ffmpegOptVideo = lines[c++];

				// < items
			}
			catch
			{ }
		}

		private string getConfFile()
		{
			return Path.Combine(Program.selfDir, Path.GetFileNameWithoutExtension(Program.selfFile) + ".conf");
		}

		// ---- args ----

		public int keepDiskFree_MB; // 1 - IMAX
		public int rFileSizeMax_MB; // 1 - IMAX
		public string ffmpegBinDir;
		public string wavMasterBinDir;
		public string rFile;
		public bool convWavMastering;
		public string wFileNoExt;
		public string wFileConvReturn;

		// ----

		public Logger logger = new Logger();
		public NamedEventObject evCancel = new NamedEventObject(Consts.EV_CANCEL);
		public AudioVideoExtensions audioVideoExtensions = new AudioVideoExtensions();
		public ConvReturn convReturn = new ConvReturn();
	}
}
