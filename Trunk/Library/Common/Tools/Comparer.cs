using System;
using System.Collections;
using System.Collections.Generic;

namespace Mnk.Library.Common.Tools
{
	public class Comparer<T> : IComparer
	{
		private readonly Func<T, T, int> comparer;
		public Comparer(Func<T, T, int> comparer) { this.comparer = comparer; }
		public int Compare(object x, object y) { return comparer((T)x, (T)y);}
	}

    public class OrderComparer<T> : IComparer<T>
    {
        private readonly IList<T> order;

        public OrderComparer(IList<T> order)
        {
            this.order = order;
        }

        public int Compare(T x, T y)
        {
            return order.IndexOf(x) - order.IndexOf(y);
        }
    }
}
