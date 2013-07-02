using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Common.Tools;
using ScriptEngine;

namespace Solution.Scripts
{
	public class SelectDatabases: IScript
	{
		[StringDictionary(false, 
			"DB1", "Data Source=(local);Initial Catalog=DB1;",
			"DB2", "Data Source=(local);Initial Catalog=DB2;",
			)]
		public IDictionary<string, string> ConnectionStrings{get;set;}

		[String(false, "add")]
		public string SectionKey { get; set; }

		[String(false, "name")]
		public string KeyAttribute { get; set; }

		[String(false, "connectionString")]
		public string ValueAttribute { get; set; }

		[String(false, "connectionStrings")]
		public string XmlPath { get; set; }

		[FileList(true)]
		public string[] WebConfigsPathes { get; set; }

		public void Run()
		{
			var xmlPathEls = XmlPath.Split(new[] {'/'}, StringSplitOptions.RemoveEmptyEntries);
			foreach (var path in WebConfigsPathes)
			{
				var doc = XDocument.Load(path, LoadOptions.PreserveWhitespace);
				ProcessFile(doc, xmlPathEls);
				doc.Save(path);
			}
		}

		private void ProcessFile(XDocument doc, IEnumerable<string> xmlPathEls)
		{
			var xml = doc.Root;
			if (xml == null) return;
			foreach (var el in xmlPathEls)
			{
				xml = xml.Elements().FirstOrDefault(x => x.Name.LocalName.EqualsIgnoreCase(el));
				if (xml == null)return;
			}
			foreach (var exist in xml.Elements()
				.Where(x => x.Name.LocalName.EqualsIgnoreCase(SectionKey)).ToArray())
			{
				exist.Remove();
			}
			foreach (var s in ConnectionStrings)
			{
				var node = new XElement(SectionKey);
				node.SetAttributeValue(KeyAttribute, s.Key);
				node.SetAttributeValue(ValueAttribute, s.Value);
				xml.Add(node);
			}
		}
	}
}