using System;

namespace Common.UI.Model
{
	[Serializable]
	public class Data : IData, ICloneable
	{
		public string Key { get; set; }
		public override string ToString()
		{
			return Key;
		}
		public virtual object Clone()
		{
			return new Data {Key = Key};
		}
	}
}
