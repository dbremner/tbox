using System.Linq;
using System.Collections.Generic;
using PluginsShared.Tools;
using ScriptEngine;

namespace Solution.Scripts
{
	public class EnableFeatures : IScript
	{
		[FileList(true)]
		public string[] ConfigFiles { get; set; }

		[StringDictionary(false, "Feature.Sample", "true")]
		public IDictionary<string, string> Features { get; set; }

		[String(false, "appSettings")]
		public string XmlSelector { get; set; }

		[String(false, "key")]
		public string KeyKey { get; set; }

		[String(false, "value")]
		public string ValueKey { get; set; }

		[String(false, "add")]
		public string SectionName { get; set; }

		public void Run()
		{
			new FeatureToggler(XmlSelector,KeyKey, ValueKey, SectionName)
				.Execute(ConfigFiles, Features.Select(x=>new KeyValuePair<string,string>(x.Key, x.Value)).ToArray());
		}
	}
}