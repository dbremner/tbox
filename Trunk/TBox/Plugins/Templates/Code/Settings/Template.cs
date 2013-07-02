using System.Collections.ObjectModel;
using Common.Tools;
using Common.UI.Model;

namespace Templates.Code.Settings
{
	public class Template : Data
	{
		public string Value { get; set; }
		public ObservableCollection<PairData> KnownValues { get; set; }

		public Template()
		{
			KnownValues = new ObservableCollection<PairData>();
			Value = string.Empty;
		}

		public override object Clone()
		{
			return new Template {Key = Key, Value = Value, KnownValues = KnownValues.Clone()};
		}
	}
}
