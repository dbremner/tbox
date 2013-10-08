using System;
using System.IO;

namespace ConsoleScriptRunner
{
	static class Folders
	{
		public static readonly string Application = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "TBox");
	}
}
