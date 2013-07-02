using System;
using System.IO;
using Common.Tools;

namespace Common.AutoUpdate
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
				case UpdateInterval.Dayly:
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

		public static bool CheckNeedUpdate(IApplicationUpdater updater)
		{
			return updater.NeedUpdate();
		}

		public static void Merge(IApplicationUpdater updater)
		{
			DeleteBackup();
			try
			{
				DoUpdate(updater);
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

		private static void DoUpdate(IApplicationUpdater updater)
		{
			MoveFiles(CurrentPath, BackupPath, "*.dll;*.exe;*.pdb");
			updater.Copy(CurrentPath);
		}
	}
}
