using System;
using System.IO;
using System.Reflection;
using Common.AutoUpdate;
using Common.Base.Log;
using Common.Tools;

namespace TBox.Code.AutoUpdate
{
	public class DirectoryApplicationUpdater : IApplicationUpdater
	{
		private static readonly ILog Log = LogManager.GetLogger<DirectoryApplicationUpdater>();
		private readonly string sourcePath;
		private readonly Version version;
		public DirectoryApplicationUpdater(string sourcePath)
		{
			this.sourcePath = sourcePath;
			var filePath = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
			var targetPath = Path.Combine(sourcePath, Path.GetFileName(filePath));
			if(!File.Exists(targetPath))
			{
				Log.Write("Can't find file: " + targetPath);
				return;
			}
			version = AssemblyName.GetAssemblyName(targetPath).Version;
		}

		public bool NeedUpdate()
		{
			return (version != null) && (version > GetCurrentAssemblyName().Version);
		}

		private static AssemblyName GetCurrentAssemblyName()
		{
			return Assembly.GetExecutingAssembly().GetName();
		}

		public void Copy(string destinationPath)
		{
			new DirectoryInfo(sourcePath).CopyFilesTo(destinationPath);
		}
	}
}
