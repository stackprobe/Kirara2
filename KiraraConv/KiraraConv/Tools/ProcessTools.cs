﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;

namespace Charlotte.Tools
{
	// バッチなので、"%" -> "%%"

	public static class ProcessTools
	{
		public static void runOnBatch(string line, string dir = null)
		{
			// app >

			Gnd.i.logger.writeLine("runOnBatch_line: " + line);

			// < app

			runOnBatch(new string[] { line }, dir);
		}

		public static void runOnBatch(string[] lines, string dir = null)
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

					// app >

					Process p = Process.Start(psi);

					while (p.WaitForExit(2000) == false)
						if (Gnd.i.evCancel.waitForMillis(0))
							throw new Cancelled();

					// < app

					//Process.Start(psi).WaitForExit(); // orig
				}
			}
		}
	}
}
