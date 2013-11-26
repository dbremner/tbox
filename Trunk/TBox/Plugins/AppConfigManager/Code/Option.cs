using System;
using Common.UI.Model;

namespace AppConfigManager.Code
{
	[Serializable]
	public class Option : CheckableData
	{
		public string Value { get; set; }

		public override object Clone()
		{
			return new Option
			{
				IsChecked = IsChecked,
				Key = Key,
				Value = Value
			};
		}
	}
}
