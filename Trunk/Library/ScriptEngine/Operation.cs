using System;
using System.Collections.ObjectModel;
using Common.Tools;
using Common.UI.Model;
using Common.UI.ModelsContainers;
using ScriptEngine.Core.Params;

namespace ScriptEngine
{
	[Serializable]
	public sealed class Operation : Data
	{
		public CheckableDataCollection<CheckableData> Pathes { get; set; }
		public ObservableCollection<Parameter> Parameters { get; set; }

		public Operation()
		{
			Pathes = new CheckableDataCollection<CheckableData>();
			Parameters = new ObservableCollection<Parameter>();
		}

		public override object Clone()
		{
			return new Operation
			{
				Pathes = Pathes.Clone(),
				Parameters = Parameters.Clone(),
				Key = Key,
			};
		}
	}
}
