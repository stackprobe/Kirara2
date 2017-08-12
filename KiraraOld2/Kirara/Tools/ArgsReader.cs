using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Charlotte.Tools
{
	public class ArgsReader
	{
		private string[] _args;
		private int _index = 0;

		public ArgsReader()
		{
			_args = Environment.GetCommandLineArgs();
			_index = 1;
		}

		public ArgsReader(string[] args)
		{
			_args = args;
		}

		public bool hasArgs(int count = 1)
		{
			return _index + count <= _args.Length;
		}

		public string getArg(int index)
		{
			return _args[_index + index];
		}

		public string nextArg()
		{
			return _args[_index++];
		}

		public bool argIs(string spell)
		{
			if (hasArgs() && StringTools.equalsIgnoreCase(getArg(0), spell))
			{
				nextArg();
				return true;
			}
			return false;
		}
	}
}
