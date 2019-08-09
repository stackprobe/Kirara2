using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Charlotte.Tools;
using System.IO;
using System.Reflection;

namespace Charlotte
{
	public static class Utils
	{
		private static int _serial;

		public static int getSerial()
		{
			if (IntTools.IMAX < _serial)
				throw new Exception("fatal: _serial counter-stopped");

			return _serial++;
		}

		public static void addColumn(DataGridView sheet, int colidx, string title, bool invisible = false)
		{
			DataGridViewColumn column = sheet.Columns[colidx];

			column.HeaderText = title;

			if (invisible)
				column.Visible = false;
			else
				column.Width = 200;
		}

		public static void adjustColumnsWidth(DataGridView sheet)
		{
			for (int colidx = 0; colidx < sheet.ColumnCount; colidx++)
			{
				if (sheet.RowCount == 0)
				{
					sheet.Columns[colidx].Width = 200;
				}
				else
				{
					sheet.Columns[colidx].Width = 10000; // 一旦思いっきり広げてからでないとダメな時がある。
					sheet.AutoResizeColumn(colidx, DataGridViewAutoSizeColumnMode.AllCells);
					//sheet.Columns[colidx].Width += 10; // margin
				}
			}
		}

		public static object secToUIStamp(int sec)
		{
			if (sec == -1)
				return "";

			return StringTools.zPad(sec / 60, 2) + ":" + StringTools.zPad(sec % 60, 2);
		}

		public static string[] droppedFilesFilter(string[] files, List<string> relFiles = null)
		{
			List<string> dest = new List<string>();

			foreach (string fFile in files)
			{
				string file = fFile;
				file = FileTools.toFullPath(file);

				if (Directory.Exists(file))
				{
					foreach (string fSubFile in FileTools.lssFiles(file))
					{
						string subFile = fSubFile;
						subFile = FileTools.toFullPath(subFile);
						dest.Add(subFile);

						if (relFiles != null)
						{
							string lDir = "" + Path.GetFileName(file);

							if (lDir == "")
								lDir = "PPP";

							relFiles.Add(FileTools.changeRoot(subFile, file, lDir));
						}
					}
				}
				else
				{
					dest.Add(file);

					if (relFiles != null)
						relFiles.Add(Path.GetFileName(file));
				}
			}
			return dest.ToArray();
		}

		public static void enableDoubleBuffer(DataGridView sheet)
		{
			sheet.GetType().InvokeMember(
				   "DoubleBuffered",
				   BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty,
				   null,
				   sheet,
				   new object[] { true }
				   );
		}

		public static bool isOgxPath(string file)
		{
			string ext = Path.GetExtension(file).ToLower();
			return ext == ".ogg" || ext == ".ogv";
		}
	}
}
