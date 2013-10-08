using System;
using System.Collections.ObjectModel;
using Common.Tools;
using Common.UI.Model;
using ScriptEngine;

namespace ConsoleScriptRunner.Settings
{
	[Serializable]
	public sealed class Profile : Data
	{
		public ObservableCollection<Operation> Operations { get; set; }

		public Profile()
		{
			Operations = new ObservableCollection<Operation>();
		}

		public override object Clone()
		{
			return new Profile
			{
				Operations = Operations.Clone(),
			};
		}
	}
}
