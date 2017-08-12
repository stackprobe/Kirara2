using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Charlotte.Tools
{
	public class CsvFile
	{
		public class Reader : IDisposable
		{
			public Reader(string file)
				: this(file, StringTools.ENCODING_SJIS)
			{ }

			private StreamReader _reader;
			private char _delimiter;

			public Reader(string file, Encoding encoding, char delimiter = ',')
			{
				_reader = new StreamReader(file, encoding);
				_delimiter = delimiter;
			}

			private int readCharOrEof()
			{
				int chr;

				do
				{
					chr = _reader.Read();
				}
				while (chr == '\r');

				return chr;
			}

			private int readChar()
			{
				int chr = readCharOrEof();

				if (chr == -1)
					throw new Exception("csvファイルの途中でストリームを読み終えました。");

				return chr;
			}

			public List<string> readRow()
			{
				List<string> cells = new List<string>();
				int chr;

				do
				{
					StringBuilder buff = new StringBuilder();
					chr = readCharOrEof();

					if (chr == -1)
						break;

					if (chr == '"')
					{
						for (; ; )
						{
							chr = readChar();

							if (chr == '"')
							{
								chr = readChar();

								if (chr != '"')
									break;
							}
							buff.Append((char)chr);
						}
					}
					else
					{
						while (chr != _delimiter && chr != '\n' && chr != -1)
						{
							buff.Append((char)chr);
							chr = readCharOrEof();
						}
					}
					cells.Add(buff.ToString());
				}
				while (chr != '\n' && chr != -1);

				if (chr == -1 && cells.Count == 0)
					return null;

				return cells;
			}

			public List<List<string>> readRows()
			{
				List<List<string>> rows = new List<List<string>>();

				for (; ; )
				{
					List<string> row = readRow();

					if (row == null)
						break;

					rows.Add(row);
				}
				return rows;
			}

			public void Dispose()
			{
				if (_reader != null)
				{
					_reader.Dispose();
					_reader = null;
				}
			}
		}

		public class Writer : IDisposable
		{
			public Writer(string file)
				: this(file, StringTools.ENCODING_SJIS)
			{ }

			private StreamWriter _writer;
			private char _delimiter;

			public Writer(string file, Encoding encoding, char delimiter = ',')
			{
				_writer = new StreamWriter(file, false, encoding);
				_delimiter = delimiter;
			}

			public void writeRow(List<string> row)
			{
				for (int index = 0; index < row.Count; index++)
				{
					if (0 < index)
						_writer.Write(_delimiter);

					string cell = row[index];

					if (cell.Contains('"') || cell.Contains(',') || cell.Contains('\t') || cell.Contains('\n'))
						_writer.Write("\"" + cell.Replace("\"", "\"\"") + "\"");
					else
						_writer.Write(cell);
				}
				_writer.Write('\n');
			}

			public void writeRows(List<List<string>> rows)
			{
				foreach (List<string> row in rows)
				{
					writeRow(row);
				}
			}

			public void Dispose()
			{
				if (_writer != null)
				{
					_writer.Dispose();
					_writer = null;
				}
			}
		}
	}
}
