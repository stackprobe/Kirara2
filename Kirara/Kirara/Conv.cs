using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Tools;
using System.IO;
using System.Diagnostics;

namespace Charlotte
{
	public class Conv : IDisposable
	{
		private WorkingDir _wd = new WorkingDir();
		private string _rFile;
		private string _wFileNoExt;

		public Conv(string rFile, string wFileNoExt)
		{
			_rFile = rFile;
			_wFileNoExt = wFileNoExt;

			doConv();
		}

		public void Dispose()
		{
			if (_wd != null)
			{
				_wd.Dispose();
				_wd = null;
			}
		}

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

		public string errorMessage = null; // null == no error
		public string rExt;
		public string midRFile;
		public string redirFile;
		public string wFile;
		public bool doConvMode;
		public string midWFile;
		public string wavFile;
		public string wavFileNew;

		private void doConv()
		{
			addOperation(delegate
			{
				rExt = Path.GetExtension(_rFile);

				if (Gnd.i.audioVideoExtensions.contains(rExt) == false)
					throw new Exception("再生可能なファイルではありません。(不明な拡張子)");

				midRFile = _wd.makePath() + rExt;

				{
					long rFileSize = -1;

					try
					{
						rFileSize = new FileInfo(_rFile).Length;
					}
					catch
					{ }

					if (Gnd.i.rFileSizeMax_MB * 1000000L < rFileSize)
						throw new Exception("ファイルが大きすぎます。");
				}

				{
					long diskFree = new DriveInfo(FileTools.getTMP().Substring(0, 1)).AvailableFreeSpace;

					if (diskFree < Gnd.i.keepDiskFree_MB * 1000000L)
						throw new Exception("ディスクの空き領域が不足しています。");
				}

				runCopyFile(_rFile, midRFile, "ファイルにアクセス出来ません。");
			});
			addOperation(delegate
			{
				redirFile = _wd.makePath();

				runCommand("ffprobe.exe " + midRFile + " 2> " + redirFile, Gnd.i.ffmpeg.binDir);
			});
			addOperation(delegate
			{
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
					throw new Exception("fatal: ffprobe _duration null");

				if (_audioStream == null)
					throw new Exception("再生可能なファイルではありません。(音声ストリームがありません)");

				if (_duration.secLength < 3)
					throw new Exception("再生時間が短すぎます。");

				if (_videoStream == null)
					_type = Consts.MediaType_e.AUDIO;
				else
					_type = Consts.MediaType_e.MOVIE;

				wFile = _wFileNoExt + (_type == Consts.MediaType_e.AUDIO ? ".ogg" : ".ogv");

				if (StringTools.equalsIgnoreCase(rExt, Path.GetExtension(wFile)))
				{
					doConvMode = false;
					midWFile = midRFile;
				}
				else
				{
					doConvMode = true;
					midWFile = _wd.makePath() + Path.GetExtension(wFile);
				}
			});
			if (Gnd.i.convWavMastering)
			{
				addOperation(delegate
				{
					if (doConvMode)
					{
						wavFile = _wd.makePath() + ".wav";

						runCommand(
							"ffmpeg.exe -i " + midRFile + " -map 0:" + _audioStream.mapIndex + " -ac 2 " + wavFile,
							Gnd.i.ffmpeg.binDir
							);
					}
				});
				addOperation(delegate
				{
					if (doConvMode)
					{
						if (File.Exists(wavFile) == false)
							throw new Exception("音声ストリームの抽出に失敗しました。");

						wavFileNew = _wd.makePath() + ".wav";

						runCommand(
							"Master.exe " + wavFile + " " + wavFileNew + " 0001.txt > " + redirFile,
							Gnd.i.wavMaster.binDir
							);
					}
				});
				addOperation(delegate
				{
					if (doConvMode)
					{
						reportToLog(redirFile, StringTools.ENCODING_SJIS);

						if (File.Exists(wavFileNew) == false) // ? 音量の均一化 不要だった。
						{
							Gnd.i.logger.writeLine("wavFileNew <- wavFile");
							wavFileNew = wavFile;
						}
						if (_type == Consts.MediaType_e.AUDIO)
						{
							runCommand(
								"ffmpeg.exe -i " + wavFileNew + " -map 0:0 " + Gnd.i.ffmpegOptAudio + " " + midWFile + " 2> " + redirFile,
								Gnd.i.ffmpeg.binDir
								);
						}
						else
						{
							runCommand(
								"ffmpeg.exe -i " + midRFile + " -i " + wavFileNew + " -map 0:" + _videoStream.mapIndex + " -map 1:0 " + Gnd.i.ffmpegOptVideo + " " + Gnd.i.ffmpegOptAudio + " " + midWFile + " 2> " + redirFile,
								Gnd.i.ffmpeg.binDir
								);
						}
					}
				});
			}
			else
			{
				addOperation(delegate
				{
					if (doConvMode)
					{
						if (_type == Consts.MediaType_e.AUDIO)
						{
							runCommand(
								"ffmpeg.exe -i " + midRFile + " -map 0:" + _audioStream.mapIndex + " " + Gnd.i.ffmpegOptAudio + " " + midWFile + " 2> " + redirFile,
								Gnd.i.ffmpeg.binDir
								);
						}
						else
						{
							runCommand(
								"ffmpeg.exe -i " + midRFile + " -map 0:" + _videoStream.mapIndex + " -map 0:" + _audioStream.mapIndex + " " + Gnd.i.ffmpegOptVideo + " " + Gnd.i.ffmpegOptAudio + " " + midWFile + " 2> " + redirFile,
								Gnd.i.ffmpeg.binDir
								);
						}
					}
				});
			}
			addOperation(delegate
			{
				if (doConvMode)
				{
					reportToLog(redirFile, Encoding.ASCII);
				}
			});
			addOperation(delegate
			{
				try
				{
					string dir = Path.GetDirectoryName(wFile);

					Gnd.i.logger.writeLine("dir: " + dir);

					if (Directory.Exists(dir) == false)
						Directory.CreateDirectory(dir);
				}
				catch
				{
					throw new Exception("ファイルを出力できません。(フォルダ作成失敗)");
				}

				runCopyFile(midWFile, wFile, "ファイルを出力できません。");
			});
		}

