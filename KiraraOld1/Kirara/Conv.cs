using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Charlotte.Tools;
using System.IO;
using System.Threading;

namespace Charlotte
{
	public class Conv
	{
		public static Conv curr = null; // null == 未実行

		private string _rFile;
		private int _wIndex;

		public Conv(string rFile, int wIndex)
		{
			_rFile = rFile;
			_wIndex = wIndex;

			startConv();
		}

		public CriticalSection critSect = new CriticalSection();

		private Thread _th = null;
		private string _errorMessage = null; // null == エラー無し
		private Duration _duration = null;
		private AudioStream _audioStream = null;
		private VideoStream _videoStream = null;
		private Consts.MediaType_e _type = Consts.MediaType_e.UNKNOWN;
		private string _wFile = null;

		private class Duration
		{
			public int secLength;
		}

		private class AudioStream
		{
			public int mapIndex;
		}

		private class VideoStream
		{
			public int mapIndex;
			public int w;
			public int h;
		}

		private void startConv()
		{
			_th = new Thread(convTh);
			_th.Start();
		}

		private void convTh()
		{
			using (critSect.critical())
			{
				try
				{
					Gnd.i.logger.writeLine("ConvStart");

					convTh_main();

					Gnd.i.logger.writeLine("ConvEnd");
				}
				catch (Exception e)
				{
					Gnd.i.logger.writeLine("ConvError: " + e);

					_errorMessage = Utils.getMessage(e);
				}
			}
		}

