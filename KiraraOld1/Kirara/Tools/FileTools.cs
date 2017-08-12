using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using System.Collections;

namespace Charlotte.Tools
{
	public class FileTools
	{
		public static void createFile(string file)
		{
			File.WriteAllBytes(file, new byte[0]);
		}

		public static void deletePath(string path)
		{
			for (int c = 0; File.Exists(path) || Directory.Exists(path); c++)
			{
				if (8 < c)
					throw new Exception("[" + path + "] の削除に失敗しました。");

				if (0 < c)
					Thread.Sleep(2000);

				try
				{
					File.Delete(path);
				}
				catch
				{ }

				try
				{
					Directory.Delete(path, true);
				}
				catch
				{ }
			}
		}

		private static string _tmp = null;

		public static string getTMP()
		{
			if (_tmp == null)
			{
				string tmp = getTMP_envName("TMP");

				if (tmp == null)
				{
					tmp = getTMP_envName("TEMP");

					if (tmp == null)
					{
						tmp = getTMP_envName("ProgramData");

						if (tmp == null)
							throw null;

						// 書き込みテスト -- ProgramDataってゲストでも書けるっぽい。
						{
							string dir = Path.Combine(tmp, StringTools.getUUID() + "_test");

							Directory.CreateDirectory(dir);
							Directory.Delete(dir);
						}
					}
				}
				tmp = Path.Combine(tmp, Program.APP_IDENT);

				for (int c = 0; ; c++)
				{
					string trialTmp = Path.Combine(tmp, StringTools.zPad(c, 4));

					if (Directory.Exists(trialTmp) == false)
					{
						tmp = trialTmp;
						break;
					}
				}
				Directory.CreateDirectory(tmp);
				_tmp = tmp;
			}
			return _tmp;
		}

		private static string getTMP_envName(string envName)
		{
			string tmp = Environment.GetEnvironmentVariable(envName);

			if (
				tmp == null ||
				tmp.Length < 3 ||
				tmp.Substring(1, 2) != ":\\" ||
				Directory.Exists(tmp) == false ||
				tmp != Encoding.ASCII.GetString(Encoding.ASCII.GetBytes(tmp)) ||
				tmp.Contains(' ')
				)
				return null;

			return tmp;
		}

		public static void clearTMP()
		{
			if (_tmp != null)
			{
				clearTMP_main();
				_tmp = null;
			}
		}

		private static void clearTMP_main()
		{
			string tmpsDir = Path.GetDirectoryName(_tmp);
			string tmp0 = Path.Combine(tmpsDir, "dead");

			deletePath(tmp0);

			foreach (string dir in Directory.GetDirectories(tmpsDir))
			{
				try
				{
					Directory.Move(dir, tmp0);
				}
				catch
				{
					return;
				}
				if (Directory.Exists(dir))
					return;

				if (Directory.Exists(tmp0) == false)
					return;

				deletePath(tmp0);
			}
			deletePath(tmpsDir);
		}

		public static string getProgramData()
		{
			string dir = getTMP_envName("ProgramData");

			if (dir == null)
				throw null;

			return dir;
		}

		// ls* -- 相対パスの場合、戻り値のリストも相対パスになる。
		// -- 存在しないディレクトリの場合、例外を投げる。

		public static string[] lsFiles(string dir)
		{
			return Directory.GetFiles(dir);
		}

		public static string[] lsDirs(string dir)
		{
			return Directory.GetDirectories(dir);
		}

		public static string[] lssFiles(string dir)
		{
			return Directory.GetFiles(dir, "*", SearchOption.AllDirectories);
		}

		public static string[] lssDirs(string dir)
		{
			return Directory.GetDirectories(dir, "*", SearchOption.AllDirectories);
		}

		public static string changeRoot(string path, string oldRoot, string rootNew)
		{
			oldRoot = putYen(oldRoot);
			rootNew = putYen(rootNew);

			if (StringTools.startsWithIgnoreCase(path, oldRoot) == false)
				throw null;

			return rootNew + path.Substring(oldRoot.Length);
		}

		public static string putYen(string path)
		{
			if (path.EndsWith("\\") == false)
				path += "\\";

			return path;
		}

		public static string makeFullPath(string path)
		{
			if (path == null)
				throw new Exception("パスが定義されていません。(null)");

			if (path == "")
				throw new Exception("パスが定義されていません。(空文字列)");

			path = Path.GetFullPath(path);

			if (path.Contains('/'))
				throw null;

			if (path.StartsWith("\\\\"))
				throw new Exception("ネットワークパスまたはデバイス名は使用出来ません。");

			if (path.Substring(1, 2) != ":\\")
				throw null;

			path = putYen(path) + ".";
			path = Path.GetFullPath(path);

			return path;
		}

		public static string toFullPath(string path)
		{
			path = Path.GetFullPath(path);
			path = putYen(path) + ".";
			path = Path.GetFullPath(path);

			return path;
		}

		public static IEnumerable<string> readAllLines(string file, Encoding encoding)
		{
			return new TextFileReader(file, encoding);
		}

		private class TextFileReader : IEnumerable<string>, IEnumerator<string>
		{
			private StreamReader _rfs;
			private string _line = null;

			public TextFileReader(string file, Encoding encoding)
			{
				_rfs = new StreamReader(file, encoding);
			}

			public IEnumerator<string> GetEnumerator()
			{
				return this;
			}

			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.GetEnumerator();
			}

			public string Current
			{
				get { return _line; }
			}

			object IEnumerator.Current
			{
				get { return this.Current; }
			}

			public bool MoveNext()
			{
				_line = _rfs.ReadLine();
				return _line != null;
			}

			public void Reset()
			{
				throw null;
			}

			public void Dispose()
			{
				if (_rfs != null)
				{
					_rfs.Dispose();
					_rfs = null;
				}
			}
		}

		public static void copyDir(string rDir, string wDir)
		{
			Queue<string> dirq = new Queue<string>();

			dirq.Enqueue(rDir);
			dirq.Enqueue(wDir);

			while (1 <= dirq.Count)
			{
				rDir = dirq.Dequeue();
				wDir = dirq.Dequeue();

				Directory.CreateDirectory(wDir);

				foreach (string dir in FileTools.lsDirs(rDir))
				{
					dirq.Enqueue(dir);
					dirq.Enqueue(Path.Combine(wDir, Path.GetFileName(dir)));
				}
				foreach (string file in FileTools.lsFiles(rDir))
				{
					File.Copy(file, Path.Combine(wDir, Path.GetFileName(file)));
				}
			}
		}
	}
}