		private Process rcProc = null;

		private void runCommand(string line, string dir)
		{
			string batch = _wd.makePath() + ".bat";

			Gnd.i.logger.writeLine("Conv_runCommand_line: " + line);

			File.WriteAllLines(batch, new string[] { line }, StringTools.ENCODING_SJIS);

			{
				ProcessStartInfo psi = new ProcessStartInfo();

				psi.FileName = "cmd.exe";
				psi.Arguments = "/C " + batch;
				psi.CreateNoWindow = true;
				psi.UseShellExecute = false;
				psi.WorkingDirectory = dir;

				Process p = Process.Start(psi);

				rcProc = p;
			}

			Gnd.i.convOc.addTopPrio(delegate
			{
				if (rcProc.HasExited == false)
					OperationCenter.retry();
			});

			addOperation(delegate
			{
				rcProc = null;
			});
		}

		private string rcf_rFile;
		private string rcf_wFile;
		private bool rcf_running;
		private bool rcf_errorFlag;
		private string rcf_errorMessage;

		private void runCopyFile(string rFile, string wFile, string pErrorMessage)
		{
			rcf_rFile = rFile;
			rcf_wFile = wFile;
			rcf_running = true;
			rcf_errorFlag = false;
			rcf_errorMessage = pErrorMessage;

			rFile = null; // 2bs
			wFile = null; // 2bs
			pErrorMessage = null; // 2bs

			Gnd.i.tc.add(delegate
			{
				try
				{
					File.Copy(rcf_rFile, rcf_wFile, true);

					if (File.Exists(rcf_wFile) == false)
						throw null;
				}
				catch
				{
					rcf_errorFlag = true;
				}
				rcf_running = false;
			});

			Gnd.i.convOc.addTopPrio(delegate
			{
				if (rcf_running)
					OperationCenter.retry();
			});

			addOperation(delegate
			{
				if (rcf_errorFlag)
				{
					throw new Exception(rcf_errorMessage);
				}
			});
		}

		private void addOperation(OperationCenter.operation_d operation)
		{
			Gnd.i.convOc.add(delegate
			{
				try
				{
					operation();
				}
				catch (Exception e)
				{
					Gnd.i.logger.writeLine("Conv_operation_error: " + e);
					errorMessage = "" + e.Message;
					Gnd.i.convOc.clear();
				}
			});
		}

		private void reportToLog(string file, Encoding encoding)
		{
			if (Gnd.i.reportToLogDisabled)
			{
				Gnd.i.logger.writeLine("REPORT-DISABLED");
			}
			else
			{
				Gnd.i.logger.writeLine("REPORT-BEGIN");

				foreach (string line in FileTools.readAllLines(file, encoding))
					Gnd.i.logger.writeLine(line);

				Gnd.i.logger.writeLine("REPORT-END");
			}
		}

		public bool completed
		{
			get
			{
				return Gnd.i.convOc.getCount() == 0;
			}
		}

		public int secLength
		{
			get
			{
				return _duration.secLength;
			}
		}

		public bool hasVideoStream
		{
			get
			{
				return _videoStream != null;
			}
		}

		public int w
		{
			get
			{
				return _videoStream.w;
			}
		}

		public int h
		{
			get
			{
				return _videoStream.h;
			}
		}

		public string rFile
		{
			get
			{
				return _rFile;
			}
		}
	}
}
