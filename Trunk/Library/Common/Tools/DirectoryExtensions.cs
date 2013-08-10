using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Common.Base.Log;

namespace Common.Tools
{
	public static class DirectoryExtensions
	{
		public static void CopyFilesTo(this DirectoryInfo info, string destination)
		{
			var dirs = info.GetDirectories();
			if (!Directory.Exists(destination))
			{
				Directory.CreateDirectory(destination);
			}
			foreach (var file in info.GetFiles())
			{
				file.CopyTo(Path.Combine(destination, file.Name), true);
			}
			foreach (var dir in dirs)
			{
				dir.CopyFilesTo(Path.Combine(destination, dir.Name));
			}
		}

		public static void MoveFilesTo(this DirectoryInfo info, string destination, string mask="*.*")
		{
			var dirs = info.GetDirectories();
			if (!Directory.Exists(destination))
			{
				Directory.CreateDirectory(destination);
			}
			foreach (var file in 
				mask.Split(new[]{';'},StringSplitOptions.RemoveEmptyEntries)
				.SelectMany(info.GetFiles))
			{
				file.MoveTo(Path.Combine(destination, file.Name));
			}
			foreach (var dir in dirs)
			{
				dir.MoveFilesTo(Path.Combine(destination, dir.Name), mask);
			}
		}

		public static IEnumerable<DirectoryInfo> SafeEnumerateDirectories(this DirectoryInfo info, ILog log)
		{
			try
			{
				return info.EnumerateDirectories();
			}
			catch (DirectoryNotFoundException){}
			catch (UnauthorizedAccessException){}
			catch (PathTooLongException ex)
			{
				log.Write(ex, "Can't access to directory: " + info.Name);
			}
			catch (Exception ex)
			{
				log.Write(ex, "Can't access to directory: " + info.FullName);
			}
			return new DirectoryInfo[0];
		}

		public static IEnumerable<FileInfo> SafeEnumerateFiles(this DirectoryInfo info, ILog log, string mask = "*.*")
		{
			try
			{
				return info.EnumerateFiles(mask);
			}
			catch (FileNotFoundException) { }
			catch (UnauthorizedAccessException) { }
			catch (PathTooLongException ex)
			{
				log.Write(ex, "Can't access to file:" + info.Name);
			}
			catch (Exception ex)
			{
				log.Write(ex, "Can't access to directory files: " + info.FullName);
			}
			return new FileInfo[0];
		}

		public static void MoveIfExist(this DirectoryInfo source, string destination)
		{
			if(!source.Exists)return;
			source.MoveFilesTo(destination);
            source.Delete();
		}

	}
}
