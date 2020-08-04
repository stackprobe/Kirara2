using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using Charlotte.Tools;

namespace Charlotte
{
	class Program
	{
		static void Main(string[] args)
		{
			try
			{
				onBoot();

				// clear TMP -- HACK 何か変だ..
				{
					using (WorkingDir wd = new WorkingDir())
					{ }

					FileTools.clearTMP();
				}

				if (1 <= args.Length && args[0].ToUpper() == "//R")
				{
					main2(File.ReadAllLines(args[1], Encoding.GetEncoding(932)));
				}
				else if (1 <= args.Length && args[0].ToUpper() == "//R8")
				{
					main2(File.ReadAllLines(args[1], Encoding.UTF8));
				}
				else
				{
					main2(args);
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
			}
		}

		public const string APP_IDENT = "{20afa8b4-03f2-48a6-858c-64bb2b744a8d}";
		public const string APP_TITLE = "KiraraConv";

		public static string selfFile;
		public static string selfDir;

		public static void onBoot()
		{
			selfFile = Assembly.GetEntryAssembly().Location;
			selfDir = Path.GetDirectoryName(selfFile);
		}

		private static void main2(string[] args)
		{
			if (args.Length == 0)
				throw new Exception("no args");

			try
			{
				Gnd.i.logger.writeLine("開始");

				Gnd.i.loadConf();

				{
					ArgsReader ar = new ArgsReader();

					Gnd.i.keepDiskFree_MB = IntTools.toInt(ar.nextArg(), 1);
					Gnd.i.rFileSizeMax_MB = IntTools.toInt(ar.nextArg(), 1);
					Gnd.i.ffmpegBinDir = FileTools.toFullPath(ar.nextArg());
					Gnd.i.wavMasterBinDir = FileTools.toFullPath(ar.nextArg());
					Gnd.i.rFile = FileTools.toFullPath(ar.nextArg());
					Gnd.i.convWavMastering = StringTools.toFlag(ar.nextArg());
					Gnd.i.wFileNoExt = FileTools.toFullPath(ar.nextArg());
					Gnd.i.wFileConvReturn = FileTools.toFullPath(ar.nextArg());
				}

				Gnd.i.logger.writeLine("keepDiskFree_MB: " + Gnd.i.keepDiskFree_MB);
				Gnd.i.logger.writeLine("rFileSizeMax_MB: " + Gnd.i.rFileSizeMax_MB);
				Gnd.i.logger.writeLine("ffmpegBinDir: " + Gnd.i.ffmpegBinDir);
				Gnd.i.logger.writeLine("wavMasterBinDir: " + Gnd.i.wavMasterBinDir);
				Gnd.i.logger.writeLine("rFile: " + Gnd.i.rFile);
				Gnd.i.logger.writeLine("convWavMastering: " + Gnd.i.convWavMastering);
				Gnd.i.logger.writeLine("wFileNoExt: " + Gnd.i.wFileNoExt);
				Gnd.i.logger.writeLine("wFileConvReturn: " + Gnd.i.wFileConvReturn);

				try
				{
					Gnd.i.logger.writeLine("コンバート開始");

					new Conv().perform();

					Gnd.i.logger.writeLine("コンバート正常終了");

					Gnd.i.convReturn.successful = true;
				}
				catch (Exception ex)
				{
					Gnd.i.logger.writeLine("コンバート異常終了：" + ex);

					Gnd.i.convReturn.errorMessage = ex.Message;
				}

				File.WriteAllLines(Gnd.i.wFileConvReturn, FieldsSerializer.serialize(Gnd.i.convReturn), Encoding.UTF8);

				Gnd.i.logger.writeLine("正常終了");
			}
			catch (Exception e)
			{
				Gnd.i.logger.writeLine("異常終了：" + e);
			}

			// release Gnd.i
			{
				Gnd.i.evCancel.Dispose();
				Gnd.i.evCancel = null;
			}

			FileTools.clearTMP();
		}
	}
}
