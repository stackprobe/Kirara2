using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Charlotte.Tools;

namespace Charlotte
{
	public class AudioVideoExtensions
	{
		private SortedList<string> _exts = new SortedList<string>(StringTools.compIgnoreCase);

		public AudioVideoExtensions()
		{
			foreach (string ext in FileTools.readAllLines(dataFile, Encoding.ASCII))
				if (ext != "")
					_exts.add("." + ext); // "ext" -> ".ext"
		}

		private static string dataFile
		{
			get
			{
				string file = "audio_movie_extensions.dat";

				if (File.Exists(file) == false)
					file = Path.GetFullPath(@"..\..\..\..\doc\audio_movie_extensions.dat");

				file = FileTools.makeFullPath(file);
				return file;
			}
		}

		public bool contains(string ext)
		{
			return _exts.contains(ext);
		}
	}
}
