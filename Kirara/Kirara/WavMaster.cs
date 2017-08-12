using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Charlotte.Tools;

namespace Charlotte
{
	public class WavMaster
	{
		/// <summary>
		/// TMPに展開したbin
		/// </summary>
		public string binDir;

		/// <summary>
		/// ロードに失敗したら例外を投げる。
		/// </summary>
		public WavMaster()
		{
			binDir = Path.Combine(FileTools.getTMP(), StringTools.getUUID());
			Directory.CreateDirectory(binDir);
			File.Copy(getMasterFile(), Path.Combine(binDir, "Master.exe"));
		}

		private static string getMasterFile()
		{
			string file = "Master.exe";

			if (File.Exists(file) == false)
				file = @"C:\Factory\Program\WavMaster\Master.exe"; // devenv

			file = FileTools.makeFullPath(file);
			return file;
		}
	}
}
