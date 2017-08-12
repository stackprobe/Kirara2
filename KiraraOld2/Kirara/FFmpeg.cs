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
		public string binDir;

		public FFmpeg()
		{
			try
			{
				tryInit();
			}
			catch (Exception e)
			{
				FaultOperation.caught(e);
				chooseFFmpegDir();
				tryInit();
			}
		}

		private void tryInit()
		{
			if (Gnd.i.ffmpegDir == "")
				throw new FaultOperation("ffmpeg のパスが設定されていません。");

			if (Directory.Exists(Gnd.i.ffmpegDir) == false)
				throw new FaultOperation("ffmpeg のパスは移動又は削除された様です。");

			string file = getFFmpegFile(Gnd.i.ffmpegDir);

			if (file == null)
				throw new FaultOperation("ffmpeg のパスに問題があります。");

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
				string mwFile = wd.makePath("muon.wav");
				string seFile = wd.makePath("muon_stderr.txt");

				File.Copy(getMuonWavFile(), mwFile);

				ProcessTools.runOnBatch("ffprobe.exe " + mwFile + " 2> " + seFile, binDir);

				if (hasAudioStream(seFile) == false)
					throw new FaultOperation("ffmpeg を正しく実行出来ません。");
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
	}
}
