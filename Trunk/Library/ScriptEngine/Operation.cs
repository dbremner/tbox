using System;
using System.Collections.ObjectModel;
using Common.Tools;
using Common.UI.Model;
using ScriptEngine.Core.Params;

namespace ScriptEngine
{
	[Serializable]
	public class Operation : CheckableData
	{
		public ObservableCollection<Parameter> Parameters { get; set; }

        public Operation()
		{
			Parameters = new ObservableCollection<Parameter>();
		}

		public override object Clone()
		{
			return new MultiFileOperation
			{
				Parameters = Parameters.Clone(),
				Key = Key,
                IsChecked = IsChecked
			};
		}
	}
}
