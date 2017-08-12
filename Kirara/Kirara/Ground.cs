using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Tools;
using System.IO;
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
		public int plCountMax = 10000;
		public int bciCountMax = 10000;
		public int rFileSizeMax_MB = 700;
		public int keepDiskFree_MB = 3000;

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
				plCountMax = IntTools.toInt(lines[c++], 1);
				bciCountMax = IntTools.toInt(lines[c++], 1);
				rFileSizeMax_MB = IntTools.toInt(lines[c++], 1);
				keepDiskFree_MB = IntTools.toInt(lines[c++], 1);

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
		public string bgConvDestDir = ""; // "" == 未初期化
		public int bgConvWin_l;
		public int bgConvWin_t;
		public int bgConvWin_w = -1; // -1 == bgConvWin_ltwh 未設定
		public int bgConvWin_h;
		public int screen_l;
		public int screen_t;
		public int screen_w = -1; // -1 == screen_ltwh 未設定
		public int screen_h;
		public int normScreen_l = 0;
		public int normScreen_t = 0;
		public int normScreen_w = Consts.SCREEN_W_MIN;
		public int normScreen_h = Consts.SCREEN_H_MIN;
		public int retPlWin_l;
		public int retPlWin_t;
		public int retPlWin_w;
		public int retPlWin_h;
		public string lastPlayListFile = ""; // "" == 未設定
		public int volume = IntTools.IMAX; // 0 ～ IMAX == vol_min ～ vol_max
		public bool autoPlayTop = true;
		public bool instantMessagesDisabled = false;
		public bool reportToLogDisabled = true;
		public bool convBypassまとめて実行 = false;

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
				normScreen_l = int.Parse(lines[c++]);
				normScreen_t = int.Parse(lines[c++]);
				normScreen_w = int.Parse(lines[c++]);
				normScreen_h = int.Parse(lines[c++]);
				volume = int.Parse(lines[c++]);
				autoPlayTop = StringTools.toFlag(lines[c++]);
				instantMessagesDisabled = StringTools.toFlag(lines[c++]);
				reportToLogDisabled = StringTools.toFlag(lines[c++]);
				convBypassまとめて実行 = StringTools.toFlag(lines[c++]);

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
				lines.Add("" + screen_l);
				lines.Add("" + screen_t);
				lines.Add("" + screen_w);
				lines.Add("" + screen_h);
				lines.Add("" + normScreen_l);
				lines.Add("" + normScreen_t);
				lines.Add("" + normScreen_w);
				lines.Add("" + normScreen_h);
				lines.Add("" + volume);
				lines.Add(StringTools.toString(autoPlayTop));
				lines.Add(StringTools.toString(instantMessagesDisabled));
				lines.Add(StringTools.toString(reportToLogDisabled));
				lines.Add(StringTools.toString(convBypassまとめて実行));

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
		public FFmpeg ffmpeg;
		public WavMaster wavMaster;
		public Monitors monitors;
		public string mediaDir;
		public OperationCenter oc = new OperationCenter();
		public OperationCenter convOc = new OperationCenter();
		public List<BgConvInfo> bgConvInfos = new List<BgConvInfo>();
		public Client client = null; // null == スクリーン停止中
		public Conv conv = null; // null == コンバート停止中, 終了時コンバート中なら放置！
		public AudioVideoExtensions audioVideoExtensions = new AudioVideoExtensions();
		public ThreadCenter tc = new ThreadCenter();
		//public bool n2Delay = false;
		public int lastPlayedSerial = -1; // -1 == 未設定
		public Process screenProc = null; // null == 停止中
		public long ignoreBRTimeSec = -1L;

		public void bootScreen()
		{
			try
			{
				if (screenProc != null)
				{
					if (screenProc.HasExited == false) // ? スクリーン実行中
						return;

					screenProc = null;
				}
			}
			catch
			{ }

			// memo: 起動時の警告で拒否すれば例外を投げるよ。

			string file = getKiraraScreenFile();

			{
				ProcessStartInfo psi = new ProcessStartInfo();

				psi.FileName = file;
				psi.Arguments = "AMANOGAWA";
				psi.WorkingDirectory = Path.GetDirectoryName(file);

				Process p = Process.Start(psi);

				// 速攻で p が終了した場合、例外になる。
				try
				{
					p.PriorityClass = ProcessPriorityClass.High;
				}
				catch
				{ }

				screenProc = p;
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

		public void waitForScreenRunning(int millis)
		{
			try
			{
				if (screenProc != null)
					if (screenProc.WaitForExit(millis))
						screenProc = null;
			}
			catch
			{ }
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
