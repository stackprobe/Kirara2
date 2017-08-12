using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Tools;
using System.IO;

namespace Charlotte
{
	public class Conv
	{
		private Duration _duration = null;
		private AudioStream _audioStream = null;
		private VideoStream _videoStream = null;
		private Consts.MediaType_e _type = Consts.MediaType_e.UNKNOWN;

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

		public void perform()
		{
			using (WorkingDir wd = new WorkingDir())
			{
				string rExt = Path.GetExtension(Gnd.i.rFile);

				if (Gnd.i.audioVideoExtensions.contains(rExt) == false)
					throw new Exception("再生可能なファイルではありません。(不明な拡張子)");

				string midRFile = wd.makePath() + rExt;

				try
				{
					File.Copy(Gnd.i.rFile, midRFile);

					if (File.Exists(midRFile) == false)
						throw null;
				}
				catch
				{
					throw new Exception("ファイルにアクセス出来ません。");
				}

				string redirFile = wd.makePath();

				ProcessTools.runOnBatch("ffprobe.exe " + midRFile + " 2> " + redirFile, Gnd.i.ffmpegBinDir);

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

							foreach (string token in tokens)
							{
								if (StringTools.toDigitFormat(token, true) == "9x9")
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

				string wFile = Utils.getFile(Gnd.i.wFileNoExt, _type);

				if (Utils.equalsExt(midRFile, wFile))
				{
					try
					{
						File.Copy(midRFile, wFile, true);

						if (File.Exists(wFile) == false)
							throw null;
					}
					catch
					{
						throw new Exception("ファイルの出力に失敗しました。(単にコピー)");
					}
				}
				else
				{
					string midWFile = wd.makePath() + Utils.getExt(_type);

					if (Gnd.i.convWavMastering)
					{
						string wavFile = wd.makePath() + ".wav";

						ProcessTools.runOnBatch(
							"ffmpeg.exe -i " + midRFile + " -map 0:" + _audioStream.mapIndex + " -ac 2 " + wavFile,
							Gnd.i.ffmpegBinDir
							);

						if (File.Exists(wavFile) == false)
							throw new Exception("音声ストリームの抽出に失敗しました。");

						string wavFileNew = wd.makePath() + ".wav";

						ProcessTools.runOnBatch(
							"Master.exe " + wavFile + " " + wavFileNew + " " + wd.makePath() + "_DMY_REP.tmp > " + redirFile,
							Gnd.i.wavMasterBinDir
							);

						Utils.textFileToLog(redirFile, StringTools.ENCODING_SJIS);

						if (File.Exists(wavFileNew) == false)
							throw new Exception("wavFileNew does not exist");

						if (_type == Consts.MediaType_e.AUDIO)
						{
							ProcessTools.runOnBatch(
								"ffmpeg.exe -i " + wavFileNew + " -map 0:0 " + Gnd.i.ffmpegOptAudio + " " + midWFile + " 2> " + redirFile,
								Gnd.i.ffmpegBinDir
								);
						}
						else
						{
							ProcessTools.runOnBatch(
								"ffmpeg.exe -i " + midRFile + " -i " + wavFileNew + " -map 0:" + _videoStream.mapIndex + " -map 1:0 " + Gnd.i.ffmpegOptVideo + " " + Gnd.i.ffmpegOptAudio + " " + midWFile + " 2> " + redirFile,
								Gnd.i.ffmpegBinDir
								);
						}
					}
					else
					{
						if (_type == Consts.MediaType_e.AUDIO)
						{
							ProcessTools.runOnBatch(
								"ffmpeg.exe -i " + midRFile + " -map 0:" + _audioStream.mapIndex + " " + Gnd.i.ffmpegOptAudio + " " + midWFile + " 2> " + redirFile,
								Gnd.i.ffmpegBinDir
								);
						}
						else
						{
							ProcessTools.runOnBatch(
								"ffmpeg.exe -i " + midRFile + " -map 0:" + _videoStream.mapIndex + " -map 0:" + _audioStream.mapIndex + " " + Gnd.i.ffmpegOptVideo + " " + Gnd.i.ffmpegOptAudio + " " + midWFile + " 2> " + redirFile,
								Gnd.i.ffmpegBinDir
								);
						}
					}
					Utils.textFileToLog(redirFile, Encoding.ASCII);

					if (File.Exists(midWFile) == false)
						throw new Exception("midWFile does not exist");

					try
					{
						File.Copy(midWFile, wFile, true);

						if (File.Exists(wFile) == false)
							throw null;
					}
					catch
					{
						throw new Exception("ファイルの出力に失敗しました。(変換した)");
					}
				}
				Gnd.i.convReturn.wFile = wFile;
			}
			if (_videoStream != null)
			{
				Gnd.i.convReturn.w = _videoStream.w;
				Gnd.i.convReturn.h = _videoStream.h;
			}
		}
	}
}
