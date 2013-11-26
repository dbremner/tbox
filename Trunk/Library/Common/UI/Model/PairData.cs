using System;

namespace Common.UI.Model
{
	[Serializable]
	public class PairData : Data
	{
		public string Value { get; set; }
		public override object Clone()
		{
			return new PairData { Key = Key, Value = Value};
		}
	}
}
