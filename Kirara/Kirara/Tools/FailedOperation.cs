using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Charlotte.Tools
{
	public class FailedOperation : Exception
	{
		public FailedOperation(Exception e = null)
			: this("失敗しました。", e)
		{ }

		public FailedOperation(string message, Exception e = null)
			: base(message, e)
		{ }

		public static void caught(Exception e, string title = Program.APP_TITLE)
		{
			e = getCarried(e);

			if (e is Completed)
			{
				MessageBox.Show(
					getMessage(e),
					title + " / 完了",
					MessageBoxButtons.OK,
					MessageBoxIcon.Information
					);
			}
			else if (e is Ended)
			{
				// noop
			}
			else if (e is Cancelled)
			{
				MessageBox.Show(
					getMessage(e),
					title + " / 中止",
					MessageBoxButtons.OK,
					MessageBoxIcon.Warning
					);
			}
			else if (e is FailedOperation)
			{
				MessageBox.Show(
					getMessage(e),
					title + " / 失敗",
					MessageBoxButtons.OK,
					MessageBoxIcon.Warning
					);
			}
			else
			{
				MessageBox.Show(
					getMessage(e) + "\n----\n" + e,
					title + " / エラー",
					MessageBoxButtons.OK,
					MessageBoxIcon.Error
					);
			}
		}

		public static Exception getCarried(Exception e)
		{
			while (e != null && e is ExceptionCarrier && e.InnerException != null)
			{
				e = e.InnerException;
			}
			return e;
		}

		public static string getMessage(Exception e)
		{
			List<string> lines = new List<string>();

			while (e != null)
			{
				lines.Add(e.Message);
				e = e.InnerException;
			}
			return string.Join("\n", lines);
		}
	}
}
