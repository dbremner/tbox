using System;
using Mnk.Library.Common.Tools;
using Mnk.Library.Common.UI.Model;
using Mnk.Library.Common.UI.ModelsContainers;

namespace Mnk.Library.ScriptEngine
{
	[Serializable]
    public sealed class MultiFileOperation : Operation
	{
		public CheckableDataCollection<CheckableData> Paths { get; set; }

		public MultiFileOperation()
		{
			Paths = new CheckableDataCollection<CheckableData>();
		}

		public override object Clone()
		{
			return new MultiFileOperation
			{
				Paths = Paths.Clone(),
				Parameters = Parameters.Clone(),
				Key = Key,
                IsChecked = IsChecked
			};
		}
	}
}
