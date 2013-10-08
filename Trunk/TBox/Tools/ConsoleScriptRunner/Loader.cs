using System;
using System.IO;
using System.Reflection;
using Common.Base.Log;

namespace ConsoleScriptRunner
{
	class Loader
	{
		private static readonly ILog Log = LogManager.GetLogger<Loader>();
		private const string LibraryPath = "Libraries";

		public void Load(string rootPath)
		{
			if (Directory.Exists(LibraryPath))
			{
				System.Threading.Tasks.Parallel.ForEach(
					Directory.GetFiles(Path.Combine(rootPath, LibraryPath), "*.dll", SearchOption.AllDirectories),
					file => LoadAssembly(file)
					);
			}
		}

		private static Assembly LoadAssembly(string filePath)
		{
			try
			{
				return AppDomain.CurrentDomain.Load(AssemblyName.GetAssemblyName(filePath));
			}
			catch (Exception ex)
			{
				Log.Write(ex, "Error loading library from: '{0}'", filePath);
			}
			return null;
		}
	}
}
