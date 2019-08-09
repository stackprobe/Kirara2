using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Charlotte.Tools;
using System.Windows.Forms;
using System.Diagnostics;

namespace Charlotte
{
	public static class FFmpeg
	{
		private static string _bDir;

		public static void init()
		{
			try
			{
				_bDir = mkBDir(Gnd.i.ffmpegDir);
			}
			catch (Exception e)
			{
				FaultOperation.caught(e);
				chooseFFmpegDir();
				_bDir = mkBDir(Gnd.i.ffmpegDir);
			}
		}

		public static string mkBDir(string dir)
		{
			if (dir == null)
				throw null;

			if (dir == "")
				throw new FaultOperation("ffmpeg のパスが設定されていません。");

			if (Directory.Exists(dir) == false)
				throw new FaultOperation("ffmpeg のパスは移動又は削除された様です。");

			dir = getBinDir(dir);

			if (dir == null)
				throw new FaultOperation("ffmpeg のパスに問題があります。");

			string bDir = Path.Combine(FileTools.getTMP(), StringTools.getUUID());
			FileTools.copyDir(dir, bDir);
			doTestBDir(bDir);
			return bDir;
		}

		private static string getBinDir(string dir)
		{
			string file = getFFmpegFile(dir);

			if (file == null)
				return null; // not found

			return Path.GetDirectoryName(file);
		}

		private static string getFFmpegFile(string dir)
		{
			foreach (string file in FileTools.lssFiles(dir))
				if (StringTools.equalsIgnoreCase(Path.GetFileName(file), "ffmpeg.exe"))
					return file;

			return null; // not found
		}

		private static void doTestBDir(string bDir)
		{
			using (WorkingDir wd = new WorkingDir())
			{
				string mwFile = wd.makePath("muon.wav");
				string seFile = wd.makePath("muon_stderr.txt");

				File.Copy(muonWavFile, mwFile);

				ProcessTools.runOnBatch("ffprobe.exe " + mwFile + " 2> " + seFile, bDir);

				if (hasAudioStream(seFile) == false)
					throw new FaultOperation("ffmpeg を正しく実行出来ません。");
			}
		}

		private static string muonWavFile
		{
			get
			{
				string file = "muon_wav.dat";

				if (File.Exists(file) == false)
					file = @"..\..\..\..\doc\muon_wav.dat"; // devenv

				file = FileTools.makeFullPath(file);
				return file;
			}
		}

		private static bool hasAudioStream(string file)
		{
			foreach (string line in FileTools.readAllLines(file, Encoding.ASCII))
				if (line.Contains("Stream") && line.Contains("Audio:"))
					return true;

			return false;
		}

		private static void chooseFFmpegDir()
		{
			string description = "ffmpeg のパスを指定してください。\n例) C:\\app\\ffmpeg-3.2.4-win64-shared";

			if (Environment.Is64BitOperatingSystem == false) // ? OS == 32-bit
				description = description.Replace("64", "32");

			//FolderBrowserDialogクラスのインスタンスを作成
			using (FolderBrowserDialog fbd = new FolderBrowserDialog())
			{
				//上部に表示する説明テキストを指定する
				fbd.Description = description;
				//ルートフォルダを指定する
				//デフォルトでDesktop
				//fbd.RootFolder = Environment.SpecialFolder.Desktop;
				fbd.RootFolder = Environment.SpecialFolder.MyComputer;
				//最初に選択するフォルダを指定する
				//RootFolder以下にあるフォルダである必要がある
				//fbd.SelectedPath = @"C:\Windows";
				fbd.SelectedPath = Gnd.i.ffmpegDir;
				//ユーザーが新しいフォルダを作成できるようにする
				//デフォルトでTrue
				fbd.ShowNewFolderButton = false;

				//ダイアログを表示する
				if (fbd.ShowDialog(MainWin.self) == DialogResult.OK)
				{
					//選択されたフォルダを表示する
					//Console.WriteLine(fbd.SelectedPath);

					{
						string dir = fbd.SelectedPath;

						dir = FileTools.toFullPath(dir);

						Gnd.i.ffmpegDir = dir;
					}
				}
			}
		}

		public static string getBDir()
		{
			return _bDir;
		}
	}
}
