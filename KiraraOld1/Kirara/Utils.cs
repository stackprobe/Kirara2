using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Charlotte.Tools;
using System.IO;

namespace Charlotte
{
	public class Utils
	{
		private static int _unqIndex = 0;

		public static int nextUniqueIndex()
		{
			return _unqIndex++;
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
				}
			}
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

		public static string secLengthToStamp(int sec)
		{
			if (sec == -1)
				return "";

			return StringTools.zPad(sec / 60, 2) + ":" + StringTools.zPad(sec % 60, 2);
		}

		public static string getTMP()
		{
			string tmp = Environment.GetEnvironmentVariable("TMP");

			if (tmp == null || tmp == "")
				throw null;

			return tmp;
		}

		public static string getMediaDir()
		{
			return Path.Combine(getTMP(), Consts.MEDIA_DIR_ID);
		}

		public static string getOgxFile(int index, Consts.MediaType_e type)
		{
			return Path.Combine(getTMP(), Consts.MEDIA_DIR_ID, StringTools.zPad(index, 10)) + (type == Consts.MediaType_e.AUDIO ? ".ogg" : ".ogv");
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		/// <returns>null, "" 以外のメッセージ</returns>
		public static string getMessage(Exception e)
		{
			try
			{
				string ret = "" + e.Message;

				if (ret == "")
				{
					ret = "" + e.GetType().Name;

					if (ret == "")
						throw null;
				}
				return ret;
			}
			catch
			{ }

			return "不明なエラー";
		}
	}
}
