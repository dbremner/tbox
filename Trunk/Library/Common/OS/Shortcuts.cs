using System;
using System.IO;

namespace Mnk.Library.Common.OS
{
	public static class Shortcuts
	{
		public static void Create(string linkPath, string appPath)
		{
		    using (var link = new ShellLink())
		    {
                link.Save(linkPath, appPath, Path.GetDirectoryName(appPath), appPath);
		    }
		}

		public static void CreateOnDesktop(string appPath)
		{
			var folder = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
			Create(Path.Combine(folder, GetLinkName(appPath)), appPath);
		}

		public static void CreateOnStartup(string appPath)
		{
			var folder = Environment.GetFolderPath(Environment.SpecialFolder.Startup);
			Create(Path.Combine(folder, GetLinkName(appPath)), appPath);
		}

		public static string GetLinkName(string appPath)
		{
			var name = Path.GetFileNameWithoutExtension(appPath);
			if (string.IsNullOrWhiteSpace(name)) 
				throw new ArgumentException("File name shouldn't be empty!");
			return name + ".lnk";
		}
	}
}
