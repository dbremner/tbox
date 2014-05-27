using System;
using System.Collections.Generic;
using System.IO;
using Mnk.Library.Common.Models;

namespace Mnk.Library.Common.SaveLoad
{
	public static class ExtFile
	{
		public const int DefaultBufferSize = 1024*1024;

		public static void Save(string path, Stream source, int bufferSize = DefaultBufferSize, Action<int> proc = null)
		{
			using (var destination = new FileStream(path, FileMode.Create, FileAccess.Write ))
			{
				Copy(destination, source, bufferSize, proc);
			}
		}

		public static void Save(FileInfo info, Stream source, int bufferSize = DefaultBufferSize, Action<int> proc = null)
		{
			using (var destination = info.OpenWrite())
			{
				Copy(destination, source, bufferSize, proc);
			}
		}

		public static void Load(string path, Stream destination, int bufferSize = DefaultBufferSize, Action<int> proc = null)
		{
			using (var source = new FileStream(path, FileMode.Open, FileAccess.Read))
			{
				Copy(destination, source, bufferSize, proc);
			}
		}

		public static void Load(FileInfo info, Stream destination, int bufferSize = DefaultBufferSize, Action<int> proc = null)
		{
			using (var source = info.OpenRead())
			{
				Copy(destination, source, bufferSize, proc);
			}
		}

		public static void Copy(Stream destination, Stream source, int bufferSize = DefaultBufferSize, Action<int> proc = null)
		{
			var buffer = new byte[bufferSize];
			int bytes;
			while ((bytes = source.Read(buffer, 0, buffer.Length)) > 0)
			{
				destination.Write(buffer, 0, bytes);
				if (proc != null) proc(bytes);
				destination.Flush();
			}
		}


		public static void Save(string path, int size, Stream source, int bufferSize = DefaultBufferSize, Action<int> proc = null)
		{
			using (var destination = new FileStream(path, FileMode.Create, FileAccess.Write))
			{
				Copy(destination, size, source, bufferSize, proc);
			}
		}

		public static void Save(FileInfo info, int size, Stream source, int bufferSize = DefaultBufferSize, Action<int> proc = null)
		{
			using (var destination = info.OpenWrite())
			{
				Copy(destination, size, source, bufferSize, proc);
			}
		}

		public static void Copy(Stream destination, int size, Stream source, int bufferSize = DefaultBufferSize, Action<int> proc = null)
		{
			var buffer = new byte[bufferSize];
			int bytes;
			while (size > 0 && (bytes = source.Read(buffer, 0, Math.Min(buffer.Length, size))) > 0)
			{
				size -= bytes;
				destination.Write(buffer, 0, bytes);
				if (proc != null) proc(bytes);
				destination.Flush();
			}
		}

		public static void DeleteIfExist(string filePath)
		{
			if(File.Exists(filePath))
			{
				File.Delete(filePath);
			}
		}

		public static void RecreateDirectory(string dirPath)
		{
			if (Directory.Exists(dirPath))
			{
				Directory.Delete(dirPath, true);
			}
			Directory.CreateDirectory(dirPath);
		}


		public static int LoadDirectoryFiles(IEnumerable<string> paths, out Pair<string, int>[] descriptions, out Stream stream)
		{
			stream = new MemoryStream();
			var length = 0;
			var descriptionsList = new List<Pair<string, int>>();
			foreach (var path in paths )
			{
				var info = new FileInfo(path);
				if (info.Attributes == FileAttributes.Hidden) continue;
				var buf = new byte[info.Length];

				using (var s = info.OpenRead())
				{
					s.Read(buf, 0, buf.Length);
					stream.Write(buf, 0, buf.Length);
					length += buf.Length;
				}
				descriptionsList.Add(new Pair<string, int>(info.Name, (int)info.Length));
			}
			stream.Position = 0;
			descriptions = descriptionsList.ToArray();
			return length;
		}

		public static void SaveDirectoryFiles(string dir, Pair<string, int>[] descriptions, Stream stream, int bufferSize = DefaultBufferSize, Action<int> proc = null)
		{
			foreach (var file in descriptions)
			{
				Save(Path.Combine(dir, file.Key), file.Value, stream, bufferSize, proc);
			}
		}
	}
}
