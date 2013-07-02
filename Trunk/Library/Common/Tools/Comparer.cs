using System;
using System.Collections;

namespace Common.Tools
{
	public class Comparer<T> : IComparer
	{
		private readonly Func<T, T, int> comparer;
		public Comparer(Func<T, T, int> comparer) { this.comparer = comparer; }
		public int Compare(object x, object y) { return comparer((T)x, (T)y);}
	}
}
