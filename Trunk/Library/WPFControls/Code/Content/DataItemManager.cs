using System;
using Mnk.Library.Common.UI.Model;

namespace Mnk.Library.WpfControls.Code.Content
{
	public sealed class DataItemManager<T> : IDataManager
		where T : Data, ICloneable, new()
	{
		public object Create(string key)
		{
			return new T {Key = key};
		}

		public object Clone(object sample, string key)
		{
			return ChangeKey(((T) sample).Clone(), key);
		}

		public object ChangeKey(object target, string newKey)
		{
			((T) target).Key = newKey;
			return target;
		}

		public string GetKey(object target)
		{
			return ((T) target).Key;
		}
	}
}
