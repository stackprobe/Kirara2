using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Charlotte.Tools
{
	/// <summary>
	/// thread safe
	/// </summary>
	public class ThreadCenter : IDisposable
	{
		public delegate void operation_d();

		private object SYNCROOT = new object();
		private Thread _th;
		private bool _death = false;
		private PostQueue<operation_d> _operations = new PostQueue<operation_d>();
		private NamedEventPair _evCatnap = new NamedEventPair();

		public ThreadCenter()
		{
			_th = new Thread((ThreadStart)delegate
			{
				while (_death == false)
				{
					operation_d operation;

					lock (SYNCROOT)
					{
						operation = _operations.dequeue();
					}

					if (operation == null)
					{
						_evCatnap.waitForMillis(2000);
					}
					else
					{
						try
						{
							operation();
						}
						catch
						{ }
					}
				}
			});
			_th.Start();
		}

		public void add(operation_d operation)
		{
			lock (SYNCROOT)
			{
				_operations.enqueue(operation);
			}
			_evCatnap.set();
		}

		public int getCount()
		{
			lock (SYNCROOT)
			{
				return _operations.getCount();
			}
		}

		public void Dispose()
		{
			if (_th != null)
			{
				_death = true;

				_evCatnap.set();

				_th.Join();
				_th = null;
			}
		}
	}
}
