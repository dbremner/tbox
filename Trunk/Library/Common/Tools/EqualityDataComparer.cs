using System;
using System.Collections.Generic;
using Common.UI.Model;

namespace Common.Tools
{
	public sealed class EqualityDataComparer<T> : IEqualityComparer<T>
		where T : UI.Model.Data
	{
		public bool Equals(T x, T y)
		{
			return string.Equals(x.Key, y.Key);
		}

		public int GetHashCode(T obj)
		{
			return obj.Key.GetHashCode();
		}
	}

    public sealed class EqualityNoCaseComparer : IEqualityComparer<string>
    {
        public bool Equals(string x, string y)
        {
            return string.Equals(x, y, StringComparison.InvariantCultureIgnoreCase);
        }

        public int GetHashCode(string obj)
        {
            return obj.ToLower().GetHashCode();
        }
    }

	public sealed class EqualityDataValueStringComparer : IEqualityComparer<CheckableData<string>>
	{
		public bool Equals(CheckableData<string> x, CheckableData<string> y)
		{
			return string.Equals(x.Value, y.Value);
		}

		public int GetHashCode(CheckableData<string> obj)
		{
			return obj.Value == null ? 0 : obj.Value.GetHashCode();
		}
	}

	public sealed class EqualityDataValueGuidComparer : IEqualityComparer<CheckableData<Guid>>
	{
		public bool Equals(CheckableData<Guid> x, CheckableData<Guid> y)
		{
			return x.Value == y.Value;
		}

		public int GetHashCode(CheckableData<Guid> obj)
		{
			return obj.Value.GetHashCode();
		}
	}

	public sealed class EqualityDataValueIntegerComparer : IEqualityComparer<CheckableData<int>>
	{
		public bool Equals(CheckableData<int> x, CheckableData<int> y)
		{
			return x.Value == y.Value;
		}

		public int GetHashCode(CheckableData<int> obj)
		{
			return obj.Value;
		}
	}

	public sealed class EqualityDataValueDoubleComparer : IEqualityComparer<CheckableData<double>>
	{
		public bool Equals(CheckableData<double> x, CheckableData<double> y)
		{
			return x.Value == y.Value;
		}

		public int GetHashCode(CheckableData<double> obj)
		{
			return obj.Value.GetHashCode();
		}
	}
}
