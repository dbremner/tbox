using System;
using Mnk.Library.Common.Tools;
using Mnk.Library.Common.UI.Model;
using Mnk.Library.Common.UI.ModelsContainers;

namespace Mnk.Library.ScriptEngine
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
