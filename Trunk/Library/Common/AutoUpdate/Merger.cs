using System;
using System.IO;
using Mnk.Library.Common.Tools;

namespace Mnk.Library.Common.AutoUpdate
{
	public static class Merger
	{
		private static readonly string CurrentPath =
					AppDomain.CurrentDomain.BaseDirectory;
		
		private static readonly string BackupPath =
					Path.Combine(CurrentPath, "Backup");

		public static bool CheckDate(DateTime date, UpdateInterval interval)
		{
			var range = DateTime.Now - date;
			switch (interval)
			{
				case UpdateInterval.Startup:
					return true;
				case UpdateInterval.Never:
					return false;
				case UpdateInterval.Daily:
					if (range.Days < 1) return false;
					break;
				case UpdateInterval.Weekly:
					if (range.Days < 7) return false;
					break;
				case UpdateInterval.Monthly:
					if (range.Days < 30) return false;
					break;
			}
			return true;
		}

		public static void Merge(IApplicationUpdater applicationUpdater)
		{
			DeleteBackup();
			try
			{
				DoUpdate(applicationUpdater);
			}
			catch (Exception)
			{
				RestoreBackup();
				throw;
			}
		}

		private static void DeleteBackup()
		{
			if(Directory.Exists(BackupPath))
				Directory.Delete(BackupPath, true);
		}

		private static void RestoreBackup()
		{
			MoveFiles(BackupPath, CurrentPath);
		}

		private static void MoveFiles(string source, string destination, string mask="*.*")
		{
			new DirectoryInfo(source).MoveFilesTo(destination, mask);
		}

		private static void DoUpdate(IApplicationUpdater applicationUpdater)
		{
			MoveFiles(CurrentPath, BackupPath, "*.dll;*.exe;*.pdb");
			applicationUpdater.Copy(CurrentPath);
		}
	}
}
