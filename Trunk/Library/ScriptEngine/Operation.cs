using System;
using System.Collections.ObjectModel;
using Mnk.Library.Common.Tools;
using Mnk.Library.Common.UI.Model;
using Newtonsoft.Json;
using ScriptEngine.Core.Params;

namespace Mnk.Library.ScriptEngine
{
	[Serializable]
	public class Operation : CheckableData
	{
        [JsonProperty(ItemTypeNameHandling = TypeNameHandling.All)]
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
