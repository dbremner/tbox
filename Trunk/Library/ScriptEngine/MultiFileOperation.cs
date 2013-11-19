using System;
using Common.Tools;
using Common.UI.Model;
using Common.UI.ModelsContainers;

namespace ScriptEngine
{
	[Serializable]
    public sealed class MultiFileOperation : Operation
	{
		public CheckableDataCollection<CheckableData> Pathes { get; set; }

		public MultiFileOperation()
		{
			Pathes = new CheckableDataCollection<CheckableData>();
		}

		public override object Clone()
		{
			return new MultiFileOperation
			{
				Pathes = Pathes.Clone(),
				Parameters = Parameters.Clone(),
				Key = Key,
                IsChecked = IsChecked
			};
		}
	}
}
