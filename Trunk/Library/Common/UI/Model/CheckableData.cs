using System;

namespace Mnk.Library.Common.UI.Model
{
	[Serializable]
	public class CheckableData : Data, ICheckableData
	{
		public bool IsChecked { get; set; }

		public CheckableData()
		{
			IsChecked = true;
		}

		public override object Clone()
		{
			return new CheckableData {Key = Key, IsChecked = IsChecked};
		}
	}

	public class CheckableData<T> : CheckableData
	{
		public T Value { get; set; }
		public CheckableData()
		{
			Value = default(T);
		}

		public override object Clone()
		{
			return new CheckableData<T> { Key = Key, IsChecked = IsChecked, Value = Value};
		}
	}
}
