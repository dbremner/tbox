using System;
using System.Globalization;
using Mnk.Library.Common.UI.Model;

namespace Mnk.Library.WpfControls.Code.Content
{
	public sealed class StringValueDataManager<T> : IDataManager
		where T : CheckableData<string>, ICloneable, new()
	{
		public object Create(string key)
		{
			return new T { Value = key };
		}

		public object Clone(object sample, string key)
		{
			return ChangeKey(((T)sample).Clone(), key);
		}

		public object ChangeKey(object target, string newKey)
		{
			((T)target).Value = newKey;
			return target;
		}

		public string GetKey(object target)
		{
			return ((T)target).Value;
		}
	}

	public sealed class GuidValueDataManager<T> : IDataManager
		where T : CheckableData<Guid>, ICloneable, new()
	{
		public object Create(string key)
		{
			return new T { Value = Guid.Parse(key) };
		}

		public object Clone(object sample, string key)
		{
			return ChangeKey(((T)sample).Clone(), key);
		}

		public object ChangeKey(object target, string newKey)
		{
			((T)target).Value = Guid.Parse(newKey);
			return target;
		}

		public string GetKey(object target)
		{
			return ((T)target).Value.ToString();
		}
	}

	public sealed class IntValueDataManager<T> : IDataManager
		where T : CheckableData<int>, ICloneable, new()
	{
		public object Create(string key)
		{
            return new T { Value = int.Parse(key, CultureInfo.InvariantCulture) };
		}

		public object Clone(object sample, string key)
		{
			return ChangeKey(((T)sample).Clone(), key);
		}

		public object ChangeKey(object target, string newKey)
		{
			((T)target).Value = int.Parse(newKey, CultureInfo.InvariantCulture);
			return target;
		}

		public string GetKey(object target)
		{
			return ((T)target).Value.ToString(CultureInfo.InvariantCulture);
		}
	}

	public sealed class DoubleValueDataManager<T> : IDataManager
		where T : CheckableData<double>, ICloneable, new()
	{
		public object Create(string key)
		{
			return new T { Value = double.Parse(key) };
		}

		public object Clone(object sample, string key)
		{
			return ChangeKey(((T)sample).Clone(), key);
		}

		public object ChangeKey(object target, string newKey)
		{
		    ((T) target).Value = double.Parse(newKey, CultureInfo.InvariantCulture);
			return target;
		}

		public string GetKey(object target)
		{
			return ((T)target).Value.ToString(CultureInfo.InvariantCulture);
		}
	}
}
