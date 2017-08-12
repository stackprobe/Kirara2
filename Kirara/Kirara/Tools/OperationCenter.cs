using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Charlotte.Tools
{
	public class OperationCenter
	{
		public delegate void operation_d();

		private Queue<operation_d> _topPrioOperations = new Queue<operation_d>();
		private Queue<operation_d> _operations = new Queue<operation_d>();

		public void addTopPrio(operation_d operation)
		{
			_topPrioOperations.Enqueue(operation);
		}

		public void add(operation_d operation)
		{
			_operations.Enqueue(operation);
		}

		public void clear()
		{
			_topPrioOperations.Clear();
			_operations.Clear();
		}

		public int getCount()
		{
			return _topPrioOperations.Count + _operations.Count;
		}

		public void eachTimerTick()
		{
			if (1 <= _topPrioOperations.Count)
			{
				callOperation(_topPrioOperations.Dequeue());
			}
			else if (1 <= _operations.Count)
			{
				callOperation(_operations.Dequeue());
			}
		}

		private void callOperation(operation_d operation)
		{
			_retry = false;

			operation();

			if (_retry)
				_topPrioOperations.Enqueue(operation);
		}

		private static bool _retry = false;

		public static void retry()
		{
			_retry = true;
		}
	}
}
