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
		public int playListCountMax = 10000;

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
				playListCountMax = IntTools.toInt(lines[c++], 1);

				// < items
			}
			catch
			{ }
		}

		private string getConfFile()
		{
			return Path.Combine(Program.selfDir, Path.GetFileNameWithoutExtension(Program.selfFile) + ".conf");
		}

		// ---- saved data ----

		public string ffmpegDir = ""; // "" == 未設定
		public bool convWavMastering = false;

		public void loadData()
		{
			try
			{
				string[] lines = File.ReadAllLines(getDataFile(), Encoding.UTF8);
				int c = 0;

				// items >

				ffmpegDir = lines[c++];
				convWavMastering = StringTools.toFlag(lines[c++]);

				// < items
			}
			catch
			{ }
		}

		public void saveData()
		{
			try
			{
				List<string> lines = new List<string>();

				// items >

				lines.Add(ffmpegDir);
				lines.Add(StringTools.toString(convWavMastering));

				// < items

				File.WriteAllLines(getDataFile(), lines, Encoding.UTF8);
			}
			catch
			{ }
		}

		private string getDataFile()
		{
			return Path.Combine(Program.selfDir, Path.GetFileNameWithoutExtension(Program.selfFile) + ".dat");
		}

		// ----

		public Logger logger = new Logger();
		public AudioVideoExtensions audioVideoExtensions = new AudioVideoExtensions();

		public string getMediaDir()
		{
			string tmp = Environment.GetEnvironmentVariable("TMP");

			if (tmp == null || tmp == "")
				throw new Exception("env 'TMP' is empty");

			return Path.Combine(tmp, Consts.MEDIA_DIR_ID);
		}
	}
}
