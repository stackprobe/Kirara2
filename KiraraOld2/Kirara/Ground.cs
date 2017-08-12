using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Tools;
using System.IO;
using System.Threading;
using System.Diagnostics;

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
		public int playListSeekSpan = 100;
		public int mediaFileSizeMax_MB = 1000; // 1 GB
		public int keepDiskFree_MB = 2500; // 2.5 GB
		public int bgConvCountMax = 10000;
		public bool productMode = false;

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
				playListSeekSpan = IntTools.toInt(lines[c++], 1);
				mediaFileSizeMax_MB = IntTools.toInt(lines[c++], 1);
				keepDiskFree_MB = IntTools.toInt(lines[c++], 1);
				bgConvCountMax = IntTools.toInt(lines[c++], 1);
				productMode = int.Parse(lines[c++]) != 0;

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
		public int plWin_l;
		public int plWin_t;
		public int plWin_w = -1; // -1 == plWin_ltwh 未設定
		public int plWin_h;
		public bool convWavMastering = false;
		public string bgConvDestDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "KiraraConvOut");
		public int bgConvWin_l;
		public int bgConvWin_t;
		public int bgConvWin_w = -1; // -1 == bgConvWin_ltwh 未設定
		public int bgConvWin_h;
		public int screen_l;
		public int screen_t;
		public int screen_w = -1; // -1 == screen_ltwh 未設定
		public int screen_h;
		public int normalScreen_w = Consts.SCREEN_W_MIN;
		public int normalScreen_h = Consts.SCREEN_H_MIN;

		public void loadData()
		{
			try
			{
				string[] lines = File.ReadAllLines(getDataFile(), Encoding.UTF8);
				int c = 0;

				// items >

				ffmpegDir = lines[c++];
				plWin_l = int.Parse(lines[c++]);
				plWin_t = int.Parse(lines[c++]);
				plWin_w = int.Parse(lines[c++]);
				plWin_h = int.Parse(lines[c++]);
				convWavMastering = StringTools.toFlag(lines[c++]);
				bgConvDestDir = lines[c++];
				bgConvWin_l = int.Parse(lines[c++]);
				bgConvWin_t = int.Parse(lines[c++]);
				bgConvWin_w = int.Parse(lines[c++]);
				bgConvWin_h = int.Parse(lines[c++]);
				screen_l = int.Parse(lines[c++]);
				screen_t = int.Parse(lines[c++]);
				screen_w = int.Parse(lines[c++]);
				screen_h = int.Parse(lines[c++]);

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
				lines.Add("" + plWin_l);
				lines.Add("" + plWin_t);
				lines.Add("" + plWin_w);
				lines.Add("" + plWin_h);
				lines.Add(StringTools.toString(convWavMastering));
				lines.Add(bgConvDestDir);
				lines.Add("" + bgConvWin_l);
				lines.Add("" + bgConvWin_t);
				lines.Add("" + bgConvWin_w);
				lines.Add("" + bgConvWin_h);

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
		public string mediaDir;
		public FFmpeg ffmpge;
		public WavMaster wavMaster;
		public Monitors monitors;
		public List<BgConvInfo> bgConvInfos = new List<BgConvInfo>();
		public PostQueue<EngineTh.Operation_d> invokers = new PostQueue<EngineTh.Operation_d>();
		public FFmpegTh ffmpegTh;
		public ClientTh clientTh;
		public ConvTh convTh;
		public BgConvTh bgConvTh;

		public void startTh(bool plOpening = false)
		{
			if (plOpening)
			{
				ffmpegTh = new FFmpegTh();
			}
			clientTh = new ClientTh();
			convTh = new ConvTh();
			bgConvTh = new BgConvTh();
		}

		public void endTh(bool plClosing = false)
		{
			BusyDlg.perform(delegate
			{
				if (plClosing)
				{
					ffmpegTh.Dispose();
					ffmpegTh = null;
				}
				clientTh.Dispose();
				clientTh = null;

				convTh.Dispose();
				convTh = null;

				bgConvTh.Dispose();
				bgConvTh = null;
			});
		}

		// ---- EngineThs 通信用 ----

		public bool n2Delay = false;
		public bool xRequested = false;

		// ----

		public void bootScreen()
		{
			// memo: 起動時の警告で拒否すれば例外を投げるよ。

			string file = getKiraraScreenFile();

			{
				ProcessStartInfo psi = new ProcessStartInfo();

				psi.FileName = file;
				psi.Arguments = "AMANOGAWA";
				psi.WorkingDirectory = Path.GetDirectoryName(file);

				Process p = Process.Start(psi);

				p.PriorityClass = ProcessPriorityClass.High;
			}
		}

		public static string getKiraraScreenFile()
		{
			string file = "KiraraScreen.exe";

			if (File.Exists(file) == false)
				file = @"..\..\..\..\KiraraScreen\Release\KiraraScreen.exe";

			file = FileTools.makeFullPath(file);
			return file;
		}

		public int screen_r
		{
			get
			{
				return screen_l + screen_w;
			}
		}

		public int screen_b
		{
			get
			{
				return screen_t + screen_h;
			}
		}
	}
}
