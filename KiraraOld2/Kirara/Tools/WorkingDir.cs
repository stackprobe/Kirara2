using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Charlotte.Tools
{
	public class WorkingDir : IDisposable
	{
		public WorkingDir()
			: this(StringTools.getUUID())
		{ }

		private string _dir;

		public WorkingDir(string lDir)
		{
			_dir = Path.Combine(FileTools.getTMP(), lDir);

			FileTools.deletePath(_dir);
			Directory.CreateDirectory(_dir);
		}

		public string getDir()
		{
			return _dir;
		}

		public string makePath()
		{
			return makePath(StringTools.getUUID());
		}

		public string makePath(string relPath)
		{
			return Path.Combine(_dir, relPath);
		}

		public void Dispose()
		{
			if (_dir != null)
			{
				string dir = _dir;
				_dir = null;
				FileTools.deletePath(dir);
			}
		}
	}
}
