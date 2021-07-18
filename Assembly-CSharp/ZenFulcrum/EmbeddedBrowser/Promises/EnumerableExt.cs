using System;
using System.Collections.Generic;

namespace ZenFulcrum.EmbeddedBrowser.Promises
{
	public static class EnumerableExt
	{
		public static IEnumerable<T> Empty<T>()
		{
			return new T[0];
		}

		public static IEnumerable<T> LazyEach<T>(this IEnumerable<T> source, Action<T> fn)
		{
			foreach (T item in source)
			{
				fn(item);
				yield return item;
			}
			yield break;
		}

		public static void Each<T>(this IEnumerable<T> source, Action<T> fn)
		{
			foreach (T obj in source)
			{
				fn(obj);
			}
		}

		public static void Each<T>(this IEnumerable<T> source, Action<T, int> fn)
		{
			int num = 0;
			foreach (T arg in source)
			{
				fn(arg, num);
				num++;
			}
		}
	}
}