		private void convTh_main()
		{
			using (WorkingDir wd = new WorkingDir())
			{
				string rExt = Path.GetExtension(_rFile);

				if (Gnd.i.audioVideoExtensions.contains(rExt) == false)
					throw new Exception("再生可能なファイルではありません。(不明な拡張子)");

				string midFile = wd.makePath() + rExt;

				try
				{
					using (critSect.parallel())
					{
						File.Copy(_rFile, midFile);
					}
					if (File.Exists(midFile) == false)
						throw null;
				}
				catch
				{
					throw new Exception("ファイルにアクセス出来ません。");
				}
				string redirFile = wd.makePath();

				ProcessTools.runOnBatch("ffprobe.exe " + _rFile + " 2> " + redirFile, FFmpeg.getBDir(), critSect);

				foreach (string line in FileTools.readAllLines(redirFile, Encoding.ASCII))
				{
					if (line.Contains("Duration:"))
					{
						_duration = new Duration();

						List<string> tokens = StringTools.tokenize(line, " :.,", false, true);

						if (tokens[1] == "N/A")
							throw new Exception("再生可能なファイルではありません。(Duration)");

						int h = int.Parse(tokens[1]);
						int m = int.Parse(tokens[2]);
						int s = int.Parse(tokens[3]);

						int sec = h * 3600 + m * 60 + s;

						if (sec < 1)
							throw new Exception("映像又は曲の長さが短すぎます。");

						if (IntTools.IMAX < sec)
							throw new Exception("映像又は曲の長さが長すぎます。");

						_duration.secLength = sec;
					}
					else if (_audioStream == null && line.Contains("Stream") && line.Contains("Audio:"))
					{
						_audioStream = new AudioStream();

						List<string> tokens = StringTools.tokenize(line, StringTools.DIGIT, true, true);

						_audioStream.mapIndex = int.Parse(tokens[1]);
					}
					else if (_videoStream == null && line.Contains("Stream") && line.Contains("Video:"))
					{
						_videoStream = new VideoStream();

						{
							List<string> tokens = StringTools.tokenize(line, StringTools.DIGIT, true, true);

							_videoStream.mapIndex = int.Parse(tokens[1]);
						}

						{
							List<string> tokens = StringTools.tokenize(line, " ,");

							foreach (string fToken in tokens)
							{
								string token = fToken;

								if (StringTools.toFormat(token, true) == "9x9")
								{
									List<string> whTokens = StringTools.tokenize(token, "x");

									_videoStream.w = int.Parse(whTokens[0]);
									_videoStream.h = int.Parse(whTokens[1]);
								}
							}
						}

						if (_videoStream.w < Consts.VIDEO_W_MIN)
							throw new Exception("映像の幅が小さすぎます。");

						if (_videoStream.h < Consts.VIDEO_H_MIN)
							throw new Exception("映像の高さが小さすぎます。");

						if (IntTools.IMAX < _videoStream.w)
							throw new Exception("映像の幅が大きすぎます。");

						if (IntTools.IMAX < _videoStream.h)
							throw new Exception("映像の高さが大きすぎます。");
					}
				}
				if (_duration == null)
					throw null;

				if (_audioStream == null)
					throw new Exception("再生可能なファイルではありません。(音声ストリームがありません)");

				if (_videoStream == null)
					_type = Consts.MediaType_e.AUDIO;
				else
					_type = Consts.MediaType_e.MOVIE;

				string wFile = Utils.getOgxFile(_wIndex, _type);

				if (Gnd.i.convWavMastering)
				{
					string wavFile = wd.makePath() + ".wav";

					ProcessTools.runOnBatch(
						"ffmpeg.exe -i " + _rFile + " -map 0:" + _audioStream.mapIndex + " -ac 2 " + wavFile,
						FFmpeg.getBDir(),
						critSect
						);

					if (File.Exists(wavFile) == false)
						throw new Exception("音声ストリームの抽出に失敗しました。");

					string wmDir = wd.makePath();
					string wmFile = Path.Combine(wmDir, "Master.exe");
					string wavFileNew = wd.makePath() + ".wav";

					Directory.CreateDirectory(wmDir);
					File.Copy(wavMasteringFile, wmFile);

					ProcessTools.runOnBatch(
						"Master.exe " + wavFile + " " + wavFileNew + " 0001.txt",
						wmDir,
						critSect
						);

					if (File.Exists(wavFileNew) == false)
						throw new Exception("wavFileNew does not exist");

					if (_type == Consts.MediaType_e.AUDIO)
					{
						ProcessTools.runOnBatch(
							"ffmpeg.exe -i " + wavFileNew + " -map 0:" + _audioStream.mapIndex + " " + Gnd.i.ffmpegOptAudio + " " + wFile,
							FFmpeg.getBDir(),
							critSect
							);
					}
					else
					{
						ProcessTools.runOnBatch(
							"ffmpeg.exe -i " + _rFile + " -i " + wavFileNew + " -map 0:" + _videoStream.mapIndex + " -map 1:" + _audioStream.mapIndex + " " + Gnd.i.ffmpegOptVideo + " " + Gnd.i.ffmpegOptAudio + " " + wFile,
							FFmpeg.getBDir(),
							critSect
							);
					}
				}
				else
				{
					if (_type == Consts.MediaType_e.AUDIO)
					{
						ProcessTools.runOnBatch(
							"ffmpeg.exe -i " + _rFile + " -map 0:" + _audioStream.mapIndex + " " + Gnd.i.ffmpegOptAudio + " " + wFile,
							FFmpeg.getBDir(),
							critSect
							);
					}
					else
					{
						ProcessTools.runOnBatch(
							"ffmpeg.exe -i " + _rFile + " -map 0:" + _videoStream.mapIndex + " -map 0:" + _audioStream.mapIndex + " " + Gnd.i.ffmpegOptVideo + " " + Gnd.i.ffmpegOptAudio + " " + wFile,
							FFmpeg.getBDir(),
							critSect
							);
					}
				}
				if (File.Exists(wFile) == false)
					throw new Exception("wFile does not exist");

				_wFile = wFile;
			}
		}

		private string wavMasteringFile
		{
			get
			{
				string file = "Master.exe";

				if (File.Exists(file) == false)
					file = @"C:\Factory\Program\WavMaster\Master.exe";

				file = FileTools.makeFullPath(file);
				return file;
			}
		}

		public void end()
		{
			if (_th != null)
			{
				_th.Join();
				_th = null;
			}
		}

		public bool isEnded()
		{
			if (_th != null && _th.IsAlive == false)
				_th = null;

			return _th == null;
		}

		public bool hasError()
		{
			return _errorMessage != null;
		}

		public string getErrorMessage()
		{
			return _errorMessage;
		}

		public string getOgxFile()
		{
			return _wFile;
		}

		public Consts.MediaType_e getMediaType()
		{
			return _type;
		}
	}
}
