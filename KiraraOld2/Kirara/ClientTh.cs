using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Charlotte.Tools;
using System.IO;
using System.Diagnostics;

namespace Charlotte
{
	public class ClientTh : EngineTh
	{
		private Nectar2.Wrapper _n2w = new Nectar2.Wrapper(
			new Nectar2.Sender(Consts.N2_SEND_IDENT),
			new Nectar2.Recver(Consts.N2_RECV_IDENT),
			StringTools.ENCODING_SJIS
			);

		public override void init()
		{
			// TODO
		}

		public override void fnlz()
		{
			// TODO
		}

		public override void perform()
		{
			// TODO
		}
	}
}
