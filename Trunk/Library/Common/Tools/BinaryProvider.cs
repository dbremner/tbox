using System;
using System.Collections.Generic;

namespace Mnk.Library.Common.Tools
{
	public class BinaryComparer<T> : IComparer<T>
	{
		private readonly Func<T, string> nameProvider;
		public BinaryComparer(Func<T, string> nameProvider) { this.nameProvider = nameProvider; }
		public int Compare(T x, T y) { return string.Compare(nameProvider(x), nameProvider(y), StringComparison.CurrentCultureIgnoreCase); }
	}
}
