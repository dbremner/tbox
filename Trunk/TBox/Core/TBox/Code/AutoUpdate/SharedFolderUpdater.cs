using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Common.AutoUpdate;
using Common.Base.Log;
using Common.Tools;
using WPFControls.Code.OS;

namespace TBox.Code.AutoUpdate
{
	public class SharedFolderApplicationUpdater : IApplicationUpdater
	{
		private static readonly ILog Log = LogManager.GetLogger<SharedFolderApplicationUpdater>();
		private readonly string sourcePath;
		private readonly Version version;
		public SharedFolderApplicationUpdater(string sourcePath)
		{
			this.sourcePath = sourcePath;
			var filePath = Process.GetCurrentProcess().MainModule.FileName;
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
			return (version != null) && (version > GetCurrentAssemblyVersion());
		}

		private static Version GetCurrentAssemblyVersion()
		{
			return Assembly.GetExecutingAssembly().GetName().Version;
		}

		public void Copy(string destinationPath)
		{
			new DirectoryInfo(sourcePath).CopyFilesTo(destinationPath);
		}

		public void Update()
		{
			Merger.Merge(this);
			OneInstance.App.AfterExit += AfterExit;
		}

		private static void AfterExit(object sender, EventArgs e)
		{
			using(Process.Start(Process.GetCurrentProcess().MainModule.FileName)){}
		}

	}
}
