using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Charlotte.Tools;
using System.Windows.Forms;

namespace Charlotte
{
	public abstract class EngineTh : IDisposable
	{
		public MutexObject m = new MutexObject(Consts.MTX_ENGINE_TH);
		public bool death = false;
		public Thread th;

		public EngineTh()
		{
			try
			{
				init();
			}
			catch (Exception e)
			{
				FaultOperation.caught(e);
			}

			th = new Thread((ThreadStart)delegate
			{
				while (death == false)
				{
					using (MutexObject.section(m))
					{
						try
						{
							perform();
						}
						catch (Exception e)
						{
							Gnd.i.logger.writeLine("EngineTh error: " + e);

							doInvoke(delegate
							{
								FaultOperation.caught(e);
							});

							Gnd.i.logger.writeLine("EngineTh MBoxラッシュ回避 wait start");

							for (int c = 0; c < 15 && death == false; c++) // MBoxラッシュ回避
								Thread.Sleep(2000);

							Gnd.i.logger.writeLine("EngineTh MBoxラッシュ回避 wait end");
						}
					}
					Thread.Sleep(100);
				}
			});
			th.Start();

			Gnd.i.logger.writeLine("EngineTh started");
		}

		public abstract void init();
		public abstract void fnlz();
		public abstract void perform();

		public void Dispose()
		{
			if (th != null)
			{
				death = true;

				th.Join();
				th = null;

				try
				{
					fnlz();
				}
				catch (Exception e)
				{
					FaultOperation.caught(e);
				}

				Gnd.i.logger.writeLine("EngineTh ended");
			}
		}

		public delegate void Operation_d();

		public void doInvoke(Operation_d operation)
		{
			Exception ie = null;

			using (NamedEventPair ev = new NamedEventPair())
			{
				Gnd.i.invokers.enqueue(delegate
				{
					try
					{
						operation();
					}
					catch (Exception e)
					{
						ie = e;
					}
					ev.set();
				});

				while (ev.waitForMillis(2000))
					if (death)
						throw new Ended();
			}
			if (ie != null)
			{
				throw new ExceptionCarrier(ie);
			}
		}
	}
}
