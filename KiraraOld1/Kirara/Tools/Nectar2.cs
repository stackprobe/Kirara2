using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Charlotte.Tools
{
	public class Nectar2 : IDisposable
	{
		private const string COMMON_ID = "{8cf92c5e-c4f7-4867-9e1a-5371bb53aa63}"; // shared_uuid@g

		public enum E_INDEX
		{
			E_SEND,
			E_RECV,
			E_BIT_0,
			E_BIT_1,
			E_BIT_2,
			E_BIT_3,
			E_BIT_4,
			E_BIT_5,
			E_BIT_6,
			E_BIT_7,

			E_MAX, // == num of E_*
		}

		private NamedEventObject[] _evs;

		/// <summary>
		///
		/// </summary>
		/// <param name="ident">名前付きイベント名に使うので注意！</param>
		public Nectar2(string ident)
		{
			_evs = new NamedEventObject[(int)E_INDEX.E_MAX];

			for (int index = 0; index < (int)E_INDEX.E_MAX; index++)
			{
				_evs[index] = new NamedEventObject("Nectar2_" + COMMON_ID + "_" + ident + "_" + index);
			}
		}

		public void set(E_INDEX index)
		{
			_evs[(int)index].set();
		}

		public bool get(E_INDEX index, int millis = 0)
		{
			return _evs[(int)index].waitForMillis(millis);
		}

		public void Dispose()
		{
			if (_evs != null)
			{
				for (int index = 0; index < (int)E_INDEX.E_MAX; index++)
				{
					_evs[index].Dispose();
				}
				_evs = null;
			}
		}

		public class Sender : IDisposable
		{
			private const int MESSAGE_SIZE_MAX = 2000; // 2 KB
			private const int BUFF_MAX = 1000;

			private Nectar2 _n;
			private Thread _th;
			private bool _death;
			private object SYNCROOT = new object();
			private NamedEventPair _evDoSend;
			private Queue<byte[]> _messages = new Queue<byte[]>();
			private bool _sending;

			public Sender(string ident)
			{
				_n = new Nectar2(ident);
				_evDoSend = new NamedEventPair();
				_th = new Thread((ThreadStart)delegate
				{
					while (_death == false)
					{
						byte[] message;

						lock (SYNCROOT)
						{
							if (_messages.Count == 0)
							{
								message = null;
								_sending = false;
							}
							else
							{
								message = _messages.Dequeue();
								_sending = true;
							}
						}
						if (message == null)
						{
							_evDoSend.waitForMillis(2000);
						}
						else
						{
							foreach (byte chr in message)
							{
								for (int bit = 0; bit < 8; bit++)
									if ((chr & (1 << bit)) != 0)
										_n.set((E_INDEX)((int)E_INDEX.E_BIT_0 + bit));

								_n.get(E_INDEX.E_RECV); // clear
								_n.set(E_INDEX.E_SEND);

								if (syncRecv() == false) // ? 送信タイムアウト
								{
									lock (SYNCROOT)
									{
										_messages.Clear(); // 全部失敗扱い。
									}
									break;
								}
							}
						}
					}
				});
				_th.Start();
			}

			private bool syncRecv() // ret: ? ! 送信タイムアウト
			{
				for (int c = 0; c < 5 && _death == false; c++)
					if (_n.get(E_INDEX.E_RECV, 3000))
						return true;

				return false;
			}

			/// <summary>
			/// thread safe
			/// </summary>
			/// <param name="message"></param>
			public void send(byte[] message)
			{
				if (message == null)
					throw new ArgumentNullException();

				if (MESSAGE_SIZE_MAX < message.Length)
					return;

				lock (SYNCROOT)
				{
					if (BUFF_MAX < _messages.Count)
						return;

					_messages.Enqueue(message);
				}
				_evDoSend.set();
			}

			/// <summary>
			/// thread safe
			/// </summary>
			/// <returns></returns>
			public bool isBusy()
			{
				lock (SYNCROOT)
				{
					return 1 <= _messages.Count || _sending;
				}
			}

			public void Dispose()
			{
				if (_n != null)
				{
					_death = true;
					_evDoSend.set();

					_th.Join();
					_th = null;

					_n.Dispose();
					_n = null;

					_evDoSend.Dispose();
					_evDoSend = null;
				}
			}
		}

		public class Recver : IDisposable
		{
			private const int MESSAGE_SIZE_MAX = 2000; // 2 KB
			private const int BUFF_MAX = 1000;

			private Nectar2 _n;
			private Thread _th;
			private bool _death;
			private object SYNCROOT = new object();
			private Queue<byte> _buff = new Queue<byte>();
			private Queue<byte[]> _messages = new Queue<byte[]>();
			private int _delimiter;

			public Recver(string ident, int delimiter = 0x00)
			{
				_n = new Nectar2(ident);
				_delimiter = delimiter;
				_th = new Thread((ThreadStart)delegate
				{
					while (_death == false)
					{
						if (_n.get(E_INDEX.E_SEND, 2000))
						{
							int chr = 0x00;

							for (int bit = 0; bit < 8; bit++)
								if (_n.get((E_INDEX)((int)E_INDEX.E_BIT_0 + bit)))
									chr |= 1 << bit;

							_n.set(E_INDEX.E_RECV);

							if (chr == _delimiter)
							{
								byte[] message = _buff.ToArray();

								lock (SYNCROOT)
								{
									if (_messages.Count < BUFF_MAX)
										_messages.Enqueue(message);
								}
								_buff.Clear();
							}
							else
							{
								if (_buff.Count < MESSAGE_SIZE_MAX)
									_buff.Enqueue((byte)chr);
							}
						}
					}
				});
				_th.Start();
			}

			/// <summary>
			/// thread safe
			/// </summary>
			/// <returns></returns>
			public byte[] recv()
			{
				lock (SYNCROOT)
				{
					return _messages.Count == 0 ? null : _messages.Dequeue();
				}
			}

			public void Dispose()
			{
				if (_n != null)
				{
					_death = true;

					_th.Join();
					_th = null;

					_n.Dispose();
					_n = null;
				}
			}
		}
	}
}
