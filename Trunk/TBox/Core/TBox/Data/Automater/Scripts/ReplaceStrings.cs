using System.Collections.Generic;
using System.IO;
using Mnk.TBox.Core.PluginsShared.Automator;
using Mnk.Library.ScriptEngine;

namespace Solution.Scripts
{
	public class ReplaceStrings : IScript
	{
		[File]
		public string TargetPath { get; set; }
		
		public IDictionary<string, string> ValuesToReplace { get; set; }

		public void Run(IScriptContext context)
		{
			var text = File.ReadAllText(TargetPath);
			foreach(var item in ValuesToReplace)
			{
				text = text.Replace(item.Key, item.Value);	
			}
			File.WriteAllText(TargetPath, text);
		}
	}
}
