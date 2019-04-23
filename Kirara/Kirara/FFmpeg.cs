using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Tools;
using System.IO;
using System.Windows.Forms;

namespace Charlotte
{
	public class FFmpeg
	{
		/// <summary>
		/// TMPに展開したbin
		/// </summary>
		public string binDir;

		/// <summary>
		/// ロードに失敗したら例外を投げる。
		/// </summary>
		public FFmpeg()
		{
			try
			{
				tryInit();
			}
			catch (Exception e)
			{
				FailedOperation.caught(e);
				chooseFFmpegDir();
				tryInit();
			}
		}

		private void tryInit()
		{
			if (Gnd.i.ffmpegDir == "")
				throw new FailedOperation("ffmpeg のパスが設定されていません。");

			if (Directory.Exists(Gnd.i.ffmpegDir) == false)
				throw new FailedOperation("ffmpeg のパスは移動又は削除された様です。");

			string file = getFFmpegFile(Gnd.i.ffmpegDir);

			if (file == null)
				throw new FailedOperation("ffmpeg のパスに問題があります。");

			string dir = Path.GetDirectoryName(file);

			binDir = Path.Combine(FileTools.getTMP(), StringTools.getUUID());
			FileTools.copyDir(dir, binDir);
			doTest();
		}

		private string getFFmpegFile(string dir)
		{
			foreach (string file in FileTools.lssFiles(dir))
				if (StringTools.equalsIgnoreCase(Path.GetFileName(file), "ffmpeg.exe"))
					return file;

			return null; // not found
		}

		private void doTest()
		{
			using (WorkingDir wd = new WorkingDir())
			{
				string muonFile = wd.makePath("muon.wav");
				string redirFile = wd.makePath("muon_redir.txt");

				File.Copy(getMuonWavFile(), muonFile);

				ProcessTools.runOnBatch("ffprobe.exe " + muonFile + " 2> " + redirFile, binDir);

				if (hasAudioStream(redirFile) == false)
					throw new FailedOperation("ffmpeg を正しく実行出来ません。");
			}
		}

		private static string getMuonWavFile()
		{
			string file = "muon_wav.dat";

			if (File.Exists(file) == false)
				file = @"..\..\..\..\doc\muon_wav.dat"; // devenv

			file = FileTools.makeFullPath(file);
			return file;
		}

		private bool hasAudioStream(string file)
		{
			foreach (string line in FileTools.readAllLines(file, Encoding.ASCII))
				if (line.Contains("Stream") && line.Contains("Audio:"))
					return true;

			return false;
		}

		private void chooseFFmpegDir()
		{
			string description = "ffmpeg のパスを指定してください。\n例) C:\\app\\ffmpeg-3.2.4-win64-shared";

			if (Environment.Is64BitOperatingSystem == false) // ? OS == 32-bit
				description = description.Replace("64", "32");

			if (SaveLoadDialogs.SelectFolder(ref Gnd.i.ffmpegDir, description, dlg =>
			{
				dlg.RootFolder = Environment.SpecialFolder.MyComputer;
				dlg.ShowNewFolderButton = false;
			}
			))
			{
				Gnd.i.ffmpegDir = FileTools.toFullPath(Gnd.i.ffmpegDir);
			}
		}
	}
}
