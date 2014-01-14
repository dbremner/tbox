using System;
using System.IO;

namespace Mnk.TBox.Plugins.Market.Client.Code
{
	static class Constants
	{
		public static string Ext = ".dll";
		public static string PluginsFolder = Path.Combine(Environment.CurrentDirectory,
			"Plugins");
		public static string DependeciesFolder = Path.Combine(Environment.CurrentDirectory,
			"Dependecies");
	}
}
