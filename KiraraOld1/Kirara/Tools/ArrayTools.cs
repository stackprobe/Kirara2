using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Charlotte.Tools
{
	public static class ArrayTools
	{
		public static bool contains<T>(T[] arr, T target, Comparison<T> comp)
		{
			return indexOf<T>(arr, target, comp) != -1;
		}

		public static int indexOf<T>(T[] arr, T target, Comparison<T> comp)
		{
			for (int index = 0; index < arr.Length; index++)
				if (comp(arr[index], target) == 0)
					return index;

			return -1;
		}

		public static int arrComp<T>(T[] a, T[] b, Comparison<T> comp)
		{
			int minlen = Math.Min(a.Length, b.Length);

			for (int index = 0; index < minlen; index++)
			{
				int ret = comp(a[index], b[index]);

				if (ret != 0)
					return ret;
			}
			return IntTools.comp(a.Length, b.Length);
		}

		public static void sort<T>(T[] arr, Comparison<T> comp)
		{
			Array.Sort(arr, comp);
		}

		public static void sort<T>(List<T> list, Comparison<T> comp)
		{
			list.Sort(comp);
		}

		public static List<T> toList<T>(params T[] arr)
		{
			List<T> dest = new List<T>();

			foreach (T element in arr)
				dest.Add(element);

			return dest;
		}
	}
}
