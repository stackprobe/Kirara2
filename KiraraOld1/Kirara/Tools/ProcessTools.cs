using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;

namespace Charlotte.Tools
{
	/// <summary>
	/// バッチなので、"%" -> "%%"
	/// </summary>
	public class ProcessTools
	{
		public static bool _終了を待っているプロセスを捨てて例外を投げろ = false;

		public static void runOnBatch(string line, string dir = null, CriticalSection paralleller = null)
		{
			runOnBatch(new string[] { line }, dir, paralleller);
		}

		public static void runOnBatch(string[] lines, string dir = null, CriticalSection paralleller = null)
		{
			using (WorkingDir wd = new WorkingDir())
			{
				string batch = wd.makePath() + ".bat";

				File.WriteAllLines(batch, lines, StringTools.ENCODING_SJIS);

				if (dir == null)
				{
					dir = wd.makePath();
					Directory.CreateDirectory(dir);
				}

				{
					ProcessStartInfo psi = new ProcessStartInfo();

					psi.FileName = "cmd.exe";
					psi.Arguments = "/C " + batch;
					psi.CreateNoWindow = true;
					psi.UseShellExecute = false;
					psi.WorkingDirectory = dir;

					if (paralleller == null)
					{
						run(psi);
					}
					else
					{
						using (paralleller.parallel())
						{
							run(psi);
						}
					}
				}
			}
		}

		private static void run(ProcessStartInfo psi)
		{
			for (Process p = Process.Start(psi); p.WaitForExit(2000) == false; )
				if (_終了を待っているプロセスを捨てて例外を投げろ)
					throw new Cancelled();
		}
	}
}
